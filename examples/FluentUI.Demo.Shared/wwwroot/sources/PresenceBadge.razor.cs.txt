using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

/// <summary>
/// Sizes for presence badge
/// </summary>
public enum BadgeSize
{
    Tiny = 10,
    ExtraSmall = 12,
    Small = 16,
    Medium = 20,
    Large = 24
}


/// <summary>
/// A presence badge is a badge that displays a status indicator such as available, away, or busy.
/// </summary>  
public partial class PresenceBadge : FluentComponentBase, IDisposable
{
    private (string name, string color, IconVariant variant, IconSize size) _config;

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("presencebadge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("left", $"{HorizontalPosition!.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.ltr)
        .AddStyle("right", $"{HorizontalPosition!.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.rtl)
        .AddStyle("bottom", $"{BottomPosition!.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}%", () => BottomPosition.HasValue)
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Child content of component, the content that the badge will be applied to.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The status to show. See <see cref="PresenceStatus"/> for options.
    /// Default is Available
    /// </summary>
    [Parameter, EditorRequired]
    public PresenceStatus Status { get; set; } = PresenceStatus.Available;

    /// <summary>
    /// Left position of the badge (percentage as number).
    /// Default value is 50.
    /// </summary>
    [Parameter]
    public int? HorizontalPosition { get; set; } = 50;

    /// <summary>
    /// Bottom position of the badge (percentage as number).
    /// Default value is -10.
    /// </summary>
    [Parameter]
    public int? BottomPosition { get; set; } = -10;

    /// <summary>
    /// Modifies the display to indicate that the user is out of office. 
    /// This can be combined with any status to display an out-of-office version of that status.
    /// </summary>
    [Parameter]
    public bool OutOfOffice { get; set; } = false;

    /// <summary>
    /// Gets or sets the <see cref="BadgeSize"/> to use.
    /// Default is Small.
    /// </summary>
    [Parameter]
    public BadgeSize Size { get; set; } = BadgeSize.Small;

    protected override Task OnParametersSetAsync()
    {
        _config = GetConfig();

        return base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        GlobalState.OnChange += StateHasChanged;
    }

    private (string name, string color, IconVariant variant, IconSize size) GetConfig()
    {
        (string name, string color, IconVariant variant, IconSize size) config = (FluentIcons.PresenceAvailable, "var(--presence-available)", IconVariant.Filled, IconSize.Size16);

        switch (Status)
        {
            case PresenceStatus.Available:
                config.name = FluentIcons.PresenceAvailable;
                config.color = "var(--presence-available)";
                config.variant = OutOfOffice ? IconVariant.Regular : IconVariant.Filled;
                break;
            case PresenceStatus.Away:
                if (OutOfOffice)
                {
                    config.name = FluentIcons.PresenceOffline;
                    config.variant = IconVariant.Regular;
                }
                else
                {
                    config.name = FluentIcons.PresenceAway;
                    config.variant = IconVariant.Filled;
                }
                config.color = "var(--presence-away)";
                break;
            case PresenceStatus.Busy:
                if (OutOfOffice)
                {
                    config.name = FluentIcons.PresenceUnknown;
                    config.variant = IconVariant.Regular;
                }
                else
                {
                    config.name = FluentIcons.PresenceBusy;
                    config.variant = IconVariant.Filled;
                }
                config.color = "var(--presence-busy)";
                break;
            case PresenceStatus.DoNotDisturb:
                config.name = FluentIcons.PresenceDND;
                config.color = "var(--presence-dnd)";
                config.variant = OutOfOffice ? IconVariant.Regular : IconVariant.Filled;
                break;
            case PresenceStatus.Offline:
                config.name = FluentIcons.PresenceOffline;
                config.color = "var(--presence-offline)";
                config.variant = IconVariant.Regular;
                break;
            case PresenceStatus.OutOfOffice:
                config.name = FluentIcons.PresenceOOF;
                config.color = "var(--presence-oof)";
                config.variant = IconVariant.Regular;
                break;
            case PresenceStatus.Unknown:
                config.name = FluentIcons.PresenceUnknown;
                config.color = "var(--presence-unknown)";
                config.variant = IconVariant.Regular;
                break;
        }

        config.size = Size switch
        {
            BadgeSize.Tiny => IconSize.Size10,
            BadgeSize.ExtraSmall => IconSize.Size12,
            BadgeSize.Small => IconSize.Size16,
            BadgeSize.Medium => IconSize.Size20,
            BadgeSize.Large => IconSize.Size24,
            _ => IconSize.Size16,
        };

        return config;

    }

    public void Dispose()
    {
        GlobalState.OnChange -= StateHasChanged;
    }
}
