using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Resilience;

/// <summary>Provides HTTP client extensions for the platform resilience policy.</summary>
public static class PlatformResilienceHttpClientBuilderExtensions
{
    /// <summary>
    /// Adds a custom HTTP resilience handler using the platform retry,
    /// circuit-breaker, and timeout behavior.
    /// </summary>
    /// <param name="builder">The HTTP client builder to configure.</param>
    /// <returns>The same builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// Register options with <see cref="ResilienceServiceCollectionExtensions.AddPlatformResilience(
    /// IServiceCollection, Action{ResilienceOptions})"/> to share customized settings.
    /// If that registration is omitted, the package defaults are used. This handler
    /// excludes HTTP 429 responses from the circuit-breaker failure count, while the
    /// retry strategy handles them and honors the <c>Retry-After</c> header. Add this
    /// handler only once to a given client.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// builder.Services.AddPlatformResilience(options =&gt;
    /// {
    ///     options.MaxRetryAttempts = 4;
    ///     options.RetryDelay = TimeSpan.FromMilliseconds(500);
    /// });
    ///
    /// builder.Services
    ///     .AddHttpClient&lt;CatalogClient&gt;()
    ///     .AddPlatformResilienceHandler();
    /// </code>
    /// </example>
    public static IHttpClientBuilder AddPlatformResilienceHandler(
        this IHttpClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ResilienceServiceCollectionExtensions.AddValidatedResilienceOptions(
            builder.Services);

        builder.AddResilienceHandler(
            "Jnvp.Platform.Resilience.Http",
            (pipelineBuilder, context) =>
            {
                var options = context.ServiceProvider
                    .GetRequiredService<IOptions<ResilienceOptions>>()
                    .Value;
                var logger = context.ServiceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Jnvp.Platform.Resilience");

                ResilienceServiceCollectionExtensions.ConfigureHttpPipeline(
                    pipelineBuilder,
                    options,
                    logger);
            });

        return builder;
    }
}
