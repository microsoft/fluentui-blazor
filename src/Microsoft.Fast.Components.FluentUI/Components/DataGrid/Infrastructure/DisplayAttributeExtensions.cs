using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
internal static class DisplayAttributeExtensions
{

    public static string? GetDisplayAttributeString(this Type itemType, string propertyName)
    {
        PropertyInfo? propertyInfo = itemType.GetProperty(propertyName);
        //if (PropertyInfo == null && typeof(ICustomTypeProvider).IsAssignableFrom(itemType))
        //    PropertyInfo = ((ICustomTypeProvider)Item).GetCustomType().GetProperty(PropertyName);
        if (propertyInfo == null)
            return null;

        DisplayAttribute? DisplayAtt = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        if (DisplayAtt is not null)
            return DisplayAtt.GetName();
        else
        {
            var metadata = itemType.GetCustomAttribute(typeof(MetadataTypeAttribute)) as MetadataTypeAttribute;
            if (metadata != null)
                return metadata.MetadataClassType.GetDisplayAttributeString(propertyName);
        }
        return null;
    }

}
