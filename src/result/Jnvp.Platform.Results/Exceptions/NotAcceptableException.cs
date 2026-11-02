namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request for a response format that cannot be produced.</summary>
/// <example><code language="csharp">throw new NotAcceptableException("Only JSON responses are available.");</code></example>
public sealed class NotAcceptableException(
    string message = ErrorMessages.NotAcceptable,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.NotAcceptable;
}