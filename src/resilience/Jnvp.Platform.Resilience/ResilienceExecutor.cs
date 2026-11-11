using Polly.Registry;

namespace Jnvp.Platform.Resilience;

/// <summary>
/// Executes operations through the default resilience pipeline registered by
/// <see cref="ResilienceServiceCollectionExtensions.AddPlatformResilience(
/// Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action{ResilienceOptions})"/>.
/// </summary>
/// <param name="pipelineProvider">The provider used to resolve the internal default pipeline.</param>
internal sealed class ResilienceExecutor(
    ResiliencePipelineProvider<string> pipelineProvider) : IResilienceExecutor
{
    /// <inheritdoc />
    public ValueTask ExecuteAsync(
        Func<CancellationToken, ValueTask> operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return pipelineProvider
            .GetPipeline(ResiliencePolicies.Default)
            .ExecuteAsync(operation, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<T> ExecuteAsync<T>(
        Func<CancellationToken, ValueTask<T>> operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return pipelineProvider
            .GetPipeline(ResiliencePolicies.Default)
            .ExecuteAsync(operation, cancellationToken);
    }
}
