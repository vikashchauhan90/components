using System;
using System.Collections.Generic;
using System.Text;

namespace Jnvp.Platform.Compression;

/// <summary>
/// Defines methods for compressing and decompressing binary data and streams.
/// </summary>
/// <remarks>
/// Implementations may use algorithms such as GZip, Brotli, or Deflate to
/// reduce the size of binary payloads for storage or transmission.
/// </remarks>
/// <example>
/// <code language="csharp">
/// ICompressor compressor = new BrotliCompressor();
///
/// // Compress a byte array.
/// byte[] original = Encoding.UTF8.GetBytes("Hello World");
/// byte[] compressed = compressor.Compress(original);
///
/// // Decompress a byte array.
/// byte[] decompressed = compressor.Decompress(compressed);
///
/// string value = Encoding.UTF8.GetString(decompressed);
///
/// // Compress a stream.
/// using var source = new MemoryStream(original);
/// using var destination = new MemoryStream();
///
/// await compressor.CompressAsync(source, destination);
///
/// // Decompress a stream.
/// destination.Position = 0;
///
/// using var restored = new MemoryStream();
///
/// await compressor.DecompressAsync(
///     destination,
///     restored);
/// </code>
/// </example>
public interface ICompressor
{
    /// <summary>
    /// Compresses the specified binary data.
    /// </summary>
    /// <param name="data">
    /// The data to compress.
    /// </param>
    /// <returns>
    /// A byte array containing the compressed representation of the input data.
    /// </returns>
    byte[] Compress(ReadOnlySpan<byte> data);

    /// <summary>
    /// Decompresses the specified binary data.
    /// </summary>
    /// <param name="data">
    /// The compressed data to decompress.
    /// </param>
    /// <returns>
    /// A byte array containing the decompressed data.
    /// </returns>
    byte[] Decompress(ReadOnlySpan<byte> data);

    /// <summary>
    /// Asynchronously compresses data from the source stream and writes the
    /// compressed output to the destination stream.
    /// </summary>
    /// <param name="source">
    /// The stream containing the data to compress.
    /// </param>
    /// <param name="destination">
    /// The stream that receives the compressed data.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous compression operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="source"/> or
    /// <paramref name="destination"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled.
    /// </exception>
    Task CompressAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously decompresses data from the source stream and writes the
    /// decompressed output to the destination stream.
    /// </summary>
    /// <param name="source">
    /// The stream containing the compressed data.
    /// </param>
    /// <param name="destination">
    /// The stream that receives the decompressed data.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous decompression operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="source"/> or
    /// <paramref name="destination"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled.
    /// </exception>
    Task DecompressAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default);
}
