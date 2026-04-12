// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The mode to use for the color picker.
/// </summary>
public enum ColorPickerView
{
    /// <summary>
    /// A palette of predefined colors.
    /// </summary>  
    [Description("swatch-palette")]
    SwatchPalette,

    /// <summary>
    /// A square view of the HSV color space, with a slider to select the hue.
    /// </summary>
    [Description("hsv-square")]
    HsvSquare,

    /// <summary>
    /// A circular view of the HSV color space, with a slider to select the saturation.
    /// </summary>
    [Description("color-wheel")]
    ColorWheel,
}