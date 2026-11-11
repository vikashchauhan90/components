using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;

namespace Jnvp.Platform.Caching.Fusion;

/// <summary>
/// Provides extension methods for registering the Redis-backed FusionCache
/// provider.
/// </summary>
public static class FusionCacheServiceCollectionExtensions
{
    /// <summary>
    /// Registers FusionCache with Redis distributed storage, Redis backplane
    /// synchronization, and a MessagePack serializer using LZ4 compression.
    /// </summary>
    /// <param name="services">The service collection to add caching services to.</param>
    /// <param name="redisConfiguration">
    /// A Redis connection string or configuration string. Keep credentials in
    /// a secret store or environment-based configuration.
    /// </param>
    /// <param name="instanceName">
    /// A namespace shared by application replicas using the same cache, and
    /// unique among applications or environments sharing Redis.
    /// </param>
    /// <param name="configureRedisCache">
    /// An optional callback for Redis distributed-cache options.
    /// </param>
    /// <param name="configureRedisBackplane">
    /// An optional callback for Redis backplane options.
    /// </param>
    /// <param name="configureFusionCache">
    /// An optional callback for FusionCache-wide options.
    /// </param>
    /// <param name="configureDefaultEntryOptions">
    /// An optional callback for FusionCache default entry behavior, such as
    /// duration, fail-safe, and factory timeouts.
    /// </param>
    /// <param name="messagePackOptions">
    /// Optional MessagePack serializer options. When omitted, contractless
    /// MessagePack serialization with LZ4 block-array compression is used.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the Redis configuration or instance name is empty or
    /// whitespace.
    /// </exception>
    public static IServiceCollection AddFusionRedisCaching(
        this IServiceCollection services,
        string redisConfiguration,
        string instanceName,
        Action<RedisCacheOptions>? configureRedisCache = null,
        Action<RedisBackplaneOptions>? configureRedisBackplane = null,
        Action<FusionCacheOptions>? configureFusionCache = null,
        Action<FusionCacheEntryOptions>? configureDefaultEntryOptions = null,
        MessagePackSerializerOptions? messagePackOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(redisConfiguration);
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceName);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration;
            options.InstanceName = $"{instanceName}:";
            configureRedisCache?.Invoke(options);
        });

        var serializerOptions = messagePackOptions
            ?? MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolver.Instance)
                .WithCompression(MessagePackCompression.Lz4BlockArray);

        var builder = services.AddFusionCache()
            .WithOptions(options =>
            {
                options.CacheName = instanceName;
                configureFusionCache?.Invoke(options);
            });

        if (configureDefaultEntryOptions is not null)
        {
            builder.WithDefaultEntryOptions(configureDefaultEntryOptions);
        }

        builder
            .WithNeueccMessagePackSerializer(serializerOptions)
            .WithStackExchangeRedisBackplane(options =>
            {
                options.Configuration = redisConfiguration;
                configureRedisBackplane?.Invoke(options);
            })
            .TryWithAutoSetup();

        services.TryAddSingleton<ICacheProvider, FusionCacheProvider>();

        return services;
    }
}
