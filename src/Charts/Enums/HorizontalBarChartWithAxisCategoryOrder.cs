// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components.Enums;

/// <summary>
/// Specifies which axis catagory order visual variant the<c>
/// HorizontalBarChartWithAxis</c> uses.
/// </summary>
public enum HorizontalBarChartWithAxisCategoryOrder
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
    /// Use the category ascending order, which sorts categories alphabetically
    /// from A to Z (or in ascending order for non-alphabetic labels).
    /// </summary>
    [Description("category ascending")]
    CategoryAscending,

    /// <summary>
    /// Use the category descending order, which sorts categories alphabetically
    /// from Z to A (or in descending order for non-alphabetic labels).
    /// </summary>
    [Description("category descending")]
    CategoryDescending,

    /// <summary>
    /// Use the total ascending order, which sorts categories based on the total
    /// value of their associated data points in ascending order (from smallest
    /// to largest total).
    /// </summary>
    [Description("total ascending")]
    TotalAscending,

    /// <summary>
    /// Use the total descending order, which sorts categories based on the
    /// total value of their associated data points in descending order (from
    /// largest to smallest total).
    /// </summary>
    [Description("total descending")]
    TotalDescending,

    /// <summary>
    /// Use the minimal ascending order, which sorts categories based on the
    /// minimum value of their associated data points in ascending order (from
    /// smallest to largest minimum).
    /// </summary>
    [Description("min ascending")]
    MinAscending,

    /// <summary>
    /// Use the minimal descending order, which sorts categories based on the
    /// minimum value of their associated data points in descending order (from
    /// largest to smallest minimum).
    /// </summary>
    [Description("min descending")]
    MinDescending,

    /// <summary>
    /// Use the maximum ascending order, which sorts categories based on the
    /// maximum value of their associated data points in ascending order (from
    /// smallest to largest maximum).
    /// </summary>
    [Description("max ascending")]
    MaxAscending,

    /// <summary>
    /// Use the maximum descending order, which sorts categories based on the
    /// maximum value of their associated data points in descending order (from
    /// largest to smallest maximum).
    /// </summary>
    [Description("max descending")]
    MaxDescending,

    /// <summary>
    /// Use the sum ascending order, which sorts categories based on the sum of
    /// their associated data points in ascending order (from smallest to
    /// largest sum).
    /// </summary>
    [Description("sum ascending")]
    SumAscending,

    /// <summary>
    /// Use the sum descending order, which sorts categories based on the sum of
    /// their associated data points in descending order (from largest to
    /// smallest sum).
    /// </summary>
    [Description("sum descending")]
    SumDescending,

    /// <summary>
    /// Use the mean ascending order, which sorts categories based on the mean
    /// (average) value of their associated data points in ascending order (from
    /// smallest to largest mean).
    /// </summary>
    [Description("mean ascending")]
    MeanAscending,

    /// <summary>
    /// Use the mean descending order, which sorts categories based on the mean
    /// (average) value of their associated data points in descending order
    /// (from largest to smallest mean).
    /// </summary>
    [Description("mean descending")]
    MeanDescending,

    /// <summary>
    /// Use the median ascending order, which sorts categories based on the
    /// median value of their associated data points in ascending order (from
    /// smallest to largest median).
    /// </summary>
    [Description("median ascending")]
    MedianAscending,

    /// <summary>
    /// Use the median descending order, which sorts categories based on the
    /// median value of their associated data points in descending order (from
    /// largest to smallest median).
    /// </summary>
    [Description("median descending")]
    MedianDescending,
}
