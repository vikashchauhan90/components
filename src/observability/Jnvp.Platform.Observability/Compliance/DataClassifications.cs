using Microsoft.Extensions.Compliance.Classification;

namespace Jnvp.Platform.Observability.Compliance;

/// <summary>Provides reusable <see cref="DataClassification"/> values for platform data categories.</summary>
/// <example>
/// <code language="csharp">
/// options.SetRedactor&lt;FixedRedactor&gt;(DataClassifications.Secret);
/// </code>
/// </example>
public static class DataClassifications
{
    // Security

    /// <summary>The classification for secret values.</summary>
    public static readonly DataClassification Secret =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Secret);

    /// <summary>The classification for passwords.</summary>
    public static readonly DataClassification Password =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Password);

    /// <summary>The classification for access tokens.</summary>
    public static readonly DataClassification AccessToken =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.AccessToken);

    /// <summary>The classification for refresh tokens.</summary>
    public static readonly DataClassification RefreshToken =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.RefreshToken);

    /// <summary>The classification for API keys.</summary>
    public static readonly DataClassification ApiKey =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.ApiKey);

    /// <summary>The classification for connection strings.</summary>
    public static readonly DataClassification ConnectionString =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.ConnectionString);

    /// <summary>The classification for sensitive values that use HMAC redaction.</summary>
    public static readonly DataClassification Sensitive =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Sensitive);


    // Personal

    /// <summary>The classification for email addresses.</summary>
    public static readonly DataClassification Email =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Email);

    /// <summary>The classification for phone numbers.</summary>
    public static readonly DataClassification Phone =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Phone);

    /// <summary>The classification for IP addresses.</summary>
    public static readonly DataClassification IpAddress =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.IpAddress);

    /// <summary>The classification for physical addresses.</summary>
    public static readonly DataClassification PhysicalAddress =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.PhysicalAddress);

    /// <summary>The classification for dates of birth.</summary>
    public static readonly DataClassification DateOfBirth =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.DateOfBirth);

    /// <summary>The classification for government identifiers.</summary>
    public static readonly DataClassification GovernmentIdentifier =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.GovernmentIdentifier);

    /// <summary>The classification for user identifiers that use HMAC redaction.</summary>
    public static readonly DataClassification UserIdentifier =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.UserIdentifier);

    /// <summary>The classification for a person's name.</summary>
    public static readonly DataClassification Name =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Name);


    // Financial

    /// <summary>The classification for credit-card numbers.</summary>
    public static readonly DataClassification CreditCard =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.CreditCard);

    /// <summary>The classification for bank-account numbers.</summary>
    public static readonly DataClassification BankAccount =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.BankAccount);

    /// <summary>The classification for other payment information.</summary>
    public static readonly DataClassification PaymentInformation =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.PaymentInformation);
}