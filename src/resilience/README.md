# Jnvp.Platform.Resilience

Reusable resilience policies for .NET applications. The package provides an
`IResilienceExecutor` that executes asynchronous operations with a configurable
Polly policy for transient HTTP failures.

## Requirements

- .NET 10 or later

## Registration

Register with package defaults:

```csharp
using Jnvp.Platform.Resilience;

builder.Services.AddPlatformResilience();
```

Or bind values from configuration:

```csharp
builder.Services.AddPlatformResilience(
    builder.Configuration.GetSection("Resilience"));
```

Example `appsettings.json`:

```json
{
  "Resilience": {
    "MaxRetryAttempts": 4,
    "RetryDelay": "00:00:00.500",
    "FailureRatio": 0.2,
    "MinimumThroughput": 20,
    "SamplingDuration": "00:00:30",
    "BreakDuration": "00:00:10",
    "Timeout": "00:00:10"
  }
}
```

Any omitted settings retain their defaults. Settings can also be supplied in
code:

```csharp
builder.Services.AddPlatformResilience(options =>
{
    options.MaxRetryAttempts = 4;
    options.RetryDelay = TimeSpan.FromMilliseconds(500);
});
```

## Executing operations

Inject `IResilienceExecutor` and pass the operation and its cancellation token:

```csharp
public sealed class CatalogClient(IResilienceExecutor resilienceExecutor)
{
    public ValueTask RefreshAsync(CancellationToken cancellationToken) =>
        resilienceExecutor.ExecuteAsync(
            token => RefreshCatalogAsync(token),
            cancellationToken);

    public ValueTask<Product> GetProductAsync(
        CancellationToken cancellationToken) =>
        resilienceExecutor.ExecuteAsync(
            token => FetchProductAsync(token),
            cancellationToken);

    private static ValueTask RefreshCatalogAsync(CancellationToken token) =>
        ValueTask.CompletedTask;

    private static ValueTask<Product> FetchProductAsync(CancellationToken token) =>
        ValueTask.FromResult(new Product());
}

public sealed class Product
{
}
```

The policy retries HTTP request failures, timeouts, HTTP 408, HTTP 429 (honoring
`Retry-After` when supplied), and HTTP 5xx responses. It uses exponential
backoff with jitter, a circuit breaker, and a per-attempt timeout. Retry and
circuit-breaker events are written through `ILogger`; the application's
configured logging providers determine where those events are sent.

## HttpClient integration

For outgoing HTTP requests, register the platform `HttpClientFactory`
resilience handler. It uses the same retry, circuit-breaker, and per-attempt
timeout settings from `ResilienceOptions`. HTTP 429 responses are retried (and
honor `Retry-After`) but are not counted as circuit-breaker failures:

```csharp
builder.Services.AddPlatformResilience(
    builder.Configuration.GetSection("Resilience"));

builder.Services
    .AddHttpClient<CatalogClient>()
    .AddPlatformResilienceHandler();
```

Add the resilience handler only once per client. Avoid wrapping a client that
already has this handler in `IResilienceExecutor`, as nested retries and
timeouts can multiply work and latency.
