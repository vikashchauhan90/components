namespace Jnvp.Platform.Results.Exceptions;

public sealed class ServiceUnavailableException(
    string message = ErrorMessages.ServiceUnavailable,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.ServiceUnavailable;
}