using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents filters which are available for values.
/// </summary>
public enum DataFilterComparisonOperator
{
    /// <summary>
    /// Equal to the filter value.
    /// </summary>
    [Description("Equal")]
    Equal,

    /// <summary>
    /// Different from the filter value.
    /// </summary>
    [Description("Not Equal")]
    NotEqual,

    /// <summary>
    /// Smaller than the filter value.
    /// </summary>
    [Description("Less Than")]
    LessThan,

    /// <summary>
    /// Smaller than, or equal to, the filter value.
    /// </summary>
    [Description("Less Than Or Equal")]
    LessThanOrEqual,

    /// <summary>
    /// Larger than the filter value.
    /// </summary>
    [Description("Greater Than")]
    GreaterThan,

    /// <summary>
    /// Larger than, or equal to, the filter value.
    /// </summary>
    [Description("Greater Than Or Equal")]
    GreaterThanOrEqual,

    /// <summary>
    /// Containing the filter value.
    /// </summary>
    [Description("Contains")]
    Contains,

    /// <summary>
    /// Which does not contain the filter value.
    /// </summary>
    [Description("Not Contains")]
    NotContains,

    /// <summary>
    /// Which starts with the filter value.
    /// </summary>
    [Description("Starts With")]
    StartsWith,

    /// <summary>
    /// Which ends with the filter value.
    /// </summary>
    [Description("Ends With")]
    EndsWith,

    /// <summary>
    /// Find null values.
    /// </summary>
    [Description("Empty")]
    Empty,

    /// <summary>
    /// Find values which are not null.
    /// </summary>
    [Description("Not Empty")]
    NotEmpty,
}
