using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Compression.GZip;

internal sealed class GZipCompressor : ICompressor
{
    private const string Format = "gzip";

    private readonly CompressionOptions options;
    private readonly ILogger<GZipCompressor> logger;

    public GZipCompressor(
        IOptions<CompressionOptions> options,
        ILogger<GZipCompressor> logger)
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
                "Compressing {ByteCount} bytes using GZip",
                data.Length);

            using var output = new MemoryStream();

            using (var compressionStream = new GZipStream(
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
                "Decompressing {ByteCount} bytes using GZip",
                data.Length);

            using var input =
                new MemoryStream(data.ToArray());

            using var gzip =
                new GZipStream(
                    input,
                    CompressionMode.Decompress);

            using var output =
                new MemoryStream();

            gzip.CopyTo(output);

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
                "Compressing stream using GZip");

            await using var gzip =
                new GZipStream(
                    destination,
                    this.options.CompressionLevel,
                    leaveOpen: true);

            await source.CopyToAsync(
                gzip,
                cancellationToken);

            await gzip.FlushAsync(
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
                "Decompressing stream using GZip");

            await using var gzip =
                new GZipStream(
                    source,
                    CompressionMode.Decompress,
                    leaveOpen: true);

            await gzip.CopyToAsync(
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
