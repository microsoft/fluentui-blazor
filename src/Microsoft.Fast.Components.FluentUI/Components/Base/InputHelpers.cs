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
        }; ; ;

        return value;
    }

    internal static void ValidateIntegerInputs(string? max, string? min)
    {
        int maxValue = int.Parse(max!, CultureInfo.InvariantCulture);
        int minValue = int.Parse(min!, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Integer Max value is smaller then Min value.");
        }
    }

    internal static void ValidateDecimalInput(string? max, string? min)
    {
        if (!decimal.TryParse(max!, out decimal maxValue))
            maxValue = decimal.MaxValue;
        if (!decimal.TryParse(min!, out decimal minValue))
            minValue = decimal.MinValue;

        if (maxValue < minValue)
        {
            throw new ArgumentException("Decimal Max value is smaller than Min value.");
        }
    }

    internal static void ValidateDoubleInput(string? max, string? min)
    {
        double maxValue = double.Parse(max!, CultureInfo.InvariantCulture);
        double minValue = double.Parse(min!, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Double Max value is smaller than Min value.");
        }
    }

    internal static void ValidateFloatInput(string? max, string? min)
    {
        float maxValue = float.Parse(max!, CultureInfo.InvariantCulture);
        float minValue = float.Parse(min!, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Float Max value is smaller than Min value.");
        }
    }

    internal static void ValidateShortInput(string? max, string? min)
    {
        short maxValue = short.Parse(max!, CultureInfo.InvariantCulture);
        short minValue = short.Parse(min!, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Short Max value is smaller than Min value.");
        }
    }

    internal static void ValidateLongInput(string? max, string? min)
    {
        long maxValue = long.Parse(max!, CultureInfo.InvariantCulture);
        long minValue = long.Parse(min!, CultureInfo.InvariantCulture);

        if (maxValue < minValue)
        {
            throw new ArgumentException("Long Max value is smaller than Min value.");
        }
    }

    internal static void ValidateInputParameters(string? max, string? min)
    {
        if (typeof(TValue) == typeof(int))
        {
            ValidateIntegerInputs(max, min);
        }

        if (typeof(TValue) == typeof(long))
        {
            ValidateLongInput(max, min);
        }

        if (typeof(TValue) == typeof(short))
        {
            ValidateShortInput(max, min);
        }

        if (typeof(TValue) == typeof(float))
        {
            ValidateFloatInput(max, min);
        }

        if (typeof(TValue) == typeof(double))
        {
            ValidateDoubleInput(max, min);
        }

        if (typeof(TValue) == typeof(decimal))
        {
            ValidateDecimalInput(max, min);
        }
    }
}
