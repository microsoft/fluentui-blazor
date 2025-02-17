using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class InputHelpers<TValue>
{
    private const string WebComponentMaxValue = "9999999999";
    private const string WebComponentMinValue = "-9999999999";
    /// <summary>
    /// Because of the limitation of the web component, the maximum value is set to 9999999999 for really large numbers.
    /// </summary>
    /// <returns>The maximum value for the underlying type</returns>
    public static string GetMaxValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);
        var value = typeCode switch
        {
            TypeCode.Decimal => WebComponentMaxValue,
            TypeCode.Double => WebComponentMaxValue,
            TypeCode.Int16 => short.MaxValue.ToString(),
            TypeCode.Int32 => int.MaxValue.ToString(),
            TypeCode.Int64 => "999999999999",
            TypeCode.SByte => sbyte.MaxValue.ToString(),
            TypeCode.Single => WebComponentMaxValue,
            TypeCode.UInt16 => ushort.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MaxValue.ToString(),
            TypeCode.UInt64 => "999999999999",
            _ => ""
        };

        return value;
    }

    /// <summary>
    /// Because of the limitation of the web component, the minimum value is set to -9999999999 for really large negative numbers.
    /// </summary>
    /// <returns>The minimum value for the underlying type</returns>
    public static string GetMinValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);

        var value = typeCode switch
        {

            TypeCode.Decimal => WebComponentMinValue,
            TypeCode.Double => WebComponentMinValue,
            TypeCode.Int16 => short.MinValue.ToString(),
            TypeCode.Int32 => int.MinValue.ToString(),
            TypeCode.Int64 => WebComponentMinValue,
            TypeCode.SByte => sbyte.MinValue.ToString(),
            TypeCode.Single => WebComponentMinValue,
            TypeCode.UInt16 => ushort.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MinValue.ToString(),
            TypeCode.UInt64 => ulong.MinValue.ToString(),
            _ => ""

        };
        return value;
    }

    internal static void ValidateSByteInputs(string? max, string? min)
    {
        var maxValue = Convert.ToSByte(max);
        var minValue = Convert.ToSByte(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Signed Integer Max value is smaller than Min value.");
        }
    }

    internal static void ValidateIntegerInputs(string? max, string? min)
    {
        var maxValue = Convert.ToInt32(max);
        var minValue = Convert.ToInt32(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Integer Max value is smaller then Min value.");
        }
    }

    internal static void ValidateLongInputs(string? max, string? min)
    {
        var maxValue = Convert.ToInt64(max);
        var minValue = Convert.ToInt64(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Long Max value is smaller then Min value.");
        }
    }

    internal static void ValidateShortInputs(string? max, string? min)
    {
        var maxValue = Convert.ToInt16(max);
        var minValue = Convert.ToInt16(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Short Max value is smaller than Min value.");
        }
    }

    internal static void ValidateDoubleInputs(string? max, string? min)
    {
        var maxValue = Convert.ToDouble(max, CultureInfo.InvariantCulture);
        var minValue = Convert.ToDouble(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Double Max value is smaller than Min value.");
        }
    }

    internal static void ValidateFloatInputs(string? max, string? min)
    {
        var maxValue = Convert.ToSingle(max, CultureInfo.InvariantCulture);
        var minValue = Convert.ToSingle(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Float Max value is smaller than Min value.");
        }
    }

    internal static void ValidateDecimalInputs(string? max, string? min)
    {
        var maxValue = Convert.ToDecimal(max, CultureInfo.InvariantCulture);
        var minValue = Convert.ToDecimal(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Decimal Max value is smaller than Min value.");
        }
    }

    internal static void ValidateUShortInputs(string max, string min)
    {
        var maxValue = Convert.ToUInt16(max);
        var minValue = Convert.ToUInt16(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Unsigned Short Max value is smaller than Min value.");
        }
    }

    internal static void ValidateUIntegerInputs(string max, string min)
    {
        var maxValue = Convert.ToUInt32(max);
        var minValue = Convert.ToUInt32(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Unsigned integer Max value is smaller than Min value.");
        }
    }

    internal static void ValidateULongInputs(string max, string min)
    {
        var maxValue = Convert.ToUInt64(max);
        var minValue = Convert.ToUInt64(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Unsigned Long Max value is smaller than Min value.");
        }
    }

    internal static void ValidateInputParameters(string? max, string? min)
    {
        if (max == null || min == null)
        {
            // No need to validate if either max or min is null
            return;
        }

        if (typeof(TValue) == typeof(ushort))
        {
            ValidateUShortInputs(max, min);
        }

        if (typeof(TValue) == typeof(uint))
        {
            ValidateUIntegerInputs(max, min);
        }

        if (typeof(TValue) == typeof(ulong))
        {
            ValidateULongInputs(max, min);
        }

        if(typeof(TValue) == typeof(byte))
        {
            ValidateUShortInputs(max, min);
        }

        if (typeof(TValue) == typeof(sbyte))
        {
            ValidateSByteInputs(max, min);
        }

        if (typeof(TValue) == typeof(int))
        {
            ValidateIntegerInputs(max, min);
        }

        if (typeof(TValue) == typeof(long))
        {
            ValidateLongInputs(max, min);
        }

        if (typeof(TValue) == typeof(short))
        {
            ValidateShortInputs(max, min);
        }

        if (typeof(TValue) == typeof(double))
        {
            ValidateDoubleInputs(max, min);
        }

        if (typeof(TValue) == typeof(float))
        {
            ValidateFloatInputs(max, min);
        }

        if (typeof(TValue) == typeof(decimal))
        {
            ValidateDecimalInputs(max, min);
        }
    }
}
