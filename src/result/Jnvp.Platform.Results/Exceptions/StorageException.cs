namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a failure while accessing or updating storage.</summary>
/// <example><code language="csharp">throw new StorageException("The record could not be written.");</code></example>
public sealed class StorageException(
    string message = ErrorMessages.StorageError,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.StorageError;
}