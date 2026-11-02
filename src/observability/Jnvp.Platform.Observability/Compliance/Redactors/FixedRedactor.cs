using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

public sealed class FixedRedactor : Redactor
{
    private const string Mask = "[REDACTED]";

    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return Mask.Length;
    }

    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        Mask.AsSpan().CopyTo(destination);

        return Mask.Length;
    }
}