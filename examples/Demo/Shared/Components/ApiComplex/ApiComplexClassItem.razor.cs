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

    [Parameter]
    public EventCallback<PropertyChildren> OnChanged { get; set; }

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
            var nullable = Property.Item.PropertyType.IsNullable();
            var type = nullable
                     ? Nullable.GetUnderlyingType(Property.Item.PropertyType) 
                     : Property.Item.PropertyType;

            if (type == typeof(double))
            {
                if (double.TryParse(value, out double newValue))
                {
                    PropertyInfoExtensions.SetPropertyValue(Item, Property.FullName, newValue);
                }
                else
                {
                    PropertyInfoExtensions.SetPropertyValue(Item, Property.FullName, nullable ? null : 0.0);
                }
            }
            else
            {
                PropertyInfoExtensions.SetPropertyValue(Item, Property.FullName, value);
            }

            if (OnChanged.HasDelegate)
            {
                OnChanged.InvokeAsync(Property);
            }
        }
    }
}
