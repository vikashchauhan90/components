
namespace Jnvp.Platform.Serialization;

/// <summary>
/// Defines methods for serializing objects to binary data and deserializing
/// binary data back into objects.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Serializes the specified object into a byte array.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to serialize.
    /// </typeparam>
    /// <param name="obj">
    /// The object instance to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the serialized representation of the object.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the serializer does not support serializing a null value.
    /// </exception>
    byte[] Serialize<T>(T obj);

    /// <summary>
    /// Asynchronously serializes the specified object and writes the serialized
    /// representation to the provided destination stream.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to serialize.
    /// </typeparam>
    /// <param name="destination">
    /// The stream that receives the serialized data.
    /// </param>
    /// <param name="obj">
    /// The object instance to serialize.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that completes when serialization has finished and returns the
    /// serialized bytes that were written to the destination stream.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled.
    /// </exception>
    Task<byte[]> SerializeAsync<T>(
        Stream destination,
        T obj,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes an object of type <typeparamref name="T"/> from the supplied
    /// binary data.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to create.
    /// </typeparam>
    /// <param name="data">
    /// The serialized data to deserialize.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="T"/> if deserialization succeeds;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    T? Deserialize<T>(ReadOnlyMemory<byte> data);

    /// <summary>
    /// Asynchronously deserializes an object of type
    /// <typeparamref name="T"/> from the specified stream.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to create.
    /// </typeparam>
    /// <param name="source">
    /// The stream containing the serialized data.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result
    /// contains the deserialized instance of <typeparamref name="T"/>,
    /// or <see langword="null"/> when deserialization produces no value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled.
    /// </exception>
    Task<T?> DeserializeAsync<T>(
        Stream source,
        CancellationToken cancellationToken = default);
}
