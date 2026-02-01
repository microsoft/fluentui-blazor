// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentInputBase<TValue>
{
    private readonly Debounce _debounce = new();

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the <see cref="ValueChanged"/> event.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 0;

    /// <summary>
    /// Handler for the OnChange event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual async Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        var _notifyCalled = false;

        if (typeof(TValue) == typeof(string))
        {
            object? value = e.Value?.ToString();  
            await SetCurrentValueAsync((TValue?)(value ?? default));

            _notifyCalled = true;
        }
        
        if (FieldBound && !_notifyCalled)
        {
            CascadedEditContext?.NotifyFieldChanged(FieldIdentifier);
        }
    }

    /// <summary>
    /// Handler for the OnInput event, with an optional delay to avoid to raise the <see cref="ValueChanged"/> event too often.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual async Task InputHandlerAsync(ChangeEventArgs e) // TODO: To update in all Input fields
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
}
