// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.FluentUI.AspNetCore.Components.ColorPicker;

/// <summary>
/// Represents a color in the HSV (Hue, Saturation, Value) color space.
/// </summary>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct HsvColor(double Hue, double Saturation, double Value)
{
    /// <summary>
    /// Gets the default HSV color (red, fully saturated, full brightness).
    /// </summary>
    public static HsvColor Default => new(0, 1, 1);

    /// <summary>
    /// Creates an <see cref="HsvColor"/> from a hexadecimal color string (e.g. "#FF0000").
    /// </summary>
    public static HsvColor FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        if (hex.Length < 6)
        {
            return Default;
        }

        var r = int.Parse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
        var g = int.Parse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
        var b = int.Parse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        double h = 0;
        if (delta > 0)
        {
            if (max == r)
            {
                h = 60.0 * ((g - b) / delta % 6);
            }
            else if (max == g)
            {
                h = 60.0 * ((b - r) / delta + 2);
            }
            else
            {
                h = 60.0 * ((r - g) / delta + 4);
            }
        }

        if (h < 0)
        {
            h += 360;
        }

        var s = max == 0 ? 0 : delta / max;
        var v = max;

        return new HsvColor(h, s, v);
    }

    /// <summary>
    /// Gets the CSS background-color style for the HSV square.
    /// </summary>
    public string SquareStyle => $"background-color: hsl({Hue.ToInvariant()}, 100%, 50%);";

    /// <summary>
    /// Gets the CSS left position of the HSV indicator.
    /// </summary>
    public string IndicatorLeft => $"{(Saturation * 100).ToInvariant()}%";

    /// <summary>
    /// Gets the CSS top position of the HSV indicator.
    /// </summary>
    public string IndicatorTop => $"{((1 - Value) * 100).ToInvariant()}%";

    /// <summary>
    /// Gets the CSS top position of the hue indicator.
    /// </summary>
    public string HueIndicatorTop => $"{(Hue / 360.0 * 100).ToInvariant()}%";
}
