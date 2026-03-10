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

    /// <inheritdoc />
    public Task<Dictionary<string, string>?> CreateCustomThemeAsync(ThemeSettings settings)
        => _jsRuntime.InvokeAsync<Dictionary<string, string>?>(
                "Blazor.theme.createBrandThemeFromSettings",
                CancellationToken.None,
                new
                {
                    color = settings.Color,
                    hueTorsion = settings.HueTorsion,
                    vibrancy = settings.Vibrancy,
                    mode = settings.Mode.ToAttributeValue(),
                    isExact = settings.IsExact,
                })
            .AsTask();

    /// <inheritdoc />
    public Task SetThemeAsync(IReadOnlyDictionary<string, string> theme)
        => InvokeVoidAsync("Blazor.theme.setBrandThemeFromTheme", theme);

    /// <inheritdoc />
    public Task SetThemeAsync(ThemeType type)
    {
        return type switch
        {
            ThemeType.Default => InvokeVoidAsync("Blazor.theme.setWebTheme"),
            ThemeType.Teams => InvokeVoidAsync("Blazor.theme.setTeamsTheme"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc />
    public Task SetThemeAsync(ThemeType type, ThemeMode mode)
    {
        return type switch
        {
            ThemeType.Default => InvokeVoidAsync("Blazor.theme.setThemeMode", mode.ToAttributeValue()),
            ThemeType.Teams => InvokeVoidAsync("Blazor.theme.setTeamsThemeMode", mode.ToAttributeValue()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc />
    public Task SetThemeAsync(string color, bool isExact = false)
        => SetThemeAsync(new ThemeSettings(color, 0, 0, ThemeMode.System, isExact));

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Task SwitchDirectionAsync()
        => InvokeVoidAsync("Blazor.theme.switchDirection");

    /// <inheritdoc />
    public Task ClearThemeSettingsAsync()
        => InvokeVoidAsync("Blazor.theme.clearThemeSettings");

    /// <inheritdoc />
    public Task<bool> IsSystemDarkAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isSystemDark").AsTask();

    /// <inheritdoc />
    public Task<bool> IsDarkModeAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isDarkMode").AsTask();

    /// <inheritdoc />
    public Task<IReadOnlyDictionary<string, string>?> GetCurrentColorRampAsync()
        => _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, string>?>("Blazor.theme.getCachedRamp").AsTask();
    /// <inheritdoc />
    public Task<bool> SwitchThemeAsync()
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.switchTheme", CancellationToken.None).AsTask();

    private Task InvokeVoidAsync(string identifier, params object?[] args)
        => _jsRuntime.InvokeVoidAsync(identifier, args).AsTask();
}
