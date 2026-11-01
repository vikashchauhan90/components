namespace Jnvp.Platform.Observability.Compliance;

public sealed class EmailAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Email);

public sealed class PhoneAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Phone);

public sealed class IpAddressAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.IpAddress);

public sealed class PhysicalAddressAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.PhysicalAddress);

public sealed class DateOfBirthAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.DateOfBirth);

public sealed class GovernmentIdentifierAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.GovernmentIdentifier);

public sealed class UserIdentifierAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.UserIdentifier);

public sealed class NameAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Name);