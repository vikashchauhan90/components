# Jnvp.Platform.Observability

`Jnvp.Platform.Observability` supplies reusable application observability components for .NET: OpenTelemetry log processing, meter and activity-source base classes, W3C trace-context propagation, and classification-based log redaction.

## Requirements

- .NET 10 or later

The package uses Microsoft.Extensions compliance and telemetry APIs and OpenTelemetry. Its dependencies are restored automatically when the package is installed.

## Installation

```shell
dotnet add package Jnvp.Platform.Observability
```

Or add a package reference to your project, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Observability" Version="x.y.z" />
```

## Quick start

Register the package services in your host and enable platform redaction on the logging builder:

The log processor requires a non-empty prefix. Add this setting to `appsettings.json` before running the sample:

```json
{
  "Logging": {
    "Processor": {
      "Prefix": "orders"
    }
  }
}
```

```csharp
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Jnvp.Platform.Observability;
using Jnvp.Platform.Observability.Metrics;
using Jnvp.Platform.Observability.Processors;
using Jnvp.Platform.Observability.Tracing;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ActivityDefaults.EnsureW3C();
builder.Logging.AddPlatformRedaction();
builder.Services.AddLogProcessorService(
    builder.Configuration.GetSection("Logging:Processor"));
builder.Logging.AddOpenTelemetry(options =>
    options.AddProcessor(serviceProvider =>
        serviceProvider.GetRequiredService<LogProcessor>()));
builder.Services.AddMeter<OrdersMeter>();
builder.Services.AddActivitySource<OrdersActivitySource>();

var app = builder.Build();
app.Run();

public sealed class OrdersMeter : MeterBase
{
    private readonly Counter<long> _created;

    public OrdersMeter() : base("Example.Orders", "1.0.0")
    {
        _created = CreateCounter<long>(
            "orders.created",
            unit: "{order}",
            description: "Number of orders created.");
    }

    public void RecordCreated() => _created.Add(1);
}

public sealed class OrdersActivitySource : ActivitySourceBase
{
    public OrdersActivitySource() : base("Example.Orders", "1.0.0") { }

    public Activity? StartLoad(string orderId)
    {
        var activity = StartActivity("orders.load");
        activity?.SetTag("order.id", orderId);
        return activity;
    }
}
```

Register the same meter and activity-source names with your OpenTelemetry metrics and tracing providers, respectively, and configure exporters in the host application. This package provides instruments and sources; it does not select or configure exporters.

## Classified-data redaction

Use the platform classification attributes on log properties that your logging pipeline expands, and enable the redaction integration with `AddPlatformRedaction`:

```csharp
using Jnvp.Platform.Observability.Compliance.Attributes;

public sealed class SignInLog
{
    [Email]
    public string EmailAddress { get; init; } = "";

    [AccessToken]
    public string AccessToken { get; init; } = "";

    [UserIdentifier]
    public string UserId { get; init; } = "";
}
```

The extension maps platform classifications to redactors:

- Security credentials (including passwords, tokens, API keys, and connection strings) use full `[REDACTED]` masking.
- `Sensitive` and `UserIdentifier` values use HMAC redaction. Configure the HMAC key through the `configureHmac` callback and a secure key source; do not hard-code key material.
- Email, phone, name, and IP address values use partial masking.
- Credit-card and bank-account values retain at most their last four characters; other financial details use full masking.
- Unclassified values use a fixed-redaction fallback.

Redaction applies when values flow through logging integrations that honor `DataClassification` metadata; it does not rewrite arbitrary objects or data stored elsewhere in the application.

## Log attribute prefixing

`AddLogProcessorService` binds and validates the `LogProcessorOptions` and registers a `LogProcessor`. Configure the required prefix in the host configuration:

```json
{
  "Logging": {
    "Processor": {
      "Prefix": "orders"
    }
  }
}
```

The processor prefixes log attribute keys (for example, `category` becomes `orders.category`) and leaves `{OriginalFormat}` unchanged. The quick-start code attaches the registered processor to the OpenTelemetry logging provider; add exporters and other pipeline configuration in your host application.

## Trace context propagation

`ActivityPropagationHelper` uses the configured OpenTelemetry text-map propagator to inject and extract context:

```csharp
var headers = new Dictionary<string, string>();
ActivityPropagationHelper.InjectCurrent(headers);

var remoteContext = ActivityPropagationHelper.Extract(headers);
using var activity = activitySource.StartActivity(
    "orders.receive",
    ActivityKind.Consumer,
    remoteContext.ActivityContext);
```

Use `Inject` when a parent `ActivityContext` is already available, or `Extract(parentContext, headers)` to provide an explicit extraction parent.

## Main API

- `ObservabilityExtensions`: registers log processors, meters, and activity sources with dependency injection.
- `RedactionExtensions`: enables classification-aware redaction with platform defaults.
- `MeterBase` and `ActivitySourceBase`: create app-specific metric instruments and activities.
- `ActivityDefaults` and `ActivityPropagationHelper`: configure W3C IDs and propagate trace context.
- `LogProcessor` and `LogProcessorOptions`: prefix OpenTelemetry log attributes.
- `DataClassifications`, `DataClassificationCategories`, and `DataClassificationCodes`: define platform data classifications.
- `Jnvp.Platform.Observability.Compliance.Attributes`: attributes for security, personal, and financial data.
- `Jnvp.Platform.Observability.Compliance.Redactors`: redactor implementations used by the platform mappings.

## License

No license is currently declared in this repository. Confirm and add the intended license before redistributing this package publicly.
