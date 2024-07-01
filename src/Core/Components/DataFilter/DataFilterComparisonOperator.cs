using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

public enum DataFilterComparisonOperator
{
    [Description("Equal")]
    Equal,

    [Description("Not Equal")]
    NotEqual,

    [Description("Less Than")]
    LessThan,

    [Description("Less Than Or Equal")]
    LessThanOrEqual,

    [Description("Greater Than")]
    GreaterThan,

    [Description("Greater Than Or Equal")]
    GreaterThanOrEqual,

    [Description("Contains")]
    Contains,

    [Description("Not Contains")]
    NotContains,

    [Description("Starts With")]
    StartsWith,

    [Description("Ends With")]
    EndsWith,

    [Description("Empty")]
    Empty,

    [Description("Not Empty")]
    NotEmpty,
}
