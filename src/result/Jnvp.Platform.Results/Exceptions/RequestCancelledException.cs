namespace Jnvp.Platform.Results.Exceptions;

public sealed class RequestCancelledException(
    string message = ErrorMessages.RequestCancelled,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.RequestCancelled;
}
