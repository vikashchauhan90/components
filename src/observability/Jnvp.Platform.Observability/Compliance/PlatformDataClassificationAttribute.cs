using Microsoft.Extensions.Compliance.Classification;

namespace Jnvp.Platform.Observability.Compliance;

/// <summary>Base class for attributes that classify a field, property, or parameter for redaction.</summary>
/// <example>
/// <code language="csharp">
/// public sealed class CustomerRecord
/// {
///     [Email]
///     public string EmailAddress { get; init; } = "";
/// }
/// </code>
/// </example>
[AttributeUsage(
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Field,
    AllowMultiple = false,
    Inherited = true)]
public abstract class PlatformDataClassificationAttribute
    : DataClassificationAttribute
{
    /// <summary>Initializes an attribute for the specified data classification.</summary>
    protected PlatformDataClassificationAttribute(
        DataClassification classification)
        : base(classification)
    {
    }
}