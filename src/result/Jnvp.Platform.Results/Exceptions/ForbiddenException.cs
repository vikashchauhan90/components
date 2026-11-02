namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an attempt to access a resource without sufficient permissions.</summary>
/// <example><code language="csharp">throw new ForbiddenException("The caller cannot administer this account.");</code></example>
public sealed class ForbiddenException(
    string message = ErrorMessages.Forbidden,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.Forbidden;
}