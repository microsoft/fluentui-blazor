using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentCounterBadge : FluentComponentBase, IDisposable
{
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluentui-counterbadge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("left", $"{HorizontalPosition.ToString(CultureInfo.InvariantCulture)}%", () => GlobalState.Dir == LocalizationDirection.LeftToRight)
        .AddStyle("right", $"{HorizontalPosition.ToString(CultureInfo.InvariantCulture)}%", () => GlobalState.Dir == LocalizationDirection.RightToLeft)
        .AddStyle("bottom", $"{VerticalPosition.ToString(CultureInfo.InvariantCulture)}%")
        .AddStyle("background-color", GetBackgroundColor().ToAttributeValue())
        .AddStyle("color", GetFontColor().ToAttributeValue())
        .AddStyle("border", $"1px solid {GetBorderColor().ToAttributeValue()}")
        .Build();

    /// <summary>
    /// Gets or sets the child content of component, the content that the badge will apply to.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the number displayed inside the badge.
    /// Can be enriched with a plus sign with <see cref="ShowOverflow"/>
    /// </summary>
    [Parameter]
    public int? Count { get; set; }

    /// <summary>
    /// Gets or sets the content you want inside the badge, to customize the badge content.
    /// </summary>
    [Obsolete("This parameter will be removed in a future version. Use BadgeTemplate instead.")]
    [Parameter]
    public RenderFragment? BadgeContent { get; set; }

    /// <summary>
    /// Gets or sets the content you want inside the badge, to customize the badge content.
    /// </summary>
    [Parameter]
    public RenderFragment<int?>? BadgeTemplate{ get; set; }

    /// <summary>
    /// Gets or sets the maximum number that can be displayed inside the badge.
    /// Default is 99.
    /// </summary>
    [Parameter]
    public int? Max { get; set; } = 99;

    /// <summary>
    /// Gets or sets the horizontal position of the badge in percentage in relation to the left of the container (right in RTL).
    /// Default value is 60 (80 when Dot=true).
    /// </summary>
    [Parameter]
    public int HorizontalPosition { get; set; }

    /// <summary>
    /// Gets or sets the bottom position of the badge in percentage.
    /// [Obsolete] This parameter will be removed in a future version. Use VerticalPosition instead.
    /// Default value is 60 (80 when Dot=true).
    /// </summary>
    [Obsolete("This parameter will be removed in a future version. Use VerticalPosition instead.")]
    [Parameter]
    public int BottomPosition
    {
        get => VerticalPosition;
        set => VerticalPosition = value;
    }

    /// <summary>
    /// Gets or sets the vertical position of the badge in percentage in relation to the bottom of the container.
    /// Default value is 60 (80 when Dot=true).
    /// </summary>
    [Parameter]
    public int VerticalPosition { get; set; }

    /// <summary>
    /// Gets or sets the default design of this badge using colors from theme.
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = AspNetCore.Components.Appearance.Accent;

    /// <summary>
    /// Gets or sets the background color to replace the color inferred from property.
    /// </summary>
    [Parameter]
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the font color to replace the color inferred from property.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    /// <summary>
    ///  Gets or sets if just a dot should be displayed. Count will be ignored if this is set to true.
    ///  Defaults to false.
    /// </summary>
    [Parameter]
    public bool Dot { get; set; } = false;

    /// <summary>
    /// If the counter badge should be displayed when the count is zero.
    /// Defaults to false.
    /// [Obsolete] This parameter will be removed in a future version. Use ShowWhen with a lambda expression instead
    /// For getting the same behavior as before, you can use ShowWhen="@(Count => Count >= 0)"
    ///</summary>
    [Obsolete("This parameter will be removed in a future version. Use ShowWhen with a lambda expression instead")]
    [Parameter]
    public bool ShowZero { get; set; } = false;

    /// <summary>
    /// Gets or sets if the counter badge is displayed based on the specified lambda expression.
    /// For example to show the badge when the count greater than 4, use ShowWhen=@(Count => Count > 4)
    /// Default the badge shows when the count is greater than 0.
    /// </summary>
    [Parameter]
    public Func<int?, bool> ShowWhen { get; set; } = Count => Count > 0;

    /// <summary>
    /// If an plus sign should be displayed when the <see cref="Count"/> is greater than <see cref="Max"/>.
    /// Defaults to true.
    ///</summary>
    [Parameter]
    public bool ShowOverflow { get; set; } = true;

    protected override Task OnParametersSetAsync()
    {
        if ((BackgroundColor is not null && Color is null) || (BackgroundColor is null && Color is not null))
        {
            throw new ArgumentException("Both BackgroundColor and Color must be set.");
        }
        if (BackgroundColor is AspNetCore.Components.Color.Custom ||
            Color is AspNetCore.Components.Color.Custom)
        {
            throw new ArgumentException("Color.Custom is not supported.");
        }

        if (Appearance != AspNetCore.Components.Appearance.Accent &&
            Appearance != AspNetCore.Components.Appearance.Lightweight &&
            Appearance != AspNetCore.Components.Appearance.Neutral)
        {
            throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral.");
        }

        return base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        HorizontalPosition = Dot ? 80 : 60;
        VerticalPosition = Dot ? 80 : 60;
        GlobalState.OnChange += StateHasChanged;
    }

    /// <summary />
    private Color? GetBackgroundColor()
    {
        if (BackgroundColor != null)
        {
            if (BackgroundColor == AspNetCore.Components.Color.Lightweight && GlobalState.Luminance == StandardLuminance.DarkMode)
            {
                return AspNetCore.Components.Color.FillInverse;
            }

            if (BackgroundColor == AspNetCore.Components.Color.Lightweight && GlobalState.Luminance == StandardLuminance.LightMode)
            {
                return AspNetCore.Components.Color.Lightweight;
            }

            return BackgroundColor;
        }

        return Appearance switch
        {
            AspNetCore.Components.Appearance.Accent => AspNetCore.Components.Color.Accent,
            AspNetCore.Components.Appearance.Lightweight => AspNetCore.Components.Color.Lightweight,
            AspNetCore.Components.Appearance.Neutral => AspNetCore.Components.Color.Neutral,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private Color? GetBorderColor()
    {
        if (BackgroundColor != null)
        {
            if (BackgroundColor == AspNetCore.Components.Color.Lightweight && GlobalState.Luminance == StandardLuminance.DarkMode)
            {
                return AspNetCore.Components.Color.FillInverse;
            }

            if (BackgroundColor == AspNetCore.Components.Color.Lightweight && GlobalState.Luminance == StandardLuminance.LightMode)
            {
                return AspNetCore.Components.Color.Lightweight;
            }

            return BackgroundColor;

        }

        return Appearance switch
        {
            AspNetCore.Components.Appearance.Accent => AspNetCore.Components.Color.Accent,
            AspNetCore.Components.Appearance.Lightweight => AspNetCore.Components.Color.Accent,
            AspNetCore.Components.Appearance.Neutral => AspNetCore.Components.Color.Neutral,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        };
    }

    /// <summary />
    private Color? GetFontColor()
    {
        if (Color != null)
        {
            if (BackgroundColor == AspNetCore.Components.Color.Error && GlobalState.Luminance == StandardLuminance.DarkMode)
            {
                return AspNetCore.Components.Color.FillInverse;
            }

            return Color;
        }

        return Appearance switch
        {
            AspNetCore.Components.Appearance.Accent => AspNetCore.Components.Color.Fill,
            AspNetCore.Components.Appearance.Lightweight => AspNetCore.Components.Color.FillInverse,
            AspNetCore.Components.Appearance.Neutral => AspNetCore.Components.Color.Fill,
            _ => throw new ArgumentException("CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral."),
        }; ;
    }

    public void Dispose()
    {
        GlobalState.OnChange -= StateHasChanged;
    }
}
