using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Jnvp.Platform.Observability.Processors;

public sealed class LogProcessor : BaseProcessor<LogRecord>
{
    private const char PrefixSeparator = '.';
    private const string OriginalFormatKey = "{OriginalFormat}";

    private readonly LogProcessorOptions _options;

    public LogProcessor(IOptions<LogProcessorOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
    }

    public override void OnEnd(LogRecord logRecord)
    {
        ArgumentNullException.ThrowIfNull(logRecord);

        var attributes = logRecord.Attributes;

        if (attributes is null || attributes.Count == 0)
        {
            return;
        }

        var prefix = _options.Prefix;

        if (string.IsNullOrWhiteSpace(prefix))
        {
            return;
        }

        var updated = new List<KeyValuePair<string, object?>>(
            attributes.Count);

        foreach (var attribute in attributes)
        {
            updated.Add(
                new KeyValuePair<string, object?>(
                    GetKeyWithPrefix(attribute.Key, prefix),
                    attribute.Value));
        }

        logRecord.Attributes = updated;
    }

    private static string GetKeyWithPrefix(string key, string prefix)
    {
        if (string.Equals(
                key,
                OriginalFormatKey,
                StringComparison.Ordinal))
        {
            return key;
        }

        return $"{prefix}{PrefixSeparator}{key}";
    }
}