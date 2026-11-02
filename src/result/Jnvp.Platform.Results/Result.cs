namespace Jnvp.Platform.Results;

/// <summary>
/// Represents the outcome of an operation that does not return a value.
/// </summary>
/// <example>
/// <code language="csharp">
/// Result result = Result.Success();
/// if (!result.IsSuccess)
/// {
///     Console.WriteLine(result.Error?.Message);
/// }
/// </code>
/// </example>
public class Result
{
    /// <summary>Initializes a result with its outcome and optional error.</summary>
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>Gets a value indicating whether the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets the error associated with a failed operation, if provided.</summary>
    public Error? Error { get; }

    /// <summary>Creates a successful result.</summary>
    public static Result Success()
        => new(true, null);

    /// <summary>Creates a failed result containing the specified error.</summary>
    public static Result Failure(Error error)
        => new(false, error);

    /// <summary>Creates a failed result without an error detail.</summary>
    public static Result Failure()
      => new(false, null);
}