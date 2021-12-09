using Microsoft.AspNetCore.Components;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor : FluentComponentBase
{
    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public Appearance? Appearance { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}