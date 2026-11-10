using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace Jnvp.Platform.Caching.Fusion;

internal sealed class FusionCacheProvider : ICacheProvider
{
    private readonly IFusionCache cache;
    private readonly ILogger<FusionCacheProvider> logger;

    public FusionCacheProvider(
        IFusionCache cache,
        ILogger<FusionCacheProvider> logger)
    {
        this.cache = cache
            ?? throw new ArgumentNullException(nameof(cache));

        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask SetStringAsync(
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
                "Caching string value for key {Key}",
                key);

            await cache.SetAsync(
                key,
                value,
                GetEntryOptions(options),
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
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        await cache.SetAsync(
            key,
            value.ToArray(),
            GetEntryOptions(options),
            token: cancellationToken);
    }

    public async ValueTask<byte[]?> GetBytesAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await cache.GetOrDefaultAsync<byte[]>(
            key,
            token: cancellationToken);
    }

    public async ValueTask SetStreamAsync(
        string key,
        Stream value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();

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

    private static FusionCacheEntryOptions? GetEntryOptions(
        CacheEntryOptions? options)
    {
        if (options is null)
        {
            return null;
        }

        var result = new FusionCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow is not null)
        {
            result.Duration =
                options.AbsoluteExpirationRelativeToNow.Value;
        }

        if (options.Tags?.Count > 0)
        {
            result.SetTags(
                options.Tags.ToArray());
        }

        return result;
    }
}
