using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Jnvp.Platform.Caching.Redis;

public static class RedisCacheServiceCollectionExtensions
{
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        string connectionString)
    {
        services.TryAddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(
                connectionString));

        services.TryAddSingleton<
            ICacheProvider,
            RedisCacheProvider>();

        return services;
    }
}
