namespace Jnvp.Platform.Results.Exceptions;

public sealed class ConflictException(
    string message = ErrorMessages.Conflict,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.Conflict;
}
