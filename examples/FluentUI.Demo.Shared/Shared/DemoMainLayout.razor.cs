using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class DemoMainLayout : IAsyncDisposable
{
    private string? _selectValue;
    private string? _version;
    private bool _inDarkMode;

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

    [Inject]
    private AccentBaseColor AccentBaseColor { get; set; } = default!;

    [Inject]
    private Direction Direction { get; set; } = default!;


    ElementReference container;

    private IJSObjectReference? _jsModule;
    bool menuchecked = true;

    ErrorBoundary? errorBoundary;

    LocalizationDirection dir;
    float baseLayerLuminance = 0.98f;

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        NavigationManager.LocationChanged += LocationChanged;
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Shared/DemoMainLayout.razor.js");

            _inDarkMode = await _jsModule!.InvokeAsync<bool>("isDarkMode");
            StateHasChanged();
        }
    }

    public async Task SwitchDirection()
    {
        dir = (dir == LocalizationDirection.rtl) ? LocalizationDirection.ltr : LocalizationDirection.rtl;

        GlobalState.SetDirection(dir);

        await Direction.SetValueFor(container, dir.ToAttributeValue());
        await _jsModule!.InvokeVoidAsync("switchDirection", dir.ToString());
    }

    public async void SwitchTheme()
    {
        await Task.Delay(50);

        if (_inDarkMode)
            baseLayerLuminance = 0.15f;
        else
            baseLayerLuminance = 0.98f;

        await BaseLayerLuminance.SetValueFor(container, baseLayerLuminance);

        GlobalState.SetLuminance((baseLayerLuminance <= 0.2f) ? Luminance.Dark : Luminance.Light);

        await _jsModule!.InvokeVoidAsync("switchHighlightStyle", baseLayerLuminance == 0.15f);
    }

    private void HandleChecked()
    {
        menuchecked = !menuchecked;
    }

    private async void HandleColorChange(ChangeEventArgs args)
    {
        string? value = args.Value?.ToString();
        if (!string.IsNullOrEmpty(value))
        {
            if (value != "default")
            {
                _selectValue = value;
                await AccentBaseColor.SetValueFor(container, value.ToSwatch());
            }
            else
            {
                _selectValue = "default";
                await AccentBaseColor.DeleteValueFor(container);
            }
        }
    }

    private async void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (e.IsNavigationIntercepted)
        {
            bool mobile = await _jsModule!.InvokeAsync<bool>("isDevice");

            if (mobile)
            {
                menuchecked = false;
                StateHasChanged();
            }
        }
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