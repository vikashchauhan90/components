namespace Jnvp.Platform.Caching;

/// <summary>
/// Represents configuration settings that control the behavior of a cached entry.
/// </summary>
/// <remarks>
/// Cache entry options can be used to configure absolute expiration and
/// associate a FusionCache entry with one or more tags.
///
/// <para>
/// Expiration settings are optional. When neither
/// <see cref="AbsoluteExpirationRelativeToNow"/> is specified, FusionCache's
/// configured default entry options determine the expiration behavior.
/// </para>
/// </remarks>
/// Pass an instance to a cache write operation to set that entry's behavior.
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
    /// Gets or sets the FusionCache tags associated with the cache entry.
    /// </summary>
    /// <value>
    /// A collection of tags, or <see langword="null"/> if no tags are
    /// associated with the entry.
    /// </value>
    /// <remarks>
    /// Tags enable multiple cache entries to be grouped and invalidated
    /// through FusionCache's tag invalidation support.
    /// </remarks>
    public IReadOnlyCollection<string>? Tags { get; set; }
}
