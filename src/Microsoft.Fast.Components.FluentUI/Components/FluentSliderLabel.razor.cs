using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSliderLabel : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the label position
    /// </summary>
    [Parameter]
    public int? Position { get; set; }

    /// <summary>
    /// Gets or sets if marks are hidden
    /// </summary>
    [Parameter]
    public bool? HideMark { get; set; }
    /// <summary>
    /// The disabled state of the label. This is generally controlled by the parent @microsoft/fast-foundation#(FASTSlider:class).
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }
}