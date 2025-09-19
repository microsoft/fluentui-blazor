// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class CalendarTValue
{
    /// <summary>
    /// Determines whether the specified type is not a supported date type.
    /// </summary>
    /// <param name="type">The type to evaluate for date type support. Supported types are DateTime, DateTime?, DateOnly, and DateOnly?.</param>
    /// <returns>true if the specified type is not DateTime, DateTime?, DateOnly, or DateOnly?; otherwise, false.</returns>
    internal static bool IsNotDateType(this Type type)
    {
        var supportedTypes = new Type[]
        {
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateOnly),
            typeof(DateOnly?)
        };
        return !supportedTypes.Contains(type);
    }

    /// <summary>
    /// Convert TValue to DateTime? for internal use
    /// </summary>
    internal static DateTime? ConvertToDateTime<TValue>(this TValue value)
    {
        if (value == null)
        {
            return null;
        }

        return value switch
        {
            DateTime dt => dt,
            DateOnly d => d.ToDateTime(),
            _ => null
        };
    }

    /// <summary>
    /// Convert DateTime? to TValue for external use
    /// </summary>
    internal static TValue ConvertToTValue<TValue>(this DateTime value)
    {
        if (typeof(TValue) == typeof(DateTime))
        {
            return (TValue)(object)value;
        }

        if (typeof(TValue) == typeof(DateTime?))
        {
            return (TValue)(object)(DateTime?)value;
        }

        if (typeof(TValue) == typeof(DateOnly))
        {
            return (TValue)(object)DateOnly.FromDateTime(value);
        }

        if (typeof(TValue) == typeof(DateOnly?))
        {
            return (TValue)(object)(DateOnly?)DateOnly.FromDateTime(value);
        }

        throw new ArgumentException($"Unsupported type: {typeof(TValue)}", nameof(value));
    }

    /// <summary>
    /// Convert DateTime? to TValue for external use
    /// </summary>
    internal static TValue? ConvertFromDateTime<TValue>(this DateTime? value)
    {
        return value.HasValue ? ConvertToTValue<TValue>(value.Value) : default;
    }
}
