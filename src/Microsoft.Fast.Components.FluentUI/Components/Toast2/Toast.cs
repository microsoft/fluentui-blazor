using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

internal class Toast2
{
    public Toast2(RenderFragment message, ToastIntent intent, ToastSettings toastSettings)
    {
        Message = message;
        Intent = intent;
        ToastSettings = toastSettings;
    }
    public Toast2(RenderFragment customComponent, ToastSettings settings)
    {
        CustomComponent = customComponent;
        ToastSettings = settings;
    }

    public string Id { get; } = Identifier.NewId();
    public DateTime TimeStamp { get; } = DateTime.Now;
    public RenderFragment? Message { get; set; }
    public ToastIntent Intent { get; }
    public ToastSettings ToastSettings { get; }
    public RenderFragment? CustomComponent { get; }
}
