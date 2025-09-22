// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
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
    /// Convert TValue to DateTime for internal use
    /// </summary>
    internal static DateTime ConvertToRequiredDateTime<TValue>(this TValue value)
    {
        if (value == null)
        {
            return DateTimeProvider.Today;
        }

        return value switch
        {
            DateTime dt => dt,
            DateOnly d => d.ToDateTime(),
            _ => DateTimeProvider.Today
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
    /// Determines whether the specified date represents today's date.
    /// </summary>
    public static int GetYear<TValue>(this TValue date, CultureInfo culture)
         where TValue : struct, IComparable
    {
        var dateValue = ConvertToDateTime(date);
        return DateTimeExtensions.GetYear(dateValue ?? DateTime.MinValue, culture);
    }

    /// <summary>
    /// Determines whether the specified value is null or the default value for its type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNull<TValue>(this TValue? value)
    {
        return EqualityComparer<TValue>.Default.Equals(value, default);
    }

    /// <summary>
    /// Determines whether the specified value is not null or not the default value for its type.
    /// </summary>
    public static bool IsNotNull<TValue>(this TValue? value) => !IsNull(value);

    /// <summary>
    /// Determines whether two specified values are equal.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare.</typeparam>
    /// <param name="value1">The first value to compare.</param>
    /// <param name="value2">The second value to compare.</param>
    /// <returns><see langword="true"/> if the specified values are equal; otherwise, <see langword="false"/>.</returns>
    public static bool AreEqual<T>(this T value1, T value2)
    {
        return EqualityComparer<T>.Default.Equals(value1, value2);
    }

    /// <summary>
    /// Determines whether the specified date represents today's date.
    /// </summary>
    public static bool IsToday<T>(this T date)
        where T : struct, IComparable
    {
        var today = DateTimeProvider.Today;
        var dateValue = ConvertToDateTime(date);
        return dateValue == today;
    }
}
