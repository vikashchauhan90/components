namespace Jnvp.Platform.Results.Exceptions;

public sealed class MethodNotAllowedException(
    string message = ErrorMessages.MethodNotAllowed,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.MethodNotAllowed;
}