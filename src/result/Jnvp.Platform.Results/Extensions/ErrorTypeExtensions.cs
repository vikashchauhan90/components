using System.Net;

namespace Jnvp.Platform.Results;

/// <summary>Provides HTTP and display metadata for <see cref="ErrorType"/> values.</summary>
/// <example>
/// <code language="csharp">
/// var status = ErrorType.NotFound.ToHttpStatusCode();
/// var code = ErrorType.NotFound.GetCode();
/// </code>
/// </example>
public static class ErrorTypeExtensions
{
    /// <summary>Gets the stable machine-readable code for an error category.</summary>
    public static string GetCode(this ErrorType errorType) =>
        errorType switch
        {
            ErrorType.BadRequest => ErrorCodes.BadRequest,
            ErrorType.Unauthorized => ErrorCodes.Unauthorized,
            ErrorType.Forbidden => ErrorCodes.Forbidden,
            ErrorType.NotFound => ErrorCodes.NotFound,
            ErrorType.MethodNotAllowed => ErrorCodes.MethodNotAllowed,
            ErrorType.NotAcceptable => ErrorCodes.NotAcceptable,
            ErrorType.Conflict => ErrorCodes.Conflict,
            ErrorType.Locked => ErrorCodes.Locked,
            ErrorType.PayloadTooLarge => ErrorCodes.PayloadTooLarge,
            ErrorType.UnsupportedMediaType => ErrorCodes.UnsupportedMediaType,
            ErrorType.UnprocessableEntity => ErrorCodes.UnprocessableEntity,
            ErrorType.TooManyRequests => ErrorCodes.TooManyRequests,
            ErrorType.RequestCancelled => ErrorCodes.RequestCancelled,
            ErrorType.RequestTimeout => ErrorCodes.RequestTimeout,
            ErrorType.InternalServerError => ErrorCodes.InternalServerError,
            ErrorType.BadGateway => ErrorCodes.BadGateway,
            ErrorType.ServiceUnavailable => ErrorCodes.ServiceUnavailable,
            ErrorType.GatewayTimeout => ErrorCodes.GatewayTimeout,
            ErrorType.StorageError => ErrorCodes.StorageError,
            _ => string.Empty
        };

    /// <summary>Gets the default human-readable message for an error category.</summary>
    public static string GetDefaultMessage(this ErrorType errorType) =>
    errorType switch
    {
        ErrorType.BadRequest => ErrorMessages.BadRequest,
        ErrorType.Unauthorized => ErrorMessages.Unauthorized,
        ErrorType.Forbidden => ErrorMessages.Forbidden,
        ErrorType.NotFound => ErrorMessages.NotFound,
        ErrorType.MethodNotAllowed => ErrorMessages.MethodNotAllowed,
        ErrorType.NotAcceptable => ErrorMessages.NotAcceptable,
        ErrorType.Conflict => ErrorMessages.Conflict,
        ErrorType.Locked => ErrorMessages.Locked,
        ErrorType.PayloadTooLarge => ErrorMessages.PayloadTooLarge,
        ErrorType.UnsupportedMediaType => ErrorMessages.UnsupportedMediaType,
        ErrorType.UnprocessableEntity => ErrorMessages.UnprocessableEntity,
        ErrorType.TooManyRequests => ErrorMessages.TooManyRequests,
        ErrorType.RequestCancelled => ErrorMessages.RequestCancelled,
        ErrorType.RequestTimeout => ErrorMessages.RequestTimeout,
        ErrorType.InternalServerError => ErrorMessages.InternalServerError,
        ErrorType.BadGateway => ErrorMessages.BadGateway,
        ErrorType.ServiceUnavailable => ErrorMessages.ServiceUnavailable,
        ErrorType.GatewayTimeout => ErrorMessages.GatewayTimeout,
        ErrorType.StorageError => ErrorMessages.StorageError,
        _ => string.Empty
    };


