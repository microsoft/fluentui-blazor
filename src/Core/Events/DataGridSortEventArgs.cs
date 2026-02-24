// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a sort change event.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class DataGridSortEventArgs<TGridItem>
{
    /// <summary>
    /// Gets the column that defines the sort order.
    /// </summary>
    public ColumnBase<TGridItem>? Column { get; init; }

    /// <summary>
    /// Gets a value indicating whether the grid is sorted ascending.
    /// </summary>
    public bool SortByAscending { get; init; }
}
