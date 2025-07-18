// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.Shared.Components.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel
{
    private const string DEFAULT_NEUTRAL_COLOR = "#808080";

    private CookieConsent? _cookie;
    private string? _status;
    private bool _popVisible, _popNIVisible;
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

    public string? NeutralColor { get; set; }

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

            NeutralColor = GlobalState.NeutralColor;
            // Same default values is used for light and dark theme
            NeutralColor ??= DEFAULT_NEUTRAL_COLOR;
        }

    }

    protected void HandleDirectionChanged(bool isLeftToRight)
    {

        _ltr = isLeftToRight;
        Direction = isLeftToRight ? LocalizationDirection.LeftToRight : LocalizationDirection.RightToLeft;
    }

    private async Task ResetSiteAsync()
    {
        var msg = "Site settings reset and cache cleared!";

        await CacheStorageAccessor.RemoveAllAsync();
        _theme?.ClearLocalStorageAsync();

        Logger.LogInformation(msg);
        _status = msg;

        OfficeColor = OfficeColorUtilities.GetRandom();
        Mode = DesignThemeModes.System;
        NeutralColor = DEFAULT_NEUTRAL_COLOR;
    }

    private async Task ManageCookieSettingsAsync()
    {
        if (_cookie != null)
        {
            await _cookie.ManageCookiesAsync();
        }
    }

    private static string? GetCustomColor(OfficeColor? color)
    {
        return color switch
        {
            null => OfficeColorUtilities.GetRandom(true).ToAttributeValue(),
            Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Default => "#036ac4",
            _ => color.ToAttributeValue(),
        };

    }
}
