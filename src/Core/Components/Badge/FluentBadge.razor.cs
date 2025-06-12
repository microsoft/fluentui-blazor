// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentBadge component is a visual indicator that communicates a status or description of an associated component.
/// It uses short text, color, and icons for quick recognition and is placed near the relavant content.
/// </summary>
public partial class FluentBadge : FluentComponentBase
{
    /// <summary />
    public FluentBadge(LibraryConfiguration configuration) : base(configuration) { }

    private bool _isAttached => ChildContent is not null;

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
         .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle("z-index", ZIndex.Badge.ToString(CultureInfo.InvariantCulture), _isAttached)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    [Parameter]
    public BadgeColor? Color { get; set; }

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
    public BadgeAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the shape of the badge.
    /// </summary>
    [Parameter]
    public BadgeShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the size of the badge.
    /// </summary>
    [Parameter]
    public BadgeSize? Size { get; set; }

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

    /// <summary>
    /// Gets or sets the badge's positioning relative to the <see cref="FluentBadge.ChildContent" />.
    /// The default value is `null`. Internally the component uses AboveEnd as its default value.
    /// </summary>
    [Parameter]
    public Positioning? Positioning { get; set; }

    /// <summary>
    /// Gets or sets how much the badge overlaps the content it wraps on the x-axis
    /// Only applied when <see cref="ChildContent"/> is not null
    /// </summary>
    [Parameter]
    public sbyte? OffsetX { get; set; }

    /// <summary>
    /// Gets or sets how much the badge overlaps the content it wraps on the y-axis
    /// Only applied when <see cref="ChildContent"/> is not null
    /// </summary>
    [Parameter]
    public sbyte? OffsetY { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(BackgroundColor) && Color is not null)
        {
            throw new ArgumentException("When setting BackgroundColor, Color must not be set.");
        }

        if (Positioning is null && _isAttached)
        {
            Positioning = Components.Positioning.AboveEnd;
        }
    }

    /// <summary />
    protected string GetIconColor()
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
