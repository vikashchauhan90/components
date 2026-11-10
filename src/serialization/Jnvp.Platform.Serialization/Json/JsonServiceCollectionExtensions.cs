using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jnvp.Platform.Serialization.Json;

/// <summary>
/// Provides extension methods for registering JSON-based
/// <see cref="ISerializer"/> services.
/// </summary>
public static class JsonServiceCollectionExtensions
{
    /// <summary>
    /// Registers the JSON serializer implementation and its associated
    /// <see cref="System.Text.Json.JsonSerializerOptions"/> configuration.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the serialization services to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <remarks>
    /// The serializer is registered as a singleton implementation of
    /// <see cref="ISerializer"/> and uses the default
    /// <see cref="System.Text.Json.JsonSerializerOptions"/> settings.
    /// </remarks>
    public static IServiceCollection AddJsonSerialization(
        this IServiceCollection services)
    {
        services.AddOptions<System.Text.Json.JsonSerializerOptions>();

        services.TryAddSingleton<ISerializer, JsonSerializer>();

        return services;
    }

    /// <summary>
    /// Registers the JSON serializer implementation and configures the
    /// underlying <see cref="System.Text.Json.JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the serialization services to.
    /// </param>
    /// <param name="configure">
    /// A delegate used to configure
    /// <see cref="System.Text.Json.JsonSerializerOptions"/>.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <example>
    /// <code language="csharp">
    /// services.AddJsonSerialization(options =>
    /// {
    ///     options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    ///     options.WriteIndented = true;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddJsonSerialization(
        this IServiceCollection services,
        Action<System.Text.Json.JsonSerializerOptions> configure)
    {
        services.AddJsonSerialization();

        services.Configure(configure);

        return services;
    }
}
