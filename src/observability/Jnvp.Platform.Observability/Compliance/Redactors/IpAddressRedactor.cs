using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

public sealed class IpAddressRedactor : Redactor
{
    private const string Mask = "****";

    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        var value = input.ToString();

        if (value.Contains('.'))
        {
            var parts = value.Split('.');

            if (parts.Length == 4)
            {
                return parts[0].Length +
                       parts[1].Length +
                       6;
            }
        }

        return value.IndexOf(':') switch
        {
            > 0 => value[..value.IndexOf(':')].Length + 1 + Mask.Length,
            _ => Mask.Length
        };
    }

    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        var value = source.ToString();

        if (value.Contains('.'))
        {
            var parts = value.Split('.');

            if (parts.Length == 4)
            {
                var redacted =
                    $"{parts[0]}.{parts[1]}.*.*";

                redacted.AsSpan().CopyTo(destination);

                return redacted.Length;
            }
        }

        var masked = Mask;

        masked.AsSpan().CopyTo(destination);

        return masked.Length;
    }
}
