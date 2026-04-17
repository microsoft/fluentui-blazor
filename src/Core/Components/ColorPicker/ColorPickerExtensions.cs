// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components.ColorPicker;

/// <summary>
/// Extension methods for the FluentColorPicker component.
/// </summary>
internal static class ColorPickerExtensions
{
    /// <summary>
    /// Formats a <see cref="double"/> value as an invariant string with one decimal place.
    /// </summary>
    internal static string ToInvariant(this double value)
    {
        return value.ToString("F1", CultureInfo.InvariantCulture);
    }
}