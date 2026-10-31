namespace Jnvp.Platform.Results.Exceptions;

public sealed class RequestTimeoutException(
    string message = ErrorMessages.RequestTimeout,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.RequestTimeout;
}