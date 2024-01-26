using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A presence badge is a badge that displays a status indicator such as available, away, or busy.
/// </summary>  
public partial class FluentPresenceBadge : FluentComponentBase, IDisposable
{
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-presence-badge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// Child content of component, the content that the badge will be applied to.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the title to show on hover the component.
    /// If not provided, the <see cref="StatusTitle"/> will be used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the status to show. See <see cref="PresenceStatus"/> for options.
    /// </summary>
    [Parameter]
    public PresenceStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the title to show on hover the status. If not provided, the status will be used.
    /// </summary>
    [Parameter]
    public string? StatusTitle { get; set; }

    /// <summary>
    /// Modifies the display to indicate that the user is out of office. 
    /// This can be combined with any status to display an out-of-office version of that status.
    /// </summary>
    [Parameter]
    public bool OutOfOffice { get; set; } = false;

    /// <summary>
    /// Gets or sets the <see cref="Status"/> size to use.
    /// Default is Small.
    /// </summary>
    [Parameter]
    public PresenceBadgeSize Size { get; set; } = PresenceBadgeSize.Small;

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
