using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentLayout : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("layout")
        .AddClass("layout--horizontal", () => Orientation == Orientation.Horizontal)
        .AddClass("layout--vertical", () => Orientation == Orientation.Vertical)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the orientation of the stacked components.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
