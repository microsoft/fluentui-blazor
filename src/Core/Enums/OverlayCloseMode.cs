// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The close mode of an overlay component.
/// </summary>
public enum OverlayCloseMode
{
    /// <summary>
    /// The overlay can be closed manually only, using code.
    /// </summary>
    [Description("manual")]
    Manual,

    /// <summary>
    /// The overlay can be closed by clicking anywhere.
    /// </summary>
    [Description("all")]
    All,

    /// <summary>
    /// The overlay can be closed by clicking inside the overlay content.
    /// </summary>
    [Description("inside")]
    InsideOnly,

    /// <summary>
    /// The overlay can be closed by clicking outside the overlay content.
    /// </summary>
    [Description("outside")]
    OutsideOnly,
}
