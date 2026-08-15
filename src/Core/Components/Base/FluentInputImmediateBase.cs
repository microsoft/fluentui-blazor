// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
    Technical notes: how "Immediate" mode avoids data loss under network/server latency
    -------------------------------------------------------------------------------------

    1. Client side (TextInput.ts, attachImmediateEvent)
       - Every native 'input' event resets a short timer (ImmediateDelay, default 200ms).
       - Once typing pauses for that long, a custom 'immediate' DOM event is dispatched,
         carrying the element's CURRENT value read at dispatch time (not at keystroke time).
       - This collapses a burst of fast keystrokes into a single event, reducing round trips.

    2. Server side: keeping the confirmed value correct (InputHandlerAsync)
       - Each 'immediate' event (@ontextimmediate) is a round trip (e.g. a SignalR message for
         Blazor Server). Under latency, several of these can be in flight/queued at once, and
         are NOT guaranteed to finish in the order they started.
       - Every call is tagged with a monotonically increasing sequence number
         (_immediateSequence) and serialized through a SemaphoreSlim (_immediateGate) so only
         one ChangeHandlerAsync ever runs at a time.
       - After acquiring the gate, a call re-checks whether it is still the latest sequence; if
         a newer keystroke was already queued behind it, it is skipped (the newer call will
         supersede it with a fresher value read from the DOM). This guarantees CurrentValue
         always converges to the text of the LAST keystroke, never an older, superseded one.

    3. Browser side: protecting what the user sees while typing (ImmediateValueAsString)
       - Step 2 alone only protects the CONFIRMED value; it cannot protect the live DOM, because
         the server can never know about a keystroke the browser hasn't transmitted yet (still
         waiting out its own client-side debounce, or still in network transit).
       - If Blazor re-renders and pushes any value back into the native element while the user is
         still typing, it can reset the field to a stale, shorter string mid-keystroke, scrambling
         what the user typed - confirmed via manual and scripted repro on a throttled/high-latency
         connection.
       - Fix: the rendered value is FROZEN while the field has focus (_hasFocus /
         _frozenValueAsString) - Blazor never touches the native `value` while the user might
         still be typing, and resyncs to the confirmed value only once the field loses focus
         (FocusOutHandlerAsync). This is a hard guarantee, not a timing heuristic: a fixed-duration
         "wait a bit before rendering" timer was considered and rejected, since it can always be
         defeated by high-enough or variable network latency.

    4. Render suppression (ShouldRender)
       - Re-renders are also skipped, more generally, while a keystroke is known but not yet
         confirmed (_pendingImmediateText is not null), avoiding flicker/unnecessary work in other
         parts of the UI that read the bound Value while typing is still in progress.
*/

/// <summary>
/// A base class for Fluent UI form input components, including an immediate validation mode (using the `OnInput` event).
/// This base class automatically integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>,
/// which must be supplied as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract class FluentInputImmediateBase<TValue> : FluentInputBase<TValue>
{
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
    /// Handler for the OnInput event. The client already debounces by <see cref="ImmediateDelay"/> before
    /// dispatching it, so no additional delay is applied here.
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

    /// <summary>
    /// Renders only when the last immediate input event has been processed,
    /// to avoid sending intermediate values back to the browser while the user is still typing.
    /// </summary>
    protected override bool ShouldRender() => _pendingImmediateText is null;

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
    /// Marks the field as touched (<see cref="IFluentField.FocusLost"/>) and unfreezes
    /// <see cref="ImmediateValueAsString"/> so the next render resyncs it to the confirmed value.
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
