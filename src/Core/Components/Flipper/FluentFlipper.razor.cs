using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentFlipper : FluentComponentBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the flipper should be hidden from assistive technology. Because flippers are often supplementary navigation, they are often hidden from assistive technology.
    /// </summary>
    [Parameter]
    public bool? AriaHidden { get; set; }

    /// <summary>
    /// Gets or sets the direction. See <see cref="AspNetCore.Components.FlipperDirection"/>.
    /// </summary>

    [Parameter]
    public FlipperDirection? Direction { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
