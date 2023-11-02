using System.Reflection;
using FluentUI.Demo.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class DemoMainLayout : IAsyncDisposable
{
    [Parameter]
    public RenderFragment? Body { get; set; }

    private OfficeColor _selectedColorOption;
    private string? _version;

    private bool _mobile;
    private string? _prevUri;
    private TableOfContents? _toc;

    ElementReference container;

    private IJSObjectReference? _jsModule;
    bool menuchecked = true;

    ErrorBoundary? errorBoundary;


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

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyVersionAttribute>()?.Version;

        OfficeColor[] colors = Enum.GetValues<OfficeColor>();
        _selectedColorOption = colors[new Random().Next(colors.Length)];

        GlobalState.SetColor(_selectedColorOption.ToAttributeValue());

        _prevUri = NavigationManager.Uri;
        NavigationManager.LocationChanged += LocationChanged;
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            GlobalState.SetContainer(container);

            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Shared/DemoMainLayout.razor.js");

            _mobile = await _jsModule!.InvokeAsync<bool>("isDevice");

            bool _dark = await _jsModule!.InvokeAsync<bool>("isDarkMode");
            GlobalState.SetLuminance(_dark ? StandardLuminance.DarkMode: StandardLuminance.LightMode);

            if (_selectedColorOption != OfficeColor.Default)
                await AccentBaseColor.WithDefault(_selectedColorOption.ToAttributeValue()!.ToSwatch());            
        }
    }

    public EventCallback OnRefreshTableOfContents => EventCallback.Factory.Create(this, RefreshTableOfContents);

    private async Task RefreshTableOfContents()
    {
        await _toc!.Refresh();
    }

    private void HandleChecked()
    {
        menuchecked = !menuchecked;
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (!e.IsNavigationIntercepted && new Uri(_prevUri!).AbsolutePath != new Uri(e.Location).AbsolutePath)
        {
            _prevUri = e.Location;
            if (_mobile && menuchecked == true)
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
