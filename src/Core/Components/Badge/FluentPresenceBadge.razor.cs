// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentCounterBadge component is a visual indicator that communicates a value about an associated component.
/// It uses short postive numbers, color, and icons for quick recognition and is placed near the relevant content.
/// </summary>
public partial class FluentPresenceBadge : FluentBadge
{
    private int _iconWidth;
    private string _ariaLabel = string.Empty;

    private bool _isAttached => ChildContent is not null;

    /// <summary />
    protected override string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-presence-badge")
        .Build();

    /// <summary />
    protected override string? StyleValue => DefaultStyleBuilder
        .AddStyle("aspect-ratio", "1", Size == BadgeSize.Tiny || Size == BadgeSize.Large || Size == BadgeSize.ExtraLarge)
        .AddStyle("width", "6px", Size == BadgeSize.Tiny)
        .AddStyle("background-clip", "unset", Size == BadgeSize.Tiny)
        .AddStyle("width", "20px", Size == BadgeSize.Large)
        .AddStyle("width", "28px", Size == BadgeSize.ExtraLarge)
        .Build();

    /// <summary />
    public FluentPresenceBadge(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary>
    /// Gets or sets the presence status.
    /// </summary>
    [Parameter]
    public PresenceStatus? Status { get; set; } = PresenceStatus.Available;

    /// <summary>
    ///  Gets or sets the out of office state.
    /// </summary>
    [Parameter]
    public bool OutOfOffice { get; set; }

    /// <summary>
    /// Gets or sets the slot where the badge is displayed in.
    /// Only FluentSlot.Badge is supported.
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets whether the status is considered busy.
    /// </summary>
    /// <param name="status">The status to check.</param>
    /// <returns></returns>
    public bool IsBusyStatus(PresenceStatus? status) => status is PresenceStatus.Busy or PresenceStatus.DoNotDisturb or PresenceStatus.Blocked;

    /// <summary />
    protected override void OnInitialized()
    {
        if (!string.IsNullOrWhiteSpace(BackgroundColor))
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the BackgroundColor parameter.");
        }

        if (Appearance is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the Appearance parameter.");
        }

        if (Shape is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the Shape parameter.");
        }

        if (Content is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the Content parameter.");
        }

        if (Color is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the Color parameter.");
        }

        if (IconStart is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the IconStart parameter.");
        }

        if (IconEnd is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the IconEnd parameter.");
        }

        if (IconLabel is not null)
        {
            throw new ArgumentException("FluentPresenceBadge does not support setting the IconLabel parameter.");
        }

        if (Positioning is null && _isAttached)
        {
            Positioning = Components.Positioning.BelowEnd;
        }

        _ariaLabel = GetAriaLabel(Status, OutOfOffice);
        _iconWidth = GetIconSize(Size);
        IconStart = GetPresenceIcon(Status, OutOfOffice);
        Size ??= Slot is FluentSlot.Badge ? BadgeSize.ExtraSmall : null;
    }

    /// <summary />
    protected override string GetIconColor()
    {
        return Status switch
        {
            PresenceStatus.Available => "var(--colorPaletteLightGreenForeground3)",
            PresenceStatus.Away => OutOfOffice ? "var(--colorPaletteBerryForeground3)" : "var(--colorPaletteMarigoldBackground3)",
            PresenceStatus.Busy => "var(--colorPaletteRedBackground3)",
            PresenceStatus.DoNotDisturb => "var(--colorPaletteRedBackground3)",
            PresenceStatus.Offline => OutOfOffice ? "var(--colorPaletteBerryForeground3)" : "var(--colorNeutralForeground3)",
            PresenceStatus.OutOfOffice => "var(--colorPaletteBerryForeground3)",
            PresenceStatus.Blocked => "var(--colorPaletteRedBackground3)",
            PresenceStatus.Unknown => "var(--colorNeutralForeground3)",
            _ => "var(--colorNeutralForeground3)",
        };
    }

    private static int GetIconSize(BadgeSize? size)
    {
        return size switch
        {
            BadgeSize.Tiny => 6,
            BadgeSize.ExtraSmall => 10,
            BadgeSize.Small => 12,
            BadgeSize.Medium => 16,
            BadgeSize.Large => 20,
            BadgeSize.ExtraLarge => 28,
            _ => 16,
        };
    }

    private static Icon GetPresenceIcon(PresenceStatus? status, bool outOfOffice)
    {
        return (status, outOfOffice) switch
        {
            (PresenceStatus.Available, false) => new CoreIcons.Filled.Size20.PresenceAvailable(),  // Filled for available when not OOF
            (PresenceStatus.Available, true) => new CoreIcons.Regular.Size20.PresenceAvailable(),  // Regular for OOF
            (PresenceStatus.Away, false) => new CoreIcons.Filled.Size20.PresenceAway(),
            (PresenceStatus.Away, true) => new CoreIcons.Regular.Size20.PresenceOof(),  // OOF icon for away+OOF
            (PresenceStatus.Busy, false) => new CoreIcons.Filled.Size20.PresenceBusy(),
            (PresenceStatus.Busy, true) => new CoreIcons.Regular.Size20.PresenceUnknown(),  // Unknown for busy+OOF
            (PresenceStatus.DoNotDisturb, false) => new CoreIcons.Filled.Size20.PresenceDnd(),
            (PresenceStatus.DoNotDisturb, true) => new CoreIcons.Regular.Size20.PresenceDnd(),
            (PresenceStatus.Offline, false) => new CoreIcons.Regular.Size20.PresenceOffline(),
            (PresenceStatus.Offline, true) => new CoreIcons.Regular.Size20.PresenceOof(),
            (PresenceStatus.OutOfOffice, _) => new CoreIcons.Regular.Size20.PresenceOof(),
            (PresenceStatus.Unknown, _) => new CoreIcons.Regular.Size20.PresenceUnknown(),
            (PresenceStatus.Blocked, _) => new CoreIcons.Regular.Size20.PresenceBlocked(),
            _ => new CoreIcons.Regular.Size20.PresenceUnknown(),
        };
    }

    private string GetAriaLabel(PresenceStatus? status, bool outOfOffice)
    {
        var statusText = status switch
        {
            PresenceStatus.Available => Localizer[Localization.LanguageResource.PresenceStatus_Available],
            PresenceStatus.Busy => Localizer[Localization.LanguageResource.PresenceStatus_Busy],
            PresenceStatus.Away => Localizer[Localization.LanguageResource.PresenceStatus_Away],
            PresenceStatus.OutOfOffice => Localizer[Localization.LanguageResource.PresenceStatus_OutOfOffice],
            PresenceStatus.Offline => Localizer[Localization.LanguageResource.PresenceStatus_Offline],
            PresenceStatus.DoNotDisturb => Localizer[Localization.LanguageResource.PresenceStatus_DoNotDisturb],
            PresenceStatus.Unknown => Localizer[Localization.LanguageResource.PresenceStatus_Unknown],
            PresenceStatus.Blocked => Localizer[Localization.LanguageResource.PresenceStatus_Blocked],
            _ => Localizer[Localization.LanguageResource.PresenceStatus_Unknown],
        };

        var oofText = outOfOffice && status != PresenceStatus.OutOfOffice ? $" {Localizer[Localization.LanguageResource.PresenceStatus_OutOfOffice]}" : "";
        return statusText + oofText;
    }
}
