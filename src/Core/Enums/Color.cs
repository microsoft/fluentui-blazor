using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The color to use for the drawing icons, badges, etc.
/// </summary>
public enum Color
{
    /// <summary>
    /// Use the '--colorNeutralForeground1Rest' CSS variable color to draw the icon.
    /// Icon is drawn in the same color as the standard text.
    /// </summary>
    [Description("var(--colorNeutralForeground1Rest)")]
    Neutral,

    /// <summary>
    /// Use the '--colorBrandForeground1' CSS variable color.
    /// This is the default value.
    /// </summary>
    [Description("var(--colorBrandForeground1)")]
    Accent,

    /// <summary>
    /// Use the '--colorStatusWarningForeground1' CSS variable color.
    /// </summary>
    [Description("var(--colorStatusWarningForeground1)")]
    Warning,

    /// <summary>
    /// Use the '--colorPaletteAnchorForeground2' CSS variable color.
    /// </summary>
    [Description("var(--colorPaletteAnchorBackground2)")]
    Info,

    /// <summary>
    /// Use the '--colorStatusDagerForeground1' CSS variable color.
    /// </summary>
    [Description("var(--colorStatusDangerForeground1)")]
    Error,

    /// <summary>
    /// Use the '--colorStatusSuccessForeground1' CSS variable color.
    /// </summary>
    [Description("var(--colorStatusSuccessForeground1)")]
    Success,

    /// <summary>
    /// Use the '-colorNeutralForeground1Rest' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForeground1Rest)")]
    Fill,

    /// <summary>
    /// Use the '--colorNeutralForegroundStaticInverted' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForegroundStaticInverted)")]
    FillInverse,

    /// <summary>
    /// Use the '--colorNeutralStroke2' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralStroke2)")]
    Lightweight,

    /// <summary>
    ///  Use the --colorNeutralStrokeDisabled CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralStrokeDisabled)")]
    Disabled,

    /// <summary>
    /// Supply an HTML hex color string value (#rrggbb or #rgb) for the CustomColor parameter.
    /// </summary>
    [Description("var(--colorNeutralForeground1Rest)")]
    Custom,
}
