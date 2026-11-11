# Jnvp.Platform

Reusable .NET libraries for caching, compression, observability, result-based
error handling, and serialization.

## Packages

All packages target .NET 10 (`net10.0`).

| NuGet package | Purpose | Documentation |
| --- | --- | --- |
| `Jnvp.Platform.Caching` | Redis-backed FusionCache provider | [Caching README](src/caching/README.md) |
| `Jnvp.Platform.Compression` | GZip, Brotli, and Deflate compression | [Compression README](src/compression/README.md) |
| `Jnvp.Platform.Observability` | OpenTelemetry logging, metrics, tracing, and redaction | [Observability README](src/observability/README.md) |
| `Jnvp.Platform.Results` | Typed results, errors, and exception types | [Results README](src/result/README.md) |
| `Jnvp.Platform.Serialization` | JSON and MessagePack serializers | [Serialization README](src/serialization/README.md) |

Install a package with the .NET CLI:

```shell
dotnet add package Jnvp.Platform.Caching
```

Replace the package name with the library you need. NuGet resolves the package
dependencies automatically.

## Build

The repository requires the .NET 10 SDK:

```shell
dotnet restore Jnvp.Platform.slnx
dotnet build Jnvp.Platform.slnx --configuration Release --no-restore
```

## NuGet publishing

Pushing a version tag such as `v1.2.3` packs and publishes all five packages
with version `1.2.3`. Configure these GitHub Actions repository secrets:

- `NUGET_API_KEY`: a NuGet.org API key with permission to publish the packages.
- `STRONG_NAME_KEY_BASE64`: the base64-encoded private strong-name `.snk` file
  used to sign package assemblies.

The tag must use a valid NuGet package version. Publishing skips packages that
already exist at that version.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for the
full license text.
