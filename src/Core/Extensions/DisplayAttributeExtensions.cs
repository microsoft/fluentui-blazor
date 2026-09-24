// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

internal static class DisplayAttributeExtensions
{
    /// <summary>
    /// Returns the Display attribute of a Type value if present.
    /// </summary>
    /// <param name="itemType">The type to investigate</param>
    /// <param name="propertyName"> The name of the property to get the Display attribute for</param>
    /// <returns></returns>
    public static string? GetDisplayAttributeString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type itemType, string propertyName)
    {
        var propertyInfo = itemType.GetProperty(propertyName);

        return propertyInfo?.GetDisplayAttributeString();
    }

    /// <summary>
    /// Returns the display name of a property if present.
    /// </summary>
    /// <param name="propertyInfo">The property to investigate.</param>
    /// <returns>The configured display name, or <see langword="null"/>.</returns>
    public static string? GetDisplayAttributeString(this PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        if (displayAttribute is not null)
        {
            return displayAttribute.GetName();
        }

        if (propertyInfo.DeclaringType?.GetCustomAttribute<MetadataTypeAttribute>() is MetadataTypeAttribute metadata)
        {
            return metadata.MetadataClassType.GetDisplayAttributeString(propertyInfo.Name);
        }

        return null;
    }
}
