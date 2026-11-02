namespace Jnvp.Platform.Observability.Compliance.Attributes;

/// <summary>Classifies a credit-card number for masking that preserves at most its final four characters.</summary>
/// <example><code language="csharp">public sealed class Payment { [CreditCard] public string CardNumber { get; init; } = ""; }</code></example>
public sealed class CreditCardAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.CreditCard);

/// <summary>Classifies a bank-account number for masking that preserves at most its final four characters.</summary>
/// <example><code language="csharp">public sealed class Payment { [BankAccount] public string AccountNumber { get; init; } = ""; }</code></example>
public sealed class BankAccountAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.BankAccount);

/// <summary>Classifies payment information for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Payment { [PaymentInformation] public string Details { get; init; } = ""; }</code></example>
public sealed class PaymentInformationAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.PaymentInformation);