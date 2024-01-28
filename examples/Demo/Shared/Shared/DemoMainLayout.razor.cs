using System.Reflection;
using FluentUI.Demo.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class DemoMainLayout
{
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.Shared/Shared/DemoMainLayout.razor.js";
    private string? _version;
    private bool _mobile;
    private string? _prevUri;
    private TableOfContents? _toc;
    private bool _menuChecked = true;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public RenderFragment? Body { get; set; }

    protected override void OnInitialized()
    {
        var versionAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (versionAttribute != null)
        {
            var version = versionAttribute.InformationalVersion;
            var plusIndex = version.IndexOf('+');
            if (plusIndex >= 0 && plusIndex + 9 < version.Length)
            {
                _version = version[..(plusIndex + 9)];
            }
            else
            {
                _version = version;
            }
        }

        _prevUri = NavigationManager.Uri;
        NavigationManager.LocationChanged += LocationChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _mobile = await jsModule.InvokeAsync<bool>("isDevice");
            await jsModule.DisposeAsync();
        }
    }

    public EventCallback OnRefreshTableOfContents => EventCallback.Factory.Create(this, RefreshTableOfContentsAsync);

    private async Task RefreshTableOfContentsAsync()
    {
        await _toc!.Refresh();
    }

    private void HandleChecked()
    {
        _menuChecked = !_menuChecked;
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (!e.IsNavigationIntercepted && new Uri(_prevUri!).AbsolutePath != new Uri(e.Location).AbsolutePath)
        {
            _prevUri = e.Location;
            if (_mobile && _menuChecked == true)
            {
                _menuChecked = false;
                StateHasChanged();
            }
        }
    }
}
