using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.Fast.Components.FluentUI.Helpers;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class DemoMainLayout : IAsyncDisposable
{
    private string? selectValue;

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
        }
        await BaseLayerLuminance.SetValueFor(container, baseLayerLuminance);
        //await DesignTokens.Direction.SetValueFor(container, dir.ToString());
    }

    public async Task SwitchDirection()
    {
        dir = dir == LocalizationDirection.rtl ? LocalizationDirection.ltr : LocalizationDirection.rtl;
        await Direction.SetValueFor(container, dir.ToAttributeValue());
        await JSRuntime.InvokeVoidAsync("switchDirection", dir.ToString());
    }

    public async void SwitchTheme()
    {
        baseLayerLuminance = baseLayerLuminance == 0.15f ? 0.98f : 0.15f;

        await _jsModule!.InvokeVoidAsync("switchHighlightStyle", baseLayerLuminance == 0.15f ? true : false);
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
                selectValue = value;
                await AccentBaseColor.SetValueFor(container, value.ToSwatch());
            }
            else
            {
                selectValue = "default";
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