namespace Jnvp.Platform.Observability.Compliance;

/// <summary>Defines the classification codes used by platform data classifications.</summary>
/// <example>
/// <code language="csharp">
/// var code = DataClassificationCodes.Email;
/// </code>
/// </example>
public static class DataClassificationCodes
{
    // Security
    /// <summary>The code for a secret.</summary>
    public const string Secret = "Secret";
    /// <summary>The code for a password.</summary>
    public const string Password = "Password";
    /// <summary>The code for an access token.</summary>
    public const string AccessToken = "AccessToken";
    /// <summary>The code for a refresh token.</summary>
    public const string RefreshToken = "RefreshToken";
    /// <summary>The code for an API key.</summary>
    public const string ApiKey = "ApiKey";
    /// <summary>The code for a connection string.</summary>
    public const string ConnectionString = "ConnectionString";
    /// <summary>The code for sensitive information.</summary>
    public const string Sensitive = "Sensitive";


    // Personal
    /// <summary>The code for an email address.</summary>
    public const string Email = "Email";
    /// <summary>The code for a phone number.</summary>
    public const string Phone = "Phone";
    /// <summary>The code for an IP address.</summary>
    public const string IpAddress = "IpAddress";
    /// <summary>The code for a physical address.</summary>
    public const string PhysicalAddress = "PhysicalAddress";
    /// <summary>The code for a date of birth.</summary>
    public const string DateOfBirth = "DateOfBirth";
    /// <summary>The code for a government identifier.</summary>
    public const string GovernmentIdentifier = "GovernmentIdentifier";
    /// <summary>The code for a user identifier.</summary>
    public const string UserIdentifier = "UserIdentifier";
    /// <summary>The code for a person's name.</summary>
    public const string Name = "Name";


    // Financial
    /// <summary>The code for a credit-card number.</summary>
    public const string CreditCard = "CreditCard";
    /// <summary>The code for a bank-account number.</summary>
    public const string BankAccount = "BankAccount";
    /// <summary>The code for payment information.</summary>
    public const string PaymentInformation = "PaymentInformation";
}