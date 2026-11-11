# Jnvp.Platform.Serialization

`Jnvp.Platform.Serialization` provides a common interface for serializing and
deserializing objects as UTF-8 JSON or MessagePack binary data. Both
implementations support byte buffers and streams, and can be registered with
dependency injection.

## Requirements

- .NET 10 or later

## Installation

```shell
dotnet add package Jnvp.Platform.Serialization
```

Or add a package reference, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Serialization" Version="x.y.z" />
```

## Quick start

Register one serializer implementation and resolve it through `ISerializer`:

```csharp
using Jnvp.Platform.Serialization;
using Jnvp.Platform.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddJsonSerialization(options =>
{
    options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

using var serviceProvider = services.BuildServiceProvider();
var serializer = serviceProvider.GetRequiredService<ISerializer>();

byte[] data = serializer.Serialize(new Order { Id = 42 });
Order? order = serializer.Deserialize<Order>(data);

public sealed record Order
{
    public int Id { get; init; }
}
```

Use `AddMessagePackSerialization` from
`Jnvp.Platform.Serialization.MessagePack` to register MessagePack instead.
Register only one serializer per service collection because both
implementations provide the same `ISerializer` service. Each registration has
an overload that accepts a configuration callback for its underlying
serializer options.

Use `SerializeAsync` and `DeserializeAsync` for stream-based operations. The
methods support cancellation; serialization writes to the provided destination
stream, and deserialization reads from the provided source stream.

## Diagnostics

Serialization and deserialization operations emit activities through
`SerializationDiagnostics.ActivitySource` and log trace-level operation
messages.

## Main API

- `ISerializer`: generic serialization and deserialization for byte buffers and
  streams.
- `AddJsonSerialization`: registers the System.Text.Json implementation.
- `AddMessagePackSerialization`: registers the MessagePack implementation.
- `SerializationDiagnostics`: activity source for serialization operations.

## License

This package is licensed under the [MIT License](https://licenses.nuget.org/MIT).
