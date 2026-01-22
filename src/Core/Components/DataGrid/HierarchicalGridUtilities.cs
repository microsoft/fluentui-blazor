// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides utility methods for managing hierarchical grid data.
/// </summary>
public static class HierarchicalGridUtilities
{
    /// <summary>
    /// Orders a flat list of grid items into a hierarchical structure based on parent relationships.
    /// </summary>
    /// <typeparam name="TGridItem">The type of the grid item wrapper.</typeparam>
    /// <typeparam name="TItem">The type of the underlying data item.</typeparam>
    /// <param name="initialItems">The initial flat list of items.</param>
    /// <param name="getId">Function to get the unique identifier of an item.</param>
    /// <param name="getParentId">Function to get the parent identifier of an item.</param>
    /// <param name="isCollapsed">Function to determine if an item is initially collapsed.</param>
    /// <returns>A list of items ordered hierarchically, including child relationships.</returns>
    public static List<TGridItem> OrderHierarchically<TGridItem, TItem>(
        List<TGridItem> initialItems,
        Func<TItem, string> getId,
        Func<TItem, string?> getParentId,
        Func<TItem, bool> isCollapsed)
        where TGridItem : HierarchicalGridItem<TItem, TGridItem>
        where TItem : notnull
    {
        var result = new List<TGridItem>();

        foreach (var item in initialItems.Where(it => !HasVisibleParent(it)))
        {
            item.Depth = 0;
            item.IsCollapsed = isCollapsed(item.Item);
            item.IsHidden = false;

            result.Add(item);

            AddChildViewModel(item.Item, item, 1, hidden: item.IsCollapsed);
        }

        return result;

        void AddChildViewModel(TItem parentItem, TGridItem parentWrapper, int depth, bool hidden)
        {
            var parentId = getId(parentItem);
            foreach (var child in initialItems.Where(it => getParentId(it.Item) == parentId))
            {
                child.Depth = depth;
                child.IsCollapsed = isCollapsed(child.Item);
                child.IsHidden = hidden;

                parentWrapper.Children.Add(child);
                result.Add(child);

                AddChildViewModel(child.Item, child, depth + 1, hidden: child.IsHidden || child.IsCollapsed);
            }
        }

        bool HasVisibleParent(TGridItem wrapper)
        {
            var parentId = getParentId(wrapper.Item);
            if (string.IsNullOrEmpty(parentId))
            {
                return false;
            }

            return initialItems.Any(other => getId(other.Item) == parentId);
        }
    }
}
