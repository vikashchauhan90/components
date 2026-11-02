namespace Jnvp.Platform.Observability.Compliance.Attributes;

/// <summary>Classifies an email address for partial masking.</summary>
/// <example><code language="csharp">public sealed class Contact { [Email] public string Address { get; init; } = ""; }</code></example>
public sealed class EmailAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Email);

/// <summary>Classifies a phone number for partial masking.</summary>
/// <example><code language="csharp">public sealed class Contact { [Phone] public string Number { get; init; } = ""; }</code></example>
public sealed class PhoneAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Phone);

/// <summary>Classifies an IP address for masking.</summary>
/// <example><code language="csharp">public sealed class RequestInfo { [IpAddress] public string ClientIp { get; init; } = ""; }</code></example>
public sealed class IpAddressAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.IpAddress);

/// <summary>Classifies a physical address for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Contact { [PhysicalAddress] public string Address { get; init; } = ""; }</code></example>
public sealed class PhysicalAddressAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.PhysicalAddress);

/// <summary>Classifies a date of birth for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Customer { [DateOfBirth] public DateOnly BirthDate { get; init; } }</code></example>
public sealed class DateOfBirthAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.DateOfBirth);

/// <summary>Classifies a government identifier for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Customer { [GovernmentIdentifier] public string Identifier { get; init; } = ""; }</code></example>
public sealed class GovernmentIdentifierAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.GovernmentIdentifier);

/// <summary>Classifies a user identifier for HMAC-based pseudonymization.</summary>
/// <example><code language="csharp">public sealed class AuditEvent { [UserIdentifier] public string UserId { get; init; } = ""; }</code></example>
public sealed class UserIdentifierAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.UserIdentifier);

/// <summary>Classifies a person's name for partial masking.</summary>
/// <example><code language="csharp">public sealed class Contact { [Name] public string FullName { get; init; } = ""; }</code></example>
public sealed class NameAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Name);