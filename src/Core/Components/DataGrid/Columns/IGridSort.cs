// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IGridSort<TGridItem>
{
    IReadOnlyCollection<SortedProperty> ToPropertyList(bool ascending);
    IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending);
}
