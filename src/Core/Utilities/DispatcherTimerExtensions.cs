// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// 
/// </summary>
internal static class DispatcherTimerExtensions
{
    private static readonly ConcurrentDictionary<System.Timers.Timer, TimerDebounceItem> _debounceInstances = new();

    public static TaskCompletionSource Debounce(this System.Timers.Timer timer, Func<Task> action, double interval)
    {
        // Check and stop any existing timer
        timer.Stop();

        // Reset timer parameters
        timer.Elapsed -= Timer_Elapsed;
        timer.Interval = interval;

        // If we're not in immediate mode, then we'll execute when the current timer expires.
        timer.Elapsed += Timer_Elapsed;

        // Store/Update function
        var item = _debounceInstances.AddOrUpdate(
                        key: timer,
                        addValue: new TimerDebounceItem()
                        {
                            Status = new TaskCompletionSource(),
                            Action = action,
                        },
                        updateValueFactory: (k, v) => v.UpdateAction(action));

        // Start the timer to keep track of the last call here.
        timer.Start();

        return item.Status;
    }

    private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        // This event is only registered/run if we weren't in immediate mode above
        if (sender is System.Timers.Timer timer)
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();

            if (_debounceInstances.TryRemove(timer, out var item))
            {
                _ = (item?.Action.Invoke());
                item?.Status.SetResult();
            }
        }
    }

    private class TimerDebounceItem
    {
        public required TaskCompletionSource Status { get; init; }

        public required Func<Task> Action { get; set; }

        public TimerDebounceItem UpdateAction(Func<Task> action)
        {
            Action = action;
            return this;
        }
    }
}
