using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTab : FluentComponentBase
{
    [Parameter]
    public bool Disabled { get; set; } = false;
}