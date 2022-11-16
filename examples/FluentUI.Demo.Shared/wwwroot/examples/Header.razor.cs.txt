using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class Header : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("header")
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