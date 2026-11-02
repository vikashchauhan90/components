namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request that requires authentication or valid credentials.</summary>
/// <example><code language="csharp">throw new UnauthorizedException("A valid access token is required.");</code></example>
public sealed class UnauthorizedException(
    string message = ErrorMessages.Unauthorized,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.Unauthorized;
}
