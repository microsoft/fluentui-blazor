// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public class ColumnKeyGridSort<TGridItem> : IGridSort<TGridItem>
{
    private readonly string _columnKey;
    private readonly Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? _sortFunction;

    public ColumnKeyGridSort(
        string columnKey,
        Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? sortFunction = null)
    {
        _columnKey = columnKey;
        _sortFunction = sortFunction;
    }

    public IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending)
    {
        if (_sortFunction != null)
        {
            return _sortFunction(queryable, ascending);
        }

        // If no sort is provided, apply a sort that has no affect in order to be able to return an IOrderedQueryable
        return queryable.OrderBy(x => 0);
    }

    public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending)
    {
        return new List<SortedProperty> {
            new SortedProperty
            {
                 PropertyName = _columnKey,
                 Direction = ascending
                    ? SortDirection.Ascending
                    : SortDirection.Descending,
            }
        };
    }
}
