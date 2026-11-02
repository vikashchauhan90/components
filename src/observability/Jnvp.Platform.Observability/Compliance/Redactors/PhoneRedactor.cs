using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Masks the middle of a phone value while retaining up to the first two and last two characters.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // Mark the model property with [Phone] to apply this redactor.
/// </code>
/// </example>
public sealed class PhoneRedactor : Redactor
{
    private const string Mask = "******";

    /// <inheritdoc />
    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return input.Length <= 4
            ? Mask.Length
            : 2 + Mask.Length + 2;
    }

    /// <inheritdoc />
    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        if (source.Length <= 4)
        {
            Mask.AsSpan().CopyTo(destination);

            return Mask.Length;
        }

        var index = 0;

        source[..2].CopyTo(destination);
        index += 2;

        Mask.AsSpan().CopyTo(destination[index..]);
        index += Mask.Length;

        source[^2..].CopyTo(destination[index..]);
        index += 2;

        return index;
    }
}