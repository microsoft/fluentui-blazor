using Timer = System.Timers.Timer;

namespace Microsoft.Fast.Components.FluentUI.Utilities;

internal sealed class Debouncer : IDisposable
{
    private bool _disposed;
    private Timer? _currentTimer;
    private TaskCompletionSource<bool>? _currentTaskCompletionSource;

    public bool Busy => _currentTimer is not null && !_disposed;

    public async ValueTask<bool> DebounceAsync(double milliseconds, Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Timer? originalTimer = _currentTimer;
        TaskCompletionSource<bool>? originalTaskCompletionSource = _currentTaskCompletionSource;

        originalTimer?.Dispose();
        originalTaskCompletionSource?.SetResult(false);

        Timer newTimer = new Timer(milliseconds);
        var newTaskCompletionSource = new TaskCompletionSource<bool>();
        
        if (Interlocked.CompareExchange(ref _currentTimer, newTimer, originalTimer) != originalTimer)
        {
            newTimer.Dispose();
            return false;
        }
        _currentTaskCompletionSource = newTaskCompletionSource;

        newTimer.Elapsed += async(_, _) =>
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
                newTimer.Dispose();
                if (Interlocked.CompareExchange(ref _currentTimer, null, newTimer) == newTimer)
                {
                    newTaskCompletionSource.SetResult(false);
                }
                else
                {
                    newTaskCompletionSource.SetResult(!_disposed);
                }
                Interlocked.CompareExchange(ref _currentTaskCompletionSource, null, newTaskCompletionSource);
            }
        };

        newTimer.Start();
        return await newTaskCompletionSource.Task;
    }

    public void Dispose()
    {
        _disposed = true;
        _currentTimer?.Dispose();
    }
}

