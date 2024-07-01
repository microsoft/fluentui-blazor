using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

public enum DataFilterLogicalOperator
{
    [Description("And")]
    And,

    [Description("Or")]
    Or,

    [Description("Not And")]
    NotAnd,

    [Description("Not Or")]
    NotOr
}
