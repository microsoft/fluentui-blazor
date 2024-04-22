using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentInputBase<TValue>
{
    private PeriodicTimer? _timerForImmediate;
    private CancellationTokenSource _timerCancellationTokenSource = new();

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
        var isValid = TryParseValueFromString(e.Value?.ToString(), out TValue? result, out var validationErrorMessage);

        if (isValid)
        {
            await SetCurrentValueAsync(result ?? default);
            _notifyCalled = true;
        }
        else
        {
            if (FieldBound && CascadedEditContext != null)
            {
                _parsingValidationMessages ??= new ValidationMessageStore(CascadedEditContext);

                _parsingValidationMessages.Clear();
                _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage ?? "Unknown parsing error");
            }
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
        if (Immediate)
        {
            // Raise ChangeHandler after a delay
            if (ImmediateDelay > 0)
            {
                _timerForImmediate = GetNewPeriodicTimer(ImmediateDelay);

                while (await _timerForImmediate.WaitForNextTickAsync(_timerCancellationTokenSource.Token))
                {
                    await ChangeHandlerAsync(e);
                    _timerCancellationTokenSource.Cancel();
                }
            }
            // Raise ChangeHandler immediately
            else
            {
                // Cancel a potential existing object
                _timerForImmediate?.Dispose();
                _timerForImmediate = null;

                await ChangeHandlerAsync(e);
            }
        }

        // Cancel the previous Timer (if existing)
        // And create a new Timer with a new CancellationToken
        PeriodicTimer GetNewPeriodicTimer(int delay)
        {
            _timerCancellationTokenSource.Cancel();

            if (_timerForImmediate is not null)
            {
                _timerForImmediate.Dispose();
                _timerForImmediate = null;
            }

            _timerForImmediate = new PeriodicTimer(TimeSpan.FromMilliseconds(delay));

            _timerCancellationTokenSource.Dispose();
            _timerCancellationTokenSource = new CancellationTokenSource();

            return _timerForImmediate;
        }
    }
}
