using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Jnvp.Platform.Caching.Fusion;

/// <summary>
/// Provides extension methods for registering FusionCache-based
/// caching services.
/// </summary>
public static class FusionCacheServiceCollectionExtensions
{
    public static IServiceCollection AddFusionCaching(
        this IServiceCollection services)
    {
        services.AddFusionCache();

        services.TryAddSingleton<
            ICacheProvider,
            FusionCacheProvider>();

        return services;
    }

    public static IServiceCollection AddFusionCaching(
        this IServiceCollection services,
        Action<FusionCacheOptions> configure)
    {
        services.AddFusionCache();

        services.Configure(configure);

        services.TryAddSingleton<
            ICacheProvider,
            FusionCacheProvider>();

        return services;
    }
}
