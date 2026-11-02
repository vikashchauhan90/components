using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Jnvp.Platform.Observability.Tracing;

/// <summary>Injects and extracts trace context using the configured OpenTelemetry text-map propagator.</summary>
/// <example>
/// <code language="csharp">
/// var headers = new Dictionary&lt;string, string&gt;();
/// ActivityPropagationHelper.InjectCurrent(headers);
///
/// var remoteContext = ActivityPropagationHelper.Extract(headers);
/// using var activity = activitySource.StartActivity(
///     "orders.load",
///     ActivityKind.Consumer,
///     remoteContext.ActivityContext);
/// </code>
/// </example>
public static class ActivityPropagationHelper
{
    /// <summary>Injects the specified activity context and current baggage into a carrier.</summary>
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

    /// <summary>Injects the current activity context and baggage when an activity is active.</summary>
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

    /// <summary>Extracts a propagation context from a carrier with no explicit parent context.</summary>
    public static PropagationContext Extract(
        IDictionary<string, string> headers)
    {
        return Extract(default, headers);
    }

    /// <summary>Extracts a propagation context from a carrier using the supplied parent context.</summary>
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
