using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
internal static class DisplayAttributeExtensions
{

    public static string? GetDisplayAttributeString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type itemType, string propertyName)
    {
        PropertyInfo? propertyInfo = itemType.GetProperty(propertyName);
        //if (PropertyInfo == null && typeof(ICustomTypeProvider).IsAssignableFrom(itemType))
        //    PropertyInfo = ((ICustomTypeProvider)Item).GetCustomType().GetProperty(PropertyName);
        if (propertyInfo == null)
            return null;

        var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        if (displayAttribute is not null)
            return displayAttribute.GetName();
        else
        {
            if (itemType.GetCustomAttribute(typeof(MetadataTypeAttribute)) is MetadataTypeAttribute metadata)
                return metadata.MetadataClassType.GetDisplayAttributeString(propertyName);
        }
        return null;
    }

}
