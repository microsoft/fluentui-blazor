﻿using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentCounterBadge : FluentComponentBase, IDisposable
{
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("counterbadge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("left", $"{HorizontalPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.ltr)
        .AddStyle("right", $"{HorizontalPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.rtl)
        .AddStyle("bottom", $"{BottomPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => BottomPosition.HasValue)
        .AddStyle(Style)
        .AddStyle("background-color", GetBackgroundColor().ToAttributeValue())
        .AddStyle("color", GetFontColor().ToAttributeValue())
        .AddStyle("border", $"1px solid {GetBorderColor().ToAttributeValue()}")
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
    public int? HorizontalPosition { get; set; } = 50;

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
    public Appearance? Appearance { get; set; } = FluentUI.Appearance.Accent;

    /// <summary>
    /// Background color to replace the color inferred from property.
    /// </summary>
    [Parameter]
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Font color to replace the color inferred from property.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    ///// <summary>
    /////  If just a dot should be displayed without the count.
    /////  Defaults to false.
    ///// </summary>
    //[Parameter]
    //public bool Dot { get; set; } = false;

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
        if (BackgroundColor is not null && Color is null || BackgroundColor is null && Color is not null)
        {
            throw new ArgumentException("Both BackgroundColor and Color must be set.");
        }
        if (BackgroundColor is FluentUI.Color.Custom ||
            Color is FluentUI.Color.Custom)
        {
            throw new ArgumentException("Color.Custom is not supported.");
        }

        if (Appearance != FluentUI.Appearance.Accent &&
            Appearance != FluentUI.Appearance.Lightweight &&
            Appearance != FluentUI.Appearance.Neutral)
        {
            throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral.");
        }

        return base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        GlobalState.OnChange += StateHasChanged;
    }

    /// <summary />
    private Color? GetBackgroundColor()
    {
        if (BackgroundColor != null)
        {
            if (BackgroundColor == FluentUI.Color.Lightweight && GlobalState.Luminance == StandardLuminance.DarkMode)
                return FluentUI.Color.FillInverse;
            if (BackgroundColor == FluentUI.Color.Lightweight && GlobalState.Luminance == StandardLuminance.LightMode)
                return FluentUI.Color.Lightweight;

            return BackgroundColor;
        }

        return Appearance switch
        {
            FluentUI.Appearance.Accent => FluentUI.Color.Accent,
            FluentUI.Appearance.Lightweight => FluentUI.Color.Lightweight,
            FluentUI.Appearance.Neutral => FluentUI.Color.Neutral,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private Color? GetBorderColor()
    {
        if (BackgroundColor != null)
        {
            if (BackgroundColor == FluentUI.Color.Lightweight && GlobalState.Luminance == StandardLuminance.DarkMode)
                return FluentUI.Color.FillInverse;
            if (BackgroundColor == FluentUI.Color.Lightweight && GlobalState.Luminance == StandardLuminance.LightMode)
                return FluentUI.Color.Lightweight;

            return BackgroundColor;

        }

        return Appearance switch
        {
            FluentUI.Appearance.Accent => FluentUI.Color.Accent,
            FluentUI.Appearance.Lightweight => FluentUI.Color.Accent,
            FluentUI.Appearance.Neutral => FluentUI.Color.Neutral,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private Color? GetFontColor()
    {
        if (Color != null)
        {
            if (BackgroundColor == FluentUI.Color.Error && GlobalState.Luminance == StandardLuminance.DarkMode)
                return FluentUI.Color.FillInverse;


            return Color;
        }

        return Appearance switch
        {
            FluentUI.Appearance.Accent => FluentUI.Color.Fill,
            FluentUI.Appearance.Lightweight => FluentUI.Color.FillInverse,
            FluentUI.Appearance.Neutral => FluentUI.Color.Fill,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        }; ;
    }

    public void Dispose()
    {
        GlobalState.OnChange -= StateHasChanged;
    }
}
