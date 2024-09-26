// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;

/// <summary>
/// The DebounceTask dispatcher delays the invocation of an action until a predetermined interval has elapsed since the last call.
/// This ensures that the action is only invoked once after the calls have stopped for the specified duration.
/// </summary>
public class DebounceAction : IDisposable
{
    private bool _disposed;
    private readonly System.Timers.Timer _timer = new();
    private TaskCompletionSource? _taskCompletionSource;

    /// <summary>
    /// Gets a value indicating whether the DebounceTask dispatcher is busy.
    /// </summary>
    public bool Busy => _taskCompletionSource?.Task.Status == TaskStatus.Running && !_disposed;

    /// <summary>
    /// Gets the current task.
    /// </summary>
    public Task CurrentTask => _taskCompletionSource?.Task ?? Task.CompletedTask;

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

        // DebounceTask
        if (!_disposed)
        {
            _taskCompletionSource = _timer.Debounce(action, milliseconds);
        }
    }

    /// <summary>
    /// Delays the invocation of an action until a predetermined interval has elapsed since the last call.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <param name="action"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD003:Avoid awaiting foreign Tasks", Justification = "Required to return the current Task.")]
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Special case using CurrentTask")]
    public Task RunAsync(int milliseconds, Func<Task> action)
    {
        // Check arguments
        if (milliseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "The milliseconds must be greater than to zero.");
        }

        ArgumentNullException.ThrowIfNull(action);

        // DebounceTask
        if (!_disposed)
        {
            _taskCompletionSource = _timer.Debounce(action, milliseconds);
            return _taskCompletionSource.Task;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Releases all resources used by the DebounceTask dispatcher.
    /// </summary>
    public void Dispose()
    {
        _taskCompletionSource = null;
        _timer.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
