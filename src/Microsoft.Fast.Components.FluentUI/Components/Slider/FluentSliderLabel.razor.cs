using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSliderLabel : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the slider's label position
    /// </summary>
    [Parameter]
    public int? Position { get; set; }

    /// <summary>
    /// Gets or sets if marks are hidden
    /// </summary>
    [Parameter]
    public bool? HideMark { get; set; }
    /// <summary>
    /// The disabled state of the label. This is generally controlled by the parent .
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}