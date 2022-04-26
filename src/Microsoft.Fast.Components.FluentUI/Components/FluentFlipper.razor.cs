using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentFlipper : FluentComponentBase
{
    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public Direction? Direction { get; set; }
}