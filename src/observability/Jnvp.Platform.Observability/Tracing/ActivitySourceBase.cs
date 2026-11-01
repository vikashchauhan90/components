
using System.Diagnostics;

namespace Jnvp.Platform.Observability.Tracing;

public abstract class ActivitySourceBase : IDisposable
{
    private readonly ActivitySource _activitySource;
    private int _disposed;

    protected ActivitySourceBase(
        string sourceName,
        string? version = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceName);

        _activitySource = new ActivitySource(
            sourceName,
            version);
    }

    protected ActivitySource ActivitySource => _activitySource;

    public string Name => _activitySource.Name;

    public bool HasListeners() => _activitySource.HasListeners();

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

    protected virtual void Enrich(Activity activity)
    {
    }

    protected virtual void OnActivityStarted(Activity activity)
    {
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
