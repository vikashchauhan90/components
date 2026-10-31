namespace Jnvp.Platform.Results.Exceptions;

public sealed class InternalServerErrorException(
    string message = ErrorMessages.InternalServerError,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.InternalServerError;
}
