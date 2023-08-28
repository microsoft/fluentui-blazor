using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMainLayout : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("--header-height", $"{HeaderHeight}px", () => HeaderHeight.HasValue)
        .AddStyle("height", $"calc(100% - {HeaderHeight}px)")
        .Build();

    /// <summary>
    /// Gets or sets the header content.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Gets or sets the subheader content.
    /// </summary>
    [Parameter]
    public RenderFragment? SubHeader { get; set; }

    /// <summary>
    /// Gets or sets the height of the header (in pixels).
    /// </summary>
    [Parameter]
    public int? HeaderHeight { get; set; } = 50;

    /// <summary>
    /// Gets or set the tite of the navigation menu
    /// </summary>
    [Parameter]
    public string? NavMenuTitle { get; set; }

    /// <summary>
    /// Gets or sets the content of the navigation menu
    /// </summary>
    [Parameter]
    public RenderFragment? NavMenuContent { get; set; }

    /// <summary>
    /// Gets or sets the content of the body
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }
}
