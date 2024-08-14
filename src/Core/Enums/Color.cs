// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The color to use for the drawing icons, badges, etc.
/// TODO: WEB3 - To validate if these colors are still valid.
/// </summary>
public enum Color
{
    /// <summary>
    /// Use the '--colorNeutralForeground1' CSS variable color to draw the icon. 
    /// Icon is drawn in the same color as the standard text. 
    /// </summary>
    [Description("var(--colorNeutralForeground1)")]
    Default,

    /// <summary>
    /// Use the '--colorBrandForeground1' CSS variable color.
    /// This is the default value.
    /// </summary>
    [Description("var(--colorBrandForeground1)")]
    Primary,

    /// <summary>
    /// Use the '--colorNeutralForegroundInverted' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForegroundInverted)")]
    Lightweight,

    /// <summary>
    ///  Use the `--colorNeutralForegroundDisabled` CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForegroundDisabled)")]
    Disabled,

    /// <summary>
    /// Use the '--warning' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--warning)")]
    Warning,

    /// <summary>
    /// Use the '--info' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--info)")]
    Info,

    /// <summary>
    /// Use the '--error' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--error)")]
    Error,

    /// <summary>
    /// Use the '--success' CSS variable color.
    /// Note: This color is defined in the variables.css file. If this file is not being used, 
    /// a CSS variable with this name and appropriate value needs to be created.
    /// </summary>
    [Description("var(--success)")]
    Success,

    /// <summary>
    /// Use the '--colorNeutralForeground1' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForeground1)")]
    [Obsolete("Use Default instead.")]
    Fill,

    /// <summary>
    /// Use the '--colorNeutralForegroundInverted' CSS variable color, adapts to light/dark mode.
    /// </summary>
    [Description("var(--colorNeutralForegroundInverted)")]
    [Obsolete("Use Lightweight instead.")]
    FillInverse,

    /// <summary>
    /// Supply an HTML hex color string value (#rrggbb or #rgb) for the CustomColor parameter.
    /// </summary>
    [Description("var(--colorNeutralForeground1)")]
    Custom,
}
