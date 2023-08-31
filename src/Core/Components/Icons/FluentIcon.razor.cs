using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentIcon is a component that renders an icon from the Fluent System icon set.
/// </summary>
public partial class FluentIcon<Icon> : FluentComponentBase
    where Icon : FluentUI.Icon, new()
{
    private Icon _icon = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width ?? $"{_icon.Width}px")
        .AddStyle("fill", Color == FluentUI.Color.Custom ? CustomColor : Color.ToAttributeValue())
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle("display", "inline-block", !ContainsSVG())
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Gets or sets the slot where the icon is displayed in
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets the title for the icon
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = null;

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="FluentUI.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; } = FluentUI.Color.Accent;

    /// <summary>
    /// Gets or sets the icon drawing and fill color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or CSS variable.
    /// ⚠️ Only available when Color is set to Color.Custom.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets the icon width.
    /// If not set, the icon size will be used.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the Icon object to render.
    /// </summary>
    [Parameter]
    public Icon Value
    {
        get => _icon;
        set => _icon = value;
    }

    /// <summary>
    /// Allows for capturing a mouse click on an icon
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary />
    protected virtual Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            return OnClick.InvokeAsync(e);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (_icon == null)
        {         
            _icon = new Icon();
        }

        if (!string.IsNullOrEmpty(CustomColor) && Color != FluentUI.Color.Custom)
        {
            throw new ArgumentException("CustomColor can only be used when Color is set to Color.Custom.");
        }
    }

    /// <summary>
    /// Returns true if the icon contains a SVG content.
    /// </summary>
    /// <returns></returns>
    private bool ContainsSVG()
    {
        return !string.IsNullOrEmpty(_icon.Content) &&
               (_icon.Content.StartsWith("<path ") ||
                _icon.Content.StartsWith("<rect ") ||
                _icon.Content.StartsWith("<g ") ||
                _icon.Content.StartsWith("<circle ") ||
                _icon.Content.StartsWith("<mark "));
    }
}
