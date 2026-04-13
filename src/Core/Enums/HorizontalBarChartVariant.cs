// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Enums;

/// <summary>
/// 
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
    /// Gets or sets the absolute scaling factor applied to the object.
    /// </summary>
    [Description("absolute-scale")]
    AbsoluteScale,

    /// <summary>
    /// Represents a single bar element, typically used in charting or data visualization scenarios.
    /// </summary>
    [Description("single-bar")]
    SingleBar,
}
