// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentIcon is a component that renders an icon from the Fluent System icon set.
/// </summary>
public partial class FluentIcon<Icon> : FluentComponentBase, ITooltipComponent
    where Icon : AspNetCore.Components.Icon, new()
{
    private Icon _icon = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width ?? $"{_icon.Width.ToString(CultureInfo.InvariantCulture)}px", when: () => Width != string.Empty)
        .AddStyle("fill", GetIconColor(), when: (value) => !string.IsNullOrEmpty(value))
        .AddStyle("cursor", "pointer", when: () => OnClick.HasDelegate)
        .AddStyle("display", "inline-block", () => !_icon.ContainsSVG)
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
    /// Needs to be formatted as an HTML hex color string (#RRGGBB or #RGB) or CSS variable.
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

    /// <summary>
    /// Gets or sets whether the icon is focusable (adding tab-index="0" and role="button"),
    /// allows the icon to be focused sequentially (generally with the Tab key).
    /// </summary>
    [Parameter]
    public bool Focusable { get; set; } = false;

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

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
    protected virtual Task OnKeyDownAsync(KeyboardEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            if (string.Compare(e.Key, "Enter", StringComparison.Ordinal) == 0 ||
                string.Compare(e.Key, "NumpadEnter", StringComparison.Ordinal) == 0)
            {
                return OnClickHandlerAsync(new MouseEventArgs());
            }
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        _icon ??= new Icon();

        if (!string.IsNullOrEmpty(CustomColor) && Color != Components.Color.Custom)
        {
            throw new ArgumentException("CustomColor can only be used when Color is set to Color.Custom.", nameof(CustomColor));
        }
    }

    /// <summary>
    /// Returns FluentIcon.CustomColor, or FluentIcon.Color, or Icon.Color.
    /// </summary>
    /// <returns></returns>
    private string? GetIconColor()
    {
        if (Color == AspNetCore.Components.Color.Custom && !string.IsNullOrEmpty(CustomColor))
        {
            return CustomColor;
        }

        if (Color == AspNetCore.Components.Color.Custom && !string.IsNullOrEmpty(_icon.Color))
        {
            return _icon.Color;
        }

        if (Color != null)
        {
            return Color.ToAttributeValue();
        }

        if (!string.IsNullOrEmpty(_icon.Color))
        {
            return _icon.Color;
        }

        return null;
    }
}
