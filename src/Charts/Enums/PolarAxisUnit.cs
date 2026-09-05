// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the angular unit used by a polar axis.
/// </summary>
public enum PolarAxisUnit
{
    /// <summary>
    /// Uses radians for angular values.
    /// </summary>
    [Description("radians")]
    Radians,

    /// <summary>
    /// Uses degrees for angular values.
    /// </summary>
    [Description("degrees")]
    Degrees,
}
