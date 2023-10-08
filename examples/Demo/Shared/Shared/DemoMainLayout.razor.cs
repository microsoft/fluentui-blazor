using System.Reflection;
using FluentUI.Demo.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class DemoMainLayout : IAsyncDisposable
{
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
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        _prevUri = NavigationManager.Uri;
        NavigationManager.LocationChanged += LocationChanged;
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
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
