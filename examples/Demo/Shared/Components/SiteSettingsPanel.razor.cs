using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel
{
    private string? _status;
    private bool _popVisible;
    private FluentDesignTheme? _theme;

    [Inject]
    public ILogger<SiteSettingsPanel> Logger { get; set; } = default!;

    [Inject]
    public CacheStorageAccessor CacheStorageAccessor { get; set; } = default!;
    
    
    public DesignThemeModes Mode { get; set; }

    public OfficeColor? OfficeColor { get; set; }

    public bool Direction { get; set; } = true;

    private static IEnumerable<DesignThemeModes> AllModes => Enum.GetValues<DesignThemeModes>();

    private static IEnumerable<OfficeColor?> AllOfficeColors
    {
        get
        {
            return Enum.GetValues<OfficeColor>().Select(i => (OfficeColor?)i).Union(new[] { (OfficeColor?)null });
        }
    }

    private async Task ResetSite()
    {
        string? msg = "Site settings reset and cache cleared!";
        
        await CacheStorageAccessor.RemoveAllAsync();
        _theme?.ClearLocalStorageAsync();
        
        Logger.LogInformation(msg);
        _status = msg;
    }
}