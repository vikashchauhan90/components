namespace Jnvp.Platform.Results.Exceptions;

public sealed class PayloadTooLargeException(
    string message = ErrorMessages.PayloadTooLarge,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.PayloadTooLarge;
}