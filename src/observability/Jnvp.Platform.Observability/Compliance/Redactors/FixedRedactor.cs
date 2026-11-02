using Microsoft.Extensions.Compliance.Redaction;

namespace Jnvp.Platform.Observability.Compliance.Redactors;

/// <summary>Replaces the entire classified value with <c>[REDACTED]</c>.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
/// // For example, [Password] values are fully replaced.
/// </code>
/// </example>
public sealed class FixedRedactor : Redactor
{
    private const string Mask = "[REDACTED]";

    /// <inheritdoc />
    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        return Mask.Length;
    }

    /// <inheritdoc />
    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        Mask.AsSpan().CopyTo(destination);

        return Mask.Length;
    }
}