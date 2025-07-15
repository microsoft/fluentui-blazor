// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public static class SwatchExtensions
{
    /// <summary>
    /// Gives a RGB hex value for a Swatch
    /// </summary>
    /// <param name="swatch">The Swatch to convert</param>
    /// <returns></returns>
    private static string? ToColorHex(this Swatch swatch)
    {
        var rVal = (int)(swatch.R * 255);
        var gVal = (int)(swatch.G * 255);
        var bVal = (int)(swatch.B * 255);
        var hexVal = $"#{rVal:X2}{gVal:X2}{bVal:X2}";
        return hexVal;
    }
}
