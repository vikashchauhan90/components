using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Masks the middle of an email's local part and preserves its domain when the local part is longer than three characters.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // Mark the model property with [Email] to apply this redactor.
/// </code>
/// </example>
public sealed class EmailRedactor : Redactor
{
    private const string Mask = "****";


    /// <inheritdoc />
    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        var atIndex = input.IndexOf('@');

        if (atIndex <= 3)
        {
            return Mask.Length;
        }

        return 3 + Mask.Length + input.Length - atIndex;
    }


    /// <inheritdoc />
    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        var atIndex = source.IndexOf('@');


        if (atIndex <= 3)
        {
            Mask.AsSpan()
                .CopyTo(destination);

            return Mask.Length;
        }


        var index = 0;


        // Keep first 3 chars
        source[..3]
            .CopyTo(destination);

        index += 3;


        // Mask middle
        Mask.AsSpan()
            .CopyTo(destination[index..]);

        index += Mask.Length;


        // Keep domain
        source[atIndex..]
            .CopyTo(destination[index..]);

        index += source.Length - atIndex;


        return index;
    }
}