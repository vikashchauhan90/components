# Jnvp.Platform.Caching

`Jnvp.Platform.Caching` provides a common asynchronous cache interface with
in-memory, `IDistributedCache`, Redis, and FusionCache implementations. The
providers support string, byte, and stream values, expiration options, and
tag-based invalidation.

## Requirements

- .NET 10 or later

## Installation

```shell
dotnet add package Jnvp.Platform.Caching
```

Or add a package reference, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Caching" Version="x.y.z" />
```

## Quick start

Choose and register one provider. For example, use the built-in in-memory
provider:

```csharp
using Jnvp.Platform.Caching;
using Jnvp.Platform.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddMemoryCaching();
services.Configure<CacheEntryOptions>(options =>
{
    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
    options.Tags = ["catalog"];
});

using var serviceProvider = services.BuildServiceProvider();
var cache = serviceProvider.GetRequiredService<ICacheProvider>();

await cache.SetStringAsync("catalog:42", """{"id":42}""");
string? value = await cache.GetStringAsync("catalog:42");
await cache.InvalidateTagAsync("catalog");
```

For applications using Redis, register it instead:

```csharp
using Jnvp.Platform.Caching.Redis;

services.AddRedisCaching(connectionString);
```

`AddRedisCaching` registers Redis as the distributed cache and wires up the
platform provider. You can also use `AddDistributedCaching` after registering
another `IDistributedCache` implementation, or `AddFusionCaching` to use
FusionCache. Register only one provider per service collection; each provides
the same `ICacheProvider` service.

## Providers

- `AddMemoryCaching`: process-local cache; entries are not shared across
  application instances.
- `AddDistributedCaching`: uses a separately registered
  `IDistributedCache` implementation. Register the backend first.
- `AddRedisCaching`: configures the StackExchange.Redis distributed cache from
  a connection string or options callback.
- `AddFusionCaching`: registers FusionCache and its platform provider.

Configure common entry settings through `CacheEntryOptions`. Absolute
expiration, sliding expiration, and tags are supported by the memory and
distributed providers. FusionCache supports absolute expiration; it does not
support sliding expiration. Without an explicit expiration, the underlying
provider determines entry lifetime.

Tags allow related entries to be invalidated together. Distributed tag
invalidation uses shared tag-version records, so instances using the same
distributed backend observe the invalidation.

## Main API

- `ICacheProvider`: asynchronous operations for setting, retrieving, and
  removing values and invalidating tags.
- `CacheEntryOptions`: expiration and tag settings applied to cached entries.
- `AddMemoryCaching`, `AddDistributedCaching`, `AddRedisCaching`, and
  `AddFusionCaching`: dependency-injection registration methods.

## License

This package is licensed under the [MIT License](https://licenses.nuget.org/MIT).
