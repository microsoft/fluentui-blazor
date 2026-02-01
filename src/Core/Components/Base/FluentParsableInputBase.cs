// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public abstract class FluentParsableInputBase<TValue> : FluentInputBase<TValue>
{
    private bool _hasInitializedParameters;
    private bool _parsingFailed;
    private string? _incomingValueBeforeParsing;
    private bool _previousParsingAttemptFailed;
    private ValidationMessageStore? _parsingValidationMessages;
    private Type? _nullableUnderlyingType;

    /// <summary>
    /// Gets or sets the error message to show when the field can not be parsed.
    /// </summary>
    [Parameter]
    public virtual string ParsingErrorMessage { get; set; } = "The {0} field must have a valid format.";

    /// <summary>
    /// Gets or sets the current value of the input, represented as a string.
    /// </summary>
    protected string? CurrentValueAsString
    {
        // InputBase-derived components can hold invalid states (e.g., an InputNumber being blank even when bound
        // to an int value). So, if parsing fails, we keep the rejected string in the UI even though it doesn't
        // match what's on the .NET model. This avoids interfering with typing, but still notifies the EditContext
        // about the validation error message.
        get => _parsingFailed ? _incomingValueBeforeParsing : FormatValueAsString(CurrentValue);
        set => _ = SetCurrentValueAsStringAsync(value);

    }

    /// <summary>
    /// Attempts to set the current value of the input, represented as a string.
    /// </summary>
    /// <param name="value"></param>
    protected async Task SetCurrentValueAsStringAsync(string? value)
    {
        _incomingValueBeforeParsing = value;
        _parsingValidationMessages?.Clear();

        if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
        {
            // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
            // Then all subclasses get nullable support almost automatically (they just have to
            // not reject Nullable<T> based on the type itself).
            _parsingFailed = false;
            CurrentValue = default!;
        }
        else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
        {
            _parsingFailed = false;
            await SetCurrentValueAsync(parsedValue);
        }
        else
        {
            _parsingFailed = true;

            // EditContext may be null if the input is not a child component of EditForm.
            if (EditContext is not null && FieldBound)
            {
                _parsingValidationMessages ??= new ValidationMessageStore(EditContext);
                _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                EditContext.NotifyFieldChanged(FieldIdentifier);
            }
        }

        // We can skip the validation notification if we were previously valid and still are
        if (_parsingFailed || _previousParsingAttemptFailed)
        {
            EditContext?.NotifyValidationStateChanged();
            _previousParsingAttemptFailed = _parsingFailed;
        }
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        if(!_hasInitializedParameters)
        {
            var type = typeof(TValue);

            _nullableUnderlyingType = Nullable.GetUnderlyingType(type);

            _hasInitializedParameters = true;
        }
        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Parses a string to create an instance of <typeparamref name="TValue"/>. Derived classes can override this to change how
    /// <see cref="CurrentValueAsString"/> interprets incoming values.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">An instance of <typeparamref name="TValue"/>.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected abstract bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage);

    protected override Task SetCurrentValueAsync(TValue? value)
    {
        var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);

        if (!hasChanged)
        {
            return Task.CompletedTask;
        }

        _parsingFailed = false;

        return base.SetCurrentValueAsync(value);
    }

    /// <summary>
    /// Handler for the OnChange event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override async Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        var _notifyCalled = false;
        var isValid = TryParseValueFromString(e.Value?.ToString(), out TValue? result, out var validationErrorMessage);

        if (isValid)
        {
            await SetCurrentValueAsync(result ?? default);
            _notifyCalled = true;

            if (FieldBound && CascadedEditContext != null)
            {
                _parsingValidationMessages?.Clear(); // Clear any previous errors
            }
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
}
