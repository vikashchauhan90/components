using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Compression.Brotli;

internal sealed class BrotliCompressor : ICompressor
{
    private const string Format = "brotli";
    private readonly CompressionOptions options;
    private readonly ILogger<BrotliCompressor> logger;

    public BrotliCompressor(
        IOptions<CompressionOptions> options,
        ILogger<BrotliCompressor> logger)
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

        activity?.SetTag("compression.input_size", data.Length);

        try
        {
            this.logger.LogTrace(
                "Compressing {ByteCount} bytes using Brotli",
                data.Length);

            using var output = new MemoryStream();

            using (var compressionStream = new BrotliStream(
                output,
                this.options.CompressionLevel,
                leaveOpen: true))
            {
                compressionStream.Write(data);
            }

            var result = output.ToArray();

            activity?.SetTag(
                "compression.output_size",
                result.Length);

            return result;
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

        activity?.SetTag(
            "compression.input_size",
            data.Length);

        try
        {
            this.logger.LogTrace(
                "Decompressing {ByteCount} bytes using Brotli",
                data.Length);

            using var input =
                new MemoryStream(data.ToArray());

            using var decompressionStream =
                new BrotliStream(
                    input,
                    CompressionMode.Decompress);

            using var output =
                new MemoryStream();

            CopyToWithLimit(
                decompressionStream,
                output,
                this.options.MaxDecompressedBytes);

            var result = output.ToArray();

            activity?.SetTag(
                "compression.output_size",
                result.Length);

            return result;
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
                "Compressing stream using Brotli");

            await using var compressionStream =
                new BrotliStream(
                    destination,
                    this.options.CompressionLevel,
                    leaveOpen: true);

            await source.CopyToAsync(
                compressionStream,
                cancellationToken);

            await compressionStream.FlushAsync(
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
                "Decompressing stream using Brotli");

            await using var decompressionStream =
                new BrotliStream(
                    source,
                    CompressionMode.Decompress,
                    leaveOpen: true);

            await CopyToWithLimitAsync(
                decompressionStream,
                destination,
                this.options.MaxDecompressedBytes,
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

    private static void CopyToWithLimit(
        Stream source,
        Stream destination,
        long maxBytes)
    {
        Span<byte> buffer = stackalloc byte[8192];

        long totalBytes = 0;

        while (true)
        {
            var read = source.Read(buffer);

            if (read == 0)
            {
                break;
            }

            totalBytes += read;

            if (totalBytes > maxBytes)
            {
                throw new InvalidOperationException(
                    $"Decompressed data exceeds the configured limit of {maxBytes} bytes.");
            }

            destination.Write(buffer[..read]);
        }
    }

    private static async Task CopyToWithLimitAsync(
        Stream source,
        Stream destination,
        long maxBytes,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[8192];

        long totalBytes = 0;

        while (true)
        {
            var read = await source.ReadAsync(
                buffer,
                cancellationToken);

            if (read == 0)
            {
                break;
            }

            totalBytes += read;

            if (totalBytes > maxBytes)
            {
                throw new InvalidOperationException(
                    $"Decompressed data exceeds the configured limit of {maxBytes} bytes.");
            }

            await destination.WriteAsync(
                buffer.AsMemory(0, read),
                cancellationToken);
        }
    }
}
