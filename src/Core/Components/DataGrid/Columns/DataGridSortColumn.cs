// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents one level of a <see cref="FluentDataGrid{TGridItem}"/>'s sort: the column that is sorted on and the
/// direction it is sorted in. The position in <see cref="FluentDataGrid{TGridItem}.SortColumns"/> is the level's
/// priority, where the first entry is the primary sort.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="Column">The column that is sorted on.</param>
/// <param name="Ascending">Whether the column is sorted ascending.</param>
public sealed record DataGridSortColumn<TGridItem>(ColumnBase<TGridItem> Column, bool Ascending)
{
    /// <summary>
    /// Gets the direction the column is sorted in.
    /// </summary>
    public DataGridSortDirection Direction => Ascending ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;
}
