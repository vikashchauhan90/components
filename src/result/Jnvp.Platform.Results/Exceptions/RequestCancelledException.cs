namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an operation cancelled before it could complete.</summary>
/// <example><code language="csharp">throw new RequestCancelledException("The request was cancelled by the caller.");</code></example>
public sealed class RequestCancelledException(
    string message = ErrorMessages.RequestCancelled,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.RequestCancelled;
}
