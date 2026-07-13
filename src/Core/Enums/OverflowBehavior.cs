// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the overflow behavior for an item.
/// </summary>
public enum OverflowBehavior
{
    /// <summary>
    /// The item is kept fixed in place and not subject to overflow.
    /// </summary>
    [Description("fixed")]
    Fixed,

    /// <summary>
    /// The item can be hidden with an ellipsis indicator when space is constrained.
    /// </summary>
    [Description("ellipsis")]
    Ellipsis,
}
