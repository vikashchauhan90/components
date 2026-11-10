using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Caching.Distributed;

internal sealed class DistributedCacheProvider : ICacheProvider
{
    private const string EntryPrefix = "Jnvp.Platform.Caching:entry:";
    private const string TagVersionPrefix = "Jnvp.Platform.Caching:tag-version:";

    private readonly IDistributedCache cache;
    private readonly IOptions<CacheEntryOptions> options;
    private readonly ILogger<DistributedCacheProvider> logger;

    public DistributedCacheProvider(
        IDistributedCache cache,
        IOptions<CacheEntryOptions> options,
        ILogger<DistributedCacheProvider> logger)
    {
        this.cache = cache
            ?? throw new ArgumentNullException(nameof(cache));
        this.options = options
            ?? throw new ArgumentNullException(nameof(options));
        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask SetStringAsync(
        string key,
        string value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        var entryOptions = options.Value;
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartSet(key);

        try
        {
            var entry = await CreateEntryAsync(
                "string",
                Encoding.UTF8.GetBytes(value),
                entryOptions.Tags,
                cancellationToken);

            await cache.SetAsync(
                GetEntryKey(key),
                JsonSerializer.SerializeToUtf8Bytes(entry),
                CreateDistributedOptions(entryOptions),
                cancellationToken);

            logger.LogTrace("Setting distributed cache entry for key {Key}", key);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
            throw;
        }
    }

    public async ValueTask<string?> GetStringAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var entry = await GetEntryAsync(key, cancellationToken);
        return entry is null ? null : Encoding.UTF8.GetString(RequireKind(entry, "string"));
    }

    public async ValueTask SetBytesAsync(
        string key,
        ReadOnlyMemory<byte> value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var entryOptions = options.Value;
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartSet(key);

        try
        {
            var entry = await CreateEntryAsync(
                "bytes",
                value.ToArray(),
                entryOptions.Tags,
                cancellationToken);

            await cache.SetAsync(
                GetEntryKey(key),
                JsonSerializer.SerializeToUtf8Bytes(entry),
                CreateDistributedOptions(entryOptions),
                cancellationToken);

            logger.LogTrace("Setting distributed cache entry for key {Key}", key);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
            throw;
        }
    }

    public async ValueTask<byte[]?> GetBytesAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var entry = await GetEntryAsync(key, cancellationToken);
        return entry is null ? null : RequireKind(entry, "bytes");
    }

    public async ValueTask SetStreamAsync(
        string key,
        Stream value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        cancellationToken.ThrowIfCancellationRequested();

        using var memoryStream = new MemoryStream();
        await value.CopyToAsync(memoryStream, cancellationToken);
        await SetBytesAsync(key, memoryStream.ToArray(), cancellationToken);
    }

    public async ValueTask<Stream?> GetStreamAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var bytes = await GetBytesAsync(key, cancellationToken);
        return bytes is null ? null : new MemoryStream(bytes);
    }

    public async ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartRemove(key);

        try
        {
            await cache.RemoveAsync(GetEntryKey(key), cancellationToken);
            logger.LogTrace("Removed distributed cache entry for key {Key}", key);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
            throw;
        }
    }

    public async ValueTask InvalidateTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartInvalidateTag(tag);

        try
        {
            await cache.SetStringAsync(
                GetTagVersionKey(tag),
                Guid.NewGuid().ToString("N"),
                cancellationToken);
            logger.LogTrace("Invalidated distributed cache tag {Tag}", tag);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
            throw;
        }
    }

    private async ValueTask<CacheEntry?> GetEntryAsync(
        string key,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartGet(key);

        try
        {
            var data = await cache.GetAsync(GetEntryKey(key), cancellationToken);

            if (data is null)
            {
                activity?.SetTag("cache.hit", false);
                return null;
            }

            var entry = JsonSerializer.Deserialize<CacheEntry>(data)
                ?? throw new InvalidDataException(
                    $"The distributed cache entry for key '{key}' is invalid.");

            if (entry.TagVersions is null)
            {
                throw new InvalidDataException(
                    $"The distributed cache entry for key '{key}' has invalid tag metadata.");
            }

            foreach (var tag in entry.TagVersions)
            {
                var currentVersion = await cache.GetStringAsync(
                    GetTagVersionKey(tag.Key),
                    cancellationToken);

                if (!StringComparer.Ordinal.Equals(currentVersion, tag.Value))
                {
                    await cache.RemoveAsync(GetEntryKey(key), cancellationToken);
                    activity?.SetTag("cache.hit", false);
                    return null;
                }
            }

            await cache.RefreshAsync(GetEntryKey(key), cancellationToken);
            activity?.SetTag("cache.hit", true);
            return entry;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
            throw;
        }
    }

    private async ValueTask<CacheEntry> CreateEntryAsync(
        string kind,
        byte[] payload,
        IReadOnlyCollection<string>? tags,
        CancellationToken cancellationToken)
    {
        var tagVersions = new Dictionary<string, string>(StringComparer.Ordinal);

        if (tags is not null)
        {
            foreach (var tag in tags)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var tagKey = GetTagVersionKey(tag);
                var version = await cache.GetStringAsync(tagKey, cancellationToken);
                if (version is null)
                {
                    version = Guid.NewGuid().ToString("N");
                    await cache.SetStringAsync(tagKey, version, cancellationToken);
                }

                tagVersions[tag] = version;
            }
        }

        return new CacheEntry
        {
            Kind = kind,
            Payload = payload,
            TagVersions = tagVersions
        };
    }

    private static DistributedCacheEntryOptions CreateDistributedOptions(
        CacheEntryOptions options)
    {
        var result = new DistributedCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow is not null)
        {
            result.AbsoluteExpirationRelativeToNow =
                options.AbsoluteExpirationRelativeToNow;
        }

        if (options.SlidingExpiration is not null)
        {
            result.SlidingExpiration = options.SlidingExpiration;
        }

        return result;
    }

    private static byte[] RequireKind(CacheEntry entry, string expectedKind)
    {
        if (!StringComparer.Ordinal.Equals(entry.Kind, expectedKind))
        {
            throw new InvalidDataException(
                $"The cache entry contains '{entry.Kind}' data, not '{expectedKind}' data.");
        }

        return entry.Payload
            ?? throw new InvalidDataException("The distributed cache entry has no payload.");
    }

    private static string GetEntryKey(string key)
        => EntryPrefix + key;

    private static string GetTagVersionKey(string tag)
        => TagVersionPrefix + Convert.ToBase64String(Encoding.UTF8.GetBytes(tag));

    private sealed class CacheEntry
    {
        public CacheEntry()
        {
        }

        public string Kind { get; set; } = string.Empty;
        public byte[]? Payload { get; set; } = [];
        public Dictionary<string, string>? TagVersions { get; set; } = new(StringComparer.Ordinal);
    }
}