    /// <summary>Maps an error category to its HTTP status code when one is defined.</summary>
    public static HttpStatusCode? ToHttpStatusCode(this ErrorType errorType) =>
     errorType switch
     {
         ErrorType.BadRequest => HttpStatusCode.BadRequest,
         ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
         ErrorType.Forbidden => HttpStatusCode.Forbidden,
         ErrorType.NotFound => HttpStatusCode.NotFound,
         ErrorType.MethodNotAllowed => HttpStatusCode.MethodNotAllowed,
         ErrorType.NotAcceptable => HttpStatusCode.NotAcceptable,
         ErrorType.Conflict => HttpStatusCode.Conflict,
         ErrorType.Locked => HttpStatusCode.Locked,
         ErrorType.PayloadTooLarge => HttpStatusCode.RequestEntityTooLarge,
         ErrorType.UnsupportedMediaType => HttpStatusCode.UnsupportedMediaType,
         ErrorType.UnprocessableEntity => HttpStatusCode.UnprocessableEntity,
         ErrorType.TooManyRequests => HttpStatusCode.TooManyRequests,
         ErrorType.RequestCancelled => HttpStatusCode.BadRequest,
         ErrorType.RequestTimeout => HttpStatusCode.RequestTimeout,
         ErrorType.InternalServerError => HttpStatusCode.InternalServerError,
         ErrorType.BadGateway => HttpStatusCode.BadGateway,
         ErrorType.ServiceUnavailable => HttpStatusCode.ServiceUnavailable,
         ErrorType.GatewayTimeout => HttpStatusCode.GatewayTimeout,
         ErrorType.StorageError => HttpStatusCode.InternalServerError,
         _ => null
     };

    /// <summary>Gets the canonical problem type URI for an error category.</summary>
    public static string GetProblemType(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.BadRequest => "https://httpstatuses.com/400",
            ErrorType.Unauthorized => "https://httpstatuses.com/401",
            ErrorType.Forbidden => "https://httpstatuses.com/403",
            ErrorType.NotFound => "https://httpstatuses.com/404",
            ErrorType.MethodNotAllowed => "https://httpstatuses.com/405",
            ErrorType.NotAcceptable => "https://httpstatuses.com/406",
            ErrorType.RequestTimeout => "https://httpstatuses.com/408",
            ErrorType.Conflict => "https://httpstatuses.com/409",
            ErrorType.PayloadTooLarge => "https://httpstatuses.com/413",
            ErrorType.UnsupportedMediaType => "https://httpstatuses.com/415",
            ErrorType.UnprocessableEntity => "https://httpstatuses.com/422",
            ErrorType.Locked => "https://httpstatuses.com/423",
            ErrorType.TooManyRequests => "https://httpstatuses.com/429",
            ErrorType.RequestCancelled => "https://httpstatuses.com/499",
            ErrorType.InternalServerError => "https://httpstatuses.com/500",
            ErrorType.BadGateway => "https://httpstatuses.com/502",
            ErrorType.ServiceUnavailable => "https://httpstatuses.com/503",
            ErrorType.GatewayTimeout => "https://httpstatuses.com/504",
            ErrorType.StorageError => "https://httpstatuses.com/500",
            _ => "https://httpstatuses.com/500"
        };
    }

    /// <summary>Gets a display title for an HTTP status code.</summary>
    public static string GetErrorTitle(int statusCode)
    {
        return statusCode switch
        {
            400 =>
                "Bad Request",

            401 =>
                "Unauthorized",

            403 =>
                "Forbidden",

            404 =>
                "Not Found",

            405 =>
                "Method Not Allowed",

            406 =>
                "Not Acceptable",

            408 =>
                "Request Timeout",

            409 =>
                "Conflict",

            410 =>
                "Gone",

            411 =>
                "Length Required",

            412 =>
                "Precondition Failed",

            413 =>
                "Payload Too Large",

            414 =>
                "URI Too Long",

            415 =>
                "Unsupported Media Type",

            416 =>
                "Range Not Satisfiable",

            417 =>
                "Expectation Failed",

            421 =>
                "Misdirected Request",

            422 =>
                "Validation Failed",

            423 =>
                "Locked",

            424 =>
                "Failed Dependency",

            428 =>
                "Precondition Required",

            429 =>
                "Too Many Requests",

            431 =>
                "Request Header Fields Too Large",
            499 =>
                "Client Closed Request",

            500 =>
                "Internal Server Error",

            501 =>
                "Not Implemented",

            502 =>
                "Bad Gateway",

            503 =>
                "Service Unavailable",

            504 =>
                "Gateway Timeout",

            505 =>
                "HTTP Version Not Supported",

            507 =>
                "Insufficient Storage",

            508 =>
                "Loop Detected",

            _ when statusCode >= 500 =>
                "Internal Server Error",

            _ =>
                "Request Failed"
        };
    }
}