namespace Jnvp.Platform.Results;

/// <summary>
/// Represents the outcome of an operation that can return a value.
/// </summary>
/// <example>
/// <code language="csharp">
/// Result&lt;int&gt; result = Result&lt;int&gt;.Success(42);
/// if (result.IsSuccess)
/// {
///     Console.WriteLine(result.Value);
/// }
/// </code>
/// </example>
public sealed class Result<T> : Result
{
    private Result(
        T? value,
        bool isSuccess,
        Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>Gets the operation's value when it succeeds.</summary>
    public T? Value { get; }

    /// <summary>Creates a successful result containing the specified value.</summary>
    public static Result<T> Success(T value)
        => new(
            value,
            true,
            null);

    /// <summary>Creates a failed result containing the specified error.</summary>
    public new static Result<T> Failure(Error error)
        => new(
            default,
            false,
            error);

    /// <summary>Creates a failed result without an error detail.</summary>
    public new static Result<T> Failure()
        => new(
            default,
            false,
            null);
}