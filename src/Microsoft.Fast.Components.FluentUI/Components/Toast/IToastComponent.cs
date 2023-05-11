using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IToastComponent
{
    [Parameter, EditorRequired]
    public ToastIntent Intent { get; set; }

    [Parameter, EditorRequired]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContent EndContent { get; set; }
}
