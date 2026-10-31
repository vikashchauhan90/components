namespace Jnvp.Platform.Results;

public static class ErrorMessages
{
    public const string BadRequest = "The request is invalid.";

    public const string Unauthorized = "Authentication is required.";

    public const string Forbidden = "Access to the requested resource is forbidden.";

    public const string NotFound = "The requested resource was not found.";

    public const string MethodNotAllowed = "The requested method is not allowed.";

    public const string NotAcceptable = "The requested response format is not acceptable.";

    public const string Conflict = "The request conflicts with the current state of the resource.";

    public const string PayloadTooLarge = "The request payload is too large.";

    public const string UnsupportedMediaType = "The request media type is not supported.";

    public const string UnprocessableEntity = "The request could not be processed.";

    public const string TooManyRequests = "Too many requests have been sent.";

    public const string RequestCancelled = "The request was cancelled.";

    public const string RequestTimeout = "The request timed out.";

    public const string InternalServerError = "An internal server error occurred.";

    public const string BadGateway = "An invalid response was received from an upstream service.";

    public const string ServiceUnavailable = "The service is currently unavailable.";

    public const string GatewayTimeout = "The upstream service did not respond in time.";   

    public const string StorageError = "An error occurred while accessing the storage.";

    public const string Locked = "The resource is locked and cannot be modified.";
}