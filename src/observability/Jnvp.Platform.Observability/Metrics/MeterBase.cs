using System.Diagnostics.Metrics;

namespace Jnvp.Platform.Observability.Metrics;

/// <summary>Base class for application-specific OpenTelemetry meters and instruments.</summary>
/// <example>
/// <code language="csharp">
/// public sealed class OrdersMeter : MeterBase
/// {
///     private readonly Counter&lt;long&gt; _created;
///
///     public OrdersMeter() : base("Contoso.Orders")
///     {
///         _created = CreateCounter&lt;long&gt;(
///             "orders.created",
///             unit: "{order}",
///             description: "Number of orders created.");
///     }
///
///     public void RecordCreated() =&gt; _created.Add(1);
/// }
/// </code>
/// </example>
public abstract class MeterBase : IDisposable
{
    private readonly Meter _meter;
    private int _disposed;

    /// <summary>Initializes a meter with its instrumentation name and optional version.</summary>
    protected MeterBase(
        string meterName,
        string? version = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(meterName);

        _meter = new Meter(meterName, version);
    }

    /// <summary>Gets the underlying meter for derived instrument implementations.</summary>
    protected Meter Meter => _meter;

    /// <summary>Gets the name used to create the underlying meter.</summary>
    public string Name => _meter.Name;

    /// <summary>Creates a counter instrument.</summary>
    protected Counter<T> CreateCounter<T>(
        string name,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ValidateInstrumentName(name);

        return _meter.CreateCounter<T>(
            name,
            unit,
            description);
    }

    /// <summary>Creates a histogram instrument.</summary>
    protected Histogram<T> CreateHistogram<T>(
        string name,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ValidateInstrumentName(name);

        return _meter.CreateHistogram<T>(
            name,
            unit,
            description);
    }

    /// <summary>Creates an up/down counter instrument.</summary>
    protected UpDownCounter<T> CreateUpDownCounter<T>(
        string name,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ValidateInstrumentName(name);

        return _meter.CreateUpDownCounter<T>(
            name,
            unit,
            description);
    }

    /// <summary>Creates an observable counter instrument.</summary>
    protected ObservableCounter<T> CreateObservableCounter<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(observeValue);
        ValidateInstrumentName(name);

        return _meter.CreateObservableCounter(
            name,
            observeValue,
            unit,
            description);
    }

    /// <summary>Creates an observable gauge backed by a single observed value.</summary>
    protected ObservableGauge<T> CreateObservableGauge<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(observeValue);
        ValidateInstrumentName(name);

        return _meter.CreateObservableGauge(
            name,
            observeValue,
            unit,
            description);
    }

    /// <summary>Creates an observable up/down counter instrument.</summary>
    protected ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(observeValue);
        ValidateInstrumentName(name);

        return _meter.CreateObservableUpDownCounter(
            name,
            observeValue,
            unit,
            description);
    }

    /// <summary>Disposes the meter and its instruments.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases the meter when called from the disposal pattern.</summary>
    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        if (disposing)
        {
            _meter.Dispose();
        }
    }

    /// <summary>Creates an observable gauge backed by zero or more measurements.</summary>
    protected ObservableGauge<T> CreateObservableGauge<T>(
    string name,
    Func<IEnumerable<Measurement<T>>> observeValues,
    string? unit = null,
    string? description = null)
    where T : struct
    {
        ArgumentNullException.ThrowIfNull(observeValues);
        ValidateInstrumentName(name);

        return Meter.CreateObservableGauge(
            name,
            observeValues,
            unit,
            description);
    }

    private static void ValidateInstrumentName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
    }
}
