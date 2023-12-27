using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClassItem
{
    [Parameter]
    [EditorRequired]
    public PropertyChildren Property { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public object Item { get; set; } = default!;

    public string ItemAsString
    {
        get
        {
            object? value = PropertyInfoExtensions.GetPropertyValue(Item, Property.FullName);

            if (value == null)
            {
                return string.Empty;
            }

            // Double?
            if (value is double)
            {
                return Convert.ToDouble(value).ToString("0.00");
            }

            return Convert.ToString(value) ?? string.Empty;
        }
        set
        { 
        }
    }
}
