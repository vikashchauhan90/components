
namespace Jnvp.Platform.Results;

/// <summary>Defines stable machine-readable codes for standard error categories.</summary>
/// <example>
/// <code language="csharp">
/// Error error = new(ErrorCodes.NotFound, "Item not found.", ErrorType.NotFound);
/// </code>
/// </example>
public static class ErrorCodes
{
    /// <summary>The code for a bad request.</summary>
    public const string BadRequest = "bad_request";

    /// <summary>The code for an authentication-required error.</summary>
    public const string Unauthorized = "unauthorized";

    /// <summary>The code for an access-denied error.</summary>
    public const string Forbidden = "forbidden";

    /// <summary>The code for a missing resource.</summary>
    public const string NotFound = "not_found";

    /// <summary>The code for an unsupported request method.</summary>
    public const string MethodNotAllowed = "method_not_allowed";

    /// <summary>The code for an unacceptable response format.</summary>
    public const string NotAcceptable = "not_acceptable";

    /// <summary>The code for a resource conflict.</summary>
    public const string Conflict = "conflict";

    /// <summary>The code for a payload that exceeds the size limit.</summary>
    public const string PayloadTooLarge = "payload_too_large";

    /// <summary>The code for an unsupported media type.</summary>
    public const string UnsupportedMediaType = "unsupported_media_type";

    /// <summary>The code for an unprocessable request.</summary>
    public const string UnprocessableEntity = "unprocessable_entity";

    /// <summary>The code for a rate-limit error.</summary>
    public const string TooManyRequests = "too_many_requests";
    /// <summary>The code for a cancelled request.</summary>
    public const string RequestCancelled = "request_cancelled";

    /// <summary>The code for a request timeout.</summary>
    public const string RequestTimeout = "request_timeout";

    /// <summary>The code for an unexpected server error.</summary>
    public const string InternalServerError = "internal_server_error";

    /// <summary>The code for an invalid upstream response.</summary>
    public const string BadGateway = "bad_gateway";

    /// <summary>The code for an unavailable service.</summary>
    public const string ServiceUnavailable = "service_unavailable";

    /// <summary>The code for an upstream timeout.</summary>
    public const string GatewayTimeout = "gateway_timeout";

    /// <summary>The code for a locked resource.</summary>
    public const string Locked = "locked";
    /// <summary>The code for a storage failure.</summary>
    public const string StorageError = "storage_error";
}