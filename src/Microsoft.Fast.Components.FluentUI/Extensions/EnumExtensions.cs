using System.ComponentModel;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

public static class EnumExtensions
{

    public static string? ToAttributeValue<TEnum>(this TEnum? value, bool lowercase = true) where TEnum : struct, Enum
        => value == null ? null : ToAttributeValue(value.Value, lowercase);

    public static string? ToAttributeValue<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, Enum
        => GetDescription(value, lowercase);


    public static string? GetDescription<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
            return null;

        string? description = value.ToString();

        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString() ?? "");
        if (fieldInfo != null)
        {
            object[]? attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes?.Length > 0)
            {
                description = ((DescriptionAttribute)attributes[0]).Description;
            }
        }

        if (lowercase)
            return description?.ToLowerInvariant();

        return description;
    }
}