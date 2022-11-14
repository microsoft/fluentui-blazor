using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace.
namespace FluentUI.Demo.Shared;

public partial class Footer : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("footer")
        .Build();

    protected string? StyleValue => new StyleBuilder()
       .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}