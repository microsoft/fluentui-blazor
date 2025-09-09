// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuButton : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/MenuButton/FluentMenuButton.razor.js";
    private const string ANCHORED_REGION_JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/AnchoredRegion/FluentAnchoredRegion.razor.js";

    private bool _visible;
    private Color _iconColor = Color.Fill;
    private string? _buttonId;
    private string? _menuId;

    protected string? MenuStyleValue => new StyleBuilder(MenuStyle)
        .AddStyle("position", "relative")
        .Build();

    /// <summary>
    /// Gets or sets a reference to the button.
    /// </summary>
    [Parameter]
    public FluentButton? Button { get; set; }

    /// <summary>
    /// Gets or sets the button appearance.
    /// </summary>
    [Parameter]
    public Appearance ButtonAppearance { get; set; } = Appearance.Accent;

    /// <summary>
    /// Gets or sets a reference to the menu.
    /// </summary>
    [Parameter]
    public FluentMenu? Menu { get; set; }

    /// <summary>
    /// Gets or sets the texts shown on the button. This property will be ignored if <see cref="ButtonContent"/> is provided.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the button style.
    /// </summary>
    [Parameter]
    public string? ButtonStyle { get; set; }

    /// <summary>
    /// Gets or sets the menu style.
    /// </summary>
    [Parameter]
    public string? MenuStyle { get; set; }

    /// <summary>
    /// Gets or sets the items to show in the menu.
    /// </summary>
    [Parameter]
    public Dictionary<string, string> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the content to be shown in the menu.
    /// Should consist of <see cref="FluentMenuItem"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The callback to invoke when a menu item is chosen.
    /// Using this event prevents the execution of any OnClick event on an included FluentMenuItem.
    /// </summary>
    [Parameter]
    public EventCallback<MenuChangeEventArgs> OnMenuChanged { get; set; }

    /// <summary>
    /// The content to be rendered inside the button. This parameter should be supplied if you do not want to render a chevron
    /// on the menu button. Only one of <see cref="Text"/> or <see cref="ButtonContent"/> may be provided.
    /// </summary>
    [Parameter]
    public RenderFragment? ButtonContent { get; set; }

    private IJSObjectReference? _jsModule { get; set; }
    private IJSObjectReference? _anchoredRegionModule { get; set; }
    private DotNetObjectReference<FluentMenuButton>? _dotNetHelper;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    protected override void OnInitialized()
    {
        _buttonId = Identifier.NewId();
        _menuId = $"{_buttonId}-menu";
    }

    protected override void OnParametersSet()
    {
        if (Text is not null && ButtonContent is not null)
        {
            throw new ArgumentException($"Only one of the parameters {nameof(Text)} or {nameof(ButtonContent)} can be provided.");
        }

        _iconColor = ButtonAppearance == Appearance.Accent ? Color.Fill : Color.FillInverse;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            _anchoredRegionModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", ANCHORED_REGION_JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            _dotNetHelper = DotNetObjectReference.Create(this);
        }

        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("fluentMenuButtonOnRender", _buttonId, _visible ? _menuId : null, _anchoredRegionModule, _dotNetHelper);
        }
    }

    [JSInvokable]
    public Task ToggleMenuAsync()
    {
        _visible = !_visible;
        return InvokeAsync(StateHasChanged);
    }

    private async Task OnMenuChangeAsync(MenuChangeEventArgs args)
    {
        if (!OnMenuChanged.HasDelegate)
        {
            return;
        }

        if (args is not null && args.Id is not null)
        {
            await OnMenuChanged.InvokeAsync(args);
        }

        _visible = false;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("fluentMenuButtonDispose", _buttonId);
                await _jsModule.DisposeAsync();
            }

            if (_anchoredRegionModule is not null)
            {
                await _anchoredRegionModule.DisposeAsync();
            }

            _dotNetHelper?.Dispose();
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
