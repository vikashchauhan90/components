using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.DependencyInjection;
using Polly.Retry;
using Polly.Timeout;
using System.Net;

namespace Jnvp.Platform.Resilience;

/// <summary>Registers the standard resilience policy and its supporting services.</summary>
/// <remarks>
/// The registered policy retries transient HTTP failures, honors the
/// <c>Retry-After</c> header for HTTP 429 responses, applies a circuit breaker
/// and timeout, and logs retry and circuit-breaker state changes through
/// <see cref="ILogger"/>.
/// </remarks>
/// <example>
/// Register the policy from application configuration:
/// <code language="csharp">
/// var builder = WebApplication.CreateBuilder(args);
/// builder.Services.AddPlatformResilience();
/// var app = builder.Build();
/// </code>
/// Or bind settings from a configuration section:
/// <code language="csharp">
/// builder.Services.AddPlatformResilience(
///     builder.Configuration.GetSection("Resilience"));
/// </code>
/// Inject <see cref="IResilienceExecutor"/> into services to execute operations
/// through the registered policy.
/// </example>
public static class ResilienceServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default resilience policy and allows its settings to be
    /// configured in code.
    /// </summary>
    /// <param name="services">The service collection to register resilience services with.</param>
    /// <param name="configureOptions">
    /// An optional callback for changing <see cref="ResilienceOptions"/>. If omitted,
    /// the documented package defaults are used.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="Microsoft.Extensions.Options.OptionsValidationException">
    /// Thrown at application start if the configured policy values are invalid.
    /// </exception>
    /// <example>
    /// <code language="csharp">
    /// builder.Services.AddPlatformResilience(options =&gt;
    /// {
    ///     options.MaxRetryAttempts = 4;
    ///     options.RetryDelay = TimeSpan.FromMilliseconds(500);
    ///     options.Timeout = TimeSpan.FromSeconds(8);
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddPlatformResilience(
        this IServiceCollection services,
        Action<ResilienceOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var optionsBuilder = services.AddOptions<ResilienceOptions>();
        if (configureOptions is not null)
        {
            optionsBuilder.Configure(configureOptions);
        }

        return AddPlatformResilienceCore(services);
    }

    /// <summary>
    /// Registers the default resilience policy and binds its settings from a
    /// configuration section. Settings omitted from the section retain their
    /// package defaults.
    /// </summary>
    /// <param name="services">The service collection to register resilience services with.</param>
    /// <param name="configurationSection">
    /// The configuration section to bind to <see cref="ResilienceOptions"/>.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="configurationSection"/>
    /// is <see langword="null"/>.
    /// </exception>
    /// <exception cref="Microsoft.Extensions.Options.OptionsValidationException">
    /// Thrown at application start if the bound policy values are invalid.
    /// </exception>
    /// <example>
    /// Given an <c>appsettings.json</c> section such as:
    /// <code language="json">
    /// {
    ///   "Resilience": {
    ///     "MaxRetryAttempts": 4,
    ///     "RetryDelay": "00:00:00.500",
    ///     "Timeout": "00:00:08"
    ///   }
    /// }
    /// </code>
    /// bind it during service registration:
    /// <code language="csharp">
    /// builder.Services.AddPlatformResilience(
    ///     builder.Configuration.GetSection("Resilience"));
    /// </code>
    /// </example>
    public static IServiceCollection AddPlatformResilience(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);

        services.AddOptions<ResilienceOptions>().Bind(configurationSection);
        return AddPlatformResilienceCore(services);
    }

    private static IServiceCollection AddPlatformResilienceCore(IServiceCollection services)
    {
        AddValidatedResilienceOptions(services);

        services.AddLogging();

        services.AddResiliencePipeline(
            ResiliencePolicies.Default,
            (builder, context) =>
            {
                var options = context.ServiceProvider
                    .GetRequiredService<IOptions<ResilienceOptions>>()
                    .Value;
                var logger = context.ServiceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Jnvp.Platform.Resilience");

                ConfigureHttpPipeline(builder, options, logger);
            });

        services.TryAddSingleton<IResilienceExecutor, ResilienceExecutor>();

        return services;
    }

    internal static void AddValidatedResilienceOptions(IServiceCollection services)
    {
        services
            .AddOptions<ResilienceOptions>()
            .Validate(
                static options =>
                    options.MaxRetryAttempts >= 0 &&
                    options.RetryDelay >= TimeSpan.Zero &&
                    options.FailureRatio is > 0 and <= 1 &&
                    options.MinimumThroughput > 0 &&
                    options.SamplingDuration > TimeSpan.Zero &&
                    options.BreakDuration > TimeSpan.Zero &&
                    options.Timeout > TimeSpan.Zero,
                "Resilience settings must use non-negative retries and delays, a failure ratio greater than 0 and at most 1, and positive durations and throughput.")
            .ValidateOnStart();
    }

    internal static void ConfigureHttpPipeline(
        ResiliencePipelineBuilder builder,
        ResilienceOptions options,
        ILogger logger)
    {
        builder.AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = options.MaxRetryAttempts,
            Delay = options.RetryDelay,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            ShouldHandle = static args => new ValueTask<bool>(
                ShouldRetry(args.Outcome.Exception, args.Outcome.Result as HttpResponseMessage)),
            DelayGenerator = static args => new ValueTask<TimeSpan?>(
                GetRetryAfterDelay(args.Outcome.Result as HttpResponseMessage)),
            OnRetry = args =>
            {
                logger.LogWarning(
                    args.Outcome.Exception,
                    "Resilience policy retrying after attempt {AttemptNumber} in {RetryDelay}; HTTP status: {StatusCode}.",
                    args.AttemptNumber,
                    args.RetryDelay,
                    (int?)((args.Outcome.Result as HttpResponseMessage)?.StatusCode));

                if (args.Outcome.Result is HttpResponseMessage response)
                {
                    response.Dispose();
                }

                return default;
            }
        });

        builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            FailureRatio = options.FailureRatio,
            MinimumThroughput = options.MinimumThroughput,
            SamplingDuration = options.SamplingDuration,
            BreakDuration = options.BreakDuration,
            ShouldHandle = static args => new ValueTask<bool>(
                ShouldBreakCircuit(args.Outcome.Exception, args.Outcome.Result as HttpResponseMessage)),
            OnOpened = _ =>
            {
                logger.LogWarning("Resilience policy circuit breaker opened.");
                return default;
            },
            OnHalfOpened = _ =>
            {
                logger.LogInformation("Resilience policy circuit breaker is half-open.");
                return default;
            },
            OnClosed = _ =>
            {
                logger.LogInformation("Resilience policy circuit breaker closed.");
                return default;
            }
        });

        builder.AddTimeout(options.Timeout);
    }

    internal static void ConfigureHttpPipeline(
        ResiliencePipelineBuilder<HttpResponseMessage> builder,
        ResilienceOptions options,
        ILogger logger)
    {
        builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            MaxRetryAttempts = options.MaxRetryAttempts,
            Delay = options.RetryDelay,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            ShouldHandle = static args => new ValueTask<bool>(
                ShouldRetry(args.Outcome.Exception, args.Outcome.Result)),
            DelayGenerator = static args => new ValueTask<TimeSpan?>(
                GetRetryAfterDelay(args.Outcome.Result)),
            OnRetry = args =>
            {
                logger.LogWarning(
                    args.Outcome.Exception,
                    "HTTP resilience policy retrying after attempt {AttemptNumber} in {RetryDelay}; HTTP status: {StatusCode}.",
                    args.AttemptNumber,
                    args.RetryDelay,
                    (int?)args.Outcome.Result?.StatusCode);

                args.Outcome.Result?.Dispose();
                return default;
            }
        });

        builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
        {
            FailureRatio = options.FailureRatio,
            MinimumThroughput = options.MinimumThroughput,
            SamplingDuration = options.SamplingDuration,
            BreakDuration = options.BreakDuration,
            ShouldHandle = static args => new ValueTask<bool>(
                ShouldBreakCircuit(args.Outcome.Exception, args.Outcome.Result)),
            OnOpened = _ =>
            {
                logger.LogWarning("HTTP resilience policy circuit breaker opened.");
                return default;
            },
            OnHalfOpened = _ =>
            {
                logger.LogInformation("HTTP resilience policy circuit breaker is half-open.");
                return default;
            },
            OnClosed = _ =>
            {
                logger.LogInformation("HTTP resilience policy circuit breaker closed.");
                return default;
            }
        });

        builder.AddTimeout(options.Timeout);
    }

    private static bool ShouldRetry(
        Exception? exception,
        HttpResponseMessage? response) =>
        exception is HttpRequestException or TimeoutRejectedException ||
        response is not null &&
        (response.StatusCode == HttpStatusCode.RequestTimeout ||
         response.StatusCode == HttpStatusCode.TooManyRequests ||
         (int)response.StatusCode >= (int)HttpStatusCode.InternalServerError);

    private static bool ShouldBreakCircuit(
        Exception? exception,
        HttpResponseMessage? response) =>
        exception is HttpRequestException or TimeoutRejectedException ||
        response is not null &&
        (response.StatusCode == HttpStatusCode.RequestTimeout ||
         (int)response.StatusCode >= (int)HttpStatusCode.InternalServerError);

    private static TimeSpan? GetRetryAfterDelay(HttpResponseMessage? response)
    {
        if (response?.StatusCode != HttpStatusCode.TooManyRequests)
        {
            return null;
        }

        var retryAfter = response.Headers.RetryAfter;
        if (retryAfter?.Delta is TimeSpan delta && delta >= TimeSpan.Zero)
        {
            return delta;
        }

        if (retryAfter?.Date is DateTimeOffset date)
        {
            var delay = date - DateTimeOffset.UtcNow;
            return delay > TimeSpan.Zero ? delay : TimeSpan.Zero;
        }

        return null;
    }
}
