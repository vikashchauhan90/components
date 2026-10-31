namespace Jnvp.Platform.Results.Exceptions;

public sealed class NotFoundException(
    string message = ErrorMessages.NotFound,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.NotFound;
}