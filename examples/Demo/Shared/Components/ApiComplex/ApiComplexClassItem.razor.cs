using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

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

    private string Placeholder
    {
        get
        {
            var type = PropertyType;

            if (type == typeof(double))
            {
                return "0.0";
            }

            return string.Empty;
        }
    }

    private string InputType
    {
        get
        {
            return Property.TokenType.ToAttributeValue() ?? "text";
        }
    }

    public Type PropertyType
    {
        get
        {
            var nullable = Property.Item.PropertyType.IsNullable();
            var type = nullable
                     ? Nullable.GetUnderlyingType(Property.Item.PropertyType)
                     : Property.Item.PropertyType;
            return type ?? typeof(string);
        }
    }

    public object? PropertyDefault
    {
        get
        {
            var nullable = Property.Item.PropertyType.IsNullable();

            if (nullable)
            {
                return null;
            }

            var type = PropertyType;
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }

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
            var type = PropertyType;

            if (type == typeof(double))
            {
                if (double.TryParse(value, out double newValue))
                {
                    PropertyInfoExtensions.SetPropertyValue(Item, Property.FullName, newValue);
                }
                else
                {
                    PropertyInfoExtensions.SetPropertyValue(Item, Property.FullName, PropertyDefault);
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
