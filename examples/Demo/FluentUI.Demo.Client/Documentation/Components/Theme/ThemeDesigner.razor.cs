// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Documentation.Components.Theme;

public partial class ThemeDesigner
{
    string? _color = "#0F6CBD";
    string? _hue = "0";
    string? _vibrancy = "0";
    bool _isDark;
    bool _isExact;
    ElementReference _themePreviewElement;

    bool _switched = true;
    double _slider = 35;
    string _fruit = "Banana";

    Dictionary<string, string> _palette = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await PreviewThemeAsync();
        }
    }

    private async Task PreviewThemeAsync()
    {
        var hue = int.TryParse(_hue, out var h) ? h / 100.0 : 0;
        var vibrancy = int.TryParse(_vibrancy, out var v) ? v / 100.0 : 0;

        var settings = new ThemeSettings(
            _color!,
            hue,
            vibrancy,
            _isDark ? ThemeMode.Dark : ThemeMode.Light,
            _isExact
        );

        var jsSettings = settings.ToJsThemeSettings();

        await JSRuntime.InvokeVoidAsync("Blazor.theme.setBrandThemeToElement", _themePreviewElement, jsSettings);

        _palette = await JSRuntime.InvokeAsync<Dictionary<string, string>>("Blazor.theme.getRampFromSettings", jsSettings);
        StateHasChanged();

    }
}
