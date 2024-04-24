using Timer = System.Timers.Timer;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

internal sealed class Debouncer : IDisposable
{
    private readonly object _syncRoot = new();
    private bool _disposed;
    private Timer? _timer;
    private TaskCompletionSource<bool>? _taskCompletionSource;

    public bool Busy => _timer is not null && !_disposed;

    public Task<bool> DebounceAsync(double milliseconds, Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        lock (_syncRoot)
        {
            _taskCompletionSource?.TrySetResult(false);
            _taskCompletionSource = null;

            _timer?.Dispose();
            _timer = null;

            Timer newTimer = _timer = new Timer(milliseconds);
            TaskCompletionSource<bool> newTaskCompletionSource = _taskCompletionSource = new TaskCompletionSource<bool>();

            newTimer.Elapsed += async (_, _) =>
            {
                newTimer.Stop();
                try
                {
                    if (!_disposed)
                    {
                        await action();
                    }
                }
                finally
                {
                    lock (_syncRoot)
                    {
                        if (_timer == newTimer)
                        {
                            _timer.Dispose();
                            _timer = null;
                            newTaskCompletionSource?.SetResult(!_disposed);
                        }
                    }
                }
            };

            newTimer.Start();
            return newTaskCompletionSource.Task;
        }
    }

    public void Dispose()
    {
        _disposed = true;
        _timer?.Dispose();
    }
}

