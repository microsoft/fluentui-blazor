// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a sort change event.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class DataGridSortEventArgs<TGridItem> : EventArgs
{
    /// <summary>
    /// Gets the columns the grid is sorted by, in priority order: the first entry is the primary sort, the ones after
    /// it break its ties. Holds at most one entry unless <see cref="FluentDataGrid{TGridItem}.SortMode"/> is
    /// <see cref="DataGridSortMode.Multiple"/>, and is empty when the grid is no longer sorted.
    /// </summary>
    public IReadOnlyList<DataGridSortColumn<TGridItem>> SortColumns { get; init; } = [];
}
