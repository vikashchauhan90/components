namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents an invalid response received from an upstream service.</summary>
/// <example><code language="csharp">throw new BadGatewayException("The inventory service returned an invalid response.");</code></example>
public sealed class BadGatewayException(
    string message = ErrorMessages.BadGateway,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.BadGateway;
}