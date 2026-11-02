namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a service that is temporarily unavailable.</summary>
/// <example><code language="csharp">throw new ServiceUnavailableException("The service is undergoing maintenance.");</code></example>
public sealed class ServiceUnavailableException(
    string message = ErrorMessages.ServiceUnavailable,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.ServiceUnavailable;
}