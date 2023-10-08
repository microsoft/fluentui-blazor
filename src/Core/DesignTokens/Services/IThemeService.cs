using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public interface IThemeService
{
    /// <summary>
    /// Represents the currently selected theme.
    /// </summary>
    StandardLuminance SelectedTheme { get; }

    /// <summary>
    /// Represents the currently selected accent color.
    /// </summary>
    string? SelectedAccentColor { get; }

    /// <summary>
    /// Represents the current localization direction.
    /// </summary>
    LocalizationDirection SelectedDirection { get; }

    /// <summary>
    /// The reference to the root element of the Theme Service. This ElementRef
    /// can be used in different layouts to assign the same theme.
    /// </summary>
    ElementReference ElementRef { get; set; }

    /// <summary>
    /// Initializes the Theme Service.
    /// </summary>
    /// <param name="initialData">
    /// Initial data that can be passed by the user to achieve persistent theming.
    /// </param>
    /// <returns></returns>
    Task InitializeAsync(Theme? initialData = null);

    /// <summary>
    /// Sets the theme based on the provided StandardLuminance value.
    /// </summary>
    /// <param name="standardLuminance">The StandardLuminance value for the theme.</param>
    Task SetThemeAsync(StandardLuminance standardLuminance);

    /// <summary>
    /// Sets the theme based on the provided dark mode flag.
    /// </summary>
    /// <param name="isDarkMode">A flag indicating whether to set the theme to dark mode.</param>
    Task SetThemeAsync(bool isDarkMode);

    /// <summary>
    /// Sets the accent color based on the provided OfficeColor value.
    /// </summary>
    /// <param name="officeColor">The OfficeColor value for the accent color.</param>
    Task SetAccentColorAsync(OfficeColor officeColor);

    /// <summary>
    /// Sets the accent color based on the provided color string.
    /// </summary>
    /// <param name="color">The color string for the accent color. this accepts #hex </param>
    Task SetAccentColorAsync(string color);

    /// <summary>
    /// Sets the localization direction based on the provided RTL flag.
    /// </summary>
    /// <param name="isRTL">A flag indicating whether to set the direction to RTL (Right-to-Left).</param>
    Task SetDirectionAsync(bool isRTL);

    /// <summary>
    /// Sets the localization direction based on the provided LocalizationDirection value.
    /// </summary>
    /// <param name="localizationDirection">The localization direction value to set.</param>
    Task SetDirectionAsync(LocalizationDirection localizationDirection);

    /// <summary>
    /// Sets the highlight based on the provided StandardLuminance value.
    /// </summary>
    /// <param name="highlight">The StandardLuminance value for the highlight.</param>
    Task SetHighlightAsync(StandardLuminance highlight);

    /// <summary>
    /// Sets the highlight based on the provided dark mode flag.
    /// </summary>
    /// <param name="isDarkMode">A flag indicating whether to set the highlight to dark mode.</param>
    Task SetHighlightAsync(bool isDarkMode);


}
