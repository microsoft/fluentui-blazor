// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

//using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
//using System.Collections.Generic;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentCounterBadge component is a visual indicator that communicates a value about an associated component.
/// It uses short postive numbers, color, and icons for quick recognition and is placed near the relevant content.
/// </summary>
public partial class FluentPresenceBadge : FluentBadge
{
    /// <summary />
    public FluentPresenceBadge(LibraryConfiguration configuration) : base(configuration) { }

    private bool _isAttached => ChildContent is not null;
    private int _width = 16;
    private static bool _isBusyStatus(PresenceStatus status) => status is PresenceStatus.Busy or PresenceStatus.DoNotDisturb or PresenceStatus.Blocked;

    /// <summary>
    /// Gets or sets the presence status.
    /// </summary>
    [Parameter]
    public PresenceStatus Status { get; set; } = PresenceStatus.Available;

    /// <summary>
    ///  Gets or sets the out of office state.
    /// </summary>
    [Parameter]
    public bool OutOfOffice { get; set; }

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

        // New: Set the icon based on status, out-of-office, and size (mirroring iconMap)
        (Icon IconStart, int width) props = GetIconForPresence(Status, OutOfOffice, Size ?? BadgeSize.Medium);
        IconStart = props.IconStart;
        _width = props.width;

        // New: Set aria-label (mirroring statusText + oofText)
        //var ariaLabel = GetAriaLabel(Status, OutOfOffice);
        //AdditionalAttributes ??= new Dictionary<string, object>();
        //AdditionalAttributes["aria-label"] = ariaLabel;
        //AdditionalAttributes["role"] = "img";  // As per TS hook

        if (Positioning is null && _isAttached)
        {
            Positioning = Components.Positioning.BelowEnd;
        }

        Size ??= AdditionalAttributes?["slot"] is not null ? BadgeSize.ExtraSmall : BadgeSize.Medium;
    }

    /// <summary />
    protected override string? ClassValue => base.ClassValue + " " + GetPresenceClasses();

    /// <summary />
    protected override string? StyleValue => base.StyleValue + " " + GetPresenceStyles();

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
            _ => "var(--colorPaletteLightGreenForeground3)",
        };
    }

    /// <summary />
    protected string? IconStartStyle => "margin: -1px;";  // For icon margin (if IconStart supports style)

    // New: Build conditional classes (equivalent to useStyles and mergeClasses)
    private string? GetPresenceClasses()
    {
        var builder = new CssBuilder()
            .AddClass("fluent-presence-badge")  // Root class name
            .AddClass("status-busy", _isBusyStatus(Status))
            .AddClass("status-away", Status == PresenceStatus.Away)
            .AddClass("status-available", Status == PresenceStatus.Available)
            .AddClass("status-offline", Status == PresenceStatus.Offline)
            .AddClass("status-out-of-office", Status == PresenceStatus.OutOfOffice)
            .AddClass("status-unknown", Status == PresenceStatus.Unknown)
            .AddClass("out-of-office", OutOfOffice)
            .AddClass("out-of-office-available", OutOfOffice && Status == PresenceStatus.Available)
            .AddClass("out-of-office-busy", OutOfOffice && _isBusyStatus(Status))
            .AddClass("out-of-office-status", OutOfOffice && (Status == PresenceStatus.OutOfOffice || Status == PresenceStatus.Away || Status == PresenceStatus.Offline))
            .AddClass("out-of-office-unknown", OutOfOffice && Status == PresenceStatus.Unknown)
            .AddClass("size-tiny", Size == BadgeSize.Tiny)
            .AddClass("size-large", Size == BadgeSize.Large)
            .AddClass("size-extra-large", Size == BadgeSize.ExtraLarge);

        return builder.Build();
    }

    // New: Build base styles (equivalent to useRootClassName)
    private string? GetPresenceStyles()
    {
        var builder = new StyleBuilder()
            .AddStyle("display", "inline-flex")
            .AddStyle("box-sizing", "border-box")
            .AddStyle("align-items", "center")
            .AddStyle("justify-content", "center")
            .AddStyle("border-radius", "var(--borderRadiusCircular)")
            .AddStyle("background-color", "var(--colorNeutralBackground1)")
            .AddStyle("padding", "1px")
            .AddStyle("background-clip", "content-box")
            // Size-specific overrides (from useStyles)
            .AddStyle("aspect-ratio", "1", Size == BadgeSize.Tiny || Size == BadgeSize.Large || Size == BadgeSize.ExtraLarge)
            .AddStyle("width", "6px", Size == BadgeSize.Tiny)
            .AddStyle("background-clip", "unset", Size == BadgeSize.Tiny)
            .AddStyle("width", "20px", Size == BadgeSize.Large)
            .AddStyle("width", "28px", Size == BadgeSize.ExtraLarge);

        return builder.Build();
    }

    private static (Icon, int) GetIconForPresence(PresenceStatus status, bool outOfOffice, BadgeSize size)
    {
        // Map status to string keys (matching TS)
        var statusKey = status switch
        {
            PresenceStatus.Available => "available",
            PresenceStatus.Busy => "busy",
            PresenceStatus.Away => "away",
            PresenceStatus.OutOfOffice => "out-of-office",
            PresenceStatus.Offline => "offline",
            PresenceStatus.DoNotDisturb => "do-not-disturb",
            PresenceStatus.Unknown => "unknown",
            PresenceStatus.Blocked => "blocked",
            _ => "available",
        };

        var width = size switch
        {
            BadgeSize.Tiny => 6,
            BadgeSize.ExtraSmall => 10,
            BadgeSize.Small => 12,
            BadgeSize.Medium => 16,
            BadgeSize.Large => 20,
            BadgeSize.ExtraLarge => 28,
            _ => 16,
        };

        //var color = status switch
        //{
        //    PresenceStatus.Available => "var(--colorPaletteLightGreenForeground3)",
        //    PresenceStatus.Busy => "var(--colorPaletteRedBackground3)",
        //    PresenceStatus.Away => "var(--colorPaletteLightGreenForeground3)",
        //    PresenceStatus.OutOfOffice => "var(--colorPaletteBerryForeground3)",
        //    PresenceStatus.Offline => "var(--colorNeutralForeground3)",
        //    PresenceStatus.DoNotDisturb => "var(--presence-dnd-color)",
        //    PresenceStatus.Unknown => "var(--colorNeutralForeground3)",
        //    PresenceStatus.Blocked => "var(--presence-blocked-color)",
        //    _ => "var(--colorPaletteLightGreenForeground3)",
        //};

        // Icon selection logic (simplified; assumes CoreIcons has presence variants)
        // In TS, it uses filled/regular based on outOfOffice and status.
        // Adjust icon names/sizes to match your CoreIcons library.
        Icon icon = (statusKey, outOfOffice) switch
        {
            ("available", false) => new CoreIcons.Filled.Size20.PresenceAvailable(),  // Filled for available when not OOF
            ("available", true) => new CoreIcons.Regular.Size20.PresenceAvailable(),  // Regular for OOF
            ("away", false) => new CoreIcons.Filled.Size20.PresenceAway(),
            ("away", true) => new CoreIcons.Regular.Size20.PresenceOof(),  // OOF icon for away+OOF
            ("busy", false) => new CoreIcons.Filled.Size20.PresenceBusy(),
            ("busy", true) => new CoreIcons.Regular.Size20.PresenceUnknown(),  // Unknown for busy+OOF
            ("do-not-disturb", false) => new CoreIcons.Filled.Size20.PresenceDnd(),
            ("do-not-disturb", true) => new CoreIcons.Regular.Size20.PresenceDnd(),
            ("offline", false) => new CoreIcons.Regular.Size20.PresenceOffline(),
            ("offline", true) => new CoreIcons.Regular.Size20.PresenceOof(),
            ("out-of-office", _) => new CoreIcons.Regular.Size20.PresenceOof(),
            ("unknown", _) => new CoreIcons.Regular.Size20.PresenceUnknown(),
            ("blocked", _) => new CoreIcons.Regular.Size20.PresenceBlocked(),
            _ => new CoreIcons.Regular.Size20.PresenceUnknown(),
        };
        return (icon, width);
    }

    // New: Method to get aria-label (equivalent to statusText + oofText)
    //private static string GetAriaLabel(PresenceStatus status, bool outOfOffice)
    //{
    //    var statusText = status switch
    //    {
    //        PresenceStatus.Available => "available",
    //        PresenceStatus.Busy => "busy",
    //        PresenceStatus.Away => "away",
    //        PresenceStatus.OutOfOffice => "out of office",
    //        PresenceStatus.Offline => "offline",
    //        PresenceStatus.DoNotDisturb => "do not disturb",
    //        PresenceStatus.Unknown => "unknown",
    //        PresenceStatus.Blocked => "blocked",
    //        _ => "unknown"
    //    };

    //    var oofText = outOfOffice && status != PresenceStatus.OutOfOffice ? " out of office" : "";
    //    return statusText + oofText;
    //}
}
