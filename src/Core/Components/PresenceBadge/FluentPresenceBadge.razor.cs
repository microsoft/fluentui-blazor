using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;


/// <summary>
/// A presence badge is a badge that displays a status indicator such as available, away, or busy.
/// </summary>  
public partial class FluentPresenceBadge : FluentComponentBase, IDisposable
{
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("presencebadge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("left", $"{HorizontalPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.ltr)
        .AddStyle("right", $"{HorizontalPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.rtl)
        .AddStyle("bottom", $"{BottomPosition!.Value.ToString(CultureInfo.InvariantCulture)}%", () => BottomPosition.HasValue)
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
    /// Gets or sets the <see cref="PresenceBadgeSize"/> to use.
    /// Default is Small.
    /// </summary>
    [Parameter]
    public PresenceBadgeSize Size { get; set; } = PresenceBadgeSize.Small;

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        GlobalState.OnChange += StateHasChanged;
    }

    private Icon GetIconInstance()
    {
        return Status switch
        {
            PresenceStatus.Available => OutOfOffice
                                 ? new CoreIcons.Regular.Size24.PresenceAvailable()
                                 : new CoreIcons.Filled.Size24.PresenceAvailable(),

            PresenceStatus.Busy => OutOfOffice
                                 ? new CoreIcons.Regular.Size24.PresenceUnknown()
                                 : new CoreIcons.Filled.Size24.PresenceBusy(),

            PresenceStatus.OutOfOffice => new CoreIcons.Regular.Size24.PresenceOof(),

            PresenceStatus.Away => OutOfOffice
                                 ? new CoreIcons.Regular.Size24.PresenceOffline()
                                 : new CoreIcons.Filled.Size24.PresenceAway(),

            PresenceStatus.Offline => new CoreIcons.Regular.Size24.PresenceOffline(),

            PresenceStatus.DoNotDisturb => OutOfOffice
                                 ? new CoreIcons.Regular.Size24.PresenceDnd()
                                 : new CoreIcons.Filled.Size24.PresenceDnd(),

            PresenceStatus.Unknown => new CoreIcons.Regular.Size24.PresenceUnknown(),

            _ => new CoreIcons.Regular.Size24.PresenceUnknown(),
        };
    }

    private string GetIconColor()
    {
        return Status switch
        {
            PresenceStatus.Available => "var(--presence-available)",
            PresenceStatus.Busy => "var(--presence-busy)",
            PresenceStatus.OutOfOffice => "var(--presence-oof)",
            PresenceStatus.Away => "var(--presence-away)",
            PresenceStatus.Offline => "var(--presence-offline)",
            PresenceStatus.DoNotDisturb => "var(--presence-dnd)",
            PresenceStatus.Unknown => "var(--presence-unknown)",
            _ => "var(--presence-unknown)",
        };
    }

    private string GetIconSize()
    {
        return Size switch
        {
            PresenceBadgeSize.Tiny => "10px",
            PresenceBadgeSize.ExtraSmall => "12px",
            PresenceBadgeSize.Small => "16px",
            PresenceBadgeSize.Medium => "20px",
            PresenceBadgeSize.Large => "24px",
            _ => "16px",
        };
    }

    public void Dispose()
    {
        GlobalState.OnChange -= StateHasChanged;
    }
}
