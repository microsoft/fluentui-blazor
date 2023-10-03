using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel : IDialogContentComponent
{
    private bool _inDarkMode;
    private bool _rtl;


    [Inject]
    private IThemeService ThemeService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _rtl = ThemeService.SelectedDirection.IsRTL();
        _inDarkMode = ThemeService.SelectedTheme.IsDarkMode();
        base.OnInitialized();
    }

    public async Task UpdateDirection(bool direction)
    {

        await ThemeService.SetDirectionAsync(direction);
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