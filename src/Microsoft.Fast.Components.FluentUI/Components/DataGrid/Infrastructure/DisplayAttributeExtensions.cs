using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;


internal static class DisplayAttributeExtensions
{

    public static string GetDisplayAttributeString(this Type itemType, string PropertyName)
    {
        PropertyInfo PropertyInfo = itemType.GetProperty(PropertyName);
        if (PropertyInfo == null)
            return string.Empty;

        var DisplayAtt = PropertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
        if (DisplayAtt != null)
            return ((DisplayAttribute)DisplayAtt!).GetName();
        else
        {
            var metadata = itemType.GetCustomAttribute(typeof(MetadataTypeAttribute)) as MetadataTypeAttribute;
            if (metadata != null)
                return metadata.MetadataClassType.GetDisplayAttributeString(PropertyName);
        }

        return string.Empty;
    }

}
