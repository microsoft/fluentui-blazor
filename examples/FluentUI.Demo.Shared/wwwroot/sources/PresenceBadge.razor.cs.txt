using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

/// <summary />
public partial class PresenceBadge : FluentComponentBase, IDisposable
{
    private (string name, string color, IconVariant variant) _config;

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("presencebadge")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("left", $"{HorizontalPosition}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.ltr)
        .AddStyle("right", $"{HorizontalPosition}%", () => HorizontalPosition.HasValue && GlobalState.Dir == LocalizationDirection.rtl)
        .AddStyle("bottom", $"{BottomPosition}%", () => BottomPosition.HasValue)
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Child content of component, the content that the badge will apply to.
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
    /// Left position of the badge in percentage.
    /// Default value is 50.
    /// </summary>
    [Parameter]
    public int? HorizontalPosition { get; set; } = 50;

    /// <summary>
    /// Bottom position of the badge in percentage.
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

    protected override Task OnParametersSetAsync()
    {
        _config = GetConfig();

        return base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        GlobalState.OnChange += StateHasChanged;
    }

    private (string name, string color, IconVariant variant) GetConfig()
    {
        (string name, string color, IconVariant variant) config = (FluentIcons.PresenceAvailable, "var(--presence-available)", IconVariant.Filled);

        switch (Status)
        {
            case PresenceStatus.Available:
                config = (FluentIcons.PresenceAvailable, "var(--presence-available)", OutOfOffice ? IconVariant.Regular : IconVariant.Filled);
                break;
            case PresenceStatus.Away:
                config = OutOfOffice ? (FluentIcons.PresenceOffline, "var(--presence-away)", IconVariant.Regular) : (FluentIcons.PresenceAway, "var(--presence-away)", IconVariant.Filled);
                break;
            case PresenceStatus.Busy:
                config = OutOfOffice ? (FluentIcons.PresenceUnknown, "var(--presence-busy)", IconVariant.Regular) : (FluentIcons.PresenceBusy, "var(--presence-busy)", IconVariant.Filled);
                break;
            case PresenceStatus.DoNotDisturb:
                config = (FluentIcons.PresenceDND, "var(--presence-dnd)", OutOfOffice ? IconVariant.Regular : IconVariant.Filled);
                break;
            case PresenceStatus.Offline:
                config = (FluentIcons.PresenceOffline, "var(--presence-offline)", IconVariant.Regular);
                break;
            case PresenceStatus.OutOfOffice:
                config = (FluentIcons.PresenceOOF, "var(--presence-oof)", IconVariant.Regular);
                break;
            case PresenceStatus.Unknown:
                config = (FluentIcons.PresenceUnknown, "var(--presence-unknown)", IconVariant.Regular);
                break;
        }

        return config;

    }

    public void Dispose()
    {
        GlobalState.OnChange -= StateHasChanged;
    }
}
