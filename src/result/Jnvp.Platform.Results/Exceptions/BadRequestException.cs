namespace Jnvp.Platform.Results.Exceptions;

public sealed class BadRequestException(
    string message = ErrorMessages.BadRequest,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.BadRequest;
}