// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Enums;

/// <summary>
/// Specifies which visual variant the<c> HorizontalBarChart</c> uses.
/// </summary>
public enum HorizontalBarChartVariant
{
    /// <summary>
    /// Specifies that the data represents a part-to-whole relationship, where each value is a component of a total.
    /// </summary>
    /// <remarks>Use this value when visualizing or analyzing data in which individual elements contribute to
    /// a collective whole, such as in pie charts or stacked bar charts.</remarks>
    [Description("part-to-whole")]
    PartToWhole,

    /// <summary>
    /// Specifies that bar lengths are based on the raw data values using a
    /// shared absolute scale. Choose this variant when comparing magnitudes
    /// across bars, rather than showing each bar as part of a total as in
    /// <see cref="PartToWhole"/>, or rendering a standalone value as in
    /// <see cref="SingleBar"/>.
    /// </summary>
    [Description("absolute-scale")]
    AbsoluteScale,

    /// <summary>
    /// Represents a single bar element, typically used in charting or data visualization scenarios.
    /// </summary>
    [Description("single-bar")]
    SingleBar,
}
