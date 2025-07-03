// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// The DebounceTask dispatcher delays the invocation of an action until a predetermined interval has elapsed since the last call.
/// This ensures that the action is only invoked once after the calls have stopped for the specified duration.
/// </summary>
/// <example>
/// <code>
/// Debounce Debounce = new Debounce();
/// 
/// private async Task OnTextInput(ChangeEventArgs e)
/// {
///     await Debounce.RunAsync(400, async ct =>
///     {
///         InputValue = e.Value?.ToString() ?? string.Empty;
///
///         // Long processing task
///         // ...
///         
///         if (ct.IsCancellationRequested)
///         {
///             // ...
///         }
///     });
///
///     // The action has been completed successfully
///     if (Debounce.IsCompleted)
///     {
///         // ...
///     }
/// }
/// </code>
/// </example>
public class Debounce : IDisposable
{
    private bool _isCompleted;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _disposedValue;

    /// <summary>
    /// Debounce the execution of asynchronous tasks.
    /// Ensures that a function is invoked only once within a specified interval, even if multiple invocations are requested.
    /// </summary>
    /// <remarks>
    /// This implementation will swallow any exceptions that is thrown by the invoked task.
    /// </remarks>
    /// <param name="milliseconds">The interval in milliseconds to wait before invoking the action.</param>
    /// <param name="action">The function that returns a Task to be invoked asynchronously.</param>
    /// <param name="cancellationToken">An optional CancellationToken.</param>
    /// <returns>A Task representing the asynchronous operation with minimal delay.</returns>
    public Task RunAsync(int milliseconds, Func<Task> action, CancellationToken cancellationToken = default)
    {
        return RunAsync(milliseconds, async ct =>
        {
            await action.Invoke();
        }, cancellationToken);
    }

    /// <summary>
    /// Debounce the execution of asynchronous tasks.
    /// Ensures that a function is invoked only once within a specified interval, even if multiple invocations are requested.
    /// </summary>
    /// <remarks>
    /// This implementation will swallow any exceptions that is thrown by the invoked task.
    /// </remarks>
    /// <param name="milliseconds">The interval in milliseconds to wait before invoking the action.</param>
    /// <param name="action">The function that returns a Task to be invoked asynchronously.</param>
    /// <param name="cancellationToken">An optional CancellationToken.</param>
    /// <returns>A Task representing the asynchronous operation with minimal delay.</returns>
    public async Task RunAsync(int milliseconds, Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        // Cancel the previous debounce task if it exists
        _cancellationTokenSource?.CancelAsync();
        _isCompleted = false;

        // If the object has been disposed, do not proceed
        if (_disposedValue)
        {
            return;
        }

        // Create a new cancellation token source linked with provided token
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cancellationToken = _cancellationTokenSource.Token;

        try
        {
            await Task.Delay(milliseconds, cancellationToken);
            await action.Invoke(cancellationToken);
            _isCompleted = true;
        }
        catch (TaskCanceledException)
        {
            // If the task was canceled, ignore it
        }
    }

    /// <summary>
    /// Cancel the current debounced task.
    /// </summary>
    public void Cancel() => _cancellationTokenSource?.Cancel();

    /// <summary>
    /// Gets a value indicating whether the action has been completed.
    /// </summary>
    public bool IsCompleted => _isCompleted;

    /// <summary>
    /// Cancel the current debounced task (if running) and releases the Debounce object.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Cancel();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Cancel the current debounced task (if running) and releases the Debounce object.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
