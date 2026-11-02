namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request payload larger than the endpoint accepts.</summary>
/// <example><code language="csharp">throw new PayloadTooLargeException("The uploaded file exceeds the size limit.");</code></example>
public sealed class PayloadTooLargeException(
    string message = ErrorMessages.PayloadTooLarge,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.PayloadTooLarge;
}