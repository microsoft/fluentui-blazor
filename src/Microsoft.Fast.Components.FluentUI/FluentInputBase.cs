using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.FluentUI
{
    public abstract class FluentInputBase<TValue> : ComponentBase, IDisposable
    {
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private bool _previousParsingAttemptFailed;
        private ValidationMessageStore _parsingValidationMessages;
        private Type _nullableUnderlyingType;

        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public TValue Value { get; set; }

        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter] public Expression<Func<TValue>> ValueExpression { get; set; }

        [Parameter] public string DisplayName { get; set; }

        protected EditContext EditContext { get; set; }

        protected internal FieldIdentifier FieldIdentifier { get; set; }

        protected TValue CurrentValue
        {
            get => Value;
            set
            {
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    Value = value;
                    _ = ValueChanged.InvokeAsync(Value);
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
        }

        protected string CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                if (EditContext == null)
                {
                    if (TryParseValueFromString(value, out var parsedValue, out var _))
                    {
                        CurrentValue = parsedValue;
                    }

                    // If we are not in the context of an edit form, don't do anything with the current value.
                    return;
                }

                _parsingValidationMessages?.Clear();

                bool parsingFailed;

                if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
                {
                    // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                    // Then all subclasses get nullable support almost automatically (they just have to
                    // not reject Nullable<T> based on the type itself).
                    parsingFailed = false;
                    CurrentValue = default!;
                }
                else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    parsingFailed = false;
                    CurrentValue = parsedValue!;
                }
                else
                {
                    parsingFailed = true;

                    if (_parsingValidationMessages == null)
                    {
                        _parsingValidationMessages = new ValidationMessageStore(EditContext);
                    }

                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    EditContext.NotifyFieldChanged(FieldIdentifier);
                }

                // We can skip the validation notification if we were previously valid and still are
                if (parsingFailed || _previousParsingAttemptFailed)
                {
                    EditContext.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        protected FluentInputBase()
        {
            _validationStateChangedHandler = OnValidateStateChanged;
        }

        protected virtual string FormatValueAsString(TValue value)
            => value?.ToString();

        protected abstract bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage);

        private string FieldClass
            => EditContext?.FieldCssClass(FieldIdentifier);

        protected string CssClass
        {
            get
            {
                if (AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("class", out var @class) &&
                    !string.IsNullOrEmpty(Convert.ToString(@class, CultureInfo.InvariantCulture)))
                {
                    return $"{@class} {FieldClass}";
                }

                return FieldClass; // Never null or empty
            }
        }


        /// <inheritdoc />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if(EditContext != null || CascadedEditContext != null)
            {
                // This is the first run
                // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()
                if (CascadedEditContext == null)
                {
                    throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                        $"of type {nameof(AspNetCore.Components.Forms.EditContext)}. For example, you can use {GetType().FullName} inside " +
                        $"an {nameof(EditForm)}.");
                }

                if (ValueExpression == null)
                {
                    throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
                        $"parameter. Normally this is provided automatically when using 'bind-Value'.");
                }

                EditContext = CascadedEditContext;
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);
                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
                if (CascadedEditContext != EditContext)
                {
                    // Not the first run

                    // We don't support changing EditContext because it's messy to be clearing up state and event
                    // handlers for the previous one, and there's no strong use case. If a strong use case
                    // emerges, we can consider changing this.
                    throw new InvalidOperationException($"{GetType()} does not support changing the " +
                        $"{nameof(AspNetCore.Components.Forms.EditContext)} dynamically.");
                }

                UpdateAdditionalValidationAttributes();
            }

            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        private void OnValidateStateChanged(object sender, ValidationStateChangedEventArgs eventArgs)
        {
            UpdateAdditionalValidationAttributes();

            StateHasChanged();
        }

        private void UpdateAdditionalValidationAttributes()
        {
            if (EditContext == null)
            {
                return;
            }

            var hasAriaInvalidAttribute = AdditionalAttributes != null && AdditionalAttributes.ContainsKey("aria-invalid");
            if (EditContext.GetValidationMessages(FieldIdentifier).Any())
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
                additionalAttributes["aria-invalid"] = true;
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

        private bool ConvertToDictionary(IReadOnlyDictionary<string, object> source, out Dictionary<string, object> result)
        {
            var newDictionaryCreated = true;
            if (source == null)
            {
                result = new Dictionary<string, object>();
            }
            else if (source is Dictionary<string, object> currentDictionary)
            {
                result = currentDictionary;
                newDictionaryCreated = false;
            }
            else
            {
                result = new Dictionary<string, object>();
                foreach (var item in source)
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return newDictionaryCreated;
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            if (EditContext != null)
            {
                EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
            Dispose(disposing: true);
        }
    }
}
