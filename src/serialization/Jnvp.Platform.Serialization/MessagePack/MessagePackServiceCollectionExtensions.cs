using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jnvp.Platform.Serialization.MessagePack;

/// <summary>
/// Provides extension methods for registering MessagePack-based
/// <see cref="ISerializer"/> services.
/// </summary>
public static class MessagePackServiceCollectionExtensions
{
    /// <summary>
    /// Registers the MessagePack serializer implementation and its associated
    /// <see cref="MessagePackSerializerOptions"/> configuration.
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
    /// <see cref="MessagePackSerializerOptions.Standard"/> settings.
    /// </remarks>
    public static IServiceCollection AddMessagePackSerialization(
        this IServiceCollection services)
    {
        services.AddOptions<MessagePackSerializerOptions>();

        services.TryAddSingleton<ISerializer, MessagePackSerializer>();

        return services;
    }

    /// <summary>
    /// Registers the MessagePack serializer implementation and configures the
    /// underlying <see cref="MessagePackSerializerOptions"/>.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the serialization services to.
    /// </param>
    /// <param name="configure">
    /// A delegate used to configure
    /// <see cref="MessagePackSerializerOptions"/>.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <example>
    /// <code language="csharp">
    /// services.AddMessagePackSerialization(options =>
    /// {
    ///     options = options.WithCompression(
    ///         MessagePackCompression.Lz4BlockArray);
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddMessagePackSerialization(
        this IServiceCollection services,
        Action<MessagePackSerializerOptions> configure)
    {
        services.AddMessagePackSerialization();

        services.Configure(configure);

        return services;
    }
}
