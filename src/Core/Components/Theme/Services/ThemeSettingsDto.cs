// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents theme settings serialized for JavaScript interop.
/// </summary>
/// <param name="Color">The base color for the theme.</param>
/// <param name="HueTorsion">The hue adjustment applied to the generated theme.</param>
/// <param name="Vibrancy">The vibrancy adjustment applied to the generated theme.</param>
/// <param name="Mode">The theme mode, or <see langword="null"/> to use the system mode.</param>
/// <param name="IsExact">Whether to use the exact base color.</param>
public sealed record ThemeSettingsDto(
    string Color,
    double HueTorsion,
    double Vibrancy,
    string? Mode,
    bool IsExact);