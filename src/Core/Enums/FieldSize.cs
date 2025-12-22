// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of the label in a <see cref="FluentField"/>.
/// </summary>
public enum FieldSize
{
    /// <summary>
    /// Small label.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Medium label.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Large label.
    /// </summary>
    [Description("large")]
    Large,
}
