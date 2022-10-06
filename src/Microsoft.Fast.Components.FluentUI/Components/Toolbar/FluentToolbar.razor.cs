using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToolbar : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the toolbar's orentation. See <see cref="FluentUI.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = FluentUI.Orientation.Horizontal;
}