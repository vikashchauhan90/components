using System.IO.Compression;

namespace Jnvp.Platform.Compression;

/// <summary>
/// Represents configurable settings for compression operations.
/// </summary>
/// <remarks>
/// These settings are applied by compression implementations such as
/// GZip, Brotli, and Deflate compressors.
///
/// <para>
/// The default configuration uses
/// <see cref="CompressionLevel.Optimal"/> and limits decompressed
/// payloads to 100 MB.
/// </para>
/// </remarks>
/// <example>
/// <code language="csharp">
/// services.AddGZipCompression(options =>
/// {
///     options.CompressionLevel =
///         CompressionLevel.SmallestSize;
///
///     options.MaxDecompressedBytes =
///         50 * 1024 * 1024; // 50 MB
/// });
/// </code>
/// </example>
public sealed class CompressionOptions
{
    /// <summary>
    /// Gets or sets the compression level used when compressing data.
    /// </summary>
    /// <value>
    /// The compression level to use for compression operations.
    /// The default value is <see cref="CompressionLevel.Optimal"/>.
    /// </value>
    /// <remarks>
    /// Higher compression levels generally produce smaller output but may
    /// require additional CPU time. Lower compression levels prioritize
    /// compression speed over compression ratio.
    /// </remarks>
    public CompressionLevel CompressionLevel { get; set; }
        = CompressionLevel.Optimal;

    /// <summary>
    /// Gets or sets the maximum allowed size, in bytes, of decompressed data.
    /// </summary>
    /// <value>
    /// The maximum number of bytes that may be produced during a
    /// decompression operation. The default value is 104,857,600
    /// (100 MB).
    /// </value>
    /// <remarks>
    /// This limit can help protect against decompression bomb attacks and
    /// accidental resource exhaustion when processing compressed data from
    /// untrusted sources.
    /// </remarks>
    public long MaxDecompressedBytes { get; set; }
        = 100 * 1024 * 1024;
}
