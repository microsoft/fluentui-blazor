using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Extends the OnKeyDown blazor event to provide a more fluent way to evaluate the key code.
/// The anchor must refer to the ID of an element (or sub-element) accepting the focus.
/// </summary>
public partial class FluentKeyCode : IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/KeyCode/FluentKeyCode.razor.js";
    private string _javaScriptEventId = string.Empty;
    private DotNetObjectReference<FluentKeyCode>? _dotNetHelper = null;
    private readonly KeyCode[] _Modifiers = new[] { KeyCode.Shift, KeyCode.Alt, KeyCode.Ctrl, KeyCode.Meta };

    /// <summary>
    /// Prevent multiple KeyDown events.
    /// </summary>
    public static bool PreventMultipleKeyDown = false;

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? _jsModule { get; set; }

    /// <summary />
    private ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets whether the KeyCode engine is global (using document DOM element) or not (only for <see cref="Anchor"/> or <see cref="ChildContent"/>).
    /// </summary>
    [Parameter]
    public bool GlobalDocument { get; set; } = false;

    /// <summary>
    /// Gets or sets the control identifier associated with the KeyCode engine.
    /// If not set, the KeyCode will be applied to the FluentKeyCode content: see <see cref="ChildContent"/>.
    /// This attribute is ignored when the <see cref="ChildContent" /> is used..
    /// </summary>
    [Parameter]
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content to be managed by the KeyCode engine.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event triggered when a KeyDown event is raised.
    /// </summary>
    [Parameter]
    public EventCallback<FluentKeyCodeEventArgs> OnKeyDown { get; set; }

    /// <summary>
    /// Event triggered when a KeyUp event is raised.
    /// </summary>
    [Parameter]
    public EventCallback<FluentKeyCodeEventArgs> OnKeyUp { get; set; }

    /// <summary>
    /// Ignore modifier keys (Shift, Alt, Ctrl, Meta) when evaluating the key code.
    /// </summary>
    [Parameter]
    public bool IgnoreModifier { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of <see cref="KeyCode"/> to accept, and only this list, when evaluating the key code.
    /// </summary>
    [Parameter]
    public KeyCode[] Only { get; set; } = Array.Empty<KeyCode>();

    /// <summary>
    /// Gets or sets the list of <see cref="KeyCode"/> to ignore when evaluating the key code.
    /// </summary>
    [Parameter]
    public KeyCode[] Ignore { get; set; } = Array.Empty<KeyCode>();

    /// <summary>
    /// Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; } = false;

    /// <summary>
    /// Gets or sets a way to tells the user agent that if the event does not get explicitly handled, its default action should not be taken as it normally would be.
    /// </summary>
    [Parameter]
    public bool PreventDefault { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of <see cref="KeyCode"/> to tells the user agent that if the event does not get explicitly handled,
    /// its default action should not be taken as it normally would be.
    /// </summary>
    [Parameter]
    public KeyCode[] PreventDefaultOnly { get; set; } = Array.Empty<KeyCode>();

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary />
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (ChildContent is null && string.IsNullOrEmpty(Anchor) && !GlobalDocument)
            {
                throw new ArgumentNullException(Anchor, $"The {nameof(Anchor)} parameter must be set to the ID of an element. Or the {nameof(ChildContent)} must be set to apply the KeyCode engine to this content.");
            }

            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            _dotNetHelper = DotNetObjectReference.Create(this);

            var eventNames = string.Join(";", new[]
            {
                OnKeyDown.HasDelegate ? "KeyDown" : string.Empty,
                OnKeyUp.HasDelegate ? "KeyUp" : string.Empty,
            });

            _javaScriptEventId = await _jsModule.InvokeAsync<string>("RegisterKeyCode", GlobalDocument, eventNames.Length > 1 ? eventNames : "KeyDown", Anchor, ChildContent is null ? null : Element, Only, IgnoreModifier ? Ignore.Union(_Modifiers) : Ignore, StopPropagation, PreventDefault, PreventDefaultOnly, _dotNetHelper, PreventMultipleKeyDown);
        }
    }

    /// <summary>
    /// Internal method.
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="value"></param>
    /// <param name="ctrlKey"></param>
    /// <param name="shiftKey"></param>
    /// <param name="altKey"></param>
    /// <param name="metaKey"></param>
    /// <param name="location"></param>
    /// <param name="targetId"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyDownRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId)
    {
        if (OnKeyDown.HasDelegate)
        {
            await OnKeyDown.InvokeAsync(FluentKeyCodeEventArgs.Instance("keydown", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId));
        }
    }

    /// <summary>
    /// Internal method.
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="value"></param>
    /// <param name="ctrlKey"></param>
    /// <param name="shiftKey"></param>
    /// <param name="altKey"></param>
    /// <param name="metaKey"></param>
    /// <param name="location"></param>
    /// <param name="targetId"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyUpRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId)
    {
        if (OnKeyUp.HasDelegate)
        {
            await OnKeyUp.InvokeAsync(FluentKeyCodeEventArgs.Instance("keyup", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId));
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null && !string.IsNullOrEmpty(_javaScriptEventId))
            {
                await _jsModule.InvokeVoidAsync("UnregisterKeyCode", _javaScriptEventId);
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}

