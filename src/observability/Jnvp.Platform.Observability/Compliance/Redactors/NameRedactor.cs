using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Partially masks each name component while retaining up to its first two characters.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // Mark the model property with [Name] to apply this redactor.
/// </code>
/// </example>
public sealed class NameRedactor : Redactor
{
    private const string Mask = "***";

    /// <inheritdoc />
    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        var value = input.ToString();
        var parts = value.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        var length = 0;

        foreach (var part in parts)
        {
            length += Math.Min(2, part.Length);
            length += Mask.Length;
        }

        return length + Math.Max(0, parts.Length - 1);
    }

    /// <inheritdoc />
    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        var value = source.ToString();
        var parts = value.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        var index = 0;

        for (var i = 0; i < parts.Length; i++)
        {
            if (i > 0)
            {
                destination[index++] = ' ';
            }

            var visibleLength = Math.Min(2, parts[i].Length);

            parts[i]
                .AsSpan(0, visibleLength)
                .CopyTo(destination[index..]);

            index += visibleLength;

            Mask.AsSpan()
                .CopyTo(destination[index..]);

            index += Mask.Length;
        }

        return index;
    }
}