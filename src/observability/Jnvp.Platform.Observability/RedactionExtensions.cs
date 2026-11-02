using Jnvp.Platform.Observability.Compliance;
using Jnvp.Platform.Observability.Compliance.Redactors;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jnvp.Platform.Observability;

/// <summary>Registers classification-aware redaction for platform security, personal, and financial data.</summary>
/// <example>
/// <code language="csharp">
/// builder.Logging.AddPlatformRedaction();
///
/// public sealed class LoginEvent
/// {
///     [Email]
///     public string EmailAddress { get; init; } = "";
///
///     [Password]
///     public string Password { get; init; } = "";
/// }
/// </code>
/// </example>
public static class RedactionExtensions
{
    /// <summary>Enables redaction and maps platform data classifications to built-in redactors.</summary>
    public static ILoggingBuilder AddPlatformRedaction(
        this ILoggingBuilder loggingBuilder,
        Action<HmacRedactorOptions>? configureHmac = null)
    {
        ArgumentNullException.ThrowIfNull(loggingBuilder);

        loggingBuilder.EnableRedaction();

        if (configureHmac is not null)
        {
            loggingBuilder.Services.Configure(configureHmac);
        }

        loggingBuilder.Services.AddRedaction(options =>
        {
            // Security
            options.SetRedactor<FixedRedactor>(DataClassifications.Secret);
            options.SetRedactor<FixedRedactor>(DataClassifications.Password);
            options.SetRedactor<FixedRedactor>(DataClassifications.AccessToken);
            options.SetRedactor<FixedRedactor>(DataClassifications.RefreshToken);
            options.SetRedactor<FixedRedactor>(DataClassifications.ApiKey);
            options.SetRedactor<FixedRedactor>(DataClassifications.ConnectionString);

            // Pseudonymized
            if (configureHmac is not null)
            {
                options.SetRedactor<HmacRedactor>(DataClassifications.Sensitive);
                options.SetRedactor<HmacRedactor>(DataClassifications.UserIdentifier);
            }
            else
            {
                options.SetRedactor<FixedRedactor>(DataClassifications.Sensitive);
                options.SetRedactor<FixedRedactor>(DataClassifications.UserIdentifier);
            }

            // Personal
            options.SetRedactor<EmailRedactor>(DataClassifications.Email);
            options.SetRedactor<PhoneRedactor>(DataClassifications.Phone);
            options.SetRedactor<NameRedactor>(DataClassifications.Name);
            options.SetRedactor<IpAddressRedactor>(DataClassifications.IpAddress);
            options.SetRedactor<FixedRedactor>(DataClassifications.PhysicalAddress);
            options.SetRedactor<FixedRedactor>(DataClassifications.DateOfBirth);
            options.SetRedactor<FixedRedactor>(DataClassifications.GovernmentIdentifier);

            // Financial
            options.SetRedactor<CreditCardRedactor>(DataClassifications.CreditCard);
            options.SetRedactor<BankAccountRedactor>(DataClassifications.BankAccount);
            options.SetRedactor<FixedRedactor>(DataClassifications.PaymentInformation);

            // Safety net
            options.SetFallbackRedactor<FixedRedactor>();
        });

        return loggingBuilder;
    }
}
