namespace Jnvp.Platform.Results.Exceptions;

public sealed class UnauthorizedException(
    string message = ErrorMessages.Unauthorized,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.Unauthorized;
}
