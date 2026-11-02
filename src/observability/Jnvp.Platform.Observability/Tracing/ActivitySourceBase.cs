
using System.Diagnostics;

namespace Jnvp.Platform.Observability.Tracing;

/// <summary>
/// Base class for an application-specific <see cref="ActivitySource"/> with lifecycle hooks and parent-context support.
/// </summary>
/// <example>
/// <code language="csharp">
/// public sealed class OrdersActivitySource : ActivitySourceBase
/// {
///     public OrdersActivitySource() : base("Contoso.Orders", "1.0.0") { }
///
///     public Activity? StartLoad(string orderId)
///     {
///         var activity = StartActivity("orders.load", ActivityKind.Internal);
///         activity?.SetTag("order.id", orderId);
///         return activity;
///     }
/// }
/// </code>
/// </example>
public abstract class ActivitySourceBase : IDisposable
{
    private readonly ActivitySource _activitySource;
    private int _disposed;

    /// <summary>Initializes an activity source with its instrumentation name and optional version.</summary>
    protected ActivitySourceBase(
        string sourceName,
        string? version = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceName);

        _activitySource = new ActivitySource(
            sourceName,
            version);
    }

    /// <summary>Gets the underlying source for derived instrumentation.</summary>
    protected ActivitySource ActivitySource => _activitySource;

    /// <summary>Gets the name used to create the underlying activity source.</summary>
    public string Name => _activitySource.Name;

    /// <summary>Gets whether any listener is interested in this activity source.</summary>
    public bool HasListeners() => _activitySource.HasListeners();

    /// <summary>Starts an activity using the current activity as its parent, if present.</summary>
    public Activity? StartActivity(
        string activityName,
        ActivityKind kind = ActivityKind.Internal)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(activityName);

        return StartActivityCore(
            activityName,
            kind,
            default);
    }

    /// <summary>Starts an activity with an explicit parent activity context.</summary>
    public Activity? StartActivity(
        string activityName,
        ActivityKind kind,
        ActivityContext parentContext)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(activityName);

        return StartActivityCore(
            activityName,
            kind,
            parentContext);
    }

    /// <summary>Starts an activity using a remote W3C trace-parent header.</summary>
    public Activity? StartRemote(
        string activityName,
        ActivityKind kind,
        string traceParent,
        string? traceState = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(activityName);
        ArgumentException.ThrowIfNullOrWhiteSpace(traceParent);

        if (!ActivityContext.TryParse(
                traceParent,
                traceState,
                out ActivityContext parentContext))
        {
            throw new ArgumentException(
                "The supplied W3C trace context is invalid.",
                nameof(traceParent));
        }

        return StartActivityCore(
            activityName,
            kind,
            parentContext);
    }

    /// <summary>Enriches a newly created activity before it is returned.</summary>
    protected virtual void Enrich(Activity activity)
    {
    }

    /// <summary>Runs after an activity has been created and enriched.</summary>
    protected virtual void OnActivityStarted(Activity activity)
    {
    }

    /// <summary>Provides an overridable extension point for derived types to handle activity-stop behavior.</summary>
    protected virtual void OnActivityStopped(Activity activity)
    {
    }

    private Activity? StartActivityCore(
        string activityName,
        ActivityKind kind,
        ActivityContext parentContext)
    {
        var activity = parentContext == default
            ? _activitySource.StartActivity(activityName, kind)
            : _activitySource.StartActivity(
                activityName,
                kind,
                parentContext);

        if (activity is null)
        {
            return null;
        }

        Enrich(activity);
        OnActivityStarted(activity);

        return activity;
    }

    /// <summary>Disposes the activity source.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases resources when called from the disposal pattern.</summary>
    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        if (disposing)
        {
            _activitySource.Dispose();
        }
    }
}
