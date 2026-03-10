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
    /// Creates a custom theme based on the specified settings.
    /// The returned dictionary can be modified by the caller before it is applied.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation", Justification = "The returned dictionary is meant to be modified by the caller.")]
    Task<Dictionary<string, string>?> CreateCustomThemeAsync(ThemeSettings settings);

    /// <summary>
    /// Sets a theme by type using the current effective mode.
    /// </summary>
    /// <param name="type">The <see cref="ThemeType"/> theme type to apply.</param>
    Task SetThemeAsync(ThemeType type);

    /// <summary>
    /// Sets a theme by type and mode.
    /// </summary>
    /// <param name="type">The <see cref="ThemeType"/> theme type to apply.</param>
    /// <param name="mode">The <see cref="ThemeMode"/> mode to use.</param>
    Task SetThemeAsync(ThemeType type, ThemeMode mode);

    /// <summary>
    /// Sets a custom theme based on the specified brand color using the current effective mode.
    /// </summary>
    /// <param name="color">The base color for the brand theme, in hex format (e.g., "#0078D4").</param>
    /// <param name="isExact">An optional boolean indicating whether to use the exact color for the brand background.</param>
    Task SetThemeAsync(string color, bool isExact = false);

    /// <summary>
    /// Sets a custom theme based on the specified settings.
    /// </summary>
    Task SetThemeAsync(ThemeSettings settings);

    /// <summary>
    /// Sets a theme represented by a dictionary of tokens.
    /// Dictionary should be initially created with the <see cref="CreateCustomThemeAsync(ThemeSettings)"/> method, can then be modified before
    /// applying it here.
    /// </summary>
    Task SetThemeAsync(IReadOnlyDictionary<string, string> theme);

    /// <summary>
    /// Removes the stored theme settings from localStorage.
    /// </summary>
    Task ClearThemeSettingsAsync();

    /// <summary>
    /// Returns true if the browser prefers dark mode.
    /// </summary>
    Task<bool> IsSystemDarkAsync();

    /// <summary>
    /// Returns true if the current Fluent UI theme is dark mode.
    /// </summary>
    Task<bool> IsDarkModeAsync();

    /// <summary>
    /// Returns the currently cached custom brand ramp, or null if no custom ramp has been generated yet.
    /// </summary>
    Task<IReadOnlyDictionary<string, string>?> GetCurrentColorRampAsync();

    /// <summary>
    /// Switches the document direction between left-to-right and right-to-left.
    /// </summary>
    Task SwitchDirectionAsync();

    /// <summary>
    /// Toggles between light and dark mode.
    /// </summary>
    Task<bool> SwitchThemeAsync();
}
