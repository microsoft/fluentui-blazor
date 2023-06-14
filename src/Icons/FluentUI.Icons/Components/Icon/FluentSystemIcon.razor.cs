using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentSystemIcon is a component that renders an icon from the Fluent System icon set.
/// </summary>
public partial class FluentSystemIcon : FluentComponentBase
{
    private Icon _icon = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Size}px", Size.HasValue && Size.Value > 0)
        .AddStyle("fill", Color == FluentUI.Color.Custom ? CustomColor : Color.ToAttributeValue())
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Icon to be used can either be svg paths.
    /// </summary>
    [Parameter]
    public Icon Icon
    {
        get
        {
            return _icon;
        }
        set
        {
            _icon = value;

            if (Size == null)
            {
                Size = _icon.Size;
            }
        }
    }

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
    /// Gets or sets the icon size, used to create the viewbox, width and height attributes.
    /// </summary>
    [Parameter]
    public IconSize? Size { get; set; }

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
        if (!string.IsNullOrEmpty(CustomColor) && Color != FluentUI.Color.Custom)
        {
            throw new ArgumentException("CustomColor can only be used when Color is set to Color.Custom.");
        }
    }

    /// <summary>
    /// Returns true if the icon contains a SVG content.
    /// </summary>
    /// <returns></returns>
    private bool IsSvgIcon()
    {
        return !string.IsNullOrEmpty(Icon.Content) && Icon.Content.StartsWith("<", StringComparison.OrdinalIgnoreCase);
    }
}
