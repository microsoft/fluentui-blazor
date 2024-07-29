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
    /// <param name="lowercase"></param>
    /// <returns></returns>
    public static string? ToAttributeValue(this Enum? value, bool lowercase = true)
        => value == null ? null : GetDescription(value, lowercase);

    /// <summary>
    /// Returns the Description attribute of an enum value if present.
    /// Returns the enum name if the attribute is not found. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="lowercase"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value, bool lowercase = true)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();

        var result = attribute?.Description ?? value.ToString();

        if (lowercase)
        {
            return result.ToLowerInvariant();
        }

        return result;
    }

    /// <summary>
    /// Returns the Display attribute of an enum value if present.
    /// Returns the enum name if the attribute is not found. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="lowercase"></param>
    /// <returns></returns>
    public static string GetDisplay(this Enum value, bool lowercase = true)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();

        var result = attribute?.GetName() ?? value.ToString();

        if (lowercase)
        {
            return result.ToLowerInvariant();
        }

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
}
