using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentCard
{
    private const string DEFAULT_CARD_WIDTH = "350px";
    private const string DEFAULT_CARD_HEIGHT = "350px";

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("--card-width", Width ?? DEFAULT_CARD_WIDTH)
        .AddStyle("--card-height", Height ?? DEFAULT_CARD_HEIGHT)
        .AddStyle("content-visibility", "visible", !AreaRestricted)
        .AddStyle("contain", "style", !AreaRestricted)
        .Build();

    /// <summary>
    /// By default, content in the card is restricted to the area of the card itself. 
    /// If you want content to be able to overflow the card, set this property to false.
    /// </summary>
    [Parameter]
    public bool AreaRestricted { get; set; } = true;

    /// <summary>
    /// Specifies the width of the card. Must be a valid CSS measurement.
    /// </summary>  
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Specifies the height of the card. Must be a valid CSS measurement.
    /// </summary>  
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
