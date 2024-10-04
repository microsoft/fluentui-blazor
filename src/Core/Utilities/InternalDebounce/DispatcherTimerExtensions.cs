// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;

/// <summary>
/// Extension methods for <see cref="System.Timers.Timer"/>.
/// </summary>
/// <remarks>
/// Inspired from Microsoft.Toolkit.Uwp.UI.DispatcherQueueTimerExtensions
/// </remarks>
internal static class DispatcherTimerExtensions
{
    private static readonly ConcurrentDictionary<System.Timers.Timer, TimerDebounceItem> _debounceInstances = new();

    /// <summary>
    /// Delays the invocation of an action until a predetermined interval has elapsed since the last call.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="action"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static TaskCompletionSource Debounce(this System.Timers.Timer timer, Func<Task> action, double interval)
    {
        // Check and stop any existing timer
        timer.Stop();

        // Reset timer parameters
        timer.Elapsed -= Timer_Elapsed;
        timer.Interval = interval;

        // If we're not in immediate mode, then we'll execute when the current timer expires.
        timer.Elapsed += Timer_Elapsed;

        var item = _debounceInstances.AddOrUpdate(
                        key: timer,
                        addValue: new TimerDebounceItem()
                        {
                            Status = new TaskCompletionSource(),
                            Action = action,
                        },
                        updateValueFactory: (k, v) =>
                        {
                            v.Status.SetCanceled();
                            v.Status = new TaskCompletionSource();
                            return v.UpdateAction(action);
                        });

        // Start the timer to keep track of the last call here.
        timer.Start();

        return item.Status;
    }

    /// <summary>
    /// Timer elapsed event handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        // This event is only registered/run if we weren't in immediate mode above
        if (sender is System.Timers.Timer timer)
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();

            if (_debounceInstances.TryRemove(timer, out var item))
            {
                if (item == null)
                {
                    return;
                }

                var task = item.Action.Invoke();
                task.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        item.Status.SetException(t.Exception);
                    }
                    else if (t.IsCanceled)
                    {
                        item.Status.SetCanceled();
                    }
                    else
                    {
                        item.Status.SetResult();
                    }
                });
            }
        }
    }

    /// <summary>
    /// Timer debounce item.
    /// </summary>
    private class TimerDebounceItem
    {
        /// <summary>
        /// Gets the task completion source.
        /// </summary>
        public required TaskCompletionSource Status { get; set; }

        /// <summary>
        /// Gets or sets the action to execute.
        /// </summary>
        public required Func<Task> Action { get; set; }

        /// <summary>
        /// Updates the action to execute.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public TimerDebounceItem UpdateAction(Func<Task> action)
        {
            Action = action;
            return this;
        }
    }
}
