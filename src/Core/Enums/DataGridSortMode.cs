// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes how many columns a <see cref="FluentDataGrid{TGridItem}"/> can be sorted by at the same time.
/// </summary>
public enum DataGridSortMode
{
    /// <summary>
    /// The grid is sorted by a single column. Sorting another column replaces the current sort.
    /// </summary>
    Single,

    /// <summary>
    /// The grid can be sorted by several columns at once. Columns are added to the sort with Shift+click
    /// (Shift+Enter from the keyboard) or from the column header menu, and each column keeps its own direction.
    /// </summary>
    Multiple,
}
