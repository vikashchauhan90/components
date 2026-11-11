# Jnvp.Platform.Caching

Redis-backed FusionCache integration for .NET applications. It wires
FusionCache to Redis for distributed storage, uses a Redis backplane to
propagate cache changes between application instances, and configures
MessagePack serialization with LZ4 block-array compression by default.

This package deliberately does not provide in-memory-only, generic
`IDistributedCache`, or a custom Redis provider. Use FusionCache directly when
you need its full API, including factories, tags, fail-safe behavior, and
plugins. `ICacheProvider` is a small convenience API for string, byte, and
byte-array values.

## Requirements

- .NET 10 or later
- A Redis deployment reachable by each application instance

## Installation

```shell
dotnet add package Jnvp.Platform.Caching
```

Or add a package reference, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Caching" Version="x.y.z" />
```

## Registration

Read Redis credentials from a secret provider or environment-backed
configuration. Use the same `instanceName` for replicas of the same
application and environment, and a different name for each application or
environment that shares Redis:

```csharp
using Jnvp.Platform.Caching;
using Jnvp.Platform.Caching.Fusion;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;

var redisConfiguration =
    builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException(
        "The Redis connection string is not configured.");

builder.Services.AddFusionRedisCaching(
    redisConfiguration,
    instanceName: "orders-production",
    configureDefaultEntryOptions: options =>
    {
        options.Duration = TimeSpan.FromMinutes(5);
        options.SetFailSafe(
            isEnabled: true,
            maxDuration: TimeSpan.FromHours(1),
            throttleDuration: TimeSpan.FromSeconds(30));
        options.SetFactoryTimeouts(
            softTimeout: TimeSpan.FromMilliseconds(100),
            hardTimeout: TimeSpan.FromSeconds(2));
    });
```

The registration configures both the StackExchange Redis distributed cache and
FusionCache's Redis backplane. It also sets the FusionCache name and Redis key
prefix from `instanceName`. Both Redis integrations use the provided
connection configuration; optional callbacks let the application further
configure the distributed cache, backplane, and FusionCache options.

Set entry defaults deliberately for your data freshness and outage
requirements. The example uses a five-minute normal duration and permits
fail-safe data for up to one hour. Fail-safe is a data-consistency decision;
adjust or disable it when stale values are not acceptable.

## Using the convenience provider

Resolve `ICacheProvider` for the common string and byte-array operations.
Entry tags and absolute expiration are passed per write:

```csharp
var cache = app.Services.GetRequiredService<ICacheProvider>();

await cache.SetStringAsync(
    "catalog:42",
    """{"id":42}""",
    options: new CacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
        Tags = ["catalog"]
    });

string? value = await cache.GetStringAsync("catalog:42");
await cache.InvalidateTagAsync("catalog");
```

Tags are FusionCache's native tag feature rather than a platform-managed tag
index. They can be used to invalidate related entries across replicas through
the Redis backplane.

For generic values, factory-based reads, or other FusionCache features, inject
`IFusionCache` directly:

```csharp
var fusionCache = app.Services.GetRequiredService<IFusionCache>();

var product = await fusionCache.GetOrSetAsync(
    "product:42",
    _ => Task.FromResult(new Product(42, "Example")),
    options => options.SetDuration(TimeSpan.FromMinutes(5)),
    tags: ["products"]);
```

## Serialization and compression

By default, distributed cache values use FusionCache's contractless MessagePack
serializer with `MessagePackCompression.Lz4BlockArray`. This compresses
serialized values before they are stored in Redis. To use different MessagePack
resolver or compression settings, pass a configured
`MessagePackSerializerOptions` through the `messagePackOptions` parameter.
All application instances sharing entries must use compatible serializers and
serialization settings; changing them can make existing Redis entries
unreadable until they expire or are removed.

## Redis and deployment notes

- Use TLS and authentication as appropriate for the deployment. Supply
  credentials through secret management, not source-controlled settings.
- Configure Redis timeouts, certificates, and other deployment-specific
  settings through `configureRedisCache` and `configureRedisBackplane`.
- Use a unique `instanceName` per application/environment, but keep it
  identical across that application's replicas.
- Redis stores the distributed values; the backplane communicates changes
  between local FusionCache instances. All replicas that need coherent
  invalidation must use the same Redis deployment and cache namespace.
- Configure FusionCache default durations, fail-safe, and factory timeouts to
  match the application's consistency and availability requirements.

## Main API

- `AddFusionRedisCaching`: registers FusionCache, Redis distributed storage,
  the Redis backplane, and the compressed MessagePack serializer.
- `ICacheProvider`: convenience API for string and byte-array cache
  operations and native FusionCache tag invalidation.
- `CacheEntryOptions`: per-write absolute expiration and FusionCache tags.
- `IFusionCache`: FusionCache's full native API, available from dependency
  injection.

## License

This package is licensed under the [MIT License](https://licenses.nuget.org/MIT).
