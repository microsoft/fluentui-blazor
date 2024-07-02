using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

public static class EnumExtensions
{
    public static string? ToAttributeValue<TEnum>(this TEnum? value, bool lowercase = true) where TEnum : struct, Enum
        => value == null ? null : ToAttributeValue(value.Value, lowercase);

    public static string? ToAttributeValue<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, Enum
        => GetDisplayOrDescription(value, lowercase);

    public static string? GetDisplayOrDescription<TEnum>(this TEnum value, bool lowercase = true) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
        {
            return null;
        }

        var description = GetDisplayOrDescription(value as Enum);
        if (lowercase)
        {
            return description?.ToLowerInvariant();
        }

        return description;
    }

    public static string GetDisplayOrDescription(this Enum value) 
    {
        var description = value.ToString();
        var fieldInfo = value.GetType().GetField(value.ToString() ?? "");
        if (fieldInfo != null)
        {
            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>(true);
            if (displayAttribute?.Name == null)
            {
                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>(true);
                if (descriptionAttribute?.Description != null)
                {
                    description = descriptionAttribute.Description;
                }
            }
            else
            {
                description = displayAttribute.Name;
            }
        }

        return description;
    }
}
