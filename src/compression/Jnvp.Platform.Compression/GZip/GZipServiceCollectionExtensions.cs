using Jnvp.Platform.Compression;
using Jnvp.Platform.Compression.GZip;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Provides extension methods for registering GZip-based
/// <see cref="ICompressor"/> services.
/// </summary>
public static class GZipServiceCollectionExtensions
{
    /// <summary>
    /// Registers the GZip compressor implementation and its associated
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
    /// <see cref="System.IO.Compression.GZipStream"/> implementation.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddGZipCompression();
    /// </code>
    /// </example>
    public static IServiceCollection AddGZipCompression(
        this IServiceCollection services)
    {
        services.AddOptions<CompressionOptions>();

        services.TryAddSingleton<ICompressor, GZipCompressor>();

        return services;
    }

    /// <summary>
    /// Registers the GZip compressor implementation and configures the
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
    /// Use this overload to customize compression settings such as the
    /// compression level used during compression operations.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// services.AddGZipCompression(options =>
    /// {
    ///     options.CompressionLevel =
    ///         CompressionLevel.SmallestSize;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddGZipCompression(
        this IServiceCollection services,
        Action<CompressionOptions> configure)
    {
        services.AddGZipCompression();

        services.Configure(configure);

        return services;
    }
}
