using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Logical operator between filters.
/// </summary>
public enum DataFilterLogicalOperator
{
    [Display(Name = "And")]
    And,

    [Display(Name = "Or")]
    Or,

    [Display(Name = "Not And")]
    NotAnd,

    [Display(Name = "Not Or")]
    NotOr
}
