// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a theme API for applying Fluent UI themes via JavaScript interop.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Creates a custom theme based on the specified brand color and customizations.
    /// The returned dictionary can be modified by the caller before it is applied.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation", Justification = "The returned dictionary is meant to be modified by the caller.")]
    ValueTask<Dictionary<string, string>?> CreateCustomThemeAsync(string color, double hueTorsion, double vibrancy, bool isDark, bool isExact = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a theme by type using the current effective mode.
    /// <param name="type">The <see cref="ThemeType"/> theme type to apply.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask SetThemeAsync(ThemeType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a theme by type and mode.
    /// <param name="type">The <see cref="ThemeType"/> theme type to apply.</param>
    /// <param name="mode">The <see cref="ThemeMode"/> mode to use.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask SetThemeAsync(ThemeType type, ThemeMode mode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a custom theme based on the specified brand color using the current effective mode.
    /// <param name="color">The base color for the brand theme, in hex format (e.g., "#0078D4").</param>
    /// <param name="isExact">An optional boolean indicating whether to use the exact color for the brand background.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask SetThemeAsync(string color, bool isExact = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a custom theme based on the specified brand color and customizations.
    /// <param name="color">The base color for the brand theme, in hex format (e.g., "#0078D4").</param>
    /// <param name="hueTorsion">A number between -0.5 and 0.5 that adjusts the hue of the generated theme.</param>
    /// <param name="vibrancy">A number between -0.5 and 0.5 that adjusts the vibrancy of the generated theme.</param>
    /// <param name="mode">The <see cref="ThemeMode"/> mode to use.</param>
    /// <param name="isExact">An optional boolean indicating whether to use the exact color for the brand background.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask SetThemeAsync(string color, double hueTorsion, double vibrancy, ThemeMode mode, bool isExact = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a theme represented by a dictionary of tokens.
    /// Dictionary should be initially create with the <see cref="CreateCustomThemeAsync"/> method, can then be modified before
    /// applying it here.
    /// </summary>
    ValueTask SetThemeAsync(IReadOnlyDictionary<string, string> theme, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the stored theme settings from localStorage.
    /// </summary>
    ValueTask ClearThemeSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns true if the browser prefers dark mode.
    /// </summary>
    ValueTask<bool> IsSystemDarkAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns true if the current Fluent UI theme is dark mode.
    /// </summary>
    ValueTask<bool> IsDarkModeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the currently cached custom brand ramp, or null if no custom ramp has been generated yet.
    /// </summary>
    ValueTask<IReadOnlyDictionary<string, string>?> GetCachedRampAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Switches the document direction between left-to-right and right-to-left.
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask SwitchDirectionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles between light and dark mode.
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// </summary>
    ValueTask<bool> SwitchThemeAsync(CancellationToken cancellationToken = default);
}
