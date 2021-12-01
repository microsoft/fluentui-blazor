using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;


public class DesignToken<T>
{
    /// <summary>
    /// The value of the token
    /// </summary>
    public T? DefaultValue { get; set; }

    /// <summary>
    /// The name of the token
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// A list of elements for which the DesignToken has a value set
    /// </summary>
    public List<DesignTokenValue<T>>? AppliedTo { get; set; }

    public DesignToken()
    {
        AppliedTo = new List<DesignTokenValue<T>>();
    }

    public DesignToken(string name)
    {
        Name = name;
        AppliedTo = new List<DesignTokenValue<T>>();
    }

    public void SetValueFor(ElementReference element, T value)
    {
        if (AppliedTo! != null)
            AppliedTo.Add(new(element, value));
    }

    public T? GetValueFor(ElementReference element)
    {
        if (AppliedTo != null)
        {
            DesignTokenValue<T>? x = AppliedTo.First(x => x.Element.Equals(element));
            return x.Value;
        }

        return DefaultValue;
    }

    //DeleteValueFor

    //WithDefault

    //Subscribe
    //Unsubscribe
}

public class CSSDesignToken<T> : DesignToken<T>
{
    public string? CSSCustomProperty { get; set; }
}