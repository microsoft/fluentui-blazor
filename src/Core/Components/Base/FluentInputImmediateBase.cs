// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components, including an immediate validation mode (using the `OnInput` event).
/// This base class automatically integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>,
/// which must be supplied as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract class FluentInputImmediateBase<TValue> : FluentInputBase<TValue>, IFluentInputImmediate
{
    private readonly FluentInputImmediateManager _immediateManager;

    /// <summary />
    protected FluentInputImmediateBase(LibraryConfiguration configuration) : base(configuration)
    {
        _immediateManager = new FluentInputImmediateManager(this);
    }

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 200 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 200;

    /// <summary>
    /// Raised when the field gains focus. Since the native `onfocusin` DOM event is owned internally
    /// by <see cref="FocusInHandlerAsync"/> (an <c>@onfocusin</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-in from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusIn { get; set; }

    /// <summary>
    /// Raised when the field loses focus. Since the native `onfocusout` DOM event is owned internally
    /// by <see cref="FocusOutHandlerAsync"/> (an <c>@onfocusout</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-out from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

    /// <summary>
    /// Gets the string to render as the native element's `value`. Only applies the freeze/pending
    /// logic below when <see cref="Immediate"/> is enabled - otherwise there's no per-keystroke round
    /// trip to protect against, so this just returns <see cref="InputBase{TValue}.CurrentValueAsString"/>
    /// directly (never blocking a legitimate external value update while the field has focus).
    /// While the field has focus in Immediate mode, the value is frozen (never updated) since the
    /// server can never know about keystrokes the browser hasn't transmitted yet - re-rendering it
    /// mid-typing (e.g. under network/server latency) would reset the field to a stale, already-
    /// superseded value and scramble what the user is typing. It resyncs to the latest known text
    /// (the pending, not-yet-confirmed keystroke if any, else <see cref="InputBase{TValue}.CurrentValueAsString"/>)
    /// as soon as the field loses focus.
    /// </summary>
    protected string? ImmediateValueAsString => _immediateManager.GetImmediateValueAsString(CurrentValueAsString);

    /// <summary>
    /// Handler for the OnInput event. The client already debounces by <see cref="ImmediateDelay"/> before
    /// dispatching it, so no additional delay is applied here.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task InputHandlerAsync(ChangeEventArgs e) => _immediateManager.InputHandlerAsync(e, ChangeHandlerAsync);

    /// <summary>
    /// Renders only when the last immediate input event has been processed,
    /// to avoid sending intermediate values back to the browser while the user is still typing.
    /// </summary>
    protected override bool ShouldRender() => _immediateManager.ShouldRender();

    /// <summary>
    /// Marks the field as focused so <see cref="ImmediateValueAsString"/> freezes until it loses focus,
    /// then forwards the event to <see cref="OnFocusIn"/> if a consumer is observing it.
    /// </summary>
    /// <param name="e"></param>
    protected virtual Task FocusInHandlerAsync(FocusEventArgs e) => _immediateManager.FocusInHandlerAsync(e);

    /// <summary>
    /// Marks the field as touched (<see cref="IFluentField.FocusLost"/>) and unfreezes
    /// <see cref="ImmediateValueAsString"/> so the next render resyncs it to the confirmed value,
    /// then forwards the event to <see cref="OnFocusOut"/> if a consumer is observing it.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e) => _immediateManager.FocusOutHandlerAsync(e, () => FocusLost = true);

    /// <summary>
    /// Initializes the immediate event if the immediate mode is enabled.
    /// </summary>
    protected virtual Task InitializeImmediateAsync() => _immediateManager.InitializeImmediateAsync(JSRuntime, Id);
}
