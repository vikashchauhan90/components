using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace Jnvp.Platform.Caching.Redis;

/// <summary>
/// Provides extension methods for registering Redis-backed caching services.
/// </summary>
public static class RedisCacheServiceCollectionExtensions
{
    /// <summary>
    /// Registers Redis as the distributed cache backend and adds the platform
    /// cache provider.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="connectionString"/>
    /// is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="connectionString"/> is empty or whitespace.
    /// </exception>
    /// <example>
    /// <code language="csharp">
    /// services.AddRedisCaching(connectionString);
    /// </code>
    /// </example>
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddStackExchangeRedisCache(
            options => options.Configuration = connectionString);

        services.AddDistributedCaching();

        return services;
    }

    /// <summary>
    /// Registers Redis as the distributed cache backend using the specified
    /// Redis cache options, and adds the platform cache provider.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <param name="configure">A delegate that configures Redis cache options.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="configure"/>
    /// is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code language="csharp">
    /// services.AddRedisCaching(options =>
    /// {
    ///     options.Configuration = connectionString;
    ///     options.InstanceName = "my-application:";
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        Action<RedisCacheOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddStackExchangeRedisCache(configure);
        services.AddDistributedCaching();

        return services;
    }
}
