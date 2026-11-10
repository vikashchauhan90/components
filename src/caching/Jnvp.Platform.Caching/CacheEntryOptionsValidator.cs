using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Caching;

/// <summary>
/// Validates <see cref="CacheEntryOptions"/> instances.
/// </summary>
public sealed class CacheEntryOptionsValidator :
    IValidateOptions<CacheEntryOptions>
{

    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, CacheEntryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.AbsoluteExpirationRelativeToNow is { } absoluteExpiration
            && absoluteExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                $"{nameof(CacheEntryOptions.AbsoluteExpirationRelativeToNow)} must be greater than zero.");
        }

        if (options.SlidingExpiration is { } slidingExpiration
            && slidingExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                $"{nameof(CacheEntryOptions.SlidingExpiration)} must be greater than zero.");
        }

        if (options.Tags is not null && options.Tags.Any(string.IsNullOrWhiteSpace))
        {
            return ValidateOptionsResult.Fail(
                "Cache tags cannot be null, empty, or whitespace.");
        }

        return ValidateOptionsResult.Success;
    }
}
