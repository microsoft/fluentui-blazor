using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSplitter : FluentComponentBase
{
    protected string _direction = "row";

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-splitter")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// Gets or sets the orientation.
    /// Default is horizontal (i.e a vertical splitter bar)
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Content for the top/left panel
    /// </summary>
    [Parameter]
    public RenderFragment? Panel1 { get; set; }


    /// <summary>
    /// Content for the bottom/right panel
    /// </summary>
    [Parameter]
    public RenderFragment? Panel2 { get; set; }

    /// <summary>
    /// Size for the left/top panel. 
    /// Needs to be a valid css size like '50%' or '250px'
    /// </summary>
    [Parameter]
    public string? Panel1Size { get; set; }

    /// <summary>
    /// Size for the right/bottom panel. Needs to be a valid css size like '50%' or '250px'
    /// Uses grid-template-rows/columns with max-content to determine end width. 
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/CSS/grid-template-columns">mdn web docs</see> for more information 
    /// </summary>
    [Parameter]
    public string? Panel2Size { get; set; }

    public FluentSplitter()
    {
        Id = Identifier.NewId();   
    }

    protected override void OnParametersSet()
    {
        _direction = (Orientation == Orientation.Horizontal) ? "row" : "column";
    }
}
