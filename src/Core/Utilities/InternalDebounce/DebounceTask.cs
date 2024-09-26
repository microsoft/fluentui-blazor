// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;

/// <summary>
/// The DebounceTask dispatcher delays the invocation of an action until a predetermined interval has elapsed since the last call.
/// This ensures that the action is only invoked once after the calls have stopped for the specified duration.
/// </summary>
[Obsolete("Use Debounce, which inherits from DebounceAction.")]
internal class DebounceTask : IDisposable
{
#if NET9_0_OR_GREATER
    private readonly System.Threading.Lock _syncRoot = new();
#else
    private readonly object _syncRoot = new();
#endif

    private bool _disposed;
    private Task? _task;
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Gets a value indicating whether the DebounceTask dispatcher is busy.
    /// </summary>
    public bool Busy => _task?.Status == TaskStatus.Running && !_disposed;

    /// <summary>
    /// Gets the current task.
    /// </summary>
    public Task CurrentTask => _task ?? Task.CompletedTask;

    /// <summary>
    /// Delays the invocation of an action until a predetermined interval has elapsed since the last call.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <param name="action"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Run(int milliseconds, Func<Task> action)
    {
        // Check arguments
        if (milliseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "The milliseconds must be greater than to zero.");
        }

        ArgumentNullException.ThrowIfNull(action);

        // Cancel the previous task if it's still running
        _cts?.Cancel();

        // Create a new cancellation token source
        _cts = new CancellationTokenSource();

        try
        {
            // Wait for the specified time
            _task = Task.Delay(TimeSpan.FromMilliseconds(milliseconds), _cts.Token)
                        .ContinueWith(t =>
                        {
                            if (!_disposed && !_cts.IsCancellationRequested)
                            {
                                lock (_syncRoot)
                                {
                                    _ = action.Invoke();
                                }
                            }
                        }, _cts.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Default);
        }
        catch (TaskCanceledException)
        {
            // Task was canceled
        }
    }

    /// <summary>
    /// Delays the invocation of an action until a predetermined interval has elapsed since the last call.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <param name="action"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Task RunAsync(int milliseconds, Func<Task> action)
    {
        Run(milliseconds, action);
        return CurrentTask;
    }

    /// <summary>
    /// Releases all resources used by the DebounceTask dispatcher.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        _cts?.Cancel();
        GC.SuppressFinalize(this);
    }
}
