using System.ComponentModel;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

public static class EnumExtensions
{

    public static string? ToAttributeValue<TEnum>(this TEnum? value) where TEnum : struct, Enum
        => value == null ? null : ToAttributeValue(value.Value);

    public static string? ToAttributeValue<TEnum>(this TEnum value) where TEnum : struct, Enum
        => GetDescription(value);


    public static string? GetDescription<TEnum>(this TEnum value) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
            return null;

        string? description = value.ToString();

        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString() ?? "");
        if (fieldInfo != null)
        {
            object[]? attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs?.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return description?.ToLowerInvariant();
    }
}