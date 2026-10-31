namespace Jnvp.Platform.Results.Exceptions;

public sealed class TooManyRequestsException(
    string message = ErrorMessages.TooManyRequests,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.TooManyRequests;
}