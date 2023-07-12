using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMain: FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("main")
        .Build();

    protected string? StyleValue => new StyleBuilder()
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