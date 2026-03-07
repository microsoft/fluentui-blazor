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
    public ValueTask<Dictionary<string, string>?> CreateCustomThemeAsync(string color, double hueTorsion, double vibrancy, bool isDark, bool isExact = false, CancellationToken cancellationToken = default)
        => _jsRuntime.InvokeAsync<Dictionary<string, string>?>(
            "Blazor.theme.createBrandTheme",
            cancellationToken,
            color,
            hueTorsion,
            vibrancy,
            isDark,
            isExact);

    /// <inheritdoc />
    public ValueTask SetThemeAsync(IReadOnlyDictionary<string, string> theme, CancellationToken cancellationToken = default)
        => InvokeVoidAsync("Blazor.theme.setBrandThemeFromTheme", cancellationToken, theme);

    /// <inheritdoc />
    public ValueTask SetThemeAsync(ThemeType type, CancellationToken cancellationToken = default)
    {
        return type switch
        {
            ThemeType.Default => InvokeVoidAsync("Blazor.theme.setWebTheme", cancellationToken),
            ThemeType.Teams => InvokeVoidAsync("Blazor.theme.setTeamsTheme", cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc />
    public ValueTask SetThemeAsync(ThemeType type, ThemeMode mode, CancellationToken cancellationToken = default)
    {
        return type switch
        {
            ThemeType.Default => InvokeVoidAsync("Blazor.theme.setThemeMode", cancellationToken, mode.ToAttributeValue()),
            ThemeType.Teams => InvokeVoidAsync("Blazor.theme.setTeamsThemeMode", cancellationToken, mode.ToAttributeValue()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, message: null),
        };
    }

    /// <inheritdoc />
    public ValueTask SetThemeAsync(string color, bool isExact = false, CancellationToken cancellationToken = default)
        => SetThemeAsync(color, 0, 0, ThemeMode.System, isExact, cancellationToken);

    /// <inheritdoc />
    public ValueTask SetThemeAsync(string color, double hueTorsion, double vibrancy, ThemeMode mode, bool isExact = false, CancellationToken cancellationToken = default)
    {
        if (mode == ThemeMode.Dark)
        {
            return InvokeVoidAsync("Blazor.theme.setBrandTheme", cancellationToken, color, hueTorsion, vibrancy, true, isExact);
        }

        if (mode == ThemeMode.Light)
        {
            return InvokeVoidAsync("Blazor.theme.setBrandTheme", cancellationToken, color, hueTorsion, vibrancy, false, isExact);
        }

        return isExact
            ? InvokeVoidAsync("Blazor.theme.setBrandThemeFromColorExact", cancellationToken, color)
            : InvokeVoidAsync("Blazor.theme.setBrandThemeFromColor", cancellationToken, color);
    }

    /// <inheritdoc />
    public ValueTask SwitchDirectionAsync(CancellationToken cancellationToken = default)
        => InvokeVoidAsync("Blazor.theme.switchDirection", cancellationToken);

    /// <inheritdoc />
    public ValueTask ClearThemeSettingsAsync(CancellationToken cancellationToken = default)
        => InvokeVoidAsync("Blazor.theme.clearThemeSettings", cancellationToken);

    /// <inheritdoc />
    public ValueTask<bool> IsSystemDarkAsync(CancellationToken cancellationToken = default)
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isSystemDark", cancellationToken);

    /// <inheritdoc />
    public ValueTask<bool> IsDarkModeAsync(CancellationToken cancellationToken = default)
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.isDarkMode", cancellationToken);

    /// <inheritdoc />
    public ValueTask<IReadOnlyDictionary<string, string>?> GetCachedRampAsync(CancellationToken cancellationToken = default)
        => _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, string>?>("Blazor.theme.getCachedRamp", cancellationToken);

    /// <inheritdoc />
    public ValueTask<bool> SwitchThemeAsync(CancellationToken cancellationToken = default)
        => _jsRuntime.InvokeAsync<bool>("Blazor.theme.switchTheme", cancellationToken);

    private ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args)
        => _jsRuntime.InvokeVoidAsync(identifier, cancellationToken, args);
}
