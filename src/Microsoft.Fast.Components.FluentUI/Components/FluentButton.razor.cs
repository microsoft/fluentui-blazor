using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentButton : FluentComponentBase
{
    [Parameter]
    public Appearance? Appearance { get; set; }

    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Autofocus { get; set; }
}