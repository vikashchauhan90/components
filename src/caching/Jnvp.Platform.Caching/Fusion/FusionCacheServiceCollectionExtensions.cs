using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Jnvp.Platform.Caching.Fusion;

/// <summary>
/// Provides extension methods for registering FusionCache-based
/// caching services.
/// </summary>
public static class FusionCacheServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default FusionCache instance and the platform cache
    /// provider.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddFusionCaching(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddFusionCache()
            .TryWithAutoSetup();

        services
            .AddOptions<CacheEntryOptions>()
            .ValidateOnStart();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IValidateOptions<CacheEntryOptions>,
                CacheEntryOptionsValidator>());
        services.TryAddSingleton<
            ICacheProvider,
            FusionCacheProvider>();

        return services;
    }

    /// <summary>
    /// Registers FusionCache and configures its options.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <param name="configure">A delegate that configures FusionCache options.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="configure"/>
    /// is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddFusionCaching(
        this IServiceCollection services,
        Action<FusionCacheOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddFusionCache()
            .WithOptions(configure)
            .TryWithAutoSetup();

        services
            .AddOptions<CacheEntryOptions>()
            .ValidateOnStart();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IValidateOptions<CacheEntryOptions>,
                CacheEntryOptionsValidator>());
        services.TryAddSingleton<
            ICacheProvider,
            FusionCacheProvider>();

        return services;
    }
}
