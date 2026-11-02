namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request that exceeded its allowed processing time.</summary>
/// <example><code language="csharp">throw new RequestTimeoutException("The request exceeded the processing deadline.");</code></example>
public sealed class RequestTimeoutException(
    string message = ErrorMessages.RequestTimeout,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.RequestTimeout;
}