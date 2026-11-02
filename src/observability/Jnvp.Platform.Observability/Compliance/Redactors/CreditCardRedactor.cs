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

        return DigitSuffixMaskingRedactor
            .Redact(source, destination, visibleSuffixDigits: 4);
    }
}
