namespace Jnvp.Platform.Observability.Processors;

/// <summary>Options controlling the prefix applied to OpenTelemetry log attributes.</summary>
/// <example>
/// <code language="csharp">
/// services.AddLogProcessorService(configuration.GetSection("Logging:Processor"));
/// // appsettings.json: { "Logging": { "Processor": { "Prefix": "service" } } }
/// </code>
/// </example>
public sealed class LogProcessorOptions
{
    /// <summary>Gets or sets the non-empty prefix prepended to log attribute keys.</summary>
    public string? Prefix { get; set; }
}