// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the direction in which a <see cref="FluentDataGrid{TGridItem}"/> column is sorted.
/// </summary>
public enum DataGridSortDirection
{
    /// <summary>
    /// Automatic sort order. When used with <see cref="FluentDataGrid{TGridItem}.SortByColumnAsync(ColumnBase{TGridItem}, DataGridSortDirection)"/>,
    /// the sort order will automatically toggle between <see cref="Ascending"/> and <see cref="Descending"/> on successive calls, and
    /// resets to <see cref="Ascending"/> whenever the specified column is changed.
    /// </summary>
    Auto,

    /// <summary>
    /// Ascending order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Descending order.
    /// </summary>
    Descending,
}
