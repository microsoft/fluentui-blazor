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
    /// A square view of the HSV color space, with a slider to select the hue. This is the default view.
    /// </summary>
    [Description("hsv-square")]
    HsvSquare,

    /// <summary>
    /// A palette of predefined colors.
    /// </summary>  
    [Description("swatch-palette")]
    SwatchPalette,

    /// <summary>
    /// A circular view of the HSV color space, with a slider to select the saturation. This view is not yet implemented.
    /// </summary>
    [Description("color-wheel")]
    ColorWheel,
}