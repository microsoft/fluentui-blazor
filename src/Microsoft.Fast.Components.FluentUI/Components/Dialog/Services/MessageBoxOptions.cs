using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class MessageBoxOptions
{
    public string Title { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string? IconColor { get; set; }

    public string Message { get; set; } = string.Empty;

    public MarkupString MarkupMessage { get; set; } = default!;

    public string OkText { get; set; } = "Ok"; //FluentPanelResources.ButtonOK;

    public string CancelText { get; set; } = "Cancel"; //FluentPanelResources.ButtonCancel;

    public string? Width { get; set; }

    public string? Height { get; set; }
}
