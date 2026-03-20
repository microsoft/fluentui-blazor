// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a theme API for applying Fluent UI themes via JavaScript interop.
/// </summary>
public interface IThemeService
{

    /// <summary>
    /// Creates a custom theme based on the specified settings.
    /// The returned <see cref="Theme"/> can be modified by the caller before it is applied.
    /// </summary>
    Task<Theme> CreateCustomThemeAsync(ThemeSettings settings);

    /// <summary>
    /// Sets a theme by type using the current effective mode.
    /// </summary>
    /// <param name="type">The <see cref="ThemeColorVariant"/> theme type to apply.</param>
    Task SetThemeAsync(ThemeColorVariant type);

    /// <summary>
    /// Sets a theme by type and mode.
    /// </summary>
    /// <param name="type">The <see cref="ThemeColorVariant"/> theme type to apply.</param>
    /// <param name="mode">The <see cref="ThemeMode"/> mode to use.</param>
    Task SetThemeAsync(ThemeColorVariant type, ThemeMode mode);

    /// <summary>
    /// Sets a custom theme based on the specified brand color using the current effective mode.
    /// </summary>
    /// <param name="color">The base color for the brand theme, in hex format (e.g., "#0078D4").</param>
    /// <param name="isExact">An optional boolean indicating whether to use the exact color for the brand background.</param>
    Task SetThemeAsync(string color, bool isExact = false);

    /// <summary>
    /// Sets a theme by mode using the current effective theme type.
    /// </summary>
    /// <param name="mode">The <see cref="ThemeMode"/> mode to use.</param>
    Task SetThemeAsync(ThemeMode mode);

    /// <summary>
    /// Sets a custom theme based on the specified settings.
    /// </summary>
    Task SetThemeAsync(ThemeSettings settings);

    /// <summary>
    /// Sets a theme.
    /// <see cref="Theme"/> should be initially created with the <see cref="CreateCustomThemeAsync(ThemeSettings)"/> method, can then be modified before applying it here.
    /// </summary>
    Task SetThemeAsync(Theme theme);

    /// <summary>
    /// Removes the stored theme settings from localStorage.
    /// </summary>
    Task ClearStoredThemeSettingsAsync();

    /// <summary>
    /// Returns true if the browser prefers dark mode.
    /// </summary>
    Task<bool> IsSystemDarkAsync();

    /// <summary>
    /// Returns true if the current Fluent UI theme is dark mode.
    /// </summary>
    Task<bool> IsDarkModeAsync();

    /// <summary>
    /// Returns the current, cached, custom brand ramp, or null if no custom ramp has been generated yet.
    /// </summary>
    Task<IReadOnlyDictionary<string, string>?> GetColorRampAsync();

    /// <summary>
    /// Returns a custom brand ramp based on the specified settings, or null if invalid settings provided.
    /// </summary>
    Task<IReadOnlyDictionary<string, string>?> GetColorRampFromSettingsAsync(ThemeSettings settings);

    /// <summary>
    /// Returns the current brand color (default if no custom brand color is set).
    /// </summary>
    Task<string> GetBrandColorAsync();

    /// <summary>
    /// Switches the document direction between left-to-right and right-to-left.
    /// </summary>
    Task SwitchDirectionAsync();

    /// <summary>
    /// Toggles between light and dark mode.
    /// </summary>
    Task<bool> SwitchThemeAsync();

    /// <summary>
    /// Sets a custom theme on a specific element based on the specified <see cref="ThemeSettings"/>.
    /// This does not affect the global theme.
    /// </summary>
    Task SetThemeToElementAsync(ElementReference element, ThemeSettings settings);

    /// <summary>
    /// Sets a custom theme on a specific element based on the specified <see cref="Theme"/>.
    /// This does not affect the global theme.
    /// </summary>
    Task SetThemeToElementAsync(ElementReference element, Theme theme);
}
