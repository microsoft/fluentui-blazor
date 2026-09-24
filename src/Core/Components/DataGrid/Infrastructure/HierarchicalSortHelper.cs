// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

/// <summary>
/// Restores the parent/child order of hierarchical grid items after they have been sorted.
/// </summary>
/// <remarks>
/// Sorting a hierarchical grid moves every item independently, which separates children from their parents.
/// <see cref="RestoreHierarchyOrder{TGridItem}(IQueryable{TGridItem}, IOrderedQueryable{TGridItem})"/> walks the
/// sorted items depth first, so each parent is followed by its own (sorted) children again.
/// This runs once, after every sort level has been applied.
/// </remarks>
internal static class HierarchicalSortHelper
{
    /// <summary>
    /// Gets whether <paramref name="queryable"/> holds hierarchical items that are sorted in memory. Hierarchy can
    /// only be restored for those, because it needs to enumerate the sorted items.
    /// </summary>
    public static bool IsHierarchicalInMemoryQueryable<TGridItem>(IQueryable<TGridItem> queryable)
        => typeof(IHierarchicalGridItem).IsAssignableFrom(typeof(TGridItem))
            && queryable.Provider is EnumerableQuery<TGridItem>;

    /// <summary>
    /// Reorders <paramref name="sortedQueryable"/> so that every item is directly followed by its visible children.
    /// </summary>
    /// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
    /// <param name="queryable">The unsorted collection the sort was applied to.</param>
    /// <param name="sortedQueryable">The collection with all sort levels applied.</param>
    /// <returns>The hierarchically ordered collection.</returns>
    public static IOrderedQueryable<TGridItem> RestoreHierarchyOrder<TGridItem>(IQueryable<TGridItem> queryable, IOrderedQueryable<TGridItem> sortedQueryable)
    {
        var sortedItems = sortedQueryable.ToList();
        if (sortedItems.Count == 0)
        {
            return sortedQueryable;
        }

        var itemOrder = sortedItems
            .Select((item, index) => (Item: (object)item!, Index: index))
            .ToDictionary(x => x.Item, x => x.Index, ReferenceEqualityComparer.Instance);

        var visibleItemsSet = new HashSet<object>(sortedItems.Select(item => (object)item!), ReferenceEqualityComparer.Instance);
        var rootItems = sortedItems
            .Where(item => item is IHierarchicalGridItem { Depth: 0 })
            .ToList();

        if (rootItems.Count == 0)
        {
            return sortedQueryable;
        }

        var orderedItems = new List<TGridItem>(sortedItems.Count);
        var orderedItemsSet = new HashSet<object>(ReferenceEqualityComparer.Instance);

        AppendSortedHierarchy(rootItems, visibleItemsSet, orderedItems, orderedItemsSet, itemOrder);

        var remainingItems = sortedItems.Where(item => !orderedItemsSet.Contains((object)item!));
        foreach (var item in remainingItems)
        {
            if (orderedItemsSet.Add((object)item!))
            {
                orderedItems.Add(item);
            }
        }

        var hierarchyOrder = orderedItems
            .Select((item, index) => (Item: (object)item!, Index: index))
            .ToDictionary(x => x.Item, x => x.Index, ReferenceEqualityComparer.Instance);

        return queryable.OrderBy(item => hierarchyOrder[(object)item!]);
    }

    private static void AppendSortedHierarchy<TGridItem>(
        IReadOnlyList<TGridItem> siblings,
        HashSet<object> visibleItemsSet,
        List<TGridItem> orderedItems,
        HashSet<object> orderedItemsSet,
        IReadOnlyDictionary<object, int> itemOrder)
    {
        foreach (var item in siblings.OrderBy(item => itemOrder[(object)item!]))
        {
            if (!orderedItemsSet.Add((object)item!))
            {
                continue;
            }

            orderedItems.Add(item);

            if (item is not IHierarchicalGridItem hierarchicalItem)
            {
                continue;
            }

            var visibleChildren = hierarchicalItem.Children
                .OfType<TGridItem>()
                .Where(child => visibleItemsSet.Contains((object)child!))
                .ToList();

            if (visibleChildren.Count > 0)
            {
                AppendSortedHierarchy(visibleChildren, visibleItemsSet, orderedItems, orderedItemsSet, itemOrder);
            }
        }
    }
}
