---
title: SortableList
route: /SortableList
icon: ArrowSort
---

# SortableList

A [SortableJS](https://sortablejs.github.io/Sortable/) library adaptation for Blazor Fluent UI. Allows for reordering elements within a list using drag and drop. 

_Inspired by and based on [Burke Holland's article and code](https://devblogs.microsoft.com/dotnet/introducing-blazor-sortable/). Re-used with permission._

The `FluentSortableList` is a generic component that takes a list of items and a `ItemTemplate` that defines how to render each item in the sortable list.

>[!Note] The SortableJS script is included in our library script. You don not need to include the SortableJS script manually!

>[!Warning] The component does not actually do any sorting or moving of items. It simply provides the hooks to do so. You will need to handle all events yourself. 
**If you don't handle any events, no sort or move will happen** as Blazor needs to make the changes to the underlying data structures so it can re-render the list.


Here is an example of how to reorder your list when the OnUpdate is fired...

```[csharp]
private void SortList(FluentSortableListEventArgs args)
{
     if (args is null || args.OldIndex == args.NewIndex)
     {
         return;
     }

     var oldIndex = args.OldIndex;
     var newIndex = args.NewIndex;

     var items = this.items;
     var itemToMove = items[oldIndex];
     items.RemoveAt(oldIndex);

     if (newIndex &lt; items.Count)
     {
         items.Insert(newIndex, itemToMove);
     }
     else
     {
         items.Add(itemToMove);
     }
}
```
## Accessabillity support
Although SortableJS does not come with a11y support, we enhanced the `FluentSortableList` component to support keyboard accessibility. Users can navigate through the list items using the arrow up/down keys. An item can be grabbed and released by using the space bar or pressing enter. A grabbed item can then be moved in the list with the arrow up and down keys.

The component also provides appropriate ARIA attributes to enhance screen reader support.

## Examples

### Simple sortable list
{{ SortableListDefault }}

### Move items between two lists

Shared lists are lists where items can be dragged from one list to the other and vice-versa. Providing the same "Group" string name for both lists is what links them together. <br/>

>[!Note] When an item is dragged into a different list, it assumes the visual style of that list. This is because Blazor controls the rendering of the list items.
    
{{ SortableListMoveBetweenLists }}

### Clone items

Cloning is enabled by setting the `Clone` parameter to `true`. This allows cloning of an item by dropping it into a shared list.

{{ SortableListCloneBetweenLists }}
    
### Disabling sorting

You can disable sorting with the `Sort` parameter set to `false`. You can also disable dropping items on a list by setting the `Drop` parameter to `false`. In the example below, you can drag from list 1 to list 2, but not from list 2 to list 1. You can sort list 2, but not list 1.
    
{{ SortableListDisabledSorting }}

### Drag Handles

When setting the `Handle` parameter to true, the items can only be sorted using the drag handle itself. The following CSS classes can be used to split the drag functionality from the content:

- sortable-grab: the grabbable part of the draggable item
- sortable-item-content: the content part of the draggable item

{{ SortableListDragHandles }}

### Filtering
    
In the lists below, you cannot drag the items in the accented color. This is because these items are filtered out with an `ItemFilter` parameter (of type `Func<TItem, bool>`).
The `ItemFilter` parameter is a function that takes an item and returns a boolean value. If the function returns true, the item is excluded from dragging in the list. If the function returns false, the item can be dragged.

In the left list below, the `ItemFilter` parameter is set to filter out (at most) 4 random items from the list. In the right list, the `ItemFilter` parameter is set to filter out items with an Id larger than 6.
See the Razor tab for how the different functions are being specified.

>[!Note] Dragging between the lists is not implmented in this example, only sorting within each list.
    
{{ SortableListFiltering }}

### Sortable list using fallback behavior

By setting Fallback parameter to true, the list will not use native HTML5 drag and drop behavior.
    
{{ SortableListFallback }}



##  API FluentSortableList

{{ API Type=FluentSortableList<int> }}
