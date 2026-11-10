using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Compression.Brotli;

/// <summary>
/// Provides extension methods for registering Brotli-based
/// <see cref="ICompressor"/> services.
/// </summary>
public static class BrotliServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Brotli compressor implementation and its associated
    /// <see cref="CompressionOptions"/> configuration.
    /// </summary>
    /// <param name="services">
    /// The service collection to add the compression services to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance so that additional
    /// calls can be chained.
    /// </returns>
    /// <remarks>
    /// The compressor is registered as a singleton implementation of
    /// <see cref="ICompressor"/> and uses the built-in
    /// <see cref="System.IO.Compression.BrotliStream"/> implementation.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddBrotliCompression();
    /// </code>
    /// </example>
    public static IServiceCollection AddBrotliCompression(
        this IServiceCollection services)
    {
        services.AddOptions<CompressionOptions>();

        services.TryAddSingleton<ICompressor, BrotliCompressor>();

        return services;
    }

    /// <summary>
    /// Registers the Brotli compressor implementation and configures the
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
    /// <remarks>
    /// Use this overload to customize compression settings such as
    /// <see cref="System.IO.Compression.CompressionLevel"/> and limits for
    /// decompressed payload sizes.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddBrotliCompression(options =>
    /// {
    ///     options.CompressionLevel =
    ///         CompressionLevel.SmallestSize;
    ///
    ///     options.MaxDecompressedBytes =
    ///         50 * 1024 * 1024; // 50 MB
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddBrotliCompression(
        this IServiceCollection services,
        Action<CompressionOptions> configure)
    {
        services.AddBrotliCompression();

        services.Configure(configure);

        return services;
    }
}
