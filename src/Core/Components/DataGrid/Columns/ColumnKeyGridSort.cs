// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a sort order specification used within <see cref="ColumnBase{TGridItem}"/> using the column key .
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class ColumnKeyGridSort<TGridItem> : IGridSort<TGridItem>
{
    private readonly string _columnKey;
    private readonly Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? _sortFunction;

    /// <summary />
    public ColumnKeyGridSort(
        string columnKey,
        Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? sortFunction = null)
    {
        _columnKey = columnKey;
        _sortFunction = sortFunction;
    }

    /// <summary>
    /// Apply the sort function to the collection
    /// </summary>
    /// <param name="queryable">The collection to sort</param>
    /// <param name="ascending">Sort ascending (true) or descending (false)</param>
    /// <returns> /// <returns>The ordered collection</returns></returns>
    public IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending)
    {
        if (_sortFunction != null)
        {
            return _sortFunction(queryable, ascending);
        }

        // If no sort is provided, apply a sort that has no affect in order to be able to return an IOrderedQueryable
        return queryable.OrderBy(x => 0);
    }

    /// <summary>
    /// Produces a readonly collection of (property name, direction) pairs representing the sorting rules.
    /// </summary>
    /// <param name="ascending"></param>
    /// <returns>The readonly collection of properties that can be sorted on</returns>
    public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending)
    {
        return [
            new SortedProperty
            {
                 PropertyName = _columnKey,
                 Direction = ascending
                    ? DataGridSortDirection.Ascending
                    : DataGridSortDirection.Descending,
            },
        ];
    }
}
