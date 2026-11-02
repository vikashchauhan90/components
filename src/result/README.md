# Jnvp.Platform.Results

`Jnvp.Platform.Results` provides typed result and error primitives for representing expected application failures without using exceptions as routine control flow. A result can carry a value, a categorized error, and optional field-level validation details.

## Requirements

- .NET 10 or later

## Installation

```shell
dotnet add package Jnvp.Platform.Results
```

Or add a package reference to your project, replacing `x.y.z` with the version you use:

```xml
<PackageReference Include="Jnvp.Platform.Results" Version="x.y.z" />
```

## Quick start

Use `Result<T>` when an operation can fail and otherwise returns a value. Use `Result` for operations that have no return value.

```csharp
using Jnvp.Platform.Results;

Result<int> FindOrder(string orderNumber)
{
    if (string.IsNullOrWhiteSpace(orderNumber))
    {
        return Result<int>.Failure(
            Error.BadRequest("An order number is required."));
    }

    // Replace this example with the application's lookup.
    return Result<int>.Success(1042);
}

var result = FindOrder("ORD-1042");
if (result.IsSuccess)
{
    Console.WriteLine($"Order ID: {result.Value}");
}
else
{
    Console.WriteLine($"{result.Error?.Code}: {result.Error?.Message}");
}
```

The `ResultFactory` provides equivalent concise methods:

```csharp
Result completed = ResultFactory.Ok();
Result<string> greeting = ResultFactory.Ok("Hello");
Result failed = ResultFactory.Fail(
    Error.NotFound("The requested resource was not found."));
```

## Validation errors

Attach one or more messages to field names using the optional `Errors` dictionary:

```csharp
using Jnvp.Platform.Results;

var validation = Error.BadRequest(
    "Correct the highlighted fields.",
    new Dictionary<string, string[]>
    {
        ["email"] = ["Enter a valid email address."],
        ["quantity"] = ["Quantity must be greater than zero."]
    });

Result result = Result.Failure(validation);
```

Errors also support caller-defined codes as well as the standard categories:

```csharp
var error = Error.Create(
    code: "inventory.out_of_stock",
    message: "The requested quantity is not available.",
    type: ErrorType.Conflict);
```

## HTTP metadata

`ErrorTypeExtensions` maps standard error categories to HTTP status codes, codes, default messages, and problem type URIs:

```csharp
using Jnvp.Platform.Results;

var errorType = ErrorType.NotFound;
var statusCode = errorType.ToHttpStatusCode(); // HttpStatusCode.NotFound
var code = errorType.GetCode();                // "not_found"
var problemType = errorType.GetProblemType();
```

## Exceptions

The `Jnvp.Platform.Results.Exceptions` namespace includes exception types such as `NotFoundException`, `ConflictException`, and `ServiceUnavailableException`. Each exposes an `ErrorType` and the error code derived from that category; an optional inner exception can preserve the original cause. Use these when exception-based integration is appropriate, and prefer `Result` / `Result<T>` for expected outcomes.

```csharp
using Jnvp.Platform.Results.Exceptions;

throw new NotFoundException("The requested order does not exist.");
```

## Main API

- `Result` and `Result<T>`: success/failure outcomes, with an optional error.
- `Error`: standard or custom error code, message, category, and validation details.
- `ErrorType`, `ErrorCodes`, and `ErrorMessages`: standard error classifications and defaults.
- `ResultFactory`: concise `Ok` and `Fail` factories.
- `ErrorTypeExtensions`: HTTP status and problem metadata mappings.
- `Jnvp.Platform.Results.Exceptions`: exception types for HTTP-style and storage failures.

## License

No license is currently declared in this repository. Confirm and add the intended license before redistributing this package publicly.
