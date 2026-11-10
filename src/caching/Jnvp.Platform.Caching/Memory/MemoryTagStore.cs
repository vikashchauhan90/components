
using System.Collections.Concurrent;


namespace Jnvp.Platform.Caching.Memory;

internal sealed class MemoryTagStore : ITagStore
{
    private readonly ConcurrentDictionary<
        string,
        ConcurrentDictionary<string, byte>> tagToKeys;

    private readonly ConcurrentDictionary<
        string,
        ConcurrentDictionary<string, byte>> keyToTags;
}
