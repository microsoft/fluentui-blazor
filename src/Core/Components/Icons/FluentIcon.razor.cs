using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentIcon is a component that renders an icon from the Fluent System icon set.
/// </summary>
public partial class FluentIcon<Icon> : FluentComponentBase
    where Icon : AspNetCore.Components.Icon, new()
{
    private Icon _icon = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width ?? $"{_icon.Width}px")
        .AddStyle("fill", GetIconColor())
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle("display", "inline-block", !_icon.ContainsSVG)
        .Build();

    /// <summary>
    /// Gets or sets the slot where the icon is displayed in.
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets the title for the icon.
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = null;

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="AspNetCore.Components.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

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
    /// Allows for capturing a mouse click on an icon.
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

        if (!string.IsNullOrEmpty(CustomColor) && Color != AspNetCore.Components.Color.Custom)
        {
            throw new ArgumentException("CustomColor can only be used when Color is set to Color.Custom.");
        }
    }

    /// <summary>
    /// Returns FluentIcon.CustomColor, or FluentIcon.Color, or Icon.Color.
    /// </summary>
    /// <returns></returns>
    private string GetIconColor()
    {
        var defaultColor = AspNetCore.Components.Color.Accent.ToAttributeValue()!;

        if (Color == AspNetCore.Components.Color.Custom && !string.IsNullOrEmpty(CustomColor))
        {
            return CustomColor;
        }

        if (Color != null)
        {
            return Color.ToAttributeValue() ?? defaultColor;
        }

        if (!string.IsNullOrEmpty(_icon.Color))
        {
            return _icon.Color;
        }

        return defaultColor;
    }
}
