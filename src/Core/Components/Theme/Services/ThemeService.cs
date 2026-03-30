// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Default implementation of <see cref="IThemeService"/>.
/// </summary>
public sealed class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Creates a new instance of <see cref="ThemeService"/>.
    /// </summary>
    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <inheritdoc cref="IThemeService.CreateCustomThemeAsync(ThemeSettings)"/>
    public async Task<Theme> CreateCustomThemeAsync(ThemeSettings settings)
    {
        var dict = await _jsRuntime.InvokeAsync<Dictionary<string, object?>?>(
                "Blazor.theme.createBrandTheme",
                new
                {
                    color = settings.Color,
                    hueTorsion = settings.HueTorsion,
                    vibrancy = settings.Vibrancy,
                    mode = settings.Mode.ToAttributeValue(),
                    isExact = settings.IsExact,
                });

        if (dict is null)
        {
            throw new InvalidOperationException("Failed to create theme");
        }

        var theme = new Theme();
        theme.FromDictionary(dict);
        return theme;
    }

    /// <inheritdoc cref="IThemeService.SetThemeAsync(Theme)"/>
    public Task SetThemeAsync(Theme theme)
        => InvokeVoidAsync("Blazor.theme.setBrandThemeFromTheme", theme?.ToDictionary());

    /// <inheritdoc cref="IThemeService.SetThemeAsync(ThemeColorVariant)"/>
    public Task SetThemeAsync(ThemeColorVariant type)
    {
        return type switch
        {
            ThemeColorVariant.Default => InvokeVoidAsync("Blazor.theme.setWebTheme"),
            ThemeColorVariant.Teams => InvokeVoidAsync("Blazor.theme.setTeamsTheme"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc cref="IThemeService.SetThemeAsync(ThemeColorVariant, ThemeMode)"/>
    public Task SetThemeAsync(ThemeColorVariant type, ThemeMode mode)
    {
        return type switch
        {
            ThemeColorVariant.Default => InvokeVoidAsync("Blazor.theme.setThemeMode", mode.ToAttributeValue()),
            ThemeColorVariant.Teams => InvokeVoidAsync("Blazor.theme.setTeamsThemeMode", mode.ToAttributeValue()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc cref="IThemeService.SetThemeAsync(string, bool)"/>
    public Task SetThemeAsync(string color, bool isExact = false)
        => SetThemeAsync(new ThemeSettings(color, 0, 0, ThemeMode.System, isExact));

    /// <inheritdoc cref="IThemeService.SetThemeAsync(ThemeMode)"/>
    public Task SetThemeAsync(ThemeMode mode)
        => InvokeVoidAsync("Blazor.theme.setThemeMode", mode.ToAttributeValue());

    /// <inheritdoc cref="IThemeService.SetThemeAsync(ThemeSettings)"/>
    public Task SetThemeAsync(ThemeSettings settings)
        => InvokeVoidAsync(
            "Blazor.theme.setBrandThemeFromSettings",
            new
            {
                color = settings.Color,
                hueTorsion = settings.HueTorsion,
                vibrancy = settings.Vibrancy,
                mode = settings.Mode == ThemeMode.System ? null : settings.Mode.ToAttributeValue(),
                isExact = settings.IsExact,
            });

    /// <inheritdoc cref="IThemeService.SwitchDirectionAsync"/>
    public Task SwitchDirectionAsync()
        => InvokeVoidAsync("Blazor.theme.switchDirection");

    /// <inheritdoc cref="IThemeService.ClearStoredThemeSettingsAsync"/>
    public Task ClearStoredThemeSettingsAsync()
        => InvokeVoidAsync("Blazor.theme.clearStoredThemeSettings");

    /// <inheritdoc cref="IThemeService.IsSystemDarkAsync"/>
    public Task<bool> IsSystemDarkAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isSystemDark").AsTask();

    /// <inheritdoc cref="IThemeService.IsDarkModeAsync"/>
    public Task<bool> IsDarkModeAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isDarkMode").AsTask();

    /// <inheritdoc cref="IThemeService.GetColorRampAsync"/>
    public Task<IReadOnlyDictionary<string, string>?> GetColorRampAsync()
        => _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, string>?>("Blazor.theme.getColorRamp").AsTask();

    /// <inheritdoc cref="IThemeService.GetColorRampFromSettingsAsync"/>
    public Task<IReadOnlyDictionary<string, string>?> GetColorRampFromSettingsAsync(ThemeSettings settings)
        => _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, string>?>(
            "Blazor.theme.getColorRampFromSettings",
            new
            {
                color = settings.Color,
                hueTorsion = settings.HueTorsion,
                vibrancy = settings.Vibrancy,
                mode = settings.Mode == ThemeMode.System ? null : settings.Mode.ToAttributeValue(),
                isExact = settings.IsExact,
            }).AsTask();

    /// <inheritdoc cref="IThemeService.GetBrandColorAsync"/>
    public Task<string> GetBrandColorAsync()
        => _jsRuntime.InvokeAsync<string>("Blazor.theme.getBrandColor").AsTask();

    /// <inheritdoc cref="IThemeService.SwitchThemeAsync" />
    public Task<bool> SwitchThemeAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.switchTheme", CancellationToken.None).AsTask();

    /// <inheritdoc cref="IThemeService.SetThemeToElementAsync(Microsoft.AspNetCore.Components.ElementReference, ThemeSettings)"/>
    public Task SetThemeToElementAsync(Microsoft.AspNetCore.Components.ElementReference element, ThemeSettings settings)
        => InvokeVoidAsync(
            "Blazor.theme.setBrandThemeToElementFromSettings",
            element,
            new
            {
                color = settings.Color,
                hueTorsion = settings.HueTorsion,
                vibrancy = settings.Vibrancy,
                mode = settings.Mode == ThemeMode.System ? null : settings.Mode.ToAttributeValue(),
                isExact = settings.IsExact,
            });

    /// <inheritdoc cref="IThemeService.SetThemeToElementAsync(Microsoft.AspNetCore.Components.ElementReference, Theme)"/>
    public Task SetThemeToElementAsync(Microsoft.AspNetCore.Components.ElementReference element, Theme theme)
        => InvokeVoidAsync("Blazor.theme.setBrandThemeToElementFromTheme", element, theme.ToDictionary());

    private Task InvokeVoidAsync(string identifier, params object?[] args)
        => _jsRuntime.InvokeVoidAsync(identifier, args).AsTask();
}
