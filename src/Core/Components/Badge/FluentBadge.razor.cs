// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentBadge component is a visual indicator that communicates a status or description of an associated component.
/// It uses short text, color, and icons for quick recognition and is placed near the relavant content.
/// </summary>
public partial class FluentBadge : FluentComponentBase
{
    private bool _isAttached => ChildContent is not null;

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
         .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle("position", "absolute", _isAttached)
        .AddStyle("transform", GetTransform(), _isAttached)
        .AddStyle(GetPosition(), _isAttached)
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
    /// Must be a valid CSS value or variable
    /// Only applied when <see cref="ChildContent"/> is not null
    /// </summary>
    [Parameter]
    public sbyte? OffsetX { get; set; }

    /// <summary>
    /// Gets or sets how much the badge overlaps the content it wraps on the y-axis
    /// Must be a valid CSS value or variable
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

    /// <summary>
    /// Calculates the position of the badge.
    /// </summary>
    /// <param name="positioning"></param>
    /// <param name="offsetX">Badge offset for the x-axis</param>
    /// <param name="offsetY">Badge offset for the y-axis</param>
    /// <returns></returns>
    protected static string? CalculatePosition(Positioning? positioning, int? offsetX, int? offsetY)
    {
        var _offsetX = $"{offsetX?.ToString(CultureInfo.InvariantCulture) ?? "9"}px";
        var _offsetY = $"{offsetY?.ToString(CultureInfo.InvariantCulture) ?? "9"}px";

        if (positioning.HasValue)
        {
            return positioning.Value switch
            {
                // Top
                Components.Positioning.AboveStart => $"calc(-50% + {_offsetY}) auto auto calc(-50% + {_offsetX})",
                Components.Positioning.Above => $"calc(-50% + {_offsetY}) auto auto auto",
                Components.Positioning.AboveEnd => $"calc(-50% + {_offsetY}) calc(-50% + {_offsetX}) auto auto",

                // Bottom
                Components.Positioning.BelowStart => $"auto auto calc(-50% + {_offsetY}) calc(-50% + {_offsetX})",
                Components.Positioning.Below => $"auto auto calc(-50% + {_offsetY}) auto",
                Components.Positioning.BelowEnd => $"auto calc(-50% + {_offsetX}) calc(-50% + {_offsetY}) auto",

                // Start
                Components.Positioning.BeforeTop => $"calc(-50% + {_offsetY}) auto auto calc(-50% + {_offsetX})",
                Components.Positioning.Before => $"auto auto auto calc(-50% + {_offsetX})",
                Components.Positioning.BeforeBottom => $"auto auto calc(-50% + {_offsetY}) calc(-50% + {_offsetX})",

                // End
                Components.Positioning.AfterTop => $"calc(-50% + {_offsetY}) calc(-50% + {_offsetX}) auto auto",
                Components.Positioning.After => $"auto calc(-50% + {_offsetX}) auto auto",
                Components.Positioning.AfterBottom => $"auto calc(-50% + {_offsetX}) calc(-50% + {_offsetY}) auto",

                Components.Positioning.CenterCenter => null,

                _ => null,
            };
        }

        return null;
    }

    /// <summary>
    /// Gets the justify-content (x) position of the badge.
    /// </summary>
    /// <param name="positioning"></param>
    /// <returns></returns>
    protected static string GetXPosition(Positioning? positioning)
    {
        if (positioning.HasValue)
        {
            return positioning.Value switch
            {
                var x when
                x == Components.Positioning.AboveStart ||
                x == Components.Positioning.BelowStart ||
                x == Components.Positioning.BeforeTop ||
                x == Components.Positioning.Before ||
                x == Components.Positioning.BeforeBottom
                => "flex-start",

                var x when
                x == Components.Positioning.Above ||
                x == Components.Positioning.CenterCenter ||
                x == Components.Positioning.Below
                => "center",

                var x when
                 x == Components.Positioning.AboveEnd ||
                 x == Components.Positioning.BelowEnd ||
                 x == Components.Positioning.AfterTop ||
                 x == Components.Positioning.After ||
                 x == Components.Positioning.AfterBottom
                 => "flex-end",

                _ => "center"
            };
        }

        return "center";
    }

