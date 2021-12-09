using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeItem : FluentComponentBase
{
    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Selected { get; set; }

    [Parameter]
    public bool? Expanded { get; set; }
}