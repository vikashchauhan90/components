using System.Net;

namespace Jnvp.Platform.Results;

public static class ErrorTypeExtensions
{
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
            ErrorType.RequestTimeout => ErrorCodes.RequestTimeout,
            ErrorType.InternalServerError => ErrorCodes.InternalServerError,
            ErrorType.BadGateway => ErrorCodes.BadGateway,
            ErrorType.ServiceUnavailable => ErrorCodes.ServiceUnavailable,
            ErrorType.GatewayTimeout => ErrorCodes.GatewayTimeout,
            ErrorType.StorageError => ErrorCodes.StorageError,
            _ => string.Empty
        };

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
        ErrorType.RequestTimeout => ErrorMessages.RequestTimeout,
        ErrorType.InternalServerError => ErrorMessages.InternalServerError,
        ErrorType.BadGateway => ErrorMessages.BadGateway,
        ErrorType.ServiceUnavailable => ErrorMessages.ServiceUnavailable,
        ErrorType.GatewayTimeout => ErrorMessages.GatewayTimeout,
        ErrorType.StorageError => ErrorMessages.StorageError,
        _ => string.Empty
    };


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
         ErrorType.RequestTimeout => HttpStatusCode.RequestTimeout,
         ErrorType.InternalServerError => HttpStatusCode.InternalServerError,
         ErrorType.BadGateway => HttpStatusCode.BadGateway,
         ErrorType.ServiceUnavailable => HttpStatusCode.ServiceUnavailable,
         ErrorType.GatewayTimeout => HttpStatusCode.GatewayTimeout,
         ErrorType.StorageError => HttpStatusCode.InternalServerError,
         _ => null
     };

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
            ErrorType.InternalServerError => "https://httpstatuses.com/500",
            ErrorType.BadGateway => "https://httpstatuses.com/502",
            ErrorType.ServiceUnavailable => "https://httpstatuses.com/503",
            ErrorType.GatewayTimeout => "https://httpstatuses.com/504",
            ErrorType.StorageError => "https://httpstatuses.com/500",
            _ => "https://httpstatuses.com/500"
        };
    }
}