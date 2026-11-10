using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Serialization.Json;

internal sealed class JsonSerializer : ISerializer
{
    private const string Format = "json";
    private readonly System.Text.Json.JsonSerializerOptions options;
    private readonly ILogger<JsonSerializer> logger;

    public JsonSerializer(
        IOptions<System.Text.Json.JsonSerializerOptions> options,
        ILogger<JsonSerializer> logger)
    {
        var value = options.Value;

        this.options = value ?? new System.Text.Json.JsonSerializerOptions();
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public byte[] Serialize<T>(T obj)
    {
        using var activity =
            SerializationDiagnostics.StartSerialize<T>(Format);

        try
        {
            this.logger.LogTrace(
                "Serializing object of type {Type} to JSON",
                typeof(T).FullName);

            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(
                obj,
                options);
        }
        catch (Exception ex)
        {
            SerializationDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async Task<byte[]> SerializeAsync<T>(
        Stream destination,
        T obj,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(destination);

        using var activity =
            SerializationDiagnostics.StartSerialize<T>(Format);

        try
        {
            this.logger.LogTrace(
                "Serializing object of type {Type} to JSON asynchronously",
                typeof(T).FullName);

            var bytes =
                System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(
                    obj,
                    options);

            await destination.WriteAsync(
                bytes,
                cancellationToken);

            return bytes;
        }
        catch (Exception ex)
        {
            SerializationDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public T? Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        using var activity =
            SerializationDiagnostics.StartDeserialize<T>(Format);

        try
        {
            this.logger.LogTrace(
                "Deserializing JSON data to object of type {Type}",
                typeof(T).FullName);

            return System.Text.Json.JsonSerializer.Deserialize<T>(
                data.Span,
                options);
        }
        catch (Exception ex)
        {
            SerializationDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async Task<T?> DeserializeAsync<T>(
        Stream source,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        using var activity =
            SerializationDiagnostics.StartDeserialize<T>(Format);

        try
        {
            this.logger.LogTrace(
                "Deserializing JSON data from stream to object of type {Type} asynchronously",
                typeof(T).FullName);

            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(
                source,
                options,
                cancellationToken);
        }
        catch (Exception ex)
        {
            SerializationDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }
}
