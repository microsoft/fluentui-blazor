namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class ToastAction
{
    public string? Text { get; set; }

    public string? Href { get; set; }

    public Func<Toast, Task>? OnClick { get; set; }
}
