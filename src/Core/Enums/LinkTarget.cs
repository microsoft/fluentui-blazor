// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the frame or window name that has the defined linking relationship.
/// </summary>
public enum LinkTarget
{

    /// <summary>
    /// Load the designated document in a new, unnamed window.
    /// </summary>
    [Description("_blank")]
    Blank,

    /// <summary>
    /// Load the document in the same frame as the element that refers to this target.
    /// </summary>
    [Description("_self")]
    Self,

    /// <summary>
    /// Load the document into the immediate FRAMESET parent of the current frame. This value is equivalent to _self if the current frame has no parent.
    /// </summary>
    [Description("_parent")]
    Parent,

    /// <summary>
    /// Load the document into the full, original window (thus canceling all other frames). This value is equivalent to _self if the current frame has no parent.
    /// </summary>
    [Description("_top")]
    Top,
}
