using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Jnvp.Platform.Caching.Fusion;

internal sealed class FusionCacheProvider : ICacheProvider
{
    private readonly IFusionCache cache;
    private readonly IOptions<CacheEntryOptions> options;
    private readonly ILogger<FusionCacheProvider> logger;

    public FusionCacheProvider(
        IFusionCache cache,
        IOptions<CacheEntryOptions> options,
        ILogger<FusionCacheProvider> logger)
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
        cancellationToken.ThrowIfCancellationRequested();
        var entryOptions = options.Value;

        using var activity =
            CacheDiagnostics.StartSet(key);

        try
        {
            logger.LogTrace(
                "Caching string value for key {Key}",
                key);

            await cache.SetAsync(
                key,
                value,
                GetEntryOptions(entryOptions),
                tags: entryOptions.Tags,
                token: cancellationToken);
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
        cancellationToken.ThrowIfCancellationRequested();

        using var activity =
            CacheDiagnostics.StartGet(key);

        try
        {
            var value = await cache.GetOrDefaultAsync<string>(
                key,
                token: cancellationToken);

            activity?.SetTag(
                "cache.hit",
                value is not null);

            return value;
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

        using var activity = CacheDiagnostics.StartSet(key);

        try
        {
            await cache.SetAsync(
                key,
                value.ToArray(),
                GetEntryOptions(entryOptions),
                tags: entryOptions.Tags,
                token: cancellationToken);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartGet(key);

        try
        {
            var value = await cache.GetOrDefaultAsync<byte[]>(
                key,
                token: cancellationToken);

            activity?.SetTag("cache.hit", value is not null);
            return value?.ToArray();
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(activity, ex);
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
        using var memoryStream = new MemoryStream();

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
            await cache.RemoveAsync(
                key,
                token: cancellationToken);
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
            await cache.RemoveByTagAsync(
                tag,
                token: cancellationToken);
        }
        catch (Exception ex)
        {
            CacheDiagnostics.RecordException(
                activity,
                ex);

            throw;
        }
    }

    private static FusionCacheEntryOptions GetEntryOptions(
        CacheEntryOptions options)
    {
        if (options.SlidingExpiration is not null)
        {
            throw new NotSupportedException(
                "FusionCache does not support sliding expiration.");
        }

        var result = new FusionCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow is not null)
        {
            result.Duration =
                options.AbsoluteExpirationRelativeToNow.Value;
        }

        return result;
    }
}
