using System.Diagnostics;

namespace Jnvp.Platform.Caching;

internal static class CacheDiagnostics
{
    public const string ActivitySourceName =
        "Jnvp.Platform.Caching";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);

    public static Activity? StartSet(string key)
    {
        var activity = ActivitySource.StartActivity(
            "cache.set",
            ActivityKind.Internal);

        activity?.SetTag("cache.key", key);

        return activity;
    }

    public static Activity? StartGet(string key)
    {
        var activity = ActivitySource.StartActivity(
            "cache.get",
            ActivityKind.Internal);

        activity?.SetTag("cache.key", key);

        return activity;
    }

    public static Activity? StartRemove(string key)
    {
        var activity = ActivitySource.StartActivity(
            "cache.remove",
            ActivityKind.Internal);

        activity?.SetTag("cache.key", key);

        return activity;
    }

    public static Activity? StartInvalidateTag(string tag)
    {
        var activity = ActivitySource.StartActivity(
            "cache.invalidate_tag",
            ActivityKind.Internal);

        activity?.SetTag("cache.tag", tag);

        return activity;
    }

    public static void RecordException(
        Activity? activity,
        Exception exception)
    {
        activity?.SetStatus(
            ActivityStatusCode.Error,
            exception.Message);

        activity?.AddException(exception);
    }
}
