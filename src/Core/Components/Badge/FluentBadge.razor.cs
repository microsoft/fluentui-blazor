// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentBadge component is a visual indicator that communicates a status or description of an associated component.
/// It uses short text, color, and icons for quick recognition and is placed near the relavant content.
/// </summary>
public partial class FluentBadge : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
         .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .Build();

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    [Parameter]
    public BadgeColor? Color { get; set; } = BadgeColor.Brand;

    /// <summary>
    /// Gets or sets the background color.
    /// Must be a valid CSS color value.
    /// When using BackgroundColor, <see cref="Color"/> must be explicitly set to null
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public BadgeAppearance Appearance { get; set; }

    /// <summary>
    /// Gets or sets the shape of the badge.
    /// </summary>
    [Parameter]
    public BadgeShape Shape { get; set; }

    /// <summary>
    /// Gets or sets the size of the badge.
    /// </summary>
    [Parameter]
    public BadgeSize Size { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of badge content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the aria-label of the <see cref="Icon"/>.
    /// </summary>
    [Parameter]
    public string? IconLabel { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of badge content.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(BackgroundColor) && Color is not null)
        {
            throw new ArgumentException("When using BackgroundColor, Color must be set to null explicitly.");
        }
    }

    private string GetIconColor()
    {
        return Color switch
        {
            BadgeColor.Informative => "var(--colorNeutralForeground3)",
            BadgeColor.Subtle => "var(--colorNeutralForeground1)",
            BadgeColor.Warning => "var(--colorNeutralForeground1Static)",
            _ => "var(--colorNeutralForegroundOnBrand)"
        };
    }
}
