// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Possibility for an element not to participate in the overflow logic and always remain displayed.
/// </summary>
public enum OverflowItemFixed
{
    /// <summary>
    /// If the item is out of the display, it disappears.
    /// </summary>
    [Description("none")]
    None = 0,

    /// <summary>
    /// The element is always visible
    /// </summary>
    [Description("fixed")]
    Fixed = 1,

    /// <summary>
    /// The element is always visible, but its width can be reduced to display "...".
    /// </summary>
    [Description("ellipsis")]
    Ellipsis = 2,
}
