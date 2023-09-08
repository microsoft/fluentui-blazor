using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel : IDialogContentComponent<GlobalState>, IAsyncDisposable
{
    private LocalizationDirection _dir;
    private StandardLuminance _baseLayerLuminance = StandardLuminance.LightMode;
    private OfficeColor _selectedColorOption;
    private bool _inDarkMode;
    private bool _rtl;
    private IJSObjectReference? _jsModule;
    private ElementReference _container;

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
        _inDarkMode = Content.Luminance == StandardLuminance.DarkMode;
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
                 "./_content/FluentUI.Demo.Shared/Shared/DemoMainLayout.razor.js");
        }
    }

    public async Task UpdateDirection()
    {
        _dir = (_rtl ? LocalizationDirection.rtl : LocalizationDirection.ltr);

        Content.Dir = _dir;

        await _jsModule!.InvokeVoidAsync("switchDirection", _dir.ToString());
        await Direction.SetValueFor(_container, _dir.ToAttributeValue());

        StateHasChanged();
    }

    public async void UpdateTheme()
    {
        if (_inDarkMode)
        {
            _baseLayerLuminance = StandardLuminance.DarkMode;
        }
        else
        {
            _baseLayerLuminance = StandardLuminance.LightMode;
        }

        Content.Luminance = _baseLayerLuminance;
        
        await BaseLayerLuminance.SetValueFor(_container, _baseLayerLuminance.GetLuminanceValue());
        await _jsModule!.InvokeVoidAsync("switchHighlightStyle", _baseLayerLuminance == StandardLuminance.DarkMode);
    }

    private async void HandleColorChange(ChangeEventArgs args)
    {
        string? value = args.Value?.ToString();
        if (!string.IsNullOrEmpty(value))
        {
            if (value != "default")
            {
                await AccentBaseColor.SetValueFor(_container, value.ToSwatch());
            }
            else
            {
                await AccentBaseColor.DeleteValueFor(_container);
            }

            Content.Color = value;
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