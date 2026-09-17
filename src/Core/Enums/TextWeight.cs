// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the weight of the text shown in the <see cref="FluentText"/>.
/// </summary>
public enum TextWeight
{
    /// <summary>
    /// Medium weight
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Regular weight
    /// </summary>
    [Description("regular")]
    Regular,

    /// <summary>
    /// Semibold weight
    /// </summary>
    [Description("semibold")]
    Semibold,

    /// <summary>
    /// Bold weight
    /// </summary>
    [Description("bold")]
    Bold,
}
