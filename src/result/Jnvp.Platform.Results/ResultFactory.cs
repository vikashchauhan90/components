namespace Jnvp.Platform.Results;

/// <summary>
/// Provides concise factory methods for successful and failed results.
/// </summary>
/// <example>
/// <code language="csharp">
/// Result&lt;string&gt; result = ResultFactory.Ok("ready");
/// Result failed = ResultFactory.Fail(
///     Error.NotFound("The requested item was not found."));
/// </code>
/// </example>
public static class ResultFactory
{
    /// <summary>Creates a successful result without a value.</summary>
    public static Result Ok()
        => Result.Success();

    /// <summary>Creates a successful result containing a value.</summary>
    public static Result<T> Ok<T>(T value)
        => Result<T>.Success(value);

    /// <summary>Creates a failed result containing an error.</summary>
    public static Result Fail(Error error)
        => Result.Failure(error);

    /// <summary>Creates a failed result without an error detail.</summary>
    public static Result Fail()
      => Result.Failure();

    /// <summary>Creates a failed result of the specified value type containing an error.</summary>
    public static Result<T> Fail<T>(Error error)
        => Result<T>.Failure(error);

    /// <summary>Creates a failed result of the specified value type without an error detail.</summary>
    public static Result<T> Fail<T>()
       => Result<T>.Failure();
}