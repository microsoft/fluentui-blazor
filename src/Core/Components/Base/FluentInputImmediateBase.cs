// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components, including an immediate validation mode (using the `OnInput` event).
/// This base class automatically integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>,
/// which must be supplied as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract partial class FluentInputImmediateBase<TValue> : FluentInputBase<TValue>
{
    private readonly Debounce _debounce = new();

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the   event.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 0;

    /// <summary>
    /// Handler for the OnInput event, with an optional delay to avoid to raise the <see cref="FluentInputBase{TValue}.ValueChanged"/> event too often.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual async Task InputHandlerAsync(ChangeEventArgs e)
    {
        if (!Immediate)
        {
            return;
        }

        if (ImmediateDelay > 0)
        {
            await _debounce.RunAsync(ImmediateDelay, async () => await ChangeHandlerAsync(e));
        }
        else
        {
            await ChangeHandlerAsync(e);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _debounce.Dispose();
        }
    }
}
