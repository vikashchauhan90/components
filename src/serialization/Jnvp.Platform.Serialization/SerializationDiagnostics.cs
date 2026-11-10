using System.Diagnostics;

namespace Jnvp.Platform.Serialization;

internal static class SerializationDiagnostics
{
    public const string ActivitySourceName =
        "Jnvp.Platform.Serialization";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);


    public static Activity? StartSerialize<T>(string format)
    {
        var activity = ActivitySource.StartActivity(
            "serialization.serialize",
            ActivityKind.Internal);

        activity?.SetTag("serialization.format", format);
        activity?.SetTag("serialization.type", typeof(T).FullName);

        return activity;
    }

    public static Activity? StartDeserialize<T>(string format)
    {
        var activity = ActivitySource.StartActivity(
            "serialization.deserialize",
            ActivityKind.Internal);

        activity?.SetTag("serialization.format", format);
        activity?.SetTag("serialization.type", typeof(T).FullName);

        return activity;
    }

    public static void RecordException(
        Activity? activity,
        Exception exception)
    {
        activity?.SetStatus(
            ActivityStatusCode.Error,
            exception.Message);
    }
}
