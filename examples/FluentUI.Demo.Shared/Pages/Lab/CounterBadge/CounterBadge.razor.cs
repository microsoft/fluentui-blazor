using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

/// <summary />
public partial class CounterBadge : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("counterbadge")
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
    /// Number displayed inside the badge.
    /// Can be enriched with a plus sign with <see cref="ShowOverflow"/>
    /// </summary>
    [Parameter, EditorRequired]
    public int? Count { get; set; }

    /// <summary>
    /// Content you want inside the badge, to customize the badge content.
    /// </summary>
    [Parameter]
    public RenderFragment? BadgeContent { get; set; }

    /// <summary>
    /// Max number that can be displayed inside the badge.
    /// Default is 99.
    /// </summary>
    [Parameter]
    public int? Max { get; set; } = 99;


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
    public Appearance? Appearance { get; set; } = Microsoft.Fast.Components.FluentUI.Appearance.Accent;

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

    /// <summary>
    ///  If just a dot should be displayed without the count.
    ///  Defaults to false.
    /// </summary>
    [Parameter]
    public bool Dot { get; set; } = false;

    /// <summary>
    /// If the counter badge should be displayed when the count is zero.
    /// Defaults to false.
    ///</summary>
    [Parameter]
    public bool ShowZero { get; set; } = false;

    /// <summary>
    /// If an plus sign should be displayed when the <see cref="Count"/> is greater than <see cref="Max"/>.
    /// Defaults to true.
    ///</summary>
    [Parameter]
    public bool ShowOverflow { get; set; } = true;


    protected override Task OnParametersSetAsync()
    {
        if (Appearance != Microsoft.Fast.Components.FluentUI.Appearance.Accent &&
            Appearance != Microsoft.Fast.Components.FluentUI.Appearance.Lightweight &&
            Appearance != Microsoft.Fast.Components.FluentUI.Appearance.Neutral)
        {
            throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral.");
        }

        return base.OnParametersSetAsync();
    }

    /// <summary />
    private string GetBackgroundColor()
    {
        if (BackgroundColor != null)
        {
            return BackgroundColor;
        }

        return Appearance switch
        {
            Microsoft.Fast.Components.FluentUI.Appearance.Accent => "var(--accent-fill-rest)",
            Microsoft.Fast.Components.FluentUI.Appearance.Lightweight => "var(--neutral-fill-stealth-rest)",
            Microsoft.Fast.Components.FluentUI.Appearance.Neutral => "var(--neutral-fill-rest)",
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private string GetBorderColor()
    {
        if (BackgroundColor != null)
        {
            return BackgroundColor;
        }

        return Appearance switch
        {
            Microsoft.Fast.Components.FluentUI.Appearance.Accent => "var(--accent-fill-hover)",
            Microsoft.Fast.Components.FluentUI.Appearance.Lightweight => "var(--neutral-fill-stealth-hover)",
            Microsoft.Fast.Components.FluentUI.Appearance.Neutral => "var(--neutral-fill-hover)",
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private string GetFontColor()
    {
        if (Color != null)
        {
            return Color;
        }

        return Appearance switch
        {
            Microsoft.Fast.Components.FluentUI.Appearance.Accent => "var(--neutral-fill-rest)",
            Microsoft.Fast.Components.FluentUI.Appearance.Lightweight => "var(--accent-fill-rest)",
            Microsoft.Fast.Components.FluentUI.Appearance.Neutral => "var(--neutral-fill-inverse-rest)",
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }
}
