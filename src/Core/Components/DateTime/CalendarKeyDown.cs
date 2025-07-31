// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides utility methods to invoke a Function when a key "Enter" or "Space" is pressed.
/// </summary>
internal static class CalendarKeyDown
{
    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<MouseEventArgs, Task> onClickAsync)
    {
        if (IsEnterOrSpaceKey(e))
        {
            return onClickAsync.Invoke(new MouseEventArgs());
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<DateTime, bool, Task> onClickAsync, DateTime value, bool dayDisabled)
    {
        if (IsEnterOrSpaceKey(e))
        {
            return onClickAsync.Invoke(value, dayDisabled);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<CalendarTitles, Task> onClickAsync, CalendarTitles title)
    {
        if (IsEnterOrSpaceKey(e))
        {
            return onClickAsync.Invoke(title);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<int, int, bool, Task> onClickAsync, int year, int month, bool isReadOnly)
    {
        if (IsEnterOrSpaceKey(e))
        {
            return onClickAsync.Invoke(year, month, isReadOnly);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<int, bool, Task> onClickAsync, int year, bool isReadOnly)
    {
        if (IsEnterOrSpaceKey(e))
        {
            return onClickAsync.Invoke(year, isReadOnly);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private static bool IsEnterOrSpaceKey(KeyboardEventArgs e)
    {
        return string.Equals(e.Key, "Enter", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(e.Key, " ", StringComparison.OrdinalIgnoreCase);
    }
}
