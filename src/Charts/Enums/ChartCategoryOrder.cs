// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Specifies the sort order applied to categorical axis groups in chart components.
/// </summary>
public enum ChartCategoryOrder
{
    /// <summary>
    /// Use the default category order, which is typically the order in which
    /// categories appear in the data source.
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// Use the data order, which is the order of categories as they are
    /// provided in the data payload. This may differ from the default order if
    /// the component applies any internal sorting or grouping logic.
    /// </summary>
    [Description("data")]
    Data,

    /// <summary>
    /// Sort categories alphabetically from A to Z (or in ascending order for
    /// non-alphabetic labels).
    /// </summary>
    [Description("category ascending")]
    CategoryAscending,

    /// <summary>
    /// Sort categories alphabetically from Z to A (or in descending order for
    /// non-alphabetic labels).
    /// </summary>
    [Description("category descending")]
    CategoryDescending,

    /// <summary>
    /// Sort categories based on the total value of their associated data points
    /// in ascending order (from smallest to largest total).
    /// </summary>
    [Description("total ascending")]
    TotalAscending,

    /// <summary>
    /// Sort categories based on the total value of their associated data points
    /// in descending order (from largest to smallest total).
    /// </summary>
    [Description("total descending")]
    TotalDescending,

    /// <summary>
    /// Sort categories based on the minimum value of their associated data
    /// points in ascending order (from smallest to largest minimum).
    /// </summary>
    [Description("min ascending")]
    MinAscending,

    /// <summary>
    /// Sort categories based on the minimum value of their associated data
    /// points in descending order (from largest to smallest minimum).
    /// </summary>
    [Description("min descending")]
    MinDescending,

    /// <summary>
    /// Sort categories based on the maximum value of their associated data
    /// points in ascending order (from smallest to largest maximum).
    /// </summary>
    [Description("max ascending")]
    MaxAscending,

    /// <summary>
    /// Sort categories based on the maximum value of their associated data
    /// points in descending order (from largest to smallest maximum).
    /// </summary>
    [Description("max descending")]
    MaxDescending,

    /// <summary>
    /// Sort categories based on the sum of their associated data points in
    /// ascending order (from smallest to largest sum).
    /// </summary>
    [Description("sum ascending")]
    SumAscending,

    /// <summary>
    /// Sort categories based on the sum of their associated data points in
    /// descending order (from largest to smallest sum).
    /// </summary>
    [Description("sum descending")]
    SumDescending,

    /// <summary>
    /// Sort categories based on the mean (average) value of their associated
    /// data points in ascending order (from smallest to largest mean).
    /// </summary>
    [Description("mean ascending")]
    MeanAscending,

    /// <summary>
    /// Sort categories based on the mean (average) value of their associated
    /// data points in descending order (from largest to smallest mean).
    /// </summary>
    [Description("mean descending")]
    MeanDescending,

    /// <summary>
    /// Sort categories based on the median value of their associated data
    /// points in ascending order (from smallest to largest median).
    /// </summary>
    [Description("median ascending")]
    MedianAscending,

    /// <summary>
    /// Sort categories based on the median value of their associated data
    /// points in descending order (from largest to smallest median).
    /// </summary>
    [Description("median descending")]
    MedianDescending,
}
