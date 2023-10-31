using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProgress : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

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
    public int? Value { get; set; }



    [Parameter]
    public bool Visible { get; set; } = true;

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