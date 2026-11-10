using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Jnvp.Platform.Caching.Memory;

internal sealed class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache cache;
    private readonly ILogger<MemoryCacheProvider> logger;

    private readonly ConcurrentDictionary<
        string,
        ConcurrentDictionary<string, byte>>
        tagMappings = new(
            StringComparer.Ordinal);

    public MemoryCacheProvider(
        IMemoryCache cache,
        ILogger<MemoryCacheProvider> logger)
    {
        this.cache = cache
            ?? throw new ArgumentNullException(nameof(cache));

        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public ValueTask SetStringAsync(
        string key,
        string value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            logger.LogTrace(
                "Setting string cache entry for key {Key}",
                key);

            SetCore(
                key,
                value,
                options);

            return ValueTask.CompletedTask;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public ValueTask<string?> GetStringAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            logger.LogTrace(
                "Getting string cache entry for key {Key}",
                key);

            var result = cache.Get<string>(key);

            activity?.SetTag(
                "cache.hit",
                result is not null);

            return ValueTask.FromResult(result);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public ValueTask SetBytesAsync(
        string key,
        ReadOnlyMemory<byte> value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            logger.LogTrace(
                "Setting byte cache entry for key {Key}",
                key);

            SetCore(
                key,
                value.ToArray(),
                options);

            return ValueTask.CompletedTask;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public ValueTask<byte[]?> GetBytesAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            var result = cache.Get<byte[]>(key);

            activity?.SetTag(
                "cache.hit",
                result is not null);

            return ValueTask.FromResult(result);
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

    public ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartRemove(key);

        try
        {
            logger.LogTrace(
                "Removing cache entry {Key}",
                key);

            cache.Remove(key);

            RemoveKeyFromTagMappings(key);

            return ValueTask.CompletedTask;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    public ValueTask InvalidateTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        using var activity =
            CacheDiagnostics.StartInvalidateTag(tag);

        try
        {
            logger.LogTrace(
                "Invalidating cache tag {Tag}",
                tag);

            if (tagMappings.TryRemove(
                tag,
                out var keys))
            {
                foreach (var key in keys.Keys)
                {
                    cache.Remove(key);
                }
            }

            return ValueTask.CompletedTask;
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    private void SetCore(
        string key,
        object value,
        CacheEntryOptions? options)
    {
        var entryOptions =
            new MemoryCacheEntryOptions();

        if (options?.AbsoluteExpirationRelativeToNow is not null)
        {
            entryOptions.AbsoluteExpirationRelativeToNow =
                options.AbsoluteExpirationRelativeToNow;
        }

        if (options?.SlidingExpiration is not null)
        {
            entryOptions.SlidingExpiration =
                options.SlidingExpiration;
        }

        entryOptions.RegisterPostEvictionCallback(
            (k, _, _, _) =>
            {
                RemoveKeyFromTagMappings(
                    k.ToString()!);
            });

        cache.Set(
            key,
            value,
            entryOptions);

        RegisterTags(
            key,
            options?.Tags);
    }

    private void RegisterTags(
        string key,
        IReadOnlyCollection<string>? tags)
    {
        if (tags is null)
        {
            return;
        }

        foreach (var tag in tags)
        {
            var keys =
                tagMappings.GetOrAdd(
                    tag,
                    static _ => new());

            keys.TryAdd(
                key,
                0);
        }
    }

    private void RemoveKeyFromTagMappings(
        string key)
    {
        foreach (var tagMapping in tagMappings.Values)
        {
            tagMapping.TryRemove(
                key,
                out _);
        }
    }
}
