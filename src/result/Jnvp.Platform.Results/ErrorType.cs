namespace Jnvp.Platform.Results;

public enum ErrorType
{
    None = 0,
    BadRequest,
    Unauthorized,
    Forbidden,
    NotFound,
    MethodNotAllowed,
    NotAcceptable,
    Conflict,
    Locked,
    PayloadTooLarge,
    UnsupportedMediaType,
    UnprocessableEntity,
    TooManyRequests,
    RequestCancelled,
    RequestTimeout,
    InternalServerError,
    BadGateway,
    ServiceUnavailable,
    GatewayTimeout,
    StorageError
}