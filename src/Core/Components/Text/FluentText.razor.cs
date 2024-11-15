// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentText component, codifies Fluent's opinions on typography to make them easy to use and standardize across products.
/// </summary>
public partial class FluentText : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("color", GetTextColor(), when: (value) => !string.IsNullOrEmpty(value))
        .Build();

    /// <summary>
    /// Gets or sets the tag to be rendered.
    /// </summary>
    [Parameter]
    public TextTag As { get; set; }

    /// <summary>
    /// Gets or sets the size of the text.
    /// </summary>
    [Parameter]
    public TextSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the weight of the text.
    /// </summary>
    [Parameter]
    public TextWeight? Weight { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the text.
    /// </summary>
    [Parameter]
    public TextAlign? Align { get; set; }

    /// <summary>
    /// Gets or sets the font family of the text.
    /// </summary>
    [Parameter]
    public TextFont? Font { get; set; }

    /// <summary>
    /// Gets or sets if the texts is set to no-wrap.
    /// </summary>
    [Parameter]
    public bool Nowrap { get; set; }

    /// <summary>
    /// Gets or sets if the text should truncate. 
    /// </summary>
    [Parameter]
    public bool Truncate { get; set; }

    /// <summary>
    /// Gets or sets if the text is set to block.
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Gets or sets if the text is shown in italic.
    /// </summary>
    [Parameter]
    public bool Italic { get; set; }

    /// <summary>
    /// Gets or sets is shown underlined .
    /// </summary>
    [Parameter]
    public bool Underline { get; set; }

    /// <summary>
    /// Gets or sets if the text is shown as strikethrough.
    /// </summary>
    [Parameter]
    public bool Strikethrough { get; set; }

    /// <summary>
    /// Gets or sets the text drawing color.
    /// Value comes from the <see cref="AspNetCore.Components.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the text drawing color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#RRGGBB or #RGB) or CSS variable.
    /// ⚠️ Only available when Color is set to Color.Custom.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets the content to be shown.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(CustomColor) && Color != Components.Color.Custom)
        {
            throw new ArgumentException("CustomColor can only be used when Color is set to Color.Custom.", nameof(CustomColor));
        }
    }

    /// <summary>
    /// Returns CustomColor, or Color.
    /// </summary>
    /// <returns></returns>
    private string? GetTextColor()
    {
        if (Color == Components.Color.Custom && !string.IsNullOrEmpty(CustomColor))
        {
            return CustomColor;
        }

        if (Color != null)
        {
            return Color.ToAttributeValue();
        }

        return null;
    }
}
