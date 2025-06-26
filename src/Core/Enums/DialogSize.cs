// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the size of a dialog or a drawer (panel).
/// </summary>
public enum DialogSize
{
    /// <summary>
    /// The width of the dialog or drawer is 320px.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// The width of the dialog or drawer is 592px.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// The width of the dialog or drawer is 940px.
    /// </summary>
    [Description("large")]
    Large,

    /// <summary>
    /// The width of the dialog or drawer is 100%.
    /// </summary>
    [Description("full")]
    Full,
}
