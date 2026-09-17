// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The alignment content of the <see cref="FluentDivider" />.
/// </summary>
public enum DividerAlignContent
{
    /// <summary>
    /// The content is aligned at the center.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// The content is aligned at the start.
    /// </summary>
    [Description("start")]
    Start,

    /// <summary>
    /// The content is aligned at the end.
    /// </summary>
    [Description("end")]
    End,
}
