using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DesignTokenValue<T>
{
    public ElementReference Element { get; set; }
    public T? Value { get; set; }

    public DesignTokenValue(ElementReference element, T value)
    {
        Element = element;
        Value = value;
    }
}