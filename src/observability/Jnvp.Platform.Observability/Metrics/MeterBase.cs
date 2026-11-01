using System.Diagnostics.Metrics;

namespace Jnvp.Platform.Observability.Metrics;

public abstract class MeterBase : IDisposable
{
    private readonly Meter _meter;
    private int _disposed;

    protected MeterBase(
        string meterName,
        string? version = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(meterName);

        _meter = new Meter(meterName, version);
    }

    protected Meter Meter => _meter;

    public string Name => _meter.Name;

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
            _meter.Dispose();
        }
    }

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