    /// <summary />
    protected static string GetYPosition(Positioning? positioning)
    {
        if (positioning.HasValue)
        {
            return positioning.Value switch
            {
                var y when
                y == Components.Positioning.BeforeTop ||
                y == Components.Positioning.AboveStart ||
                y == Components.Positioning.Above ||
                y == Components.Positioning.AboveEnd ||
                y == Components.Positioning.AfterTop
                => "flex-start",

                var y when
                y == Components.Positioning.Before ||
                y == Components.Positioning.CenterCenter
                => "center",

                var y when
                 y == Components.Positioning.BeforeBottom ||
                 y == Components.Positioning.BelowStart ||
                 y == Components.Positioning.Below ||
                 y == Components.Positioning.BelowEnd ||
                 y == Components.Positioning.AfterBottom
                 => "flex-end",

                _ => "center"
            };
        }

        return "center";
    }

    /// <summary />
    protected string GetPosition()
    {
        var _offsetX = $"{OffsetX?.ToString(CultureInfo.InvariantCulture) ?? "0"}px";
        var _offsetY = $"{OffsetY?.ToString(CultureInfo.InvariantCulture) ?? "0"}px";

        var positioning = Positioning.ToAttributeValue()?.Split('-') ?? [];

        var direction = positioning.Length > 0 ? positioning[0] : null;
        var alignment = positioning.Length > 1 ? positioning[1] : null;

        if (alignment == null)
        {
            if (string.Equals(direction, "above", StringComparison.Ordinal) || string.Equals(direction, "below", StringComparison.Ordinal))
            {
                alignment = "centerX";
            }

            if (string.Equals(direction, "before", StringComparison.Ordinal) || string.Equals(direction, "after", StringComparison.Ordinal))
            {
                alignment = "centerY";
            }
        }

        var directionCSSMap = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "above", $"top: {_offsetY};" },
            { "below", $"bottom: -{_offsetY};" },
            { "before", $"left: {_offsetX};" },
            { "after", $"right: -{_offsetX};" },
            { "center", $"right: calc(50% + {_offsetX});" },
        };

        directionCSSMap.TryGetValue(direction ?? "above", out var directionCSS);

        var alignmentCSSMap = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "start", $"left: {_offsetX};" },
            { "end", $"right: -{_offsetX};" },
            { "top", $"top: {_offsetY};" },
            { "bottom", $"bottom: {_offsetY};" },

            { "center", $"top: calc(50% + {_offsetY});" },
            { "centerX", $"left: calc(50% + {_offsetX});" },
            { "centerY", $"top: calc(50% + {_offsetY});" },
        };

        alignmentCSSMap.TryGetValue(alignment ?? "end", out var alignmentCSS);

        return $"{directionCSS} {alignmentCSS}";
    }

    /// <summary>
    /// Gets the needed transform CSS string based on the <see cref="Positioning" />
    /// </summary>
    protected string GetTransform()
    {
        var value = Positioning switch
        {
            Components.Positioning.AboveStart => "translate(-50%,-50%)",
            Components.Positioning.Above => "translate(-50%,-50%)",
            Components.Positioning.AboveEnd => "translate(50%,-50%)",

            Components.Positioning.BelowStart => "translate(-50%,50%)",
            Components.Positioning.Below => "translate(-50%,50%)",
            Components.Positioning.BelowEnd => "translate(50%,50%)",

            Components.Positioning.BeforeTop => "translate(-50%,-50%)",
            Components.Positioning.Before => "translate(-50%,-50%)",
            Components.Positioning.BeforeBottom => "translate(-50%,50%)",

            Components.Positioning.AfterTop => "translate(50%,-50%)",
            Components.Positioning.After => "translate(50%,-50%)",
            Components.Positioning.AfterBottom => "translate(50%,50%)",

            _ => "translate(50%,-50%)"
        };

        return $"{value};";
    }
}
