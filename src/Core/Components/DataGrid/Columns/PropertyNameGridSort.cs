// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components.Components;

public class PropertyNameGridSort<TGridItem> : IGridSort<TGridItem>
{
    private readonly string _propertyName;
    private readonly Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? _sortFunction;

    public PropertyNameGridSort(
        string propertyName,
        Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? sortFunction = null)
    {
        _propertyName = propertyName;
        _sortFunction = sortFunction;
    }

    public IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending)
    {
        if (_sortFunction != null)
        {
            return _sortFunction(queryable, ascending);
        }

        return queryable.OrderBy(x => 0);
    }

    public IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending)
    {
        return new List<SortedProperty> {
            new SortedProperty
            {
                 PropertyName = _propertyName,
                 Direction = ascending
                    ? SortDirection.Ascending
                    : SortDirection.Descending,
            }
        };
    }
}
