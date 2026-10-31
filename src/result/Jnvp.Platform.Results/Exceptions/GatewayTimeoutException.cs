namespace Jnvp.Platform.Results.Exceptions;

public sealed class GatewayTimeoutException(
    string message = ErrorMessages.GatewayTimeout,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.GatewayTimeout;
}