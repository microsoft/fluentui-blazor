using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class ThemeTokenTypeAttribute : Attribute
{
    public ThemeTokenTypeAttribute(TokenType value)
    {
        Value = value;
    }

    public TokenType Value { get; set; }
}

public enum TokenType
{
    [Description("text")]
    Text,

    [Description("number")]
    Number,

    [Description("color")]
    Color
}
