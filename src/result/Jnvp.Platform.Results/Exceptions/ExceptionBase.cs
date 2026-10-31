namespace Jnvp.Platform.Results.Exceptions;

public abstract class ExceptionBase(
    string message,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public abstract ErrorType ErrorType { get; }
    public string ErrorCode => ErrorType.GetCode();
}