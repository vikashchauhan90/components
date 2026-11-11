namespace Jnvp.Platform.Resilience;

/// <summary>Configures the default resilience policy used by <see cref="ResilienceExecutor"/>.</summary>
/// <remarks>
/// Values can be supplied through the <c>Resilience</c> configuration section
/// or by using the <see cref="ResilienceServiceCollectionExtensions.AddPlatformResilience(
/// Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action{ResilienceOptions})"/>
/// overload. Invalid values are rejected during options validation.
/// </remarks>
/// <example>
/// <code language="json">
/// {
///   "Resilience": {
///     "MaxRetryAttempts": 4,
///     "RetryDelay": "00:00:00.500",
///     "FailureRatio": 0.2,
///     "MinimumThroughput": 20,
///     "SamplingDuration": "00:00:30",
///     "BreakDuration": "00:00:10",
///     "Timeout": "00:00:08"
///   }
/// }
/// </code>
/// </example>
public sealed class ResilienceOptions
{
    /// <summary>Gets or sets the maximum number of retries after the initial operation.</summary>
    /// <value>The retry count; defaults to <c>3</c>. Must be zero or greater.</value>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>Gets or sets the initial delay between retries.</summary>
    /// <remarks>Exponential backoff and jitter are applied to this delay.</remarks>
    /// <value>The initial retry delay; defaults to one second. Must be non-negative.</value>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>Gets or sets the failure ratio that opens the circuit breaker.</summary>
    /// <value>A value greater than zero and at most one; defaults to <c>0.20</c>.</value>
    public double FailureRatio { get; set; } = 0.20;

    /// <summary>Gets or sets the minimum number of operations before evaluating the breaker.</summary>
    /// <value>The minimum throughput in a sampling window; defaults to <c>20</c>.</value>
    public int MinimumThroughput { get; set; } = 20;

    /// <summary>Gets or sets the circuit-breaker sampling window.</summary>
    /// <value>The sampling duration; defaults to 30 seconds. Must be positive.</value>
    public TimeSpan SamplingDuration { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>Gets or sets how long the circuit remains open before allowing a trial operation.</summary>
    /// <value>The open-circuit duration; defaults to 10 seconds. Must be positive.</value>
    public TimeSpan BreakDuration { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>Gets or sets the maximum duration of an individual operation attempt.</summary>
    /// <value>The per-attempt timeout; defaults to 10 seconds. Must be positive.</value>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
}
