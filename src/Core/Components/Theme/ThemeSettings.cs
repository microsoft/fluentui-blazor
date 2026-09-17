// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Settings used to create and apply a custom theme.
/// </summary>
/// <param name="Color">The base color for the theme, in hex format (e.g., "#0078D4").</param>
/// <param name="HueTorsion">A number between -0.5 and 0.5 that adjusts the hue of the generated theme.</param>
/// <param name="Vibrancy">A number between -0.5 and 0.5 that adjusts the vibrancy of the generated theme.</param>
/// <param name="Mode">The <see cref="ThemeMode"/> to use when applying the theme.</param>
/// <param name="IsExact">Whether to use the exact color for the brand background.</param>
public sealed record ThemeSettings(
    string Color = "0F6CBD",
    double HueTorsion = 0,
    double Vibrancy = 0,
    ThemeMode Mode = ThemeMode.Light,
    bool IsExact = false);
