using Microsoft.Extensions.Compliance.Classification;

namespace Jnvp.Platform.Observability.Compliance;

public static class DataClassifications
{
    // Security

    public static readonly DataClassification Secret =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Secret);

    public static readonly DataClassification Password =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Password);

    public static readonly DataClassification AccessToken =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.AccessToken);

    public static readonly DataClassification RefreshToken =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.RefreshToken);

    public static readonly DataClassification ApiKey =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.ApiKey);

    public static readonly DataClassification ConnectionString =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.ConnectionString);

    public static readonly DataClassification Sensitive =
        new(
            DataClassificationCategories.SecurityCategory,
            DataClassificationCodes.Sensitive);


    // Personal

    public static readonly DataClassification Email =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Email);

    public static readonly DataClassification Phone =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Phone);

    public static readonly DataClassification IpAddress =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.IpAddress);

    public static readonly DataClassification PhysicalAddress =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.PhysicalAddress);

    public static readonly DataClassification DateOfBirth =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.DateOfBirth);

    public static readonly DataClassification GovernmentIdentifier =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.GovernmentIdentifier);

    public static readonly DataClassification UserIdentifier =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.UserIdentifier);

    public static readonly DataClassification Name =
        new(
            DataClassificationCategories.PersonalCategory,
            DataClassificationCodes.Name);


    // Financial

    public static readonly DataClassification CreditCard =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.CreditCard);

    public static readonly DataClassification BankAccount =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.BankAccount);

    public static readonly DataClassification PaymentInformation =
        new(
            DataClassificationCategories.FinancialCategory,
            DataClassificationCodes.PaymentInformation);
}