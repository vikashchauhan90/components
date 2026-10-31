namespace Jnvp.Platform.Results.Exceptions;

public sealed class StorageException(
    string message = ErrorMessages.StorageError,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.StorageError;
}