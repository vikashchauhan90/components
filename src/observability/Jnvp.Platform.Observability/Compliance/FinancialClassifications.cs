namespace Jnvp.Platform.Observability.Compliance;

public sealed class CreditCardAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.CreditCard);

public sealed class BankAccountAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.BankAccount);

public sealed class PaymentInformationAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.PaymentInformation);