using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMenuItem : FluentComponentBase
{
    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Checked { get; set; }
}