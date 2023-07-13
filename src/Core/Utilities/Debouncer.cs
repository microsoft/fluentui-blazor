using Timer = System.Timers.Timer;

namespace Microsoft.Fast.Components.FluentUI.Utilities;

internal sealed class Debouncer : IDisposable
{
    private bool _disposed;
    private Timer? _currentTimer;
    private TaskCompletionSource<bool> _currentTaskCompletionSource = new();

    public bool Busy => _currentTimer is not null && !_disposed;

    public async ValueTask<bool> DebounceAsync(Func<Task> action, double milliseconds)
    {
        ArgumentNullException.ThrowIfNull(action);

        Timer? originalTimer = _currentTimer;
        TaskCompletionSource<bool> originalTaskCompletionSource = _currentTaskCompletionSource;

        originalTimer?.Dispose();
        originalTaskCompletionSource.SetResult(false);

        Timer newTimer = new Timer(milliseconds);
        var newTaskCompletionSource = new TaskCompletionSource<bool>();

        bool mayStart = Interlocked.CompareExchange(ref _currentTimer, newTimer, originalTimer) == originalTimer;
        mayStart |= Interlocked.CompareExchange(ref _currentTaskCompletionSource, newTaskCompletionSource, originalTaskCompletionSource) == originalTaskCompletionSource;

        if (!mayStart)
        {
            newTimer.Dispose();
            return false;
        }

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
                Interlocked.CompareExchange(ref _currentTimer, null, newTimer);
                newTimer.Dispose();
                newTaskCompletionSource.SetResult(!_disposed);
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

