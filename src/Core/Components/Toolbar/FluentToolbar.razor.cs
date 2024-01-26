using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentToolbar : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the toolbar's orentation. See <see cref="Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = AspNetCore.Components.Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
