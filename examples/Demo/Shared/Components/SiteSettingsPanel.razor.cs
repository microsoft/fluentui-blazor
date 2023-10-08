using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel : IDialogContentComponent
{
    private bool _inDarkMode;
    private bool _rtl;
    private OfficeColor _selectedColorOption;


    [Inject]
    private IThemeService ThemeService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _rtl = ThemeService.SelectedDirection == LocalizationDirection.rtl;
        _inDarkMode = ThemeService.SelectedTheme == StandardLuminance.DarkMode;
        _selectedColorOption = ThemeService.SelectedAccentColor.GetEnumByDescription<OfficeColor>() ?? OfficeColor.Default;
        base.OnInitialized();
    }

    public async Task UpdateDirection(bool direction)
    {
        await ThemeService.SetDirectionAsync(direction);
        StateHasChanged();
    }

    public async Task HandleColorChange(ChangeEventArgs args)
    {
        string? value = args.Value?.ToString();

        if (!string.IsNullOrEmpty(value))
        {
           await ThemeService.SetAccentColorAsync(value);
        }
        StateHasChanged();
    }

    public async void UpdateTheme(bool isDarkMode)
    {
        StandardLuminance _baseLayerLuminance = StandardLuminance.LightMode;

        if (isDarkMode)
        {
            _baseLayerLuminance = StandardLuminance.DarkMode;
        }
        
        await ThemeService.SetThemeAsync(_baseLayerLuminance);
        StateHasChanged();
    }
}