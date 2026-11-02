namespace Jnvp.Platform.Results;

/// <summary>Defines default human-readable messages for standard error categories.</summary>
/// <example>
/// <code language="csharp">
/// Error error = Error.BadRequest(ErrorMessages.BadRequest);
/// </code>
/// </example>
public static class ErrorMessages
{
    /// <summary>The default message for a bad request.</summary>
    public const string BadRequest = "The request is invalid.";

    /// <summary>The default message for an authentication-required error.</summary>
    public const string Unauthorized = "Authentication is required.";

    /// <summary>The default message for an access-denied error.</summary>
    public const string Forbidden = "Access to the requested resource is forbidden.";

    /// <summary>The default message for a missing resource.</summary>
    public const string NotFound = "The requested resource was not found.";

    /// <summary>The default message for an unsupported request method.</summary>
    public const string MethodNotAllowed = "The requested method is not allowed.";

    /// <summary>The default message for an unacceptable response format.</summary>
    public const string NotAcceptable = "The requested response format is not acceptable.";

    /// <summary>The default message for a resource conflict.</summary>
    public const string Conflict = "The request conflicts with the current state of the resource.";

    /// <summary>The default message for a payload that exceeds the size limit.</summary>
    public const string PayloadTooLarge = "The request payload is too large.";

    /// <summary>The default message for an unsupported media type.</summary>
    public const string UnsupportedMediaType = "The request media type is not supported.";

    /// <summary>The default message for an unprocessable request.</summary>
    public const string UnprocessableEntity = "The request could not be processed.";

    /// <summary>The default message for a rate-limit error.</summary>
    public const string TooManyRequests = "Too many requests have been sent.";

    /// <summary>The default message for a cancelled request.</summary>
    public const string RequestCancelled = "The request was cancelled.";

    /// <summary>The default message for a request timeout.</summary>
    public const string RequestTimeout = "The request timed out.";

    /// <summary>The default message for an unexpected server error.</summary>
    public const string InternalServerError = "An internal server error occurred.";

    /// <summary>The default message for an invalid upstream response.</summary>
    public const string BadGateway = "An invalid response was received from an upstream service.";

    /// <summary>The default message for an unavailable service.</summary>
    public const string ServiceUnavailable = "The service is currently unavailable.";

    /// <summary>The default message for an upstream timeout.</summary>
    public const string GatewayTimeout = "The upstream service did not respond in time.";   

    /// <summary>The default message for a storage failure.</summary>
    public const string StorageError = "An error occurred while accessing the storage.";

    /// <summary>The default message for a locked resource.</summary>
    public const string Locked = "The resource is locked and cannot be modified.";
}