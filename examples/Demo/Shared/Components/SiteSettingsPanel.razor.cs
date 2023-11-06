using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel : IDialogContentComponent<GlobalState>, IAsyncDisposable
{
    private const string ThemeSettingSystem = "System";
    private const string ThemeSettingDark = "Dark";
    private const string ThemeSettingLight = "Light";

    private string _currentTheme = ThemeSettingSystem;
    private LocalizationDirection _dir;
    private OfficeColor _selectedColorOption;
    private bool _rtl;
    private IJSObjectReference? _jsModule;
    private ElementReference _container;

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private AccentBaseColor AccentBaseColor { get; set; } = default!;

    [Inject]
    private Direction Direction { get; set; } = default!;

    [Parameter]
    public GlobalState Content { get; set; } = default!;

    protected override void OnInitialized()
    {
        _rtl = Content.Dir == LocalizationDirection.rtl;
        _container = Content.Container;

        OfficeColor[] colors = Enum.GetValues<OfficeColor>();
        _selectedColorOption = colors.Where(x => x.ToAttributeValue() == Content.Color).FirstOrDefault();

        if (_selectedColorOption == OfficeColor.Default)
        {
            _selectedColorOption = colors[new Random().Next(colors.Length)];

            Content.Color = _selectedColorOption.ToAttributeValue();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/js/theme.js");
            _currentTheme = await _jsModule.InvokeAsync<string>("getThemeCookieValue");
            StateHasChanged();
        }
    }

    public async Task UpdateDirectionAsync()
    {
        _dir = (_rtl ? LocalizationDirection.rtl : LocalizationDirection.ltr);

        Content.Dir = _dir;

        await _jsModule!.InvokeVoidAsync("switchDirection", _dir.ToString());
        await Direction.WithDefault(_dir.ToAttributeValue());

        StateHasChanged();
    }

    private async Task UpdateThemeAsync()
    {
        if (_jsModule is not null)
        {
            StandardLuminance newLuminance = await GetBaseLayerLuminanceForSetting(_currentTheme);

            await BaseLayerLuminance.WithDefault(newLuminance.GetLuminanceValue());
            GlobalState.SetLuminance(newLuminance);
            await _jsModule.InvokeVoidAsync("switchHighlightStyle", newLuminance == StandardLuminance.DarkMode);
            await _jsModule.InvokeVoidAsync("setThemeCookie", _currentTheme);
        }

        //_currentTheme = newValue;
    }

    private async Task UpdateColorAsync(ChangeEventArgs args)
    {
        string? value = args.Value?.ToString();
        if (!string.IsNullOrEmpty(value))
        {
            if (value != "default")
            {
                await AccentBaseColor.WithDefault(value.ToSwatch());
            }
            else
            {
                // Default FluentUI value for AccentBaseColor from 
                // https://github.com/microsoft/fluentui/blob/c0d3065982e1646c54ba00c1d524248b792dbcad/packages/web-components/src/color/utilities/color-constants.ts#L22C32-L22C39
                await AccentBaseColor.WithDefault("#0078D4".ToSwatch());
            }

            Content.Color = value;
        }
    }

    private Task<StandardLuminance> GetBaseLayerLuminanceForSetting(string setting)
    {
        if (setting == ThemeSettingLight)
        {
            return Task.FromResult(StandardLuminance.LightMode);
        }
        else if (setting == ThemeSettingDark)
        {
            return Task.FromResult(StandardLuminance.DarkMode);
        }
        else // "System"
        {
            return GetSystemThemeLuminance();
        }
    }

    private async Task<StandardLuminance> GetSystemThemeLuminance()
    {
        if (_jsModule is not null)
        {
            var systemTheme = await _jsModule.InvokeAsync<string>("getSystemTheme");
            if (systemTheme == ThemeSettingDark)
            {
                return StandardLuminance.DarkMode;
            }
        }

        return StandardLuminance.LightMode;
    }
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}