namespace Jnvp.Platform.Observability.Compliance;

/// <summary>Defines category names used to group platform data classifications.</summary>
/// <example>
/// <code language="csharp">
/// var category = DataClassificationCategories.PersonalCategory;
/// </code>
/// </example>
public static class DataClassificationCategories
{
    /// <summary>The category for credentials and security-sensitive values.</summary>
    public const string SecurityCategory = "Security";
    /// <summary>The category for personal information.</summary>
    public const string PersonalCategory = "Personal";
    /// <summary>The category for payment and financial information.</summary>
    public const string FinancialCategory = "Financial";
}
