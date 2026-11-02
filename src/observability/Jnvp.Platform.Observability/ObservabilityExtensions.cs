using Jnvp.Platform.Observability.Metrics;
using Jnvp.Platform.Observability.Processors;
using Jnvp.Platform.Observability.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;

namespace Jnvp.Platform.Observability;

public static class ObservabilityExtensions
{
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

    public static IServiceCollection AddMeter<TMeter>(
        this IServiceCollection services)
        where TMeter : MeterBase
    {
        services.AddSingleton<TMeter>();

        return services;
    }

    public static IServiceCollection AddActivitySource<TActivitySource>(
        this IServiceCollection services)
        where TActivitySource : ActivitySourceBase
    {
        services.AddSingleton<TActivitySource>();

        return services;
    }
}