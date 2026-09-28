// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Controls how the callout displays x-axis data points when multiple series are present for the same x value.
/// </summary>
public enum CalloutVariant
{
    /// <summary>
    /// Single — each callout contains only the data point for that x value.
    /// </summary>
    [Description("single")]
    Single,

    /// <summary>
    /// Stacked — each callout contains all the data points for that x value, stacked on top of each other.
    /// </summary>
    [Description("stacked")]
    Stacked,
}
