using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel
{
    private string? _status;
    private bool _popVisible;
    private bool _ltr = true;
    private FluentDesignTheme? _theme;

    [Inject]
    public required ILogger<SiteSettingsPanel> Logger { get; set; }

    [Inject]
    public required CacheStorageAccessor CacheStorageAccessor { get; set; }

    [Inject]
    public required GlobalState GlobalState { get; set; }
    
    public DesignThemeModes Mode { get; set; }

    public OfficeColor? OfficeColor { get; set; }

    public LocalizationDirection? Direction { get; set; }

    private static IEnumerable<DesignThemeModes> AllModes => Enum.GetValues<DesignThemeModes>();

    private static IEnumerable<OfficeColor?> AllOfficeColors
    {
        get
        {
            return Enum.GetValues<OfficeColor>().Select(i => (OfficeColor?)i);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Direction = GlobalState.Dir;
            _ltr = !Direction.HasValue || Direction.Value == LocalizationDirection.LeftToRight;
        }
    }

    protected void HandleDirectionChanged(bool isLeftToRight)
    {

        _ltr = isLeftToRight;
        Direction = isLeftToRight ? LocalizationDirection.LeftToRight : LocalizationDirection.RightToLeft;
    }

    private async Task ResetSite()
    {
        string? msg = "Site settings reset and cache cleared!";
        
        await CacheStorageAccessor.RemoveAllAsync();
        _theme?.ClearLocalStorageAsync();
        
        Logger.LogInformation(msg);
        _status = msg;

        OfficeColor = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Random;
        Mode = DesignThemeModes.System;

        //StateHasChanged();
    }
}