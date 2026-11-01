using Microsoft.Extensions.Options;

namespace Jnvp.Platform.Observability.Processors;

internal sealed class LogProcessorOptionValidator : IValidateOptions<LogProcessorOptions>
{

    public ValidateOptionsResult Validate(string? name, LogProcessorOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Prefix))
        {
            return ValidateOptionsResult.Fail(
                $"{nameof(LogProcessorOptions.Prefix)} cannot be null or whitespace.");
        }

        return ValidateOptionsResult.Success;

    }
}