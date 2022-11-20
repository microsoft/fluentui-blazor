using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

/// <summary />
public partial class NotificationBadge : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("notification-badge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("left", $"{LeftPosition}%", () => LeftPosition.HasValue)
        .AddStyle("bottom", $"{BottomPosition}%", () => BottomPosition.HasValue)
        .AddStyle(Style)
        .AddStyle("background-color", GetBackgroundColor())
        .AddStyle("color", GetFontColor())
        .AddStyle("border", $"1px solid {GetBorderColor()}")
        .Build();

    /// <summary>
    /// Child content of component, the content that the badge will apply to.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Content you want inside the badge.
    /// </summary>
    [Parameter]
    public string? Badge { get; set; }

    /// <summary>
    /// Content you want inside the badge, to customize the badge content.
    /// </summary>
    [Parameter]
    public RenderFragment? BadgeContent { get; set; }

    /// <summary>
    /// Left position of the badge in percentage.
    /// By default, this value is 50 to center the badge.
    /// </summary>
    [Parameter]
    public int? LeftPosition { get; set; } = 50;

    /// <summary>
    /// Bottom position of the badge in percentage.
    /// By default, this value is 50 to center the badge.
    /// </summary>
    [Parameter]
    public int? BottomPosition { get; set; } = 50;

    /// <summary>
    /// Default design of this badge using colors from theme.
    /// </summary>
    [Parameter]
    public Fill? Fill { get; set; }

    /// <summary>
    /// Background color to replace the color inferred from <see cref="Fill"/> property.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Font color to replace the color inferred from <see cref="Fill"/> property.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary />
    private string GetBackgroundColor()
    {
        if (string.IsNullOrEmpty(BackgroundColor))
        {
            return Fill switch
            {
                Microsoft.Fast.Components.FluentUI.Fill.Lowlight => "var(--neutral-fill-layer-rest)",
                _ => "var(--accent-fill-rest)",
            };
        }
        else
        {
            return BackgroundColor;
        }
    }

    /// <summary />
    private string GetBorderColor()
    {
        if (string.IsNullOrEmpty(BackgroundColor))
        {
            return Fill switch
            {
                Microsoft.Fast.Components.FluentUI.Fill.Lowlight => "var(--accent-fill-rest)",
                _ => "var(--neutral-fill-layer-rest)",
            };
        }
        else
        {
            return BackgroundColor;
        }
    }

    /// <summary />
    private string GetFontColor()
    {
        if (string.IsNullOrEmpty(Color))
        {
            return Fill switch
            {
                Microsoft.Fast.Components.FluentUI.Fill.Lowlight => "var(--accent-stroke-control-active)",
                _ => "var(--neutral-fill-layer-rest)",
            };
        }
        else
        {
            return Color;
        }
    }
}
