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
}