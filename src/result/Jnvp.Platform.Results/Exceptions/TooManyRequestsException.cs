namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a request rejected because a rate limit was exceeded.</summary>
/// <example><code language="csharp">throw new TooManyRequestsException("Please retry after the rate limit window.");</code></example>
public sealed class TooManyRequestsException(
    string message = ErrorMessages.TooManyRequests,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.TooManyRequests;
}