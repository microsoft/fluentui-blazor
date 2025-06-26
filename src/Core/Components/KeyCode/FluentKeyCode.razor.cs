// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Extends the OnKeyDown blazor event to provide a more fluent way to evaluate the key code.
/// The anchor must refer to the ID of an element (or sub-element) accepting the focus.
/// </summary>
public partial class FluentKeyCode : FluentComponentBase, IFluentComponentElementBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "KeyCode/FluentKeyCode.razor.js";
    private string _javaScriptEventId = string.Empty;
    private readonly KeyCode[] _Modifiers = [KeyCode.Shift, KeyCode.Alt, KeyCode.Ctrl, KeyCode.Meta];

    /// <summary />
    public FluentKeyCode(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

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
    public KeyCode[] Only { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of <see cref="KeyCode"/> to ignore when evaluating the key code.
    /// </summary>
    [Parameter]
    public KeyCode[] Ignore { get; set; } = [];

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
    public KeyCode[] PreventDefaultOnly { get; set; } = [];

    /// <summary>
    /// Gets or sets whether the key pressed can be repeated.
    /// </summary>
    [Parameter]
    public bool StopRepeat { get; set; }

    /// <summary>
    /// Prevent multiple KeyDown events.
    /// </summary>
    [Parameter]
    public bool PreventMultipleKeyDown { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (ChildContent is null && string.IsNullOrEmpty(Anchor) && !GlobalDocument)
            {
                throw new ArgumentNullException(Anchor, $"The {nameof(Anchor)} parameter must be set to the ID of an element. Or the {nameof(ChildContent)} must be set to apply the KeyCode engine to this content.");
            }

            var dotNetHelper = DotNetObjectReference.Create(this);
            var eventNames = string.Join(';', new[]
            {
                OnKeyDown.HasDelegate ? "KeyDown" : string.Empty,
                OnKeyUp.HasDelegate ? "KeyUp" : string.Empty,
            });

            // Import the JavaScript module
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            _javaScriptEventId = await jsModule.InvokeAsync<string>("Microsoft.FluentUI.Blazor.KeyCode.RegisterKeyCode",
                GlobalDocument,
                eventNames.Length > 1 ? eventNames : "KeyDown",
                Anchor,
                ChildContent is null ? null : Element,
                Only,
                IgnoreModifier ? Ignore.Union(_Modifiers) : Ignore,
                StopPropagation,
                PreventDefault,
                PreventDefaultOnly,
                dotNetHelper,
                PreventMultipleKeyDown,
                StopRepeat);
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
    /// <param name="repeat"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyDownRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId, bool repeat)
    {
        if (OnKeyDown.HasDelegate)
        {
            await OnKeyDown.InvokeAsync(FluentKeyCodeEventArgs.Instance("keydown", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId, repeat));
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
    /// <param name="repeat"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyUpRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId, bool repeat)
    {
        if (OnKeyUp.HasDelegate)
        {
            await OnKeyUp.InvokeAsync(FluentKeyCodeEventArgs.Instance("keyup", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId, repeat));
        }
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.KeyCode.UnregisterKeyCode", _javaScriptEventId);
    }
}

