using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTabs : FluentComponentBase
{
    [Parameter]
    public bool? ActiveIndicator { get; set; }

    [Parameter]
    public Orientation? Orientation { get; set; }
}