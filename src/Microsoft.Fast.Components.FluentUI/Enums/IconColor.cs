using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The color to use for the icon.
/// </summary>
public enum IconColor
{
    /// <summary>
    /// Use the '--neutral-foreground-rest' CSS variable color to draw the icon. 
    /// Icon is drawn in the same color as the standard text. 
    /// </summary>
    [Description("--neutral-foreground-rest")]
    Neutral,

    /// <summary>
    /// Use the '--accent-fill-rest' CSS variable color.
    /// </summary>
    [Description("--accent-fill-rest")]
    Accent,

    /// <summary>
    /// Use the '--fluentui-warning-fill' CSS variable color.
    /// Note: This color is defined in the fluentui-reboot.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("--fluentui-warning-fill")]
    Warning,

    /// <summary>
    /// Use the '--fluentui-severe-warning-fill' CSS variable color.
    /// Note: This color is defined in the fluentui-reboot.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("--fluentui-severe-warning-fill")]
    SevereWarning,

    /// <summary>
    /// Use the '--fluentui-error-fill' CSS variable color.
    /// Note: This color is defined in the fluentui-reboot.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("--fluentui-error-fill")]
    Error,

    /// <summary>
    /// Use the '--fluentui-success-fill' CSS variable color.
    /// Note: This color is defined in the fluentui-reboot.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("--fluentui-success-fill")]
    Success,



}