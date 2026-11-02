using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Masks an IPv4 address's final two octets; other address forms are replaced with a fixed mask.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // Mark the model property with [IpAddress] to apply this redactor.
/// </code>
/// </example>
public sealed class IpAddressRedactor : Redactor
{
    private const string Mask = "****";

    /// <inheritdoc />
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

    /// <inheritdoc />
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
