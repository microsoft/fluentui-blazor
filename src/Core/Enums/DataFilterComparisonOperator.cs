using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents filters which are available for values.
/// </summary>
public enum DataFilterComparisonOperator 
{
    /// <summary>
    /// Equal to the filter value.
    /// </summary>
    [Display(Name = "Equal")]
    Equal,

    /// <summary>
    /// Different from the filter value.
    /// </summary>
    [Display(Name = "Not Equal")]
    NotEqual,

    /// <summary>
    /// Smaller than the filter value.
    /// </summary>
    [Display(Name = "Less Than")]
    LessThan,

    /// <summary>
    /// Smaller than, or equal to, the filter value.
    /// </summary>
    [Display(Name = "Less Than Or Equal")]
    LessThanOrEqual,

    /// <summary>
    /// Larger than the filter value.
    /// </summary>
    [Display(Name = "Greater Than")]
    GreaterThan,

    /// <summary>
    /// Larger than, or equal to, the filter value.
    /// </summary>
    [Display(Name = "Greater Than Or Equal")]
    GreaterThanOrEqual,

    /// <summary>
    /// Containing the filter value.
    /// </summary>
    [Display(Name = "Contains")]
    Contains,

    /// <summary>
    /// Which does not contain the filter value.
    /// </summary>
    [Display(Name = "Not Contains")]
    NotContains,

    /// <summary>
    /// Which starts with the filter value.
    /// </summary>
    [Display(Name = "Starts With")]
    StartsWith,

    /// <summary>
    /// Which not starts with the filter value.
    /// </summary>
    [Display(Name = "Not Starts With")]
    NotStartsWith,

    /// <summary>
    /// Which ends with the filter value.
    /// </summary>
    [Display(Name = "Ends With")]
    EndsWith,

    /// <summary>
    /// Which not ends with the filter value.
    /// </summary>
    [Display(Name = "Not Ends With")]
    NotEndsWith,

    /// <summary>
    /// Find null values.
    /// </summary>
    [Display(Name = "Empty")]
    Empty,

    /// <summary>
    /// Find values which are not null.
    /// </summary>
    [Display(Name = "Not Empty")]
    NotEmpty,

    /// <summary>
    /// Find values which are in range.
    /// </summary>
    [Display(Name = "In")]
    In,

    /// <summary>
    /// Find values which are not in range.
    /// </summary>
    [Display(Name = "Not In")]
    NotIn,

    /// <summary>
    /// Custom filter
    /// </summary>
    Custom,
}
