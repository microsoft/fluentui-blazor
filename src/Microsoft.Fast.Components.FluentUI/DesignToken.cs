using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;


public class DesignToken<T>
{

    /// <summary>
    /// The default value of the token
    /// </summary>
    public T? DefaultValue { get; set; }

    /// <summary>
    /// The value of the token
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// The name of the token
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// A list of elements for which the DesignToken has a value set
    /// </summary>
    public List<ElementReference>? AppliedTo { get; set; } = new();

    public DesignToken()
    {

    }

    public DesignToken(string name)
    {
        Name = name;
    }

    public async Task SetValueFor(ElementReference element, T value)
    {
        Value = value;
        if (AppliedTo! != null)
            AppliedTo.Add(element);


    }

    public T? GetValueFor(ElementReference element)
    {
        return Value;

    }

    //DeleteValueFor

    //WithDefault

    //Subscribe
    //Unsubscribe
}

//public class CSSDesignToken<T> : DesignToken<T>
//{
//    public string? CSSCustomProperty { get; set; }
//}