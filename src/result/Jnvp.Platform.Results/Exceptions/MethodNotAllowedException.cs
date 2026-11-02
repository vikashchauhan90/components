namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request using a method that the resource does not support.</summary>
/// <example><code language="csharp">throw new MethodNotAllowedException("This endpoint only accepts GET requests.");</code></example>
public sealed class MethodNotAllowedException(
    string message = ErrorMessages.MethodNotAllowed,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.MethodNotAllowed;
}