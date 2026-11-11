# Jnvp.Platform.Compression

`Jnvp.Platform.Compression` provides a shared interface for synchronous
byte-buffer and asynchronous stream compression using the built-in .NET GZip,
Brotli, and Deflate implementations.

## Requirements

- .NET 10 or later

## Installation

```shell
dotnet add package Jnvp.Platform.Compression
```

Or add a package reference, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Compression" Version="x.y.z" />
```

## Quick start

Choose an algorithm and register its compressor with dependency injection:

```csharp
using System.Text;
using Jnvp.Platform.Compression;
using Jnvp.Platform.Compression.Brotli;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddBrotliCompression(options =>
{
    options.MaxDecompressedBytes = 50 * 1024 * 1024;
});

using var serviceProvider = services.BuildServiceProvider();
var compressor = serviceProvider.GetRequiredService<ICompressor>();

byte[] original = Encoding.UTF8.GetBytes("Hello, compression!");
byte[] compressed = compressor.Compress(original);
byte[] restored = compressor.Decompress(compressed);
```

Replace `AddBrotliCompression` with `AddGZipCompression` or
`AddDeflateCompression` to use another format. Register only one compressor per
service collection because each registration supplies the same `ICompressor`
service.

For stream-based processing, use `CompressAsync` and `DecompressAsync` with
source and destination streams. These operations support cancellation.

## Configuration and diagnostics

`CompressionOptions` controls the compression level and the maximum number of
bytes produced during decompression. The default decompressed-output limit is
100 MiB. Keep an appropriate limit when processing untrusted compressed input.

Compression operations also emit activities through
`CompressionDiagnostics.ActivitySource`.

## Main API

- `ICompressor`: byte-buffer and stream compression/decompression.
- `CompressionOptions`: compression level and decompressed-size limit.
- `AddGZipCompression`, `AddBrotliCompression`, and `AddDeflateCompression`:
  dependency-injection registration methods.

## License

This package is licensed under the [MIT License](https://licenses.nuget.org/MIT).
