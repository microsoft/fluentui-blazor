// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

/// <summary>
/// Extension methods for <see cref="FluentInputBase{TValue}"/>.
/// </summary>
internal static class FluentInputExtensions
{
    public static bool TryParseSelectableValueFromString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue>(
      this FluentInputBase<TValue> input,
      string? value,
      [MaybeNullWhen(false)] out TValue result,
      [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (typeof(TValue) == typeof(bool))
        {
            if (TryConvertToBool(value, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }

        else if (typeof(TValue) == typeof(bool?))
        {
            if (TryConvertToNullableBool(value, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }
        else if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var parsedValue))
        {
            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = default;
        validationErrorMessage = $"The {input.DisplayName ?? "Unknown Bound Field"} field is not valid.";
        return false;
    }

    /// <summary />
    private static bool TryConvertToBool<TValue>(string? value, out TValue result)
    {
        if (bool.TryParse(value, out var @bool))
        {
            result = (TValue)(object)@bool;
            return true;
        }

        result = default!;
        return false;
    }

    /// <summary />
    private static bool TryConvertToNullableBool<TValue>(string? value, out TValue result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default!;
            return true;
        }

        return TryConvertToBool(value, out result);
    }
}
