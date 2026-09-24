// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the interpolation curve used for a polar line.
/// </summary>
public enum PolarLineCurve
{
    /// <summary>
    /// Renders straight line segments.
    /// </summary>
    [Description("linear")]
    Linear,

    /// <summary>
    /// Renders a smoothed natural spline.
    /// </summary>
    [Description("natural")]
    Natural,

    /// <summary>
    /// Renders a step curve.
    /// </summary>
    [Description("step")]
    Step,

    /// <summary>
    /// Renders a step-after curve.
    /// </summary>
    [Description("stepAfter")]
    StepAfter,

    /// <summary>
    /// Renders a step-before curve.
    /// </summary>
    [Description("stepBefore")]
    StepBefore,
}
