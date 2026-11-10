using System.Collections.Concurrent;

namespace Jnvp.Platform.Caching.Memory;

internal sealed class MemoryTagStore : ITagStore
{
    private readonly ConcurrentDictionary<
        string,
        ConcurrentDictionary<string, byte>> tagToKeys = new(
            StringComparer.Ordinal);

    private readonly ConcurrentDictionary<
        string,
        ConcurrentDictionary<string, byte>> keyToTags = new(
            StringComparer.Ordinal);

    public ValueTask AddTagsAsync(
        string key,
        IReadOnlyCollection<string> tags,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(tags);
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var tag in tags)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        }

        foreach (var tag in tags)
        {
            tagToKeys.GetOrAdd(
                tag,
                static _ => new(StringComparer.Ordinal))
                .TryAdd(key, 0);

            keyToTags.GetOrAdd(
                key,
                static _ => new(StringComparer.Ordinal))
                .TryAdd(tag, 0);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask<IReadOnlyCollection<string>> GetKeysAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<string> keys =
            tagToKeys.TryGetValue(tag, out var keySet)
                ? keySet.Keys.ToArray()
                : Array.Empty<string>();

        return ValueTask.FromResult(keys);
    }

    public ValueTask RemoveKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        cancellationToken.ThrowIfCancellationRequested();

        if (keyToTags.TryRemove(key, out var tags))
        {
            foreach (var tag in tags.Keys)
            {
                if (tagToKeys.TryGetValue(tag, out var keys))
                {
                    keys.TryRemove(key, out _);
                }
            }
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        cancellationToken.ThrowIfCancellationRequested();

        if (tagToKeys.TryRemove(tag, out var keys))
        {
            foreach (var key in keys.Keys)
            {
                if (keyToTags.TryGetValue(key, out var tags))
                {
                    tags.TryRemove(tag, out _);
                }
            }
        }

        return ValueTask.CompletedTask;
    }
}
