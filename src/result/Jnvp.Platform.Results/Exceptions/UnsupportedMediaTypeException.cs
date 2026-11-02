namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request whose content type is not supported.</summary>
/// <example><code language="csharp">throw new UnsupportedMediaTypeException("Upload a JSON document.");</code></example>
public sealed class UnsupportedMediaTypeException(
    string message = ErrorMessages.UnsupportedMediaType,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.UnsupportedMediaType;
}
