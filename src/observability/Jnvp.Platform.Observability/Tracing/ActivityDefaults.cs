using System.Diagnostics;

namespace Jnvp.Platform.Observability.Tracing;

/// <summary>Provides helpers for configuring standard W3C activity identifiers.</summary>
/// <example>
/// <code language="csharp">
/// ActivityDefaults.EnsureW3C();
/// </code>
/// </example>
public static class ActivityDefaults
{
    /// <summary>Sets W3C as the default activity ID format and prevents fallback to another format.</summary>
    public static void EnsureW3C()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
    }
}
