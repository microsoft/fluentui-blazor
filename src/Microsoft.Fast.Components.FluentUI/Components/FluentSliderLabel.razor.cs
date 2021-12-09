using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSliderLabel : FluentComponentBase
{
    [Parameter]
    public int? Position { get; set; }

    [Parameter]
    public bool? HideMark { get; set; }
}