// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// The Debounce dispatcher delays the invocation of an action until a predetermined interval has elapsed since the last call.
/// This ensures that the action is only invoked once after the calls have stopped for the specified duration.
/// </summary>
public class Debounce2 : IDisposable
{
    private bool _disposed;
    private readonly System.Timers.Timer _timer = new();
    private TaskCompletionSource? _taskCompletionSource;

    /// <summary>
    /// Gets a value indicating whether the Debounce dispatcher is busy.
    /// </summary>
    public bool Busy => _taskCompletionSource?.Task.Status == TaskStatus.Running && !_disposed;

    public Task CurrentTask => _taskCompletionSource?.Task ?? Task.CompletedTask;

    public void Run(int milliseconds, Func<Task> action)
    {
        if (!_disposed)
        {
            _taskCompletionSource = _timer.Debounce(action, milliseconds);
        }
    }

    public void Dispose()
    {
        _taskCompletionSource = null;
        _timer.Dispose();
        _disposed = true;
    }
}
