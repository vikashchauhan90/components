namespace Jnvp.Platform.Results.Exceptions;

public sealed class UnprocessableEntityException(
    string message = ErrorMessages.UnprocessableEntity,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.UnprocessableEntity;
}
