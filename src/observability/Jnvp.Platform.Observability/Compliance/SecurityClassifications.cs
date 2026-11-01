namespace Jnvp.Platform.Observability.Compliance;

public sealed class SecretAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Secret);

public sealed class PasswordAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Password);

public sealed class AccessTokenAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.AccessToken);

public sealed class RefreshTokenAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.RefreshToken);

public sealed class ApiKeyAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.ApiKey);

public sealed class ConnectionStringAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.ConnectionString);

public sealed class SensitiveAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Sensitive);