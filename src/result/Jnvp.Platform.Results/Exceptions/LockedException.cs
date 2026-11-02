
namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an operation rejected because the resource is locked.</summary>
/// <example><code language="csharp">throw new LockedException("The document is currently being edited.");</code></example>
public sealed class LockedException(
    string message = ErrorMessages.Locked,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.Locked;
}