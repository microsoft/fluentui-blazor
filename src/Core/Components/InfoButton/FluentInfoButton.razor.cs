using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentInfoButton
{
    private bool _showIconHover = false;
    private bool _visible = false;
    private readonly string _idTooltip;

    public FluentInfoButton()
    {
        Id = Identifier.NewId();
        _idTooltip = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-info-button")
        .Build();

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="AspNetCore.Components.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color? IconColor { get; set; }

    /// <summary>
    /// Gets or sets the icon drawing and fill color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or CSS variable.
    /// ⚠️ Only available when Color is set to Color.Custom.
    /// </summary>
    [Parameter]
    public string? IconCustomColor { get; set; }

    /// <summary>
    /// Get or set the icon width.
    /// </summary>
    [Parameter]
    public string IconWidth { get; set; } = "16px";

    /// <summary>
    /// The icon to display when the mouse is over.
    /// </summary>
    [Parameter]
    public Icon IconHover { get; set; } = new CoreIcons.Filled.Size16.Info();

    /// <summary>
    /// The icon to display when the mouse is out.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size16.Info();

    /// <summary>
    /// Gets or sets the tooltip's position. See <see cref="AspNetCore.Components.TooltipPosition"/>.
    /// Don't set this if you want the tooltip to use the best position.
    /// </summary>
    [Parameter]
    public TooltipPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of tooltip panel.
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the tooltip.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private void OnIconMouseOut()
    {
        if (!_visible)
        {
            _showIconHover = false;
        }
    }

    private void OnIconFocusOut()
    {
        _visible = false;
        _showIconHover = false;
    }

    private void OnIconMouseOver() => _showIconHover = true;

    private void OnIconClick() => _visible = !_visible;
}
