namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an unexpected server-side failure.</summary>
/// <example><code language="csharp">throw new InternalServerErrorException("The operation could not be completed.");</code></example>
public sealed class InternalServerErrorException(
    string message = ErrorMessages.InternalServerError,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.InternalServerError;
}
