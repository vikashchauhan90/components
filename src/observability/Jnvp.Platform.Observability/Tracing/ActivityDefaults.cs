using System.Diagnostics;

namespace Jnvp.Platform.Observability.Tracing;

public static class ActivityDefaults
{
    public static void EnsureW3C()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
    }
}
