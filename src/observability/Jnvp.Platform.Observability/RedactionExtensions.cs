using Jnvp.Platform.Observability.Compliance;
using Jnvp.Platform.Observability.Compliance.Redactors;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jnvp.Platform.Observability;

public static class RedactionExtensions
{
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
            options.SetRedactor<HmacRedactor>(DataClassifications.Sensitive);
            options.SetRedactor<HmacRedactor>(DataClassifications.UserIdentifier);

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

