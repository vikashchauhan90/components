namespace Jnvp.Platform.Results.Exceptions;

/// <summary>Represents a syntactically valid request that fails semantic validation.</summary>
/// <example><code language="csharp">throw new UnprocessableEntityException("The selected delivery date is unavailable.");</code></example>
public sealed class UnprocessableEntityException(
    string message = ErrorMessages.UnprocessableEntity,
    Exception? innerException = null)
    : ExceptionBase(message, innerException)
{
    /// <inheritdoc />
    public override ErrorType ErrorType => ErrorType.UnprocessableEntity;
}
