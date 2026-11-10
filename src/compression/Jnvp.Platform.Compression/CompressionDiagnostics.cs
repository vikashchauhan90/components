using System.Diagnostics;

namespace Jnvp.Platform.Compression;

internal static class CompressionDiagnostics
{
    public const string ActivitySourceName =
        "Jnvp.Platform.Compression";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);

    public static Activity? StartCompress(string algorithm)
    {
        var activity = ActivitySource.StartActivity(
            "compression.compress",
            ActivityKind.Internal);

        activity?.SetTag("compression.algorithm", algorithm);

        return activity;
    }

    public static Activity? StartDecompress(string algorithm)
    {
        var activity = ActivitySource.StartActivity(
            "compression.decompress",
            ActivityKind.Internal);

        activity?.SetTag("compression.algorithm", algorithm);

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
