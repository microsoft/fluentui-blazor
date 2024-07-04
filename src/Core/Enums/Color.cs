using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The color to use for the drawing icons, badges, etc.
/// </summary>
public enum Color
{
    /// <summary>
    /// Use the '--neutral-foreground-rest' CSS variable color to draw the icon. 
    /// Icon is drawn in the same color as the standard text. 
    /// </summary>
    [Display(Name = "var(--neutral-foreground-rest)")]
    Neutral,

    /// <summary>
    /// Use the '--accent-fill-rest' CSS variable color.
    /// This is the default value.
    /// </summary>
    [Display(Name = "var(--accent-fill-rest)")]
    Accent,

    /// <summary>
    /// Use the '--warning' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Display(Name = "var(--warning)")]
    Warning,

    /// <summary>
    /// Use the '--info' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Display(Name = "var(--info)")]
    Info,

    /// <summary>
    /// Use the '--error' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Display(Name = "var(--error)")]
    Error,

    /// <summary>
    /// Use the '--success' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Display(Name = "var(--success)")]
    Success,

    /// <summary>
    /// Use the '--neutral-fill-rest' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Display(Name = "var(--neutral-fill-rest)")]
    Fill,

    /// <summary>
    /// Use the '--neutral-fill-inverse-rest' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Display(Name = "var(--neutral-fill-inverse-rest)")]
    FillInverse,

    /// <summary>
    /// Use the '--neutral-layer-1' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Display(Name = "var(--neutral-layer-1)")]
    Lightweight,

    /// <summary>
    ///  Use the --neutral-stroke-rest CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Display(Name = "var(--neutral-stroke-rest)")]
    Disabled,

    /// <summary>
    /// Supply an HTML hex color string value (#rrggbb or #rgb) for the CustomColor parameter.
    /// </summary>
    [Display(Name = "var(--neutral-foreground-rest)")]
    Custom,
}
