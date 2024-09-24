// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components. This base class automatically
/// integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>, which must be supplied
/// as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract partial class FluentInputBase<TValue> : FluentComponentBase, IDisposable
{
    private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;

    private bool _disposed;
    private bool _hasInitializedParameters;
    private bool _parsingFailed;
    private string? _incomingValueBeforeParsing;
    private bool _previousParsingAttemptFailed;
    private ValidationMessageStore? _parsingValidationMessages;
    private Type? _nullableUnderlyingType;

    /// <summary>
    /// Constructs an instance of <see cref="InputBase{TValue}"/>.
    /// </summary>
    protected FluentInputBase()
    {
        _validationStateChangedHandler = OnValidateStateChanged;
    }

    [CascadingParameter]
    private EditContext? CascadedEditContext { get; set; }

    /// <summary>
    /// Gets a CSS class string that combines the `Class` attribute and and a string indicating
    /// the status of the field being edited (a combination of "modified", "valid", and "invalid").
    /// Derived components should typically use this value for the primary HTML element class attribute.
    /// </summary>
    protected virtual string? ClassValue
    {
        get
        {
            // TODO: To check if we need to add the `cssClass` for the input element
            //       var cssClass = CombineClassNames(AdditionalAttributes, fieldClass);
            // See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Shared/src/AttributeUtilities.cs

            var fieldClass = (FieldBound && !Embedded) ? EditContext?.FieldCssClass(FieldIdentifier) : null;

            if (!string.IsNullOrEmpty(fieldClass) || !string.IsNullOrEmpty(Class))
            {
                return new CssBuilder(Class)
                    .AddClass(fieldClass)
                    .Build();
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the optional in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    protected virtual string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public virtual bool Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the text used on `aria-label` attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the form control is disabled and doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public virtual bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the display name for this field.
    /// <para>This value is used when generating error messages when the input value fails to parse correctly.</para>
    /// </summary>
    [Parameter]
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets if the derived component is embedded in another component.
    /// If true, the <see cref="ClassValue" /> property will not include the <see cref="EditContext" />'s FieldCssClass.
    /// </summary>
    [Parameter]
    public virtual bool Embedded { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="FieldIdentifier"/> that identifies the bound value.
    /// If set, this parameter takes precedence over <see cref="ValueExpression"/>.
    /// </summary>
    [Parameter]
    public virtual FieldIdentifier? Field { get; set; }

    /// <summary>
    /// Gets or sets the text to label the input. This is usually displayed just above the input.
    /// </summary>
    [Parameter]
    public virtual string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content to label the input component.
    /// This is usually displayed just above the input
    /// </summary>
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the name of the element.
    /// Allows access by name from the associated form.
    /// ⚠️ This value needs to be set manually for SSR scenarios to work correctly.
    /// </summary>
    [Parameter]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets whether the control will be immutable by user interaction.
    /// </summary>
    [Parameter]
    public virtual bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element needs to have a value.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the value of the input. This should be used with two-way binding.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    [Parameter]
    public virtual TValue? Value { get; set; }

    /// <summary>
    /// Gets or sets the value of the input, represented as a string.
    /// </summary>
    protected string? ValueAsString
    {
        // InputBase-derived components can hold invalid states (e.g., an InputNumber being blank even when bound
        // to an int value). So, if parsing fails, we keep the rejected string in the UI even though it doesn't
        // match what's on the .NET model. This avoids interfering with typing, but still notifies the EditContext
        // about the validation error message.
        get => _parsingFailed ? _incomingValueBeforeParsing : FormatValueAsString(Value);
        set => _ = SetValueAsStringAsync(value);
    }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public virtual EventCallback<TValue> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets an expression that identifies the bound value.
    /// </summary>
    [Parameter]
    public virtual Expression<Func<TValue>>? ValueExpression { get; set; }

    /// <summary>
    /// Gets the associated <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>.
    /// This property is uninitialized if the input does not have a parent <see cref="EditForm"/>.
    /// </summary>
    protected virtual EditContext EditContext { get; set; } = default!;

    /// <summary>
    /// Gets the <see cref="FieldIdentifier"/> for the bound value.
    /// </summary>
    protected internal virtual FieldIdentifier FieldIdentifier { get; set; }

    /// <summary>
    /// Gets true when the <see cref="Field"/> or <see cref="ValueExpression"/> is set.
    /// </summary>
    internal virtual bool FieldBound => Field is not null || ValueExpression is not null || ValueChanged.HasDelegate;

    /// <summary>
    /// Sets the value of the input, formatted as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual async Task SetValueAsync(TValue? value)
    {
        var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
        if (!hasChanged)
        {
            return;
        }

        _parsingFailed = false;

        // If we don't do this, then when the user edits from A to B, we'd:
        //   - Do a render that changes back to A
        //   - Then send the updated value to the parent, which sends the B back to this component
        //   - Do another render that changes it to B again
        // The unnecessary reversion from B to A can cause selection to be lost while typing
        // A better solution would be somehow forcing the parent component's render to occur first,
        // but that would involve a complex change in the renderer to keep the render queue sorted
        // by component depth or similar.

        Value = value;

        if (ValueChanged.HasDelegate)
        {
            // Thread Safety: Force `ValueChanged` to be re-associated with the Dispatcher, prior to invocation.
            await InvokeAsync(async () => await ValueChanged.InvokeAsync(value));
        }

        if (FieldBound)
        {
            // Thread Safety: Force `EditContext` to be re-associated with the Dispatcher
            await InvokeAsync(() => EditContext?.NotifyFieldChanged(FieldIdentifier));
        }
    }

    /// <summary>
    /// Attempts to set the value of the input, represented as a string.
    /// </summary>
    /// <param name="value"></param>
    protected virtual async Task SetValueAsStringAsync(string? value)
    {
        _incomingValueBeforeParsing = value;
        _parsingValidationMessages?.Clear();

        if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
        {
            // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
            // Then all subclasses get nullable support almost automatically (they just have to
            // not reject Nullable<T> based on the type itself).
            _parsingFailed = false;
            await SetValueAsync(default);
        }

        else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
        {
            _parsingFailed = false;
            await SetValueAsync(parsedValue);
        }

        else
        {
            _parsingFailed = true;

            // EditContext may be null if the input is not a child component of EditForm.
            if (EditContext is not null && FieldBound)
            {
                _parsingValidationMessages ??= new ValidationMessageStore(EditContext);
                _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                // Since we're not writing to Value, we'll need to notify about modification from here
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

    /// <summary>
    /// Formats the value as a string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected virtual string? FormatValueAsString(TValue? value)
    {
        return value?.ToString();
    }

    /// <summary>
    /// Parses a string to create an instance of <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">An instance of <typeparamref name="TValue"/>.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected abstract bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage);

    /// <summary>
    /// Handler for the `OnChange` event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual async Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        var _notifyCalled = false;
        var isValid = TryParseValueFromString(e.Value?.ToString(), out var result, out var validationErrorMessage);

        if (isValid)
        {
            await SetValueAsync(result ?? default);
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

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!_hasInitializedParameters)
        {
            // This is the first run
            // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

            if (Field is not null)
            {
                FieldIdentifier = (FieldIdentifier)Field;
            }

            else if (ValueExpression is not null)
            {
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);
            }

            else if (ValueChanged.HasDelegate)
            {
                FieldIdentifier = FieldIdentifier.Create(() => Value);
            }

            if (CascadedEditContext != null)
            {
                EditContext = CascadedEditContext;
                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }

            _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));
            _hasInitializedParameters = true;
        }

        else if (CascadedEditContext != EditContext)
        {
            // Not the first run

            // We don't support changing EditContext because it's messy to be clearing up state and event
            // handlers for the previous one, and there's no strong use case. If a strong use case
            // emerges, we can consider changing this.
            throw new InvalidOperationException($"{GetType()} does not support changing the " +
                $"{nameof(Microsoft.AspNetCore.Components.Forms.EditContext)} dynamically.");
        }

        UpdateAdditionalValidationAttributes();

        // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// Update additional validations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
    {
        UpdateAdditionalValidationAttributes();
        _ = InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Updates the additional validation attributes.
    /// </summary>
    private void UpdateAdditionalValidationAttributes()
    {
        if (EditContext is null)
        {
            return;
        }

        var hasAriaInvalidAttribute = AdditionalAttributes != null && AdditionalAttributes.ContainsKey("aria-invalid");

        if (FieldBound && EditContext.GetValidationMessages(FieldIdentifier).Any())
        {
            if (hasAriaInvalidAttribute)
            {
                // Do not overwrite the attribute value
                return;
            }

            if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
            {
                AdditionalAttributes = additionalAttributes;
            }

            // To make the `Input` components accessible by default
            // we will automatically render the `aria-invalid` attribute when the validation fails
            // value must be "true" see https://www.w3.org/TR/wai-aria-1.1/#aria-invalid
            additionalAttributes["aria-invalid"] = "true";
        }

        else if (hasAriaInvalidAttribute)
        {
            // No validation errors. Need to remove `aria-invalid` if it was rendered already
            if (AdditionalAttributes!.Count == 1)
            {
                // Only aria-invalid argument is present which we don't need any more
                AdditionalAttributes = null;
            }

            else
            {
                if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
                {
                    AdditionalAttributes = additionalAttributes;
                }

                additionalAttributes.Remove("aria-invalid");
            }
        }
    }

    /// <summary>
    /// Returns a dictionary with the same values as the specified <paramref name="source"/>.
    /// </summary>
    /// <returns>true, if a new dictionary with copied values was created. false - otherwise.</returns>
    private static bool ConvertToDictionary(IReadOnlyDictionary<string, object>? source, out Dictionary<string, object> result)
    {
        var newDictionaryCreated = true;

        if (source == null)
        {
            result = [];
        }

        else if (source is Dictionary<string, object> currentDictionary)
        {
            result = currentDictionary;
            newDictionaryCreated = false;
        }

        else
        {
            result = [];
            foreach (var item in source)
            {
                result.Add(item.Key, item.Value);
            }
        }

        return newDictionaryCreated;
    }

    /// <summary>
    /// Disposes the component.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed state (managed objects).
            // ...

            if (EditContext is not null)
            {
                EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
        }

        // Free unmanaged resources (unmanaged objects) and override a finalizer below.
        // ...

        _disposed = true;
    }

    /// <summary>
    /// Disposes the component.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }
}
