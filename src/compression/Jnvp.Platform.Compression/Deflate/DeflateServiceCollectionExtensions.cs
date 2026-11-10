using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jnvp.Platform.Compression.Deflate;

/// <summary>
/// Provides extension methods for registering Deflate-based
/// <see cref="ICompressor"/> services.
/// </summary>
public static class DeflateServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Deflate compressor implementation and its associated
    /// <see cref="CompressionOptions"/> configuration.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the compression services to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    public static IServiceCollection AddDeflateCompression(
        this IServiceCollection services)
    {
        services.AddOptions<CompressionOptions>();

        services.TryAddSingleton<ICompressor, DeflateCompressor>();

        return services;
    }

    /// <summary>
    /// Registers the Deflate compressor implementation and configures the
    /// associated <see cref="CompressionOptions"/>.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the compression services to.
    /// </param>
    /// <param name="configure">
    /// A delegate used to configure compression behavior.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <example>
    /// <code language="csharp">
    /// services.AddDeflateCompression(options =>
    /// {
    ///     options.CompressionLevel =
    ///         CompressionLevel.SmallestSize;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddDeflateCompression(
        this IServiceCollection services,
        Action<CompressionOptions> configure)
    {
        services.AddDeflateCompression();

        services.Configure(configure);

        return services;
    }
}
