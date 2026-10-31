namespace Jnvp.Platform.Results.Exceptions;

public sealed class ForbiddenException(
    string message = ErrorMessages.Forbidden,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.Forbidden;
}