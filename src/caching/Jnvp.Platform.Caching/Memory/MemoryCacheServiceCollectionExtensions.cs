using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jnvp.Platform.Caching.Memory;

/// <summary>
/// Provides extension methods for registering an in-memory
/// <see cref="ICacheProvider"/> implementation.
/// </summary>
public static class MemoryCacheServiceCollectionExtensions
{
    /// <summary>
    /// Registers the in-memory cache provider and its dependencies.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the caching services to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <remarks>
    /// This method registers:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="IMemoryCache"/> using the default Microsoft memory cache implementation.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="ICacheProvider"/> using <see cref="MemoryCacheProvider"/>.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// The registered provider supports absolute expiration, sliding
    /// expiration, cache entry removal, and tag-based invalidation.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddMemoryCaching();
    /// </code>
    /// </example>
    public static IServiceCollection AddMemoryCaching(
        this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.TryAddSingleton<
            ICacheProvider,
            MemoryCacheProvider>();

        return services;
    }

    /// <summary>
    /// Registers the in-memory cache provider and configures the underlying
    /// <see cref="MemoryCacheOptions"/>.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the caching services to.
    /// </param>
    /// <param name="configure">
    /// A delegate used to configure the underlying
    /// <see cref="MemoryCacheOptions"/>.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <example>
    /// <code language="csharp">
    /// services.AddMemoryCaching(options =>
    /// {
    ///     options.SizeLimit = 10_000;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddMemoryCaching(
        this IServiceCollection services,
        Action<MemoryCacheOptions> configure)
    {
        services.AddMemoryCaching();

        services.Configure(configure);

        return services;
    }
}
