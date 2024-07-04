using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

public static class EnumExtensions
{
    [Obsolete("This method will be removed in a future version. Use GetDisplayName instead.")]
    public static string? ToAttributeValue<TEnum>(this TEnum? value, bool lowercase = true) where TEnum : struct, Enum
        => value == null ? null : ToAttributeValue(value.Value, lowercase);

    [Obsolete("This method will be removed in a future version. Use GetDisplayName instead.")]
    public static string? ToAttributeValue<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, Enum
        => GetDescription(value, lowercase);

    [Obsolete("This method will be removed in a future version. Use GetDisplayName instead.")]
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

    public static string? GetDisplayName<TEnum>(this TEnum? value, bool lowercase = true) where TEnum : struct, Enum
        => value?.GetDisplayName(lowercase);

    public static string? GetDisplayName<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, Enum
    {
        var description = value.GetDisplayNameInt();
        return lowercase
                ? description?.ToLowerInvariant()
                : description;
    }

    public static string GetDisplayName(this Enum value) => GetDisplayNameInt(value);

    private static string GetDisplayNameInt(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.Name ?? value.ToString();
    }
}
