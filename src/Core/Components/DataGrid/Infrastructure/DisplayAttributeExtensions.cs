using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
internal static class DisplayAttributeExtensions
{

    public static string? GetDisplayAttributeString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type itemType, string propertyName)
    {
        PropertyInfo? propertyInfo = itemType.GetProperty(propertyName);
        //if (PropertyInfo == null && typeof(ICustomTypeProvider).IsAssignableFrom(itemType))
        //    PropertyInfo = ((ICustomTypeProvider)Item).GetCustomType().GetProperty(PropertyName);
        if (propertyInfo == null)
            return null;

        DisplayAttribute? displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
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
