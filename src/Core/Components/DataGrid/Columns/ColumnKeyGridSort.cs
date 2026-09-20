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
    private readonly Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? _thenSortFunction;

    /// <summary />
    /// <param name="columnKey">The key reported by <see cref="ToPropertyList(bool)"/>, for a data source that sorts the data itself.</param>
    /// <param name="sortFunction">An optional function that sorts the collection, used when the grid sorts the data.</param>
    /// <param name="thenSortFunction">
    /// An optional function that appends this sort to an already ordered collection, using
    /// <see cref="Queryable.ThenBy{TSource, TKey}(IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{Func{TSource, TKey}})"/>.
    /// Supply it together with <paramref name="sortFunction"/> to let this column take part in a multi-column sort
    /// (see <see cref="FluentDataGrid{TGridItem}.SortMode"/>).
    /// </param>
    public ColumnKeyGridSort(
        string columnKey,
        Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? sortFunction = null,
        Func<IOrderedQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? thenSortFunction = null)
    {
        _columnKey = columnKey;
        _sortFunction = sortFunction;
        _thenSortFunction = thenSortFunction;
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

    /// <inheritdoc />
    /// <remarks>
    /// Without a sort function the data source does the sorting from <see cref="ToPropertyList(bool)"/>, so this sort
    /// can be used at any level. With one, it also needs the then-sort function to be used as a secondary level.
    /// </remarks>
    public bool CanApplyThen => _sortFunction is null || _thenSortFunction is not null;

    /// <summary>
    /// Appends this sort's rules to a collection that is already ordered.
    /// </summary>
    /// <param name="queryable">The already ordered collection.</param>
    /// <param name="ascending">Sort ascending (true) or descending (false)</param>
    /// <returns>The ordered collection</returns>
    public IOrderedQueryable<TGridItem> ApplyThen(IOrderedQueryable<TGridItem> queryable, bool ascending)
    {
        // As in Apply: without a sort function, the ordering is left to the data source.
        return _thenSortFunction is not null
            ? _thenSortFunction(queryable, ascending)
            : queryable;
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
