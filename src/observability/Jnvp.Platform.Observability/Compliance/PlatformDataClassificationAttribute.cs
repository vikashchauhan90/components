using Microsoft.Extensions.Compliance.Classification;

namespace Jnvp.Platform.Observability.Compliance;

[AttributeUsage(
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Field,
    AllowMultiple = false,
    Inherited = true)]
public abstract class PlatformDataClassificationAttribute
    : DataClassificationAttribute
{
    protected PlatformDataClassificationAttribute(
        DataClassification classification)
        : base(classification)
    {
    }
}