using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

public sealed class BankAccountRedactor : Redactor
{
    private const char Mask = '*';

    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return input.Length;
    }

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
