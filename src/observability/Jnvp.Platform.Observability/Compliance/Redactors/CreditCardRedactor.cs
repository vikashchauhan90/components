using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Masks a credit-card value while retaining its final four characters when available.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // Mark the model property with [CreditCard] to apply this redactor.
/// </code>
/// </example>
public sealed class CreditCardRedactor : Redactor
{
    private const char Mask = '*';

    /// <inheritdoc />
    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return input.Length;
    }

    /// <inheritdoc />
    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        if (source.Length <= 4)
        {
            destination.Fill(Mask);
            return source.Length;
        }

        destination[..^4].Fill(Mask);

        source[^4..]
            .CopyTo(destination[^4..]);

        return source.Length;
    }
}
