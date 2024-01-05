using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentCard : IAsyncDisposable
{
    private static string[] InteractiveEvents = {
        "onclick", "ondoubleclick", "onmouseup", "onmousedown",
        "onpointerup", "onpointerdown", "ontouchstart", "ontouchend",
        "ondragstart", "ondragend"
    };

    private bool _firstRender = true;
    private Task _applySelectionParametersTask = Task.CompletedTask;
    private ElementReference _defaultCheckboxElement;
    private ElementReference? _initializedDefaultCheckboxElement;
    private IJSObjectReference? _jsSelectionHandlers;
    private FocusableGroupMode? _previousFocusMode;

    private FluentCardFloatingAction? _floatingAction;
    private FluentCardHeader? _header;
    private FluentCardPreview? _preview;
    private FluentCardFooter? _footer;

    private bool V2Mode => _header is not null || _preview is not null || _footer is not null;

    private bool SelectableWithDefaultCheckbox =>
        V2Mode && Selectable && _floatingAction is null;

    private bool DefaultCheckboxInitialized =>
        _initializedDefaultCheckboxElement is not null &&
        !string.IsNullOrEmpty(_defaultCheckboxElement.Id) &&
        _initializedDefaultCheckboxElement.Value.Id == _defaultCheckboxElement.Id;

    private IJSObjectReference? _jsModule;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private IFocusManager FocusManager { get; set; } = default!;

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("--card-width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("--card-height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("content-visibility", "visible", !AreaRestricted)
        .AddStyle("contain", "style", !AreaRestricted)
        .AddStyle("overflow", "visible", !AreaRestricted && V2Mode)
        .Build();

    protected string? ClassValue => new CssBuilder()
        .AddClass("fluent-card", V2Mode)
        .AddClass(Class)
        .Build();

    private bool Interactive =>
        AdditionalAttributes?.Any(x => InteractiveEvents.Contains(x.Key)) ?? false;

    private bool Selectable =>
        Selected || SelectedChanged.HasDelegate;

    /// <summary>
    /// By default, content in the card is restricted to the area of the card itself. 
    /// If you want content to be able to overflow the card, set this property to false.
    /// </summary>
    [Parameter]
    public bool AreaRestricted { get; set; } = true;

    /// <summary>
    /// Gets or sets the width of the card. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the card. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Sets the appearance of the card.
    /// </summary>
    [Parameter]
    public CardAppearance? Appearance { get; set; } = CardAppearance.Filled;

    /// <summary>
    /// Defines the orientation of the card.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = AspNetCore.Components.Orientation.Vertical;

    /// <summary>
    /// Controls the card's border radius and padding between inner elements.
    /// </summary>
    [Parameter]
    public CardSize? Size { get; set; } = CardSize.Medium;

    /// <summary>
    /// Sets the focus behavior for the card.
    /// </summary>
    [Parameter]
    public FocusableGroupMode? FocusMode { get; set; }

    /// <summary>
    /// When true, the control will appear selected by user interaction.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Called whenever <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _firstRender = false;
            StateHasChanged();
            return;
        }

        if (!V2Mode)
        {
            return;
        }

        // if parameters are already being applied - e.g. JS interop call is in progress,
        // another call is chained to let the current finish and apply parameters if they changed.
        _applySelectionParametersTask = _applySelectionParametersTask.IsCompleted
            ? Task.Run(ApplySelectionParametersAsync)
            : _applySelectionParametersTask.ContinueWith(_ => ApplySelectionParametersAsync()).Unwrap();

        await _applySelectionParametersTask;

        var defaultFocusMode = Interactive ? FocusableGroupMode.NoTab : FocusableGroupMode.Off;
        var focusMode = FocusMode ?? defaultFocusMode;

        if (_previousFocusMode != focusMode)
        {
            await FocusManager.SetFocusParametersAsync(
                Element,
                focusableGroup: new FocusableGroupParameters { Mode = focusMode },
                tracking:
                    new FocusTrackingParameters
                    {
                        FocusVisible = !Interactive && focusMode != FocusableGroupMode.Off,
                        FocusWithin = true
                    });
            _previousFocusMode = focusMode;
        }
    }

    /// <summary />
    internal void RegisterFloatingAction(FluentCardFloatingAction floatingAction)
    {
        _floatingAction = floatingAction;

        if (!_firstRender)
        {
            StateHasChanged();
        }
    }

    /// <summary />
    internal void UnregisterFloatingAction(FluentCardFloatingAction floatingAction)
    {
        if (_floatingAction != floatingAction)
        {
            return;
        }

        _floatingAction = null;
        StateHasChanged();
    }

    /// <summary />
    internal void RegisterHeader(FluentCardHeader header)
    {
        _header = header;

        if (!_firstRender)
        {
            StateHasChanged();
        }
    }

    /// <summary />
    internal void UnregisterHeader(FluentCardHeader header)
    {
        if (_header != header)
        {
            return;
        }

        _header = null;
        StateHasChanged();
    }

    /// <summary />
    internal void RegisterPreview(FluentCardPreview preview)
    {
        _preview = preview;

        if (!_firstRender)
        {
            StateHasChanged();
        }
    }

    /// <summary />
    internal void UnregisterPreview(FluentCardPreview preview)
    {
        if (_preview != preview)
        {
            return;
        }

        _preview = null;
        StateHasChanged();
    }

    /// <summary />
    internal void RegisterFooter(FluentCardFooter footer)
    {
        _footer = footer;

        if (!_firstRender)
        {
            StateHasChanged();
        }
    }

    /// <summary />
    internal void UnregisterFooter(FluentCardFooter footer)
    {
        if (_footer != footer)
        {
            return;
        }

        _footer = null;
        StateHasChanged();
    }

    /// <summary />
    private Task OnCardSelectedChangeHandler()
    {
        return OnSelectedChange();
    }

    /// <summary />
    private async Task OnSelectedChange()
    {
        if (!Selectable)
        {
            return;
        }

        Selected = !Selected;

        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }
    }

    /// <summary />
    private async Task ApplySelectionParametersAsync()
    {
        if (Selectable && _jsSelectionHandlers is null)
        {
            await InitJsSelectionHandlersAsync();
        }
        else if (!Selectable && _jsSelectionHandlers is not null)
        {
            await RemoveJsSelectionHandlersAsync();
        }

        if (SelectableWithDefaultCheckbox && !DefaultCheckboxInitialized)
        {
            await InitDefaultSelectionCheckboxAsync();
        }
        else if (!SelectableWithDefaultCheckbox)
        {
            _initializedDefaultCheckboxElement = null;
        }
    }

    /// <summary />
    private async Task InitJsSelectionHandlersAsync()
    {
        await EnsureJsModuleLoadedAsync();

        var defaultCheckboxElement = string.IsNullOrEmpty(_defaultCheckboxElement.Id) ?
            (ElementReference?)null : _defaultCheckboxElement;

        _jsSelectionHandlers =
            await _jsModule!.InvokeAsync<IJSObjectReference>("initSelectionHandlers", Element, defaultCheckboxElement);

        if (SelectableWithDefaultCheckbox && defaultCheckboxElement is not null)
        {
            _initializedDefaultCheckboxElement = defaultCheckboxElement;
        }
    }

    /// <summary />
    private async Task RemoveJsSelectionHandlersAsync()
    {
        if (_jsSelectionHandlers is null)
        {
            return;
        }

        await _jsSelectionHandlers.InvokeVoidAsync("removeHandlers");
        await _jsSelectionHandlers.DisposeAsync();
    }

    /// <summary />
    private async Task InitDefaultSelectionCheckboxAsync()
    {
        await EnsureJsModuleLoadedAsync();
        var initializedDefaultCheckboxElement = _defaultCheckboxElement;
        await _jsModule!.InvokeVoidAsync("initSelectionCheckbox", Element, initializedDefaultCheckboxElement);
        _initializedDefaultCheckboxElement = initializedDefaultCheckboxElement;
    }

    /// <summary />
    private async Task EnsureJsModuleLoadedAsync()
    {
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Card/FluentCard.razor.js");
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
            _jsModule = null;
        }
    }
}
