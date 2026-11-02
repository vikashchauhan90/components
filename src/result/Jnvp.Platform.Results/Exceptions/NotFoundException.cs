namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request for a resource that does not exist.</summary>
/// <example><code language="csharp">throw new NotFoundException("The requested customer was not found.");</code></example>
public sealed class NotFoundException(
    string message = ErrorMessages.NotFound,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.NotFound;
}