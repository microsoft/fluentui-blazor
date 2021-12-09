using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialog : FluentComponentBase
{
    [Parameter]
    public bool? Modal { get; set; }

    [Parameter]
    public bool Hidden { get; set; } = false;

    public void Show() => Hidden = false;
    public void Hide() => Hidden = true;
}