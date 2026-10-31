namespace Jnvp.Platform.Results;

public sealed record Error(
    string Code,
    string Message,
    ErrorType Type,
    IReadOnlyDictionary<string, string[]>? Errors = null)
{
    public static Error BadRequest(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.BadRequest, message, errors);

    public static Error Unauthorized(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Unauthorized, message, errors);

    public static Error Forbidden(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Forbidden, message, errors);

    public static Error NotFound(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.NotFound, message, errors);

    public static Error MethodNotAllowed(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.MethodNotAllowed, message, errors);

    public static Error NotAcceptable(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.NotAcceptable, message, errors);

    public static Error Conflict(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Conflict, message, errors);

    public static Error PayloadTooLarge(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.PayloadTooLarge, message, errors);

    public static Error UnsupportedMediaType(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.UnsupportedMediaType, message, errors);

    public static Error UnprocessableEntity(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.UnprocessableEntity, message, errors);

    public static Error TooManyRequests(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.TooManyRequests, message, errors);

    public static Error RequestTimeout(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.RequestTimeout, message, errors);

    public static Error InternalServerError(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.InternalServerError, message, errors);

    public static Error BadGateway(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.BadGateway, message, errors);

    public static Error ServiceUnavailable(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.ServiceUnavailable, message, errors);

    public static Error GatewayTimeout(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.GatewayTimeout, message, errors);

    public static Error Create(
        ErrorType type,
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => new(
            type.GetCode(),
            message,
            type,
            errors);

    public static Error Create(
        string code,
        string message,
        ErrorType type,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => new(
            code,
            message,
            type,
            errors);
}