using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Jnvp.Platform.Observability.Tracing;

public static class ActivityPropagationHelper
{
    public static void Inject(
        ActivityContext context,
        IDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        Propagators.DefaultTextMapPropagator.Inject(
            new PropagationContext(context, Baggage.Current),
            headers,
            static (carrier, key, value) =>
                carrier[key] = value);
    }

    public static void InjectCurrent(
        IDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        var activity = Activity.Current;

        if (activity is null)
        {
            return;
        }

        Inject(activity.Context, headers);
    }

    public static PropagationContext Extract(
        IDictionary<string, string> headers)
    {
        return Extract(default, headers);
    }

    public static PropagationContext Extract(
        ActivityContext parentContext,
        IDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        return Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(parentContext, Baggage.Current),
            headers,
            static (carrier, key) =>
                carrier.TryGetValue(key, out var value)
                    ? [value]
                    : []);
    }
}
