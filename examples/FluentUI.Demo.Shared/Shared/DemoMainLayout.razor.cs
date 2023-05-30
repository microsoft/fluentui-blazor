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
    private OfficeColor _selectedColorOption;
    private string? _version;
    private bool _inDarkMode;
    private bool _ltr;
    private bool _mobile;
    private string? _prevUri;
    private TableOfContents? _toc;

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
    StandardLuminance baseLayerLuminance = StandardLuminance.LightMode;

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;


        OfficeColor[] colors = Enum.GetValues<OfficeColor>().ToArray();
        _selectedColorOption = colors[new Random().Next(colors.Length)];

        _prevUri = NavigationManager.Uri;
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
            _mobile = await _jsModule!.InvokeAsync<bool>("isDevice");

            if (_selectedColorOption != OfficeColor.Default)
                await AccentBaseColor.SetValueFor(container, _selectedColorOption.ToAttributeValue()!.ToSwatch());

            StateHasChanged();
        }
    }

    public EventCallback OnRefreshToC => EventCallback.Factory.Create(this, RefreshToC);

    private async Task RefreshToC()
    {
        await _toc!.Refresh();
    }

    public async Task SwitchDirection()
    {
        dir = (dir == LocalizationDirection.rtl) ? LocalizationDirection.ltr : LocalizationDirection.rtl;

        GlobalState.SetDirection(dir);

        await _jsModule!.InvokeVoidAsync("switchDirection", dir.ToString());
        await Direction.SetValueFor(container, dir.ToAttributeValue());
    }

    public async void SwitchTheme()
    {
        await Task.Delay(50);

        if (_inDarkMode)
            baseLayerLuminance = StandardLuminance.DarkMode;
        else
            baseLayerLuminance = StandardLuminance.LightMode;

        await BaseLayerLuminance.SetValueFor(container, baseLayerLuminance.GetLuminanceValue());

        GlobalState.SetLuminance(baseLayerLuminance);

        await _jsModule!.InvokeVoidAsync("switchHighlightStyle", baseLayerLuminance == StandardLuminance.DarkMode);
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
                //_selectValue = value;
                await AccentBaseColor.SetValueFor(container, value.ToSwatch());
            }
            else
            {
                //_selectValue = "default";
                await AccentBaseColor.DeleteValueFor(container);
            }
        }
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
