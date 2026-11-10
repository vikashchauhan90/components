using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Caching.Memory;

internal sealed class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache cache;
    private readonly ITagStore tagStore;
    private readonly IOptions<CacheEntryOptions> options;
    private readonly ILogger<MemoryCacheProvider> logger;

    public MemoryCacheProvider(
        IMemoryCache cache,
        ITagStore tagStore,
        IOptions<CacheEntryOptions> options,
        ILogger<MemoryCacheProvider> logger)
    {
        this.cache = cache
            ?? throw new ArgumentNullException(nameof(cache));

        this.tagStore = tagStore
            ?? throw new ArgumentNullException(nameof(tagStore));

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
        cancellationToken.ThrowIfCancellationRequested();
        var entryOptions = options.Value;

        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            logger.LogTrace(
                "Setting string cache entry for key {Key}",
                key);

            await tagStore.RemoveKeyAsync(
                key,
                cancellationToken);

            SetCore(
                key,
                value,
                entryOptions);

            if (entryOptions.Tags is { Count: > 0 } tags)
            {
                await tagStore.AddTagsAsync(
                    key,
                    tags,
                    cancellationToken);
            }
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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

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

    public async ValueTask SetBytesAsync(
        string key,
        ReadOnlyMemory<byte> value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();
        var entryOptions = options.Value;

        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            logger.LogTrace(
                "Setting byte cache entry for key {Key}",
                key);

            await tagStore.RemoveKeyAsync(
                key,
                cancellationToken);

            SetCore(
                key,
                value.ToArray(),
                entryOptions);

            if (entryOptions.Tags is { Count: > 0 } tags)
            {
                await tagStore.AddTagsAsync(
                    key,
                    tags,
                    cancellationToken);
            }
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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            var result = cache.Get<byte[]>(key)?.ToArray();

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
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        cancellationToken.ThrowIfCancellationRequested();
        using var memoryStream =
            new MemoryStream();

        await value.CopyToAsync(
            memoryStream,
            cancellationToken);

        await SetBytesAsync(
            key,
            memoryStream.ToArray(),
            cancellationToken);
    }

    public async ValueTask<Stream?> GetStreamAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

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
        cancellationToken.ThrowIfCancellationRequested();

        using var activity =
            CacheDiagnostics.StartRemove(key);

        try
        {
            logger.LogTrace(
                "Removing cache entry {Key}",
                key);

            cache.Remove(key);

            await tagStore.RemoveKeyAsync(
                key,
                cancellationToken);
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
        cancellationToken.ThrowIfCancellationRequested();

        using var activity =
            CacheDiagnostics.StartInvalidateTag(tag);

        try
        {
            logger.LogTrace(
                "Invalidating cache tag {Tag}",
                tag);

            var keys = await tagStore.GetKeysAsync(
                tag,
                cancellationToken);

            foreach (var key in keys)
            {
                cancellationToken.ThrowIfCancellationRequested();
                cache.Remove(key);
            }

            await tagStore.RemoveTagAsync(
                tag,
                cancellationToken);
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
        CacheEntryOptions options)
    {
        var entryOptions =
            new MemoryCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow is not null)
        {
            entryOptions.AbsoluteExpirationRelativeToNow =
                options.AbsoluteExpirationRelativeToNow;
        }

        if (options.SlidingExpiration is not null)
        {
            entryOptions.SlidingExpiration =
                options.SlidingExpiration;
        }

        entryOptions.RegisterPostEvictionCallback(
            (k, _, _, _) =>
            {
                _ = tagStore.RemoveKeyAsync(
                    k.ToString()!);
            });

        cache.Set(
            key,
            value,
            entryOptions);

    }
}
