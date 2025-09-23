// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Calendar;

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
            typeof(DateOnly?),
        };

        return !supportedTypes.Contains(type);
    }

    /// <summary>
    /// Convert TValue to DateTime? for internal use
    /// </summary>
    internal static DateTime? ConvertToDateTime<TValue>(this TValue value)
    {
        if (value == null || value.IsNull())
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
            throw new ArgumentNullException(nameof(value));
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
    /// Returns the DateTime resulting from adding the given number of
    /// months to the specified DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="months"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static TValue AddMonths<TValue>(this TValue? value, int months, CultureInfo culture)
    {
        if (value == null)
        {
            return default!;
        }

        return DateTimeExtensions.AddMonths(value.ConvertToRequiredDateTime(), months, culture).ConvertToTValue<TValue>();
    }

    /// <summary>
    /// Returns the DateTime resulting from adding the given number of
    /// years to the specified DateTime.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="years"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static TValue AddYears<TValue>(this TValue? value, int years, CultureInfo culture)
    {
        if (value == null)
        {
            return default!;
        }

        return DateTimeExtensions.AddYears(value.ConvertToRequiredDateTime(), years, culture).ConvertToTValue<TValue>();
    }

    /// <summary>
    /// Determines whether the specified date represents today's date.
    /// </summary>
    public static int GetYear<TValue>(this TValue date, CultureInfo culture)
    {
        var dateValue = ConvertToDateTime(date);
        return DateTimeExtensions.GetYear(dateValue ?? DateTime.MinValue, culture);
    }

    /// <summary>
    /// Returns the minimum of Date in <paramref name="values"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static DateTime MinDateTime<TValue>(this IEnumerable<TValue> values)
    {
        return values.Select(i => i.ConvertToRequiredDateTime()).Min();
    }

    /// <summary>
    /// Returns the maximum of Date in <paramref name="values"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static DateTime MaxDateTime<TValue>(this IEnumerable<TValue> values)
    {
        return values.Select(i => i.ConvertToRequiredDateTime()).Max();
    }
}
