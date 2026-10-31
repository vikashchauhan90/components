namespace Jnvp.Platform.Results.Exceptions;

public sealed class NotAcceptableException(
    string message = ErrorMessages.NotAcceptable,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.NotAcceptable;
}