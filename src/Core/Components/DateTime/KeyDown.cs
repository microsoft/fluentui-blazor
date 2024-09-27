using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class KeyDown
{
    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<MouseEventArgs, Task> onClickAsync)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            return onClickAsync.Invoke(new MouseEventArgs());
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<DateTime, bool, Task> onClickAsync, DateTime value, bool dayDisabled)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            return onClickAsync.Invoke(value, dayDisabled);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<CalendarTitles, Task> onClickAsync, CalendarTitles title)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            return onClickAsync.Invoke(title);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<int, int, bool, Task> onClickAsync, int year, int month, bool isReadOnly)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            return onClickAsync.Invoke(year, month, isReadOnly);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public static Task SimulateClickAsync(KeyboardEventArgs e, Func<int, bool, Task> onClickAsync, int year, bool isReadOnly)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            return onClickAsync.Invoke(year, isReadOnly);
        }

        return Task.CompletedTask;
    }
}
