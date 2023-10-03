using System.Drawing;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

internal sealed class ThemeService : IThemeService
{
    private readonly IThemeStorageService _themeStorageService;
    private readonly GlobalState _globalState;
    private readonly AccentBaseColor _accentBase;
    private readonly BaseLayerLuminance _baseLayerLuminance;
    private readonly Direction _direction;

    public ThemeService(IThemeStorageService themeStorageService, GlobalState globalState, AccentBaseColor accentBaseColor, BaseLayerLuminance baseLayerLuminance, Direction direction)
    {
        _themeStorageService = themeStorageService;
        _globalState = globalState;
        _baseLayerLuminance = baseLayerLuminance;
        _accentBase = accentBaseColor;
        _direction = direction;
    }

   
    bool _isMobile = false;
    StandardLuminance _selectedTheme = StandardLuminance.LightMode;
    OfficeColor _selectedAccentColor = OfficeColor.Default;
    LocalizationDirection _selectedDirection = LocalizationDirection.rtl;

    bool _isInitialized = false;
    bool _isPersistent = false;
    public bool IsMobile => _isMobile;
    public StandardLuminance SelectedTheme => _selectedTheme;
    public OfficeColor SelectedAccentColor => _selectedAccentColor;
    public ElementReference ElementRef { get; set; }
    public LocalizationDirection SelectedDirection => _selectedDirection;

    public async Task InitializeAsync(bool isPersistent = true)
    {
        if (_isInitialized && !isPersistent && !await _themeStorageService.IsStorageEnabledAndSupported()) return;

        string? themeString = await _themeStorageService.GetThemeAsync();
        if (!string.IsNullOrEmpty(themeString))
        {
            _selectedTheme = (StandardLuminance)Convert.ToInt32(themeString);
            await SetThemeForElementRefAsync(SelectedTheme.GetLuminanceValue());
        }

        _isMobile = await _themeStorageService.IsMobile();

        string? accent = await _themeStorageService.GetAccentColorAsync();

        if (!string.IsNullOrEmpty(accent))
        {
            _selectedAccentColor = accent.GetEnumByDescription<OfficeColor>();
            await SetAccentColorForRefAsync(SelectedAccentColor.GetDescription()!);
        }

        bool? direction = await _themeStorageService.GetDirectionAsync();

        if (direction.HasValue && direction.Value.GetDirectionFromBoolean() != SelectedDirection)
        {
            _selectedDirection = direction.Value.GetDirectionFromBoolean();
            await SetDirectionForElementRefAsync(direction.Value);
        }

        _isPersistent = isPersistent;
        _isInitialized = true;

    }

    public async Task SetAccentColorAsync(OfficeColor officeColor)
    {
        var color = officeColor.GetDescription()!;
        await SetAccentColorForRefAsync(color);

        if(_isPersistent)
            await _themeStorageService.SetAccentColorAsync(color);
    }

    public async Task SetAccentColorAsync(string color)
    {
        await SetAccentColorForRefAsync(color);

        if(_isPersistent)
            await _themeStorageService.SetAccentColorAsync(color);

    }

    private async Task SetAccentColorForRefAsync(string color)
    {
        if (string.IsNullOrEmpty(color)) return;

        if (color is not "default")
            await _accentBase.SetValueFor(ElementRef, color.ToSwatch());
        else
            await _accentBase.DeleteValueFor(ElementRef);
    }

    public async Task SetThemeAsync(StandardLuminance standardLuminance)
    {
        await SetThemeForElementRefAsync(standardLuminance.GetLuminanceValue());
        _selectedTheme = standardLuminance;

        if(_isPersistent)
            await _themeStorageService.SetThemeAsync(standardLuminance);
    }

    public async Task SetThemeAsync(bool isDarkMode)
    {
        StandardLuminance theme = StandardLuminance.LightMode;

        if (isDarkMode)
            theme = StandardLuminance.DarkMode;

        await SetThemeForElementRefAsync(theme.GetLuminanceValue());

        if(_isPersistent)
            await _themeStorageService.SetThemeAsync(theme);
    }

    private async Task SetThemeForElementRefAsync(float? theme)
    {
        await Task.Delay(50);
        await _baseLayerLuminance.SetValueFor(ElementRef, theme);
        _globalState.SetLuminance(SelectedTheme);
    }

    public async Task SetDirectionAsync(bool isRTL)
    {
        await SetDirectionForElementRefAsync(isRTL);
        await _themeStorageService.SetDirectionAsync(isRTL);
    }

    private async Task SetDirectionForElementRefAsync(bool isRTL)
    {
        await Task.Delay(50);

        if (isRTL)
            _selectedDirection = LocalizationDirection.rtl;
        else
            _selectedDirection = LocalizationDirection.ltr;

        _globalState.Dir = SelectedDirection;

        await _direction.SetValueFor(ElementRef, SelectedDirection.ToAttributeValue());

        _globalState.SetDirection(SelectedDirection);

        await _themeStorageService.ChangeDirection(isRTL);
    }
}
