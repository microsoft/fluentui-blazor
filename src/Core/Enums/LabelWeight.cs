// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The weight of the labeltext.
/// </summary>
public enum LabelWeight
{
    /// <summary>
    /// Regular label.
    /// </summary>
    [Description("regular")]
    Regular,

    /// <summary>
    /// Semibold label.
    /// </summary>
    [Description("semibold")]
    Semibold,
}
