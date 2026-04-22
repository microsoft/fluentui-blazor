// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentPopover : FluentComponentBase, IAsyncDisposable
{
    private const string ANCHORED_REGION_JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/AnchoredRegion/FluentAnchoredRegion.razor.js";

    private FluentAnchoredRegion AnchoredRegion = default!;
    private DotNetObjectReference<FluentPopover>? _dotNetHelper;
    private IJSObjectReference? _anchoredRegionModule;
    private bool _disposed;
    private bool _previousOpen;

    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the id of the component the popover is positioned relative to.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default horizontal position of the region relative to the anchor element.
    /// Default is unset. See <seealso cref="HorizontalPosition"/>
    /// </summary>
    [Parameter]
    public HorizontalPosition? HorizontalPosition { get; set; } = AspNetCore.Components.HorizontalPosition.Unset;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the horizontal axis.
    /// Default is true which places the region aligned with the anchor element.
    /// </summary>
    [Parameter]
    public bool HorizontalInset { get; set; } = true;

    /// <summary>
    /// Gets or sets the default vertical position of the region relative to the anchor element.
    /// Default is unset. See <seealso cref="VerticalPosition"/>
    /// </summary>
    [Parameter]
    public VerticalPosition? VerticalPosition { get; set; } = AspNetCore.Components.VerticalPosition.Bottom;

    /// <summary>
    /// How short the space allocated to the default position has to be before the tallest area is selected for layout.
    /// </summary>
    [Parameter]
    public int VerticalThreshold { get; set; } = 0;

    /// <summary>
    /// How narrow the space allocated to the default position has to be before the widest area is selected for layout.
    /// </summary>
    [Parameter]
    public int HorizontalThreshold { get; set; } = 0;

    /// <summary>
    /// Gets or sets a value indicating whether the region is positioned using css "position: fixed".
    /// Otherwise the region uses "position: absolute".
    /// Fixed placement allows the region to break out of parent containers.
    /// </summary>
    [Parameter]
    public bool? FixedPlacement { get; set; }

    /// <summary>
    /// Gets or sets popover opened state.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Callback for when open state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the content of the header part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Gets or sets the content of the body part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    /// Gets or sets the content of the footer part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }

    /// <summary>
    /// Gets or sets whether the element should receive the focus when the component is loaded.
    /// If this is the case, the user cannot navigate to other elements of the page while the Popup is open.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = true;

    /// <summary>
    /// Gets or sets the keys that can be used to close the popover.
    /// By default, Escape
    /// </summary>
    [Parameter]
    public KeyCode[]? CloseKeys { get; set; } = new[] { KeyCode.Escape };

    /// <summary />
    protected override void OnInitialized()
    {
        // Id is always needed to identify the popover content element.
        if (string.IsNullOrEmpty(Id))
        {
            Id = Identifier.NewId();
        }
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (Header is null && Body is null && Footer is null)
        {
            throw new ArgumentException("At least one of Header, Body or Footer must be set.");
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _anchoredRegionModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ANCHORED_REGION_JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            _dotNetHelper = DotNetObjectReference.Create(this);
        }

        if (!_disposed && _anchoredRegionModule is not null && Open != _previousOpen)
        {
            _previousOpen = Open;
            if (Open)
            {
                var closeKeyCodes = CloseKeys?.Select(k => (int)k).ToArray() ?? Array.Empty<int>();
                await _anchoredRegionModule.InvokeVoidAsync("initializeKeyboardNavigation", AnchorId, Id, _dotNetHelper, closeKeyCodes);
            }
            else
            {
                await _anchoredRegionModule.InvokeVoidAsync("disposeKeyboardNavigation", AnchorId);
            }
        }
    }

    /// <summary>
    /// Closes the popover. Called from JavaScript keyboard navigation.
    /// </summary>
    [JSInvokable]
    public async Task CloseAsync()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
        StateHasChanged();
    }

    /// <summary>
    /// Closes the popover and returns focus to the original element (used by the overlay on outside-click).
    /// </summary>
    protected virtual async Task CloseOverlayAsync()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
        await AnchoredRegion.FocusToOriginalElementAsync();
    }

    /// <summary />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _dotNetHelper?.Dispose();

        try
        {
            if (_anchoredRegionModule is not null)
            {
                await _anchoredRegionModule.InvokeVoidAsync("disposeKeyboardNavigation", AnchorId);
                await _anchoredRegionModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
