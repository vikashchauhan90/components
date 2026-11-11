namespace Jnvp.Platform.Resilience;

/// <summary>Executes asynchronous operations using the application's configured resilience policy.</summary>
/// <remarks>
/// Register this service with
/// <see cref="ResilienceServiceCollectionExtensions.AddPlatformResilience(
/// Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action{ResilienceOptions})"/>
/// and inject it into application services that need resilient execution.
/// </remarks>
/// <example>
/// <code language="csharp">
/// var product = await resilienceExecutor.ExecuteAsync(
///     token =&gt; productClient.GetAsync(productId, token),
///     cancellationToken);
/// </code>
/// </example>
public interface IResilienceExecutor
{
    /// <summary>Executes an asynchronous operation that has no result.</summary>
    /// <param name="operation">The operation to execute. The supplied token is the policy's execution token.</param>
    /// <param name="cancellationToken">A token that can cancel the operation and its retries.</param>
    /// <returns>A task that completes when the operation succeeds or fails permanently.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    ValueTask ExecuteAsync(
        Func<CancellationToken, ValueTask> operation,
        CancellationToken cancellationToken = default);

    /// <summary>Executes an asynchronous operation and returns its result.</summary>
    /// <typeparam name="T">The type of the operation result.</typeparam>
    /// <param name="operation">The operation to execute. The supplied token is the policy's execution token.</param>
    /// <param name="cancellationToken">A token that can cancel the operation and its retries.</param>
    /// <returns>A task containing the operation result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    ValueTask<T> ExecuteAsync<T>(
        Func<CancellationToken, ValueTask<T>> operation,
        CancellationToken cancellationToken = default);
}
