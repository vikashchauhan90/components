using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Compression.Deflate;

internal sealed class DeflateCompressor : ICompressor
{
    private const string Format = "deflate";

    private readonly CompressionOptions options;
    private readonly ILogger<DeflateCompressor> logger;

    public DeflateCompressor(
        IOptions<CompressionOptions> options,
        ILogger<DeflateCompressor> logger)
    {
        this.options = options?.Value
            ?? throw new ArgumentNullException(nameof(options));

        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public byte[] Compress(ReadOnlySpan<byte> data)
    {
        using var activity =
            CompressionDiagnostics.StartCompress(Format);

        try
        {
            this.logger.LogTrace(
                "Compressing {ByteCount} bytes using Deflate",
                data.Length);

            using var output = new MemoryStream();

            using (var compressionStream = new DeflateStream(
                output,
                this.options.CompressionLevel,
                leaveOpen: true))
            {
                compressionStream.Write(data);
            }

            return output.ToArray();
        }
        catch (Exception ex)
        {
            CompressionDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public byte[] Decompress(ReadOnlySpan<byte> data)
    {
        using var activity =
            CompressionDiagnostics.StartDecompress(Format);

        try
        {
            this.logger.LogTrace(
                "Decompressing {ByteCount} bytes using Deflate",
                data.Length);

            using var input =
                new MemoryStream(data.ToArray());

            using var deflate =
                new DeflateStream(
                    input,
                    CompressionMode.Decompress);

            using var output =
                new MemoryStream();

            deflate.CopyTo(output);

            return output.ToArray();
        }
        catch (Exception ex)
        {
            CompressionDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async Task CompressAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        using var activity =
            CompressionDiagnostics.StartCompress(Format);

        try
        {
            this.logger.LogTrace(
                "Compressing stream using Deflate");

            await using var deflate =
                new DeflateStream(
                    destination,
                    this.options.CompressionLevel,
                    leaveOpen: true);

            await source.CopyToAsync(
                deflate,
                cancellationToken);

            await deflate.FlushAsync(
                cancellationToken);
        }
        catch (Exception ex)
        {
            CompressionDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async Task DecompressAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        using var activity =
            CompressionDiagnostics.StartDecompress(Format);

        try
        {
            this.logger.LogTrace(
                "Decompressing stream using Deflate");

            await using var deflate =
                new DeflateStream(
                    source,
                    CompressionMode.Decompress,
                    leaveOpen: true);

            await deflate.CopyToAsync(
                destination,
                cancellationToken);
        }
        catch (Exception ex)
        {
            CompressionDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }
}
