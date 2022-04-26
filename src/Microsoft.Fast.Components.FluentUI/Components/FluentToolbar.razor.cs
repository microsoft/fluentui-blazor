using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToolbar : FluentComponentBase
{
    [Parameter]
    public Orientation? Orientation { get; set; } = FluentUI.Orientation.Horizontal;
}