namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a conflict with the current state of a resource.</summary>
/// <example><code language="csharp">throw new ConflictException("The item has already been updated.");</code></example>
public sealed class ConflictException(
    string message = ErrorMessages.Conflict,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.Conflict;
}
