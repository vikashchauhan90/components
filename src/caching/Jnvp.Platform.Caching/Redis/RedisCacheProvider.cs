using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Jnvp.Platform.Caching.Redis;

internal sealed class RedisCacheProvider : ICacheProvider
{
    private const string CachePrefix = "cache:";
    private const string TagPrefix = "cache:tag:";
    private const string KeyTagPrefix = "cache:key:";

    private readonly IDatabase database;
    private readonly ILogger<RedisCacheProvider> logger;

    public RedisCacheProvider(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisCacheProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(connectionMultiplexer);

        this.database = connectionMultiplexer.GetDatabase();

        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask SetStringAsync(
        string key,
        string value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            await database.StringSetAsync(
                GetCacheKey(key),
                value,
                options?.AbsoluteExpirationRelativeToNow);

            await RegisterTagsAsync(
                key,
                options?.Tags);

            logger.LogTrace(
                "Cached string value for key {Key}",
                key);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async ValueTask<string?> GetStringAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            var value = await database.StringGetAsync(
                GetCacheKey(key));

            activity?.SetTag(
                "cache.hit",
                value.HasValue);

            return value.HasValue
                ? value.ToString()
                : null;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async ValueTask SetBytesAsync(
        string key,
        ReadOnlyMemory<byte> value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            await database.StringSetAsync(
                GetCacheKey(key),
                value.ToArray(),
                options?.AbsoluteExpirationRelativeToNow);

            await RegisterTagsAsync(
                key,
                options?.Tags);

            logger.LogTrace(
                "Cached byte array for key {Key}",
                key);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async ValueTask<byte[]?> GetBytesAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            var value = await database.StringGetAsync(
                GetCacheKey(key));

            activity?.SetTag(
                "cache.hit",
                value.HasValue);

            return value.HasValue
                ? (byte[])value!
                : null;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async ValueTask SetStreamAsync(
        string key,
        Stream value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(value);

        using var memoryStream =
            new MemoryStream();

        await value.CopyToAsync(
            memoryStream,
            cancellationToken);

        await SetBytesAsync(
            key,
            memoryStream.ToArray(),
            options,
            cancellationToken);
    }

    public async ValueTask<Stream?> GetStreamAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var bytes = await GetBytesAsync(
            key,
            cancellationToken);

        return bytes is null
            ? null
            : new MemoryStream(bytes);
    }

    public async ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        using var activity =
            CacheDiagnostics.StartRemove(key);

        try
        {
            logger.LogTrace(
                "Removing cache entry {Key}",
                key);

            await RemoveKeyMappingsAsync(key);

            await database.KeyDeleteAsync(
                GetCacheKey(key));
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public async ValueTask InvalidateTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        using var activity =
            CacheDiagnostics.StartInvalidateTag(tag);

        try
        {
            logger.LogTrace(
                "Invalidating cache tag {Tag}",
                tag);

            var members =
                await database.SetMembersAsync(
                    GetTagKey(tag));

            foreach (var member in members)
            {
                await RemoveAsync(
                    member.ToString(),
                    cancellationToken);
            }

            await database.KeyDeleteAsync(
                GetTagKey(tag));
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    private async Task RegisterTagsAsync(
        string key,
        IReadOnlyCollection<string>? tags)
    {
        if (tags is null || tags.Count == 0)
        {
            return;
        }

        foreach (var tag in tags)
        {
            await database.SetAddAsync(
                GetTagKey(tag),
                key);

            await database.SetAddAsync(
                GetKeyTagKey(key),
                tag);
        }
    }

    private async Task RemoveKeyMappingsAsync(
        string key)
    {
        var tags =
            await database.SetMembersAsync(
                GetKeyTagKey(key));

        foreach (var tag in tags)
        {
            await database.SetRemoveAsync(
                GetTagKey(tag.ToString()),
                key);
        }

        await database.KeyDeleteAsync(
            GetKeyTagKey(key));
    }

    private static string GetCacheKey(string key)
        => $"{CachePrefix}{key}";

    private static string GetTagKey(string tag)
        => $"{TagPrefix}{tag}";

    private static string GetKeyTagKey(string key)
        => $"{KeyTagPrefix}{key}";
}
