// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components, including an immediate validation mode (using the `OnInput` event).
/// This base class automatically integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>,
/// which must be supplied as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract partial class FluentInputImmediateBase<TValue> : FluentInputBase<TValue>
{
    private int _immediateHandlersInProgress;
    private long _immediateSequence;
    private readonly SemaphoreSlim _immediateGate = new(1, 1);
    private string? _pendingImmediateText;
    private bool _hasFocus;
    private string? _frozenValueAsString;

    /// <summary />
    protected FluentInputImmediateBase(LibraryConfiguration configuration) : base(configuration) { }

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
    /// Gets the string to render as the native element's `value`. While the field has focus, the value is
    /// frozen (never updated) since the server can never know about keystrokes the browser hasn't transmitted
    /// yet - re-rendering it mid-typing (e.g. under network/server latency) would reset the field to a stale,
    /// already-superseded value and scramble what the user is typing. It resyncs to the latest known text
    /// (the pending, not-yet-confirmed keystroke if any, else <see cref="InputBase{TValue}.CurrentValueAsString"/>)
    /// as soon as the field loses focus.
    /// </summary>
    protected string? ImmediateValueAsString => _hasFocus
        ? _frozenValueAsString ??= _pendingImmediateText ?? CurrentValueAsString
        : _pendingImmediateText ?? CurrentValueAsString;

    /// <summary>
    /// Handler for the OnInput event, with an optional delay to avoid to raise the <see cref="InputBase{TValue}.ValueChanged"/> event too often.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual async Task InputHandlerAsync(ChangeEventArgs e)
    {
        if (!Immediate)
        {
            return;
        }

        // Cache what the browser currently shows before any await, so a render triggered while this
        // call is still in flight reflects the latest typed text instead of an older confirmed value.
        _pendingImmediateText = e.Value?.ToString();

        // Order of arrival isn't guaranteed under network/server latency (e.g. a slow connection),
        // so tag this call and serialize execution below to prevent an older, still in-flight call
        // from overwriting a value applied by a newer one.
        var sequence = Interlocked.Increment(ref _immediateSequence);

        try
        {
            Interlocked.Increment(ref _immediateHandlersInProgress);

            await _immediateGate.WaitAsync();
            try
            {
                // A newer keystroke was already queued while this call waited: let it win instead.
                if (Volatile.Read(ref _immediateSequence) != sequence)
                {
                    return;
                }

                await ChangeHandlerAsync(e);

                // Nothing newer arrived meanwhile: the confirmed value now matches, drop the override.
                if (Volatile.Read(ref _immediateSequence) == sequence)
                {
                    _pendingImmediateText = null;
                }
            }
            finally
            {
                _immediateGate.Release();
            }
        }
        finally
        {
            Interlocked.Decrement(ref _immediateHandlersInProgress);
        }
    }

    /// <summary>
    /// Renders only when the last immediate input event has been processed,
    /// to avoid sending intermediate values back to the browser while the user is still typing.
    /// </summary>
    protected override bool ShouldRender() => Volatile.Read(ref _immediateHandlersInProgress) == 0;

    /// <summary>
    /// Marks the field as focused so <see cref="ImmediateValueAsString"/> freezes until it loses focus.
    /// </summary>
    /// <param name="e"></param>
    protected virtual Task FocusInHandlerAsync(FocusEventArgs e)
    {
        _hasFocus = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Unfreezes <see cref="ImmediateValueAsString"/> so the next render resyncs it to the confirmed value.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        _hasFocus = false;
        _frozenValueAsString = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Initializes the immediate event if the immediate mode is enabled.
    /// </summary>
    protected virtual async Task InitializeImmediateAsync()
    {
        // Initialize the 'immediate' custom event for the immediate mode
        if (Immediate)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.TextInput.attachImmediateEvent", Id, ImmediateDelay);
        }
    }
}
