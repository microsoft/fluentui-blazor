// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IGridSort<TGridItem>
{
    IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending);
    IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending);
}
