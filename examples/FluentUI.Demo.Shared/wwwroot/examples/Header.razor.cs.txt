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
        .AddStyle("height", Height)
        .Build();

    /// <summary>
    /// Gets or sets the height of the header.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "50px";

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}