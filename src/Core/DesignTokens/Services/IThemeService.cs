using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public interface IThemeService
{
    StandardLuminance SelectedTheme { get; }
    OfficeColor SelectedAccentColor { get; }
    LocalizationDirection SelectedDirection { get; }
    ElementReference ElementRef { get; set; }
    Task InitializeAsync(bool isPersistent = true);
    Task SetThemeAsync(StandardLuminance standardLuminance);
    Task SetThemeAsync(bool isDarkMode);
    Task SetAccentColorAsync(OfficeColor officeColor);
    Task SetAccentColorAsync(string color);
    Task SetDirectionAsync(bool isRTL);
}
