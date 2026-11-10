
namespace Jnvp.Platform.Caching;

internal interface ITagStore
{
    ValueTask AddTagsAsync(
        string key,
        IReadOnlyCollection<string> tags,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyCollection<string>> GetKeysAsync(
        string tag,
        CancellationToken cancellationToken = default);

    ValueTask RemoveKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    ValueTask RemoveTagAsync(
        string tag,
        CancellationToken cancellationToken = default);
}
