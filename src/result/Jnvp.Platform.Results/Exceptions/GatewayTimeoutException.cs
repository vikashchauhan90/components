namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an upstream service that did not respond before the deadline.</summary>
/// <example><code language="csharp">throw new GatewayTimeoutException("The payment provider timed out.");</code></example>
public sealed class GatewayTimeoutException(
    string message = ErrorMessages.GatewayTimeout,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.GatewayTimeout;
}