using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentBadge : FluentComponentBase
{
    [Parameter]
    public Color? Color { get; set; }

    [Parameter]
    public Fill? Fill { get; set; }

    [Parameter]
    public Appearance? Appearance { get; set; }
}