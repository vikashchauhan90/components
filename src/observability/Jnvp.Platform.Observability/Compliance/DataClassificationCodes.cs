namespace Jnvp.Platform.Observability.Compliance;

public static class DataClassificationCodes
{
    // Security
    public const string Secret = "Secret";
    public const string Password = "Password";
    public const string AccessToken = "AccessToken";
    public const string RefreshToken = "RefreshToken";
    public const string ApiKey = "ApiKey";
    public const string ConnectionString = "ConnectionString";
    public const string Sensitive = "Sensitive";


    // Personal
    public const string Email = "Email";
    public const string Phone = "Phone";
    public const string IpAddress = "IpAddress";
    public const string PhysicalAddress = "PhysicalAddress";
    public const string DateOfBirth = "DateOfBirth";
    public const string GovernmentIdentifier = "GovernmentIdentifier";
    public const string UserIdentifier = "UserIdentifier";
    public const string Name = "Name";


    // Financial
    public const string CreditCard = "CreditCard";
    public const string BankAccount = "BankAccount";
    public const string PaymentInformation = "PaymentInformation";
}