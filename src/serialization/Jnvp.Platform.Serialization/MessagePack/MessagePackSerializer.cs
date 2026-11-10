using MessagePack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Serialization.MessagePack;

internal sealed class MessagePackSerializer : ISerializer
{
    private const string Format = "messagepack";
    private readonly MessagePackSerializerOptions options;
    private readonly ILogger<MessagePackSerializer> logger;

    public MessagePackSerializer(
        IOptions<MessagePackSerializerOptions> options,
        ILogger<MessagePackSerializer> logger)
    {
        var value = options.Value;

        this.options = value ?? MessagePackSerializerOptions.Standard;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public byte[] Serialize<T>(T obj)
    {
        using var activity =
            SerializationDiagnostics.StartSerialize<T>(Format);

        try
        {
            this.logger.LogTrace(
                "Serializing object of type {Type} to MessagePack",
                typeof(T).FullName);

            return global::MessagePack.MessagePackSerializer.Serialize(
                obj,
                this.options);
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
                "Serializing object of type {Type} to MessagePack asynchronously",
                typeof(T).FullName);

            var bytes =
                global::MessagePack.MessagePackSerializer.Serialize(
                    obj,
                    this.options);

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
                "Deserializing MessagePack data to object of type {Type}",
                typeof(T).FullName);

            return global::MessagePack.MessagePackSerializer.Deserialize<T>(
                data,
                this.options);
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
                "Deserializing MessagePack data from stream to object of type {Type} asynchronously",
                typeof(T).FullName);

            using var memoryStream = new MemoryStream();

            await source.CopyToAsync(
                memoryStream,
                cancellationToken);

            return global::MessagePack.MessagePackSerializer.Deserialize<T>(
                memoryStream.ToArray(),
                this.options);
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
