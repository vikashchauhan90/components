namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request that is invalid or cannot be processed as submitted.</summary>
/// <example><code language="csharp">throw new BadRequestException("The page number must be positive.");</code></example>
public sealed class BadRequestException(
    string message = ErrorMessages.BadRequest,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.BadRequest;
}