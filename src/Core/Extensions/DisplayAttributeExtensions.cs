// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.",
                   Justification = "In the context of the Enum, the 'Display' attribute will not be trimmed.")]
    public static string? GetDisplayAttributeString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type itemType, string propertyName)
    {
        var propertyInfo = itemType.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            return null;
        }

        var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        if (displayAttribute is not null)
        {
            return displayAttribute.GetName();
        }

        if (itemType.GetCustomAttribute<MetadataTypeAttribute>() is MetadataTypeAttribute metadata)
        {
            return metadata.MetadataClassType.GetDisplayAttributeString(propertyName);
        }

        return null;
    }
}
