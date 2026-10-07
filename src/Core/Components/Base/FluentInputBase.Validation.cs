// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public abstract partial class FluentInputBase<TValue>
{
    /// <summary>
    /// Provides validation logic for the FluentInputBase component.
    /// </summary>
    protected class FluentInputBaseValidation<T>
    {
        private readonly FluentInputBase<T> _input;

        /// <summary />
        public FluentInputBaseValidation(FluentInputBase<T> input)
        {
            _input = input;
        }

        /// <summary>
        /// Creates the default required-field message condition for an input component.
        /// </summary>
        /// <param name="isEmpty">Determines whether the current value is empty.</param>
        /// <param name="useFieldFocusLost">Whether to use the field's focus state instead of this component's.</param>
        /// <param name="fieldIdentifierProvider">
        /// Provides the field identifier when it differs from this component's value expression.
        /// </param>
        public Func<IFluentField, bool> CreateRequiredMessageCondition(Func<bool> isEmpty, bool useFieldFocusLost = false, Func<FieldIdentifier>? fieldIdentifierProvider = null)
        {
            return field =>
            {
                var fieldIdentifier = fieldIdentifierProvider?.Invoke() ?? _input.FieldIdentifier;
                if (_input.EditContext?.GetValidationMessages(fieldIdentifier).Any() == true)
                {
                    return false;
                }

                SetRequiredErrorMessage(field, fieldIdentifier);
                return IsRequiredMessageConditionMet(field, isEmpty, useFieldFocusLost, fieldIdentifier);
            };
        }

     /// <summary>
        /// Determines whether a required-field message condition is met.
        /// </summary>
        /// <param name="field">The field whose focus state may be used.</param>
        /// <param name="isEmpty">Determines whether the current value is empty.</param>
        /// <param name="useFieldFocusLost">Whether to use the field's focus state instead of this component's.</param>
        /// <param name="fieldIdentifier">
        /// The field identifier to use for validation messages, if different from this component's.
        /// </param>
        private bool IsRequiredMessageConditionMet(IFluentField field, Func<bool> isEmpty, bool useFieldFocusLost = false, FieldIdentifier? fieldIdentifier = null)
        {
            return _input.EditContext?.GetValidationMessages(fieldIdentifier ?? _input.FieldIdentifier).Any() != true &&
                   (useFieldFocusLost ? field.FocusLost : _input.FocusLost) &&
                   (_input.Required ?? false) &&
                   !(_input.Disabled ?? false) &&
                   !_input.ReadOnly &&
                   isEmpty();
        }

        /// <summary>
        /// Sets the default required-field message on the supplied field.
        /// </summary>
        /// <param name="field">The field receiving the message.</param>
        /// <param name="fieldIdentifier">
        /// The field identifier whose RequiredAttribute should supply the message, if different from this component's.
        /// </param>
        private void SetRequiredErrorMessage(IFluentField field, FieldIdentifier? fieldIdentifier = null)
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = GetRequiredErrorMessage(fieldIdentifier ?? _input.FieldIdentifier);
        }

        private string GetRequiredErrorMessage(FieldIdentifier fieldIdentifier)
        {
            var property = FindValidationProperty(fieldIdentifier);

            var requiredAttribute =
                property?.GetCustomAttribute<RequiredAttribute>();

            if (requiredAttribute is null ||
                (requiredAttribute.ErrorMessage is null &&
                 requiredAttribute.ErrorMessageResourceName is null))
            {
                return _input.Localizer[Localization.LanguageResource.FluentInputBase_RequiredMessage];
            }

            var displayName =
                property?.GetCustomAttribute<DisplayAttribute>()?.GetName() ??
                fieldIdentifier.FieldName;

            return requiredAttribute.FormatErrorMessage(displayName);
        }

        private PropertyInfo? FindValidationProperty(FieldIdentifier fieldIdentifier)
        {
            var property = GetProperty(ValidationFieldExpression);

            return property is not null &&
                   string.Equals(property.Name, fieldIdentifier.FieldName, StringComparison.Ordinal)
                ? property
                : null;
        }

        private static PropertyInfo? GetProperty(LambdaExpression? expression)
        {
            if (expression is null)
            {
                return null;
            }

            var body = expression.Body;

            while (body is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked, } conversion)
            {
                body = conversion.Operand;
            }

            return body is MemberExpression { Member: PropertyInfo property, } ? property : null;
        }

        /// <summary>
        /// Gets the expression identifying the model field used for validation.
        /// </summary>
        private LambdaExpression? ValidationFieldExpression => _input.ValidationFieldFor ?? _input.ValueExpression;
    }
}