using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

internal class Toast
{
    public Toast(ToastIntent intent, string title, ToastSettings settings)
    {
        Title = title;
        Intent = intent;
        Settings = settings;
    }
    public Toast(RenderFragment customContent, ToastSettings settings)
    {
        CustomContent = customContent;
        Settings = settings;
    }

    public string Id { get; } = Identifier.NewId();
    public DateTime TimeStamp { get; } = DateTime.Now;
    public string? Title { get; set; }
    public ToastIntent Intent { get; }
    public ToastSettings Settings { get; }
    public RenderFragment? CustomContent { get; }
}
