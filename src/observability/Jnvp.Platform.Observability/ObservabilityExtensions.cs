using Jnvp.Platform.Observability.Metrics;
using Jnvp.Platform.Observability.Processors;
using Jnvp.Platform.Observability.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;

namespace Jnvp.Platform.Observability;

/// <summary>Registers platform logging, metrics, and tracing services with dependency injection.</summary>
/// <example>
/// <code language="csharp">
/// services.AddLogProcessorService(configuration.GetSection("Logging:Processor"));
/// services.AddMeter&lt;OrdersMeter&gt;();
/// services.AddActivitySource&lt;OrdersActivitySource&gt;();
/// </code>
/// </example>
public static class ObservabilityExtensions
{
    /// <summary>Binds and validates options, and registers the platform log processor.</summary>
    public static IServiceCollection AddLogProcessorService(
        this IServiceCollection services,
        IConfigurationSection section)
    {
        services
            .AddOptions<LogProcessorOptions>()
            .Bind(section)
            .ValidateOnStart();

        services.AddSingleton<
            IValidateOptions<LogProcessorOptions>,
            LogProcessorOptionValidator>();

        services.AddSingleton<LogProcessor>();

        return services;
    }

    /// <summary>Registers a singleton application meter.</summary>
    public static IServiceCollection AddMeter<TMeter>(
        this IServiceCollection services)
        where TMeter : MeterBase
    {
        services.AddSingleton<TMeter>();

        return services;
    }

    /// <summary>Registers a singleton application activity source.</summary>
    public static IServiceCollection AddActivitySource<TActivitySource>(
        this IServiceCollection services)
        where TActivitySource : ActivitySourceBase
    {
        services.AddSingleton<TActivitySource>();

        return services;
    }
}