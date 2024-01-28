using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMain : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("main")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("height", $"{Height}px", () => Height.HasValue)
        .Build();

    /// <summary>
    /// Gets or sets the height of the header (in pixels).
    /// </summary>
    [Parameter]
    public int? Height { get; set; } = 50;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
