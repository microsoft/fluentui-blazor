using System.Globalization;

namespace Microsoft.Fast.Components.FluentUI;

internal static class InputHelpers<TValue>
{

    public static string GetMaxValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);
        string value = typeCode switch
        {
            TypeCode.Decimal => decimal.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Double => double.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Int16 => short.MaxValue.ToString(),
            TypeCode.Int32 => int.MaxValue.ToString(),
            TypeCode.Int64 => "999999999999",
            TypeCode.SByte => sbyte.MaxValue.ToString(),
            TypeCode.Single => float.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt16 => ushort.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MaxValue.ToString(),
            TypeCode.UInt64 => "999999999999",
            _ => ""
        };

        return value;
    }

    public static string GetMinValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);

        string value = typeCode switch
        {

            TypeCode.Decimal => decimal.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Double => double.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Int16 => short.MinValue.ToString(),
            TypeCode.Int32 => int.MinValue.ToString(),
            TypeCode.Int64 => "-999999999999",
            TypeCode.SByte => sbyte.MinValue.ToString(),
            TypeCode.Single => float.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt16 => ushort.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MinValue.ToString(),
            TypeCode.UInt64 => "-999999999999",
            _ => ""
        };

        return value;
    }

    internal static void ValidateIntegerInputs(TValue? max, TValue? min)
    {
        int maxValue = Convert.ToInt32(max);
        int minValue = Convert.ToInt32(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Integer Max value is smaller then Min value.");
        }

    }

    internal static void ValidateLongInputs(TValue? max, TValue? min)
    {
        long maxValue = Convert.ToInt64(max);
        long minValue = Convert.ToInt64(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Long Max value is smaller then Min value.");
        }

        if (maxValue > 999999999999)
        {
            throw new ArgumentException("Long Max value can not be bigger than 999999999999.");
        }

        if (minValue < -999999999999)
        {
            throw new ArgumentException("Long Min value can not be less than -999999999999.");
        }
    }

    internal static void ValidateShortInputs(TValue? max, TValue? min)
    {
        short maxValue = Convert.ToInt16(max);
        short minValue = Convert.ToInt16(min);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Short Max value is smaller than Min value.");
        }
    }

    internal static void ValidateDoubleInputs(TValue? max, TValue? min)
    {
        double maxValue = Convert.ToDouble(max, CultureInfo.InvariantCulture);
        double minValue = Convert.ToDouble(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Double Max value is smaller than Min value.");
        }
    }

    internal static void ValidateFloatInputs(TValue? max, TValue? min)
    {
        float maxValue = Convert.ToSingle(max, CultureInfo.InvariantCulture);
        float minValue = Convert.ToSingle(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Float Max value is smaller than Min value.");
        }
    }

    internal static void ValidateDecimalInputs(TValue? max, TValue? min)
    {
        decimal maxValue = Convert.ToDecimal(max, CultureInfo.InvariantCulture);
        decimal minValue = Convert.ToDecimal(min, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Decimal Max value is smaller than Min value.");
        }
    }

    internal static void ValidateInputParameters(TValue? max, TValue? min)
    {
        if (max == null || min == null)
        {
            return; //nothing to validate
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
