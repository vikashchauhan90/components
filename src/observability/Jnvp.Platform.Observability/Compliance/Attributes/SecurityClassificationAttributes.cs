namespace Jnvp.Platform.Observability.Compliance.Attributes;

/// <summary>Classifies a value as a secret for redaction.</summary>
/// <example><code language="csharp">public sealed class Credentials { [Secret] public string Value { get; init; } = ""; }</code></example>
public sealed class SecretAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Secret);

/// <summary>Classifies a value as a password for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Credentials { [Password] public string Password { get; init; } = ""; }</code></example>
public sealed class PasswordAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Password);

/// <summary>Classifies a value as an access token for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Tokens { [AccessToken] public string Value { get; init; } = ""; }</code></example>
public sealed class AccessTokenAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.AccessToken);

/// <summary>Classifies a value as a refresh token for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Tokens { [RefreshToken] public string Value { get; init; } = ""; }</code></example>
public sealed class RefreshTokenAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.RefreshToken);

/// <summary>Classifies a value as an API key for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class Integration { [ApiKey] public string Key { get; init; } = ""; }</code></example>
public sealed class ApiKeyAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.ApiKey);

/// <summary>Classifies a value as a connection string for fixed redaction.</summary>
/// <example><code language="csharp">public sealed class DatabaseOptions { [ConnectionString] public string Value { get; init; } = ""; }</code></example>
public sealed class ConnectionStringAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.ConnectionString);

/// <summary>Classifies a value as sensitive information for HMAC-based pseudonymization.</summary>
/// <example><code language="csharp">public sealed class Event { [Sensitive] public string Value { get; init; } = ""; }</code></example>
public sealed class SensitiveAttribute()
    : PlatformDataClassificationAttribute(DataClassifications.Sensitive);