namespace Jnvp.Platform.Results.Exceptions;

public sealed class UnsupportedMediaTypeException(
    string message = ErrorMessages.UnsupportedMediaType,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.UnsupportedMediaType;
}
