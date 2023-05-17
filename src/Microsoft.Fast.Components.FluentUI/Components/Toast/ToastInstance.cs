using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

internal class ToastInstance
{
    public ToastInstance(ToastIntent intent, string title, ToastSettings settings)
    {
        Title = title;
        Intent = intent;
        Settings = settings;
    }
    public ToastInstance(RenderFragment customContent, ToastSettings settings)
    {
        CustomContent = customContent;
        Settings = settings;
    }

    public ToastInstance(ToastIntent intent, RenderFragment customContent, ToastSettings settings)
    {
        Intent = intent;
        CustomContent = customContent;
        Settings = settings;
    }

    public string Id { get; } = Identifier.NewId();
    public DateTime TimeStamp { get; } = DateTime.Now;
    
    public ToastIntent Intent { get; }
    public string? Title { get; set; }
    

    public ToastSettings Settings { get; }


    public RenderFragment? CustomContent { get; }
}
