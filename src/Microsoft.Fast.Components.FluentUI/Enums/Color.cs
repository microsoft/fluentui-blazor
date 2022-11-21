using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The color to use for the drawing icons, badges, etc.
/// </summary>
public enum Color
{
    /// <summary>
    /// Use the '--neutral-foreground-rest' CSS variable color to draw the icon. 
    /// Icon is drawn in the same color as the standard text. 
    /// </summary>
    [Description("var(--neutral-foreground-rest)")]
    Neutral,

    /// <summary>
    /// Use the '--accent-fill-rest' CSS variable color.
    /// This is the default value.
    /// </summary>
    [Description("var(--accent-fill-rest)")]
    Accent,

    /// <summary>
    /// Use the '--warning-fill' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--warning-fill)")]
    Warning,

    /// <summary>
    /// Use the '--severe-warning-fill' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--severe-warning-fill)")]
    SevereWarning,

    /// <summary>
    /// Use the '--error-fill' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--error-fill)")]
    Error,

    /// <summary>
    /// Use the '--success-fill' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--success-fill)")]
    Success,

    /// <summary>
    /// Use the '--neutral-fill-rest' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--neutral-fill-rest)")]
    Fill,

    /// <summary>
    /// Use the '--neutral-fill-inverse-rest' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--neutral-fill-inverse-rest)")]
    FillInverse,

    /// <summary>
    /// Use the '--neutral-layer-1' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--neutral-layer-1)")]
    Lightweight,

    /// <summary>
    /// Supply an HTML hex color string value (#rrggbb or #rgb) for the CustomColor parameter.
    /// </summary>
    [Description("var(--neutral-foreground-rest)")]
    Custom,
}