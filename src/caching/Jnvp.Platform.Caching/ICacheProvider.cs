namespace Jnvp.Platform.Caching;

/// <summary>
/// Provides methods for storing, retrieving, and invalidating cached data.
/// </summary>
/// <remarks>
/// The platform implementation is backed by FusionCache and is configured
/// with Redis for distributed storage and backplane synchronization.
///
/// <para>
/// Entries can optionally be associated with FusionCache tags, allowing
/// related entries to be invalidated as a group across application instances.
/// </para>
/// </remarks>
/// <example>
/// <code language="csharp">
/// await cacheProvider.SetStringAsync(
///     "user:123",
///     json,
///     options: new CacheEntryOptions { Tags = ["users"] });
///
/// string? value = await cacheProvider.GetStringAsync(
///     "user:123");
///
/// await cacheProvider.InvalidateTagAsync(
///     "users");
/// </code>
/// </example>
public interface ICacheProvider
{
    /// <summary>
    /// Stores a string value in the cache.
    /// </summary>
    /// <param name="key">
    /// The unique cache key.
    /// </param>
    /// <param name="value">
    /// The string value to cache.
    /// </param>
    /// <param name="options">
    /// Optional per-entry expiration and FusionCache tags.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous cache operation.
    /// </returns>
    ValueTask SetStringAsync(
        string key,
        string value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a cached string value.
    /// </summary>
    /// <param name="key">
    /// The unique cache key.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The cached value if found; otherwise, <see langword="null"/>.
    /// </returns>
    ValueTask<string?> GetStringAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores a binary value in the cache.
    /// </summary>
    /// <param name="key">
    /// The unique cache key.
    /// </param>
    /// <param name="value">
    /// The binary data to cache.
    /// </param>
    /// <param name="options">
    /// Optional per-entry expiration and FusionCache tags.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous cache operation.
    /// </returns>
    ValueTask SetBytesAsync(
        string key,
        ReadOnlyMemory<byte> value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves cached binary data.
    /// </summary>
    /// <param name="key">
    /// The unique cache key.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The cached binary data if found; otherwise, <see langword="null"/>.
    /// </returns>
    ValueTask<byte[]?> GetBytesAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached entry associated with the specified key.
    /// </summary>
    /// <param name="key">
    /// The unique cache key.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous remove operation.
    /// </returns>
    ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Invalidates all cache entries associated with the specified tag.
    /// </summary>
    /// <param name="tag">
    /// The cache tag to invalidate.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous invalidation operation.
    /// </returns>
    ValueTask InvalidateTagAsync(
        string tag,
        CancellationToken cancellationToken = default);
}
