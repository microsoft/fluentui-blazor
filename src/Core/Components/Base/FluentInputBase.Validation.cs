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
    /// Determines whether a required-field message condition is met.
    /// </summary>
    /// <param name="field">The field whose focus state may be used.</param>
    /// <param name="isEmpty">Determines whether the current value is empty.</param>
    /// <param name="useFieldFocusLost">Whether to use the field's focus state instead of this component's.</param>
    /// <param name="fieldIdentifier">
    /// The field identifier to use for validation messages, if different from this component's.
    /// </param>
    protected bool IsRequiredMessageConditionMet(IFluentField field, Func<bool> isEmpty, bool useFieldFocusLost = false, FieldIdentifier? fieldIdentifier = null)
    {
        return EditContext?.GetValidationMessages(fieldIdentifier ?? FieldIdentifier).Any() != true &&
               (useFieldFocusLost ? field.FocusLost : FocusLost) &&
               (Required ?? false) &&
               !(Disabled ?? false) &&
               !ReadOnly &&
               isEmpty();
    }

    /// <summary>
    /// Sets the default required-field message on the supplied field.
    /// </summary>
    /// <param name="field">The field receiving the message.</param>
    /// <param name="defaultMessage">An optional message to use when the model has no custom required message.</param>
    /// <param name="fieldIdentifier">
    /// The field identifier whose RequiredAttribute should supply the message, if different from this component's.
    /// </param>
    protected void SetRequiredErrorMessage(IFluentField field, string? defaultMessage = null, FieldIdentifier? fieldIdentifier = null)
    {
        field.MessageIcon = FluentStatus.ErrorIcon;
        field.Message = GetRequiredErrorMessage(defaultMessage, fieldIdentifier ?? FieldIdentifier);
    }

    /// <summary>
    /// Creates the default required-field message condition for an input component.
    /// </summary>
    /// <param name="isEmpty">Determines whether the current value is empty.</param>
    /// <param name="useFieldFocusLost">Whether to use the field's focus state instead of this component's.</param>
    /// <param name="defaultMessage">An optional message to use when the model has no custom required message.</param>
    /// <param name="fieldIdentifierProvider">
    /// Provides the field identifier when it differs from this component's value expression.
    /// </param>
    protected Func<IFluentField, bool> CreateRequiredMessageCondition(Func<bool> isEmpty, bool useFieldFocusLost = false, string? defaultMessage = null, Func<FieldIdentifier>? fieldIdentifierProvider = null)
    {
        return field =>
        {
            var fieldIdentifier = fieldIdentifierProvider?.Invoke() ?? FieldIdentifier;
            if (EditContext?.GetValidationMessages(fieldIdentifier).Any() == true)
            {
                return false;
            }

            SetRequiredErrorMessage(field, defaultMessage, fieldIdentifier);
            return IsRequiredMessageConditionMet(field, isEmpty, useFieldFocusLost, fieldIdentifier);
        };
    }

    private string GetRequiredErrorMessage(string? defaultMessage, FieldIdentifier fieldIdentifier)
    {
        var property = FindValidationProperty(fieldIdentifier);

        var requiredAttribute =
            property?.GetCustomAttribute<RequiredAttribute>();

        if (requiredAttribute is null ||
            (requiredAttribute.ErrorMessage is null &&
             requiredAttribute.ErrorMessageResourceName is null))
        {
            return defaultMessage ??
                   Localizer[Localization.LanguageResource.TextInput_RequiredMessage];
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
    protected virtual LambdaExpression? ValidationFieldExpression => ValidationFieldFor ?? ValueExpression;

    /// <summary>
    /// Gets a value indicating whether the value expression was supplied for a bound model field.
    /// </summary>
    protected bool HasExplicitValueExpression
        => GetProperty(ValueExpression) is { } property &&
           (property.DeclaringType != typeof(FluentInputBase<TValue>) ||
            !string.Equals(property.Name, nameof(CurrentValueOrDefault), StringComparison.Ordinal));
}
