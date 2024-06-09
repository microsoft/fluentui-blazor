using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentCard
{
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("--card-width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("--card-height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("content-visibility", "visible", !AreaRestricted)
        .AddStyle("contain", "none", !AreaRestricted)
        .Build();

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-card-minimal-style", when: MinimalStyle)
        .Build();

    /// <summary>
    /// By default, content in the card is restricted to the area of the card itself.
    /// If you want content to be able to overflow the card, set this property to false.
    /// </summary>
    [Parameter]
    public bool AreaRestricted { get; set; } = true;

    /// <summary>
    /// Gets or sets the width of the card. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the card. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public bool MinimalStyle { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
