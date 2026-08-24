// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
    Use this Manager class to coordinate delayed input events and protect the rendered value while the user is typing.
    Example usage in a component:

    public class MyComponent : IFluentInputImmediate
    {
        private readonly FluentInputImmediateManager _immediateManager;

        protected MyComponent()
        {
            _immediateManager = new FluentInputImmediateManager(this);
        }

        [Parameter]
        public bool Immediate { get; set; } = false;

        [Parameter]
        public int ImmediateDelay { get; set; } = 200;

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocusIn { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

        protected string? ImmediateValueAsString
            => _immediateManager.GetImmediateValueAsString(CurrentValueAsString);

        protected virtual Task InputHandlerAsync(ChangeEventArgs e)
            => _immediateManager.InputHandlerAsync(e, ChangeHandlerAsync);

        protected override bool ShouldRender()
            => _immediateManager.ShouldRender();

        protected virtual Task FocusInHandlerAsync(FocusEventArgs e)
            => _immediateManager.FocusInHandlerAsync(e);

        protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
            => _immediateManager.FocusOutHandlerAsync(e, () => FocusLost = true);

        protected virtual Task InitializeImmediateAsync()
            => _immediateManager.InitializeImmediateAsync(JSRuntime, Id);
    }    
*/

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
       - Fix: while the field has focus the rendered value is FROZEN (_hasFocus / _isFrozen /
         _frozenValueAsString) - it is kept CONSTANT, so Blazor's diff never touches the native
         `value` while the user might still be typing, and it resyncs to the confirmed value once
         the field loses focus (FocusOutHandlerAsync). This is a hard guarantee, not a timing
         heuristic: a fixed-duration "wait a bit before rendering" timer was considered and
         rejected, since it can always be defeated by high-enough or variable network latency.
       - The freeze is scoped to the user's own typing, not to the whole focused period. It holds
         while the confirmed value keeps following what typing produced (_expectedConfirmedValue,
         re-baselined on each confirmed keystroke via _typingConfirmed), and lifts as soon as the
         confirmed value diverges from that: a divergence means the CONSUMER assigned the value
         from the outside (a lookup dialog selection, a validation rule rewriting the field, a
         value pushed by another component), which is newer than anything the user typed and has
         to reach the browser. Freezing on focus alone made such an assignment invisible until the
         field lost focus - and permanently invisible for a consumer that returns focus to the
         field after closing a dialog, a common keyboard-navigation pattern.
       - This cannot be keyed on _pendingImmediateText instead ("freeze only while a keystroke is
         in flight"): that flag only covers the server-side processing window, leaving the
         client-side debounce - and the gap between one confirmed keystroke and the next -
         unprotected, which is exactly what step 3 exists to protect.

    4. Render suppression (ShouldRender)
       - Re-renders are also skipped, more generally, while a keystroke is known but not yet
         confirmed (_pendingImmediateText is not null), avoiding flicker/unnecessary work in other
         parts of the UI that read the bound Value while typing is still in progress.
*/

/// <summary>
/// Coordinates delayed input events and protects the rendered value while the user is typing.
/// </summary>
internal sealed class FluentInputImmediateManager
{
    private readonly IFluentInputImmediate _input;

    private long _immediateSequence;
    private readonly SemaphoreSlim _immediateGate = new(1, 1);
    private string? _pendingImmediateText;
    private bool _hasFocus;
    private bool _isFrozen;
    private string? _frozenValueAsString;

    // Confirmed value the freeze was taken against, kept up to date with every value the user's own
    // typing confirms. While the confirmed value follows this trajectory there is nothing new to show
    // and the freeze holds; the moment it diverges, the consumer assigned the value from the outside
    // and that assignment has to reach the browser (see GetImmediateValueAsString).
    private string? _expectedConfirmedValue;

    // Set when this field's own typing has just confirmed a value: the next read re-baselines
    // _expectedConfirmedValue to whatever the confirmed value became, instead of reading the move as
    // an external assignment. Deliberately not predicted from the event value - the component's own
    // parse/format round trip (numeric, date, masked input) can legitimately change the string.
    private bool _typingConfirmed;

    public FluentInputImmediateManager(IFluentInputImmediate input)
    {
        _input = input;
    }

    /// <summary>
    /// Gets the string to render as the native element's `value`. Only applies the freeze/pending
    /// logic below when Immediate is enabled - otherwise there's no per-keystroke round
    /// trip to protect against, so this just returns CurrentValueAsString
    /// directly (never blocking a legitimate external value update while the field has focus).
    /// While the field has focus in Immediate mode, the value is frozen since the
    /// server can never know about keystrokes the browser hasn't transmitted yet - re-rendering it
    /// mid-typing (e.g. under network/server latency) would reset the field to a stale, already-
    /// superseded value and scramble what the user is typing. Freezing works by keeping the rendered
    /// string CONSTANT, so Blazor's diff never touches the native `value` at all. It resyncs to the
    /// latest known text (the pending, not-yet-confirmed keystroke if any, else CurrentValueAsString)
    /// as soon as the field loses focus.
    /// <para>
    /// The freeze is scoped to the user's own typing, NOT to the whole focused period: it holds while
    /// the confirmed value keeps following what typing produced (<see cref="_expectedConfirmedValue"/>),
    /// and lifts as soon as the confirmed value diverges from that. A divergence means the consumer
    /// assigned the value from the outside - picking a row in a lookup dialog, a validation rule
    /// rewriting the field, a value pushed by another component - and that assignment is newer than
    /// anything the user typed, so it must reach the browser. Without this, an external assignment
    /// made while the field has focus stayed invisible until the field lost focus, and then appeared
    /// all at once; a consumer that deliberately returns focus to the field after a dialog (a common
    /// keyboard-navigation pattern) never showed the value at all.
    /// </para>
    /// <para>
    /// Note this cannot be keyed on <see cref="_pendingImmediateText"/> instead: that is only set
    /// while a keystroke is being processed server-side, so it leaves the client-side debounce window
    /// (and the gap between one confirmed keystroke and the next) unprotected - precisely the window
    /// the freeze exists for.
    /// </para>
    /// </summary>
    ///
    /// <code>
    /// Use this method in your component to get the string to render as the native element's `value`.
    /// <example>
    /// protected string? ImmediateValueAsString => _immediateManager.GetImmediateValueAsString(CurrentValueAsString);
    /// </example>
    /// </code>
    internal string? GetImmediateValueAsString(string? currentValueAsString)
    {
        if (!_input.Immediate)
        {
            return currentValueAsString;
        }

        if (!_hasFocus)
        {
            return _pendingImmediateText ?? currentValueAsString;
        }

        // First render while focused: take the freeze, and remember the confirmed value it is
        // relative to. An explicit flag (not a null check on the frozen string) so that freezing on
        // an empty/null value still sticks.
        if (!_isFrozen)
        {
            _isFrozen = true;
            _frozenValueAsString = _pendingImmediateText ?? currentValueAsString;
            _expectedConfirmedValue = currentValueAsString;
        }
        else if (_typingConfirmed)
        {
            // The confirmed value moved because of this field's own typing: re-baseline and KEEP the
            // freeze, so the rendered string stays constant and the browser is left alone.
            _typingConfirmed = false;
            _expectedConfirmedValue = currentValueAsString;
        }
        else if (!string.Equals(currentValueAsString, _expectedConfirmedValue, StringComparison.Ordinal))
        {
            // The confirmed value moved for a reason other than this field's typing: adopt it, so the
            // rendered string changes and Blazor pushes it to the browser.
            _frozenValueAsString = currentValueAsString;
            _expectedConfirmedValue = currentValueAsString;
        }

        return _frozenValueAsString;
    }

    /// <summary>
    /// Handler for the OnInput event. The client already debounces by ImmediateDelay before
    /// dispatching it, so no additional delay is applied here.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="changeHandlerAsync"></param>
    /// 
    /// <code>
    /// Use this method in your component to handle the OnInput event.
    /// <example>
    /// protected virtual Task InputHandlerAsync(ChangeEventArgs e) => _immediateManager.InputHandlerAsync(e, ChangeHandlerAsync);
    /// </example>
    /// </code>
    internal async Task InputHandlerAsync(ChangeEventArgs e, Func<ChangeEventArgs, Task> changeHandlerAsync)
    {
        if (!_input.Immediate)
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

            try
            {
                await changeHandlerAsync(e);
            }
            finally
            {
                // Always clear (success, exception, or component teardown mid-flight) so a failure
                // here can't leave ShouldRender() stuck returning false forever; the exception, if
                // any, still propagates normally after this.
                if (Volatile.Read(ref _immediateSequence) == sequence)
                {
                    _pendingImmediateText = null;

                    // Whatever the confirmed value became, it came from this field's typing - the next
                    // read re-baselines against it instead of mistaking it for an external assignment.
                    _typingConfirmed = true;
                }
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
    /// 
    /// <code>
    /// Use this method in your component to determine whether to render.
    /// <example>
    /// protected override bool ShouldRender() => _immediateManager.ShouldRender();
    /// </example>
    /// </code>
    internal bool ShouldRender() => _pendingImmediateText is null;

    /// <summary>
    /// Marks the field as focused and invokes the focus-in callback.
    /// </summary>
    internal async Task FocusInHandlerAsync(FocusEventArgs e)
    {
        _hasFocus = true;

        if (_input.OnFocusIn.HasDelegate)
        {
            await _input.OnFocusIn.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Marks the field as unfocused and invokes the focus-out callback.
    /// </summary>
    internal async Task FocusOutHandlerAsync(FocusEventArgs e, Func<Task>? action = null)
    {
        _hasFocus = false;
        _isFrozen = false;
        _frozenValueAsString = null;
        _expectedConfirmedValue = null;
        _typingConfirmed = false;

        if (action is not null)
        {
            await action();
        }

        if (_input.OnFocusOut.HasDelegate)
        {
            await _input.OnFocusOut.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Initializes the immediate event if the immediate mode is enabled.
    /// </summary>
    internal async Task InitializeImmediateAsync(IJSRuntime jsRuntime, string? id)
    {
        if (_input.Immediate)
        {
            await jsRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.TextInput.attachImmediateEvent", id, _input.ImmediateDelay);
        }
    }
}