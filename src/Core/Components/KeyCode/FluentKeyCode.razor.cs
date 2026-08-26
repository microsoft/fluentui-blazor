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
    private DotNetObjectReference<FluentKeyCode>? _dotNetHelper;
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
    /// Gets or sets the HTML tag name to use when rendering the component.
    /// The default value is "Span".
    /// </summary>
    [Parameter]
    public FluentKeyCodeTag TagName { get; set; } = FluentKeyCodeTag.Span;

    /// <summary>
    /// Gets or sets the control identifier associated with the KeyCode engine.
    /// If not set, the KeyCode will be applied to the FluentKeyCode content: see <see cref="ChildContent"/>.
    /// This parameter is ignored when <see cref="ChildContent"/> is used.
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
    /// Gets or sets whether modifier keys (Shift, Alt, Ctrl, Meta) should be ignored when evaluating the key code.
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
    /// Gets or sets whether the default browser action should be prevented for all key events.
    /// Use <see cref="PreventDefaultOnly"/> to restrict prevention to specific keys.
    /// </summary>
    [Parameter]
    public bool PreventDefault { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of <see cref="KeyCode"/> values for which the default browser action should be prevented.
    /// Use <see cref="PreventDefault"/> to prevent the default action for all key events.
    /// </summary>
    [Parameter]
    public KeyCode[] PreventDefaultOnly { get; set; } = [];

    /// <summary>
    /// Gets or sets whether the key pressed can be repeated.
    /// </summary>
    [Parameter]
    public bool StopRepeat { get; set; }

    /// <summary>
    /// Gets or sets whether multiple consecutive KeyDown events (key held down) should be suppressed.
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

            _dotNetHelper = DotNetObjectReference.Create(this);
            var eventNames = string.Join(';', new[]
            {
                OnKeyDown.HasDelegate ? "KeyDown" : string.Empty,
                OnKeyUp.HasDelegate ? "KeyUp" : string.Empty,
            });

            _javaScriptEventId = await JSRuntime.InvokeAsync<string>("Microsoft.FluentUI.Blazor.Components.KeyCode.RegisterKeyCode",
                GlobalDocument,
                eventNames.Length > 1 ? eventNames : "KeyDown",
                Anchor,
                ChildContent is null ? null : Element,
                Only,
                IgnoreModifier ? Ignore.Union(_Modifiers).ToArray() : Ignore,
                StopPropagation,
                PreventDefault,
                PreventDefaultOnly,
                _dotNetHelper,
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
    public override async ValueTask DisposeAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(_javaScriptEventId))
            {
                await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.KeyCode.UnregisterKeyCode", _javaScriptEventId);
                _javaScriptEventId = string.Empty;
            }
        }
        finally
        {
            _dotNetHelper?.Dispose();
            _dotNetHelper = null;
            await base.DisposeAsync();
        }
    }
}

