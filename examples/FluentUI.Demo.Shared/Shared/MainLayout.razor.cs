using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class MainLayout : IAsyncDisposable
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
    private Microsoft.Fast.Components.FluentUI.DesignTokens.Direction Direction { get; set; } = default!;


    ElementReference container;

    private IJSObjectReference? module;
    bool menuchecked = true;

    ErrorBoundary? errorBoundary;

    LocalizationDirection? dir;
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
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Shared/MainLayout.razor.js");
        }
        await BaseLayerLuminance.SetValueFor(container, baseLayerLuminance);
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

        await module!.InvokeVoidAsync("switchHighlightStyle", baseLayerLuminance == 0.15f ? true : false);
    }

    private void HandleChecked()
    {
        menuchecked = !menuchecked;
    }

    private async void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (e.IsNavigationIntercepted)
        {
            bool mobile = await module!.InvokeAsync<bool>("isDevice");

            if (mobile)
            {
                menuchecked = false;
                StateHasChanged();
            }
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (module is not null)
        {
            await module.DisposeAsync();
        }
    }
}