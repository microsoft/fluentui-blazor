// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the alignment of the text shown in the <see cref="FluentText"/>.
/// </summary>
public enum TextAlign
{
    /// <summary>
    /// Start alignment
    /// </summary>
    [Description("start")]
    Start,

    /// <summary>
    /// End alignment
    /// </summary>
    [Description("end")]
    End,

    /// <summary>
    /// Center alignment
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// Justify alignment
    /// </summary>
    [Description("justify")]
    Justify,
}
