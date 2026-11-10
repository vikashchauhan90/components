using Jnvp.Platform.Caching.Distributed;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Caching;

/// <summary>
/// Provides extension methods for registering distributed caching services.
/// </summary>
public static class DistributedCacheServiceCollectionExtensions
{
    /// <summary>
    /// Registers an <see cref="ICacheProvider"/> backed by the configured
    /// <see cref="IDistributedCache"/> implementation.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <remarks>
    /// Register an <see cref="IDistributedCache"/> implementation before
    /// calling this method. Tag invalidation uses distributed tag-version
    /// records, so entries tagged before an invalidation become cache misses
    /// across all application instances sharing the distributed cache.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddStackExchangeRedisCache(options =>
    ///     options.Configuration = connectionString);
    /// services.AddDistributedCaching();
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddDistributedCaching(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOptions<CacheEntryOptions>()
            .ValidateOnStart();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IValidateOptions<CacheEntryOptions>,
                CacheEntryOptionsValidator>());
        services.TryAddSingleton<
            ICacheProvider,
            DistributedCacheProvider>();

        return services;
    }
}
