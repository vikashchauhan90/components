namespace Jnvp.Platform.Caching;

/// <summary>
/// Represents configuration settings that control the behavior of a cached entry.
/// </summary>
/// <remarks>
/// Cache entry options can be used to configure expiration policies and
/// associate cache entries with one or more tags for group invalidation.
///
/// <para>
/// Expiration settings are optional. When neither
/// <see cref="AbsoluteExpirationRelativeToNow"/> nor
/// <see cref="SlidingExpiration"/> is specified, the expiration behavior
/// is determined by the underlying cache provider.
/// </para>
/// </remarks>
/// <example>
/// <code language="csharp">
/// await cacheProvider.SetStringAsync(
///     "user:123",
///     json,
///     new CacheEntryOptions
///     {
///         AbsoluteExpirationRelativeToNow =
///             TimeSpan.FromHours(1),
///
///         SlidingExpiration =
///             TimeSpan.FromMinutes(15),
///
///         Tags =
///         [
///             "users",
///             "user:123"
///         ]
///     });
/// </code>
/// </example>
public sealed class CacheEntryOptions
{
    /// <summary>
    /// Gets or sets the amount of time from now after which the cache entry
    /// should expire.
    /// </summary>
    /// <value>
    /// The relative expiration time, or <see langword="null"/> if no
    /// absolute expiration is configured.
    /// </value>
    /// <remarks>
    /// Once the specified duration has elapsed, the cache entry becomes
    /// eligible for removal regardless of access frequency.
    /// </remarks>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Gets or sets the amount of time a cache entry can remain inactive
    /// before it expires.
    /// </summary>
    /// <value>
    /// The sliding expiration interval, or <see langword="null"/> if
    /// sliding expiration is not configured.
    /// </value>
    /// <remarks>
    /// Each successful access resets the expiration timer.
    /// If the entry is not accessed within the configured interval,
    /// it becomes eligible for removal.
    /// </remarks>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Gets or sets the tags associated with the cache entry.
    /// </summary>
    /// <value>
    /// A collection of tags, or <see langword="null"/> if no tags are
    /// associated with the entry.
    /// </value>
    /// <remarks>
    /// Tags enable multiple cache entries to be grouped and invalidated
    /// together using tag-based invalidation operations such as
    /// <c>InvalidateTagAsync</c>.
    /// </remarks>
    /// <example>
    /// <code language="csharp">
    /// var options = new CacheEntryOptions
    /// {
    ///     Tags =
    ///     [
    ///         "products",
    ///         "category:electronics"
    ///     ]
    /// };
    /// </code>
    /// </example>
    public IReadOnlyCollection<string>? Tags { get; set; }
}
