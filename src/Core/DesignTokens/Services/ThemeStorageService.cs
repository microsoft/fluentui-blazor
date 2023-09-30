using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

internal sealed class ThemeStorageService : JSModule, IThemeStorageService
{
    readonly IJSRuntime _jsRuntime;
    readonly ILogger<IThemeStorageService> _logger;

    public ThemeStorageService(IJSRuntime jSRuntime, ILogger<IThemeStorageService> logger) 
        : base(jSRuntime, "./_content/Microsoft.Fast.Components.FluentUI/js/themeService.js")
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
    }

    public async ValueTask<bool> IsStorageEnabledAndSupported()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync(ThemeConstant.SetItem, ThemeConstant.ThemeStorageValidationKey, ThemeConstant.ThemeStorageValidationData);
            string? result = await _jsRuntime.InvokeAsync<string>(ThemeConstant.GetItem, ThemeConstant.ThemeStorageValidationKey);

            if (result is not null && result == ThemeConstant.ThemeStorageValidationData) return true;
            _logger.LogWarning("Your browser doesn't support or disabled Storage Support that needed for Theme Persistency, Therefore Theme Persistency is Disabled");
            return false;
        }
        catch (JSException)
        {
            _logger.LogWarning("Your browser doesn't support or disabled Storage Support that needed for Theme Persistency, Therefore Theme Persistency is Disabled");
            return false;
        }
        
    }

    public async ValueTask<bool> IsMobile()
    {
        try
        {
            return await InvokeAsync<bool>("isMobile");
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Getting Device Type, Reverting to Default {0}", ex.Message);
            return false;
        }
    }
    public async ValueTask<string?> GetAccentColorAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>(ThemeConstant.GetItem, ThemeConstant.SelectedAccentColorKey);
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Getting Accent Color, Reverting to Default {0}", ex.Message);
            return null;
        }
    }

    public async ValueTask SetAccentColorAsync(string color)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync(ThemeConstant.SetItem, ThemeConstant.SelectedAccentColorKey, color);
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Accent Color, Recent Accent Color Changes were not persisted {0}", ex.Message);
        }
    }

    public async ValueTask<string?> GetThemeAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>(ThemeConstant.GetItem, ThemeConstant.SelectedThemeKey);
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Getting Theme, Reverting to Default {0}", ex.Message);
            return null;
        }
    }

    public async ValueTask SetThemeAsync(StandardLuminance theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync(ThemeConstant.SetItem, ThemeConstant.SelectedThemeKey, (int)theme);
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Theme, Recent Theme Changes were not persisted {0}", ex.Message);
        }
    }

    public async ValueTask<bool?> GetDirectionAsync()
    {
        try
        {
            var val = await _jsRuntime.InvokeAsync<string?>(ThemeConstant.GetItem, ThemeConstant.SelectedDirectionKey);
            if (!string.IsNullOrEmpty(val)) return Convert.ToBoolean(val);
            return null;
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Getting Theme, Reverting to Default {0}", ex.Message);
            return null;
        }
    }

    public async ValueTask SetDirectionAsync(bool isRTL)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync(ThemeConstant.SetItem, ThemeConstant.SelectedDirectionKey, isRTL.ToString());

            LocalizationDirection dir = LocalizationDirection.rtl;

            if(!isRTL)
                await InvokeVoidAsync("switchDirection", dir.ToString());
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Direction, Recent Direction Changes were not persisted {0}", ex.Message);
        }
    }

    public async ValueTask SetDirectionLocallyAsync(bool isRTL)
    {
        try
        {
            LocalizationDirection dir = LocalizationDirection.rtl;

            if (!isRTL)
                await InvokeVoidAsync("switchDirection", dir.ToString());
        }
        catch (JSException ex)
        {
            _logger.LogWarning("Something went wrong on Setting Direction, Recent Direction Changes were not persisted {0}", ex.Message);
        }
    }
}
