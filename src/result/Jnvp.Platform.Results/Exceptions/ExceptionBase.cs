namespace Jnvp.Platform.Results.Exceptions;

/// <summary>
/// Base class for exceptions that carry a platform <see cref="ErrorType"/> and its derived error code.
/// </summary>
/// <example>
/// <code language="csharp">
/// catch (ExceptionBase exception)
/// {
///     Console.WriteLine($"{exception.ErrorCode}: {exception.Message}");
/// }
/// </code>
/// </example>
public abstract class ExceptionBase(
    string message,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>Gets the category represented by this exception.</summary>
    public abstract ErrorType ErrorType { get; }

    /// <summary>Gets the stable machine-readable code for <see cref="ErrorType"/>.</summary>
    public string ErrorCode => ErrorType.GetCode();
}