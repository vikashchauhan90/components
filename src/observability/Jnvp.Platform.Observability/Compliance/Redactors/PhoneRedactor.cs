using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

public sealed class PhoneRedactor : Redactor
{
    private const string Mask = "******";

    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return input.Length <= 4
            ? Mask.Length
            : 2 + Mask.Length + 2;
    }

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