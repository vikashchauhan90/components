
namespace Jnvp.Platform.Results.Exceptions;

public sealed class LockedException(
    string message = ErrorMessages.Locked,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    public override ErrorType ErrorType => ErrorType.Locked;
}