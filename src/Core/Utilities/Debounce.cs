// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// The Debounce dispatcher delays the invocation of an action until a predetermined interval has elapsed since the last call.
/// This ensures that the action is only invoked once after the calls have stopped for the specified duration.
/// </summary>
internal sealed class Debounce : IDisposable
{
    // https://learn.microsoft.com/en-us/dotnet/standard/threading/timers

#if NET9_0_OR_GREATER
    private readonly System.Threading.Lock _syncRoot = new();
#else
    private readonly object _syncRoot = new();
#endif

    private bool _disposed;
    private Task? _task;
    private CancellationTokenSource _cts = new();

    /// <summary>
    /// Gets a value indicating whether the Debounce dispatcher is busy.
    /// </summary>
    public bool Busy => _task?.Status == TaskStatus.Running && !_disposed;

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
                            lock (_syncRoot)
                            {
                                if (!_disposed && !_cts.IsCancellationRequested)
                                {
                                    _ = action.Invoke();
                                }
                            }
                        }, _cts.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);

        }
        catch (TaskCanceledException)
        {
            // Task was canceled
        }
    }

    public Task CurrentTask => _task ?? Task.CompletedTask;

    /*
    /// <summary>
    /// Delays the invocation of an action until a predetermined interval has elapsed since the last call.
    /// </summary>
    /// <param name="interval">The interval to wait before invoking the action.</param>
    /// <param name="action">An action to be invoked after the interval has elapsed.</param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0004:Use Task.ConfigureAwait", Justification = "XXX")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<bool> DebounceAsync(TimeSpan interval, Func<Task> action)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        ArgumentNullException.ThrowIfNull(action);
        Console.WriteLine("DebounceAsync " + interval.Milliseconds);

        _timer = GetNewPeriodicTimer(interval.Milliseconds);

        try
        {
            _ = Task.Run(async () =>
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    lock (_syncRoot)
                    {
                        if (!_disposed || !_cts.IsCancellationRequested)
                        {
                            _cts.CancelAsync();
                            Console.WriteLine("Action executing");
                            action.Invoke();
                        }
                    }
                }
            });

            return true;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Canceled");
            return false; // Execution was canceled
        }
    }

    // Cancel the previous Timer (if existing)
    // And create a new Timer with a new CancellationToken
    private PeriodicTimer GetNewPeriodicTimer(int delay)
    {
        _cts.Cancel();

        if (_timer is not null)
        {
            _timer.Dispose();
            _timer = null;
        }

        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(delay));

        _cts.Dispose();
        _cts = new CancellationTokenSource();

        return _timer;
    }
    */

    /// <summary>
    /// Releases all resources used by the Debounce dispatcher.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        _cts?.Cancel();
    }
}
