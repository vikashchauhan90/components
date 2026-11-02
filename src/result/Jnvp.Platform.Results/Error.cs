namespace Jnvp.Platform.Results;

/// <summary>
/// Describes an operation failure using a stable code, message, category, and optional validation details.
/// </summary>
/// <example>
/// <code language="csharp">
/// Error error = Error.BadRequest(
///     "One or more fields are invalid.",
///     new Dictionary&lt;string, string[]&gt;
///     {
///         ["email"] = ["A valid email address is required."]
///     });
/// Result&lt;int&gt; result = Result&lt;int&gt;.Failure(error);
/// </code>
/// </example>
public sealed record Error(
    string Code,
    string Message,
    ErrorType Type,
    IReadOnlyDictionary<string, string[]>? Errors = null)
{
    /// <summary>Creates a bad-request error.</summary>
    public static Error BadRequest(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.BadRequest, message, errors);

    /// <summary>Creates an authentication-required error.</summary>
    public static Error Unauthorized(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Unauthorized, message, errors);

    /// <summary>Creates an access-denied error.</summary>
    public static Error Forbidden(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Forbidden, message, errors);

    /// <summary>Creates a resource-not-found error.</summary>
    public static Error NotFound(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.NotFound, message, errors);

    /// <summary>Creates an unsupported-method error.</summary>
    public static Error MethodNotAllowed(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.MethodNotAllowed, message, errors);

    /// <summary>Creates a not-acceptable-response error.</summary>
    public static Error NotAcceptable(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.NotAcceptable, message, errors);

    /// <summary>Creates a conflict error.</summary>
    public static Error Conflict(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.Conflict, message, errors);

    /// <summary>Creates a payload-too-large error.</summary>
    public static Error PayloadTooLarge(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.PayloadTooLarge, message, errors);

    /// <summary>Creates an unsupported-media-type error.</summary>
    public static Error UnsupportedMediaType(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.UnsupportedMediaType, message, errors);

    /// <summary>Creates an unprocessable-entity error.</summary>
    public static Error UnprocessableEntity(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.UnprocessableEntity, message, errors);

    /// <summary>Creates a too-many-requests error.</summary>
    public static Error TooManyRequests(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.TooManyRequests, message, errors);

    /// <summary>Creates a request-timeout error.</summary>
    public static Error RequestTimeout(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.RequestTimeout, message, errors);

    /// <summary>Creates an internal-server-error.</summary>
    public static Error InternalServerError(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.InternalServerError, message, errors);

    /// <summary>Creates a bad-gateway error.</summary>
    public static Error BadGateway(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.BadGateway, message, errors);

    /// <summary>Creates a service-unavailable error.</summary>
    public static Error ServiceUnavailable(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.ServiceUnavailable, message, errors);

    /// <summary>Creates a gateway-timeout error.</summary>
    public static Error GatewayTimeout(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => Create(ErrorType.GatewayTimeout, message, errors);

    /// <summary>Creates an error with the standard code for the specified category.</summary>
    public static Error Create(
        ErrorType type,
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
        => new(
            type.GetCode(),
            message,
            type,
            errors);

    /// <summary>Creates an error with a caller-provided code and category.</summary>
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