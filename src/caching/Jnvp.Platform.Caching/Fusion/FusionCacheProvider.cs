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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        cancellationToken.ThrowIfCancellationRequested();
        ValidateEntryOptions(options);

        using var activity = CacheDiagnostics.StartSet();

        try
        {
            logger.LogTrace("Caching string value");

            await cache.SetAsync(
                key,
                value,
                GetEntryOptions(options),
                tags: options?.Tags,
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

        using var activity = CacheDiagnostics.StartGet();

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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();
        ValidateEntryOptions(options);

        using var activity = CacheDiagnostics.StartSet();

        try
        {
            await cache.SetAsync(
                key,
                value.ToArray(),
                GetEntryOptions(options),
                tags: options?.Tags,
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

        using var activity = CacheDiagnostics.StartGet();

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

    public async ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        using var activity = CacheDiagnostics.StartRemove();

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

        using var activity = CacheDiagnostics.StartInvalidateTag();

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

    private FusionCacheEntryOptions? GetEntryOptions(
        CacheEntryOptions? options)
    {
        if (options?.AbsoluteExpirationRelativeToNow is not { } expiration)
        {
            return null;
        }

        var entryOptions = cache.DefaultEntryOptions.Duplicate();
        entryOptions.Duration = expiration;
        return entryOptions;
    }

    private static void ValidateEntryOptions(
        CacheEntryOptions? options)
    {
        if (options?.AbsoluteExpirationRelativeToNow is { } expiration
            && expiration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "The absolute expiration must be greater than zero.");
        }

        if (options?.Tags is not null
            && options.Tags.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException(
                "Cache tags cannot be null, empty, or whitespace.",
                nameof(options));
        }
    }
}
