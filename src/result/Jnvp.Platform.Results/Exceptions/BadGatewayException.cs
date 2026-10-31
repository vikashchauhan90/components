namespace Jnvp.Platform.Results.Exceptions;

public sealed class BadGatewayException(
    string message = ErrorMessages.BadGateway,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.BadGateway;
}