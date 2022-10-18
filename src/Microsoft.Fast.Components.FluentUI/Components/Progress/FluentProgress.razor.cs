using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentProgress : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the minimum value 
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum value 
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the current value 
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets if the progress element is paused
    /// </summary>
    [Parameter]
    public bool? Paused { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}