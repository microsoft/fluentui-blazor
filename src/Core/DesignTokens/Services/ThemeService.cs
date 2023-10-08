using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public sealed class ThemeService : JSModule, IThemeService
{
    private readonly ILogger<IThemeService> _logger;
    private readonly GlobalState _globalState;
    private readonly AccentBaseColor _accentBase;
    private readonly BaseLayerLuminance _baseLayerLuminance;
    private readonly Direction _direction;

    public ThemeService(IJSRuntime jsRuntime, ILogger<IThemeService> logger, GlobalState globalState, AccentBaseColor accentBaseColor, BaseLayerLuminance baseLayerLuminance, Direction direction)
        : base(jsRuntime, "./_content/Microsoft.Fast.Components.FluentUI/DesignTokens/FluentThemeProvider.razor.js")
    {
        _logger = logger;
        _globalState = globalState;
        _baseLayerLuminance = baseLayerLuminance;
        _accentBase = accentBaseColor;
        _direction = direction;
    }

    bool _isInitialized = false;
    Theme _themeConfigs = default!;
    public StandardLuminance SelectedTheme => _globalState.Luminance;
    public string? SelectedAccentColor => _globalState.Color;
    public ElementReference ElementRef { get; set; }
    public LocalizationDirection SelectedDirection => _globalState.Dir;

    public async Task InitializeAsync(Theme? initialData = null)
    {
        _globalState.SetContainer(ElementRef);

        if (_isInitialized || initialData == null) return;

        _themeConfigs = initialData;

        await SetThemeAsync(_themeConfigs.SelectedTheme);

        await SetAccentColorAsync(_themeConfigs.SelectedAccentColor ?? "default");

        await SetDirectionAsync(_themeConfigs.SelectedDirection);

        _isInitialized = true;
    }

    public async Task SetAccentColorAsync(OfficeColor officeColor)
    {
        var color = officeColor.GetDescription()!;
        await SetAccentColorAsync(color);
    }

    public async Task SetAccentColorAsync(string color)
    {
        if (string.IsNullOrEmpty(color)) return;

        if (color is not "default")
            await _accentBase.SetValueFor(ElementRef, color.ToSwatch());
        else
            await _accentBase.DeleteValueFor(ElementRef);

        _globalState.SetColor(color);
    }

    public async Task SetThemeAsync(StandardLuminance standardLuminance)
    {
        await SetThemeAsync(standardLuminance == StandardLuminance.DarkMode);
    }

    public async Task SetThemeAsync(bool isDarkMode)
    {
        await Task.Delay(50);

        StandardLuminance theme;

        if (isDarkMode)
            theme = StandardLuminance.DarkMode;
        else
            theme = StandardLuminance.LightMode;

        await _baseLayerLuminance.SetValueFor(ElementRef, theme.GetLuminanceValue());

        await SetHighlightAsync(isDarkMode);

        _globalState.SetLuminance(theme);
    }

    public async Task SetDirectionAsync(bool isRTL)
    {
        try
        {
            await Task.Delay(50);

            LocalizationDirection dir;

            if (isRTL)
                dir = LocalizationDirection.rtl;
            else
                dir = LocalizationDirection.ltr;

            _globalState.SetDirection(dir);

            await InvokeVoidAsync("switchDirection", dir.ToString());

            await _direction.SetValueFor(ElementRef, dir.ToAttributeValue());
            
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Direction, Recent Direction Changes were not applied {0}", ex.Message);
        }
    }

    public async Task SetDirectionAsync(LocalizationDirection localizationDirection)
    {
        await SetDirectionAsync(localizationDirection == LocalizationDirection.rtl);
    }

    public async Task SetHighlightAsync(StandardLuminance highlight)
    {
        await SetHighlightAsync(highlight == StandardLuminance.DarkMode);
    }

    public async Task SetHighlightAsync(bool isDarkMode)
    {
        try
        {
            await InvokeVoidAsync("switchHighlightStyle", isDarkMode);
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Highlights, Recent Theme Changes Not Successfully Applied {message}", ex.Message);
        }
    }


}
