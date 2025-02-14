// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

/// <summary>
/// Provides a set of static extension methods designed to enhance and simplify the use of enumerations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Returns the Description attribute of an enum value if present.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="returnEmptyAsNull">If True, returns null if the description is empty.</param>
    /// <returns></returns>
    public static string? ToAttributeValue(this Enum? value, bool? returnEmptyAsNull = false)
    {
        if (value == null)
        {
            return null;
        }

        var description = GetDescription(value);
        if (returnEmptyAsNull == true && string.IsNullOrEmpty(description))
        {
            return null;
        }

        return description;
    }

    /// <summary>
    /// Returns the Description attribute of an enum value if present.
    /// Returns the enum name if the attribute is not found (in lower-case).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());

        if (memberInfo.Length == 0)
        {
            return string.Empty;
        }

        var attribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
        var result = attribute?.Description ?? value.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

        return result;
    }

    /// <summary>
    /// Returns the Display attribute of an enum value if present.
    /// Returns the enum name if the attribute is not found (in lower-case).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDisplay(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();

        var result = attribute?.GetName() ?? value.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

        return result;
    }

    /// <summary>
    /// Returns True if the type is a nullable enum.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsNullableEnum(this Type t)
    {
        return Nullable.GetUnderlyingType(t)?.IsEnum == true;
    }

    /// <summary>
    /// Returns True if the enum value is marked as obsolete.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsObsolete(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<ObsoleteAttribute>();

        return attribute != null;
    }
}
