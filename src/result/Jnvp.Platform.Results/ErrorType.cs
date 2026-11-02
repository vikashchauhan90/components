namespace Jnvp.Platform.Results;

/// <summary>Identifies the category of a result error.</summary>
/// <example>
/// <code language="csharp">
/// HttpStatusCode? status = ErrorType.NotFound.ToHttpStatusCode();
/// </code>
/// </example>
public enum ErrorType
{
    /// <summary>No error category was specified.</summary>
    None = 0,
    /// <summary>The request is invalid.</summary>
    BadRequest,
    /// <summary>Authentication is required.</summary>
    Unauthorized,
    /// <summary>The caller is not allowed to perform the operation.</summary>
    Forbidden,
    /// <summary>The requested resource does not exist.</summary>
    NotFound,
    /// <summary>The request method is not supported.</summary>
    MethodNotAllowed,
    /// <summary>The requested response format is not supported.</summary>
    NotAcceptable,
    /// <summary>The request conflicts with the current resource state.</summary>
    Conflict,
    /// <summary>The resource is locked.</summary>
    Locked,
    /// <summary>The request payload exceeds the allowed size.</summary>
    PayloadTooLarge,
    /// <summary>The request media type is not supported.</summary>
    UnsupportedMediaType,
    /// <summary>The request is valid syntactically but cannot be processed.</summary>
    UnprocessableEntity,
    /// <summary>The request rate limit was exceeded.</summary>
    TooManyRequests,
    /// <summary>The request was cancelled.</summary>
    RequestCancelled,
    /// <summary>The request exceeded its allowed processing time.</summary>
    RequestTimeout,
    /// <summary>An unexpected server-side failure occurred.</summary>
    InternalServerError,
    /// <summary>An invalid response was received from an upstream service.</summary>
    BadGateway,
    /// <summary>The service is temporarily unavailable.</summary>
    ServiceUnavailable,
    /// <summary>An upstream service did not respond in time.</summary>
    GatewayTimeout,
    /// <summary>A storage operation failed.</summary>
    StorageError
}