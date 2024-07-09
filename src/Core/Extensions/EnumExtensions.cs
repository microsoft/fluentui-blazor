using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

public static class EnumExtensions
{

    public static string? ToAttributeValue<TEnum>(this TEnum? value, bool lowercase = true) where TEnum : struct, Enum
        => value == null ? null : ToAttributeValue(value.Value, lowercase);

    public static string? ToAttributeValue<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, Enum
        => GetDescription(value, lowercase);

    public static string? GetDescription<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
        {
            return null;
        }

        var description = value.ToString();

        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString() ?? "");
        if (fieldInfo != null)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes?.Length > 0)
            {
                description = ((DescriptionAttribute)attributes[0]).Description;
            }
        }

        if (lowercase)
        {
            return description?.ToLowerInvariant();
        }

        return description;
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());
        var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.GetName() ?? enumValue.ToString();
    }

    public static bool IsNullableEnum(this Type t)
    {
        return Nullable.GetUnderlyingType(t)?.IsEnum == true;
    }
}
