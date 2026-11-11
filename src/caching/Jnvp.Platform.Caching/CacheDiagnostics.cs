using System.Diagnostics;

namespace Jnvp.Platform.Caching;

internal static class CacheDiagnostics
{
    public const string ActivitySourceName =
        "Jnvp.Platform.Caching";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);

    public static Activity? StartSet()
    {
        return ActivitySource.StartActivity(
            "cache.set",
            ActivityKind.Internal);
    }

    public static Activity? StartGet()
    {
        return ActivitySource.StartActivity(
            "cache.get",
            ActivityKind.Internal);
    }

    public static Activity? StartRemove()
    {
        return ActivitySource.StartActivity(
            "cache.remove",
            ActivityKind.Internal);
    }

    public static Activity? StartInvalidateTag()
    {
        return ActivitySource.StartActivity(
            "cache.invalidate_tag",
            ActivityKind.Internal);
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
