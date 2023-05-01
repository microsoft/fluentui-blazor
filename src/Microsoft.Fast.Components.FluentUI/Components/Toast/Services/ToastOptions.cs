namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class ToastOptions
{
    internal ToastOptions(ToastGlobalOptions commonOptions)
    {
        Global = commonOptions;
    }

    internal ToastGlobalOptions Global { get; }

    public string Message { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public Func<Toast, Task>? OnClick { get; set; }

    public Func<Toast, Task>? OnClose { get; set; }

    public ToastAction Action { get; set; } = new ToastAction();

    public ToastIntent Intent { get; set; } = ToastIntent.Neutral;

    public bool ClearAfterNavigation { get; set; }
}
