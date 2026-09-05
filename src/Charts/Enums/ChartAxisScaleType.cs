// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the scale type used for a continuous numeric chart axis.
/// </summary>
public enum ChartAxisScaleType
{
    /// <summary>
    /// Use a linear scale.
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// Use a logarithmic scale.
    /// </summary>
    [Description("log")]
    Log,
}
