
---
title: TreeView
route: /TreeView
---

# TreeView

A hierarchical list structure component for displaying data in a collapsible and expandable way.

Use this component when you need to present your users with a clear visual structure of content or data,
allowing them to efficiently interact and navigate through the information.
If the information is less hierarchical or node-based, consider using a list or table instead.

When using this component, it is possible not to select an item.
However, once an item has been selected, it is mandatory to keep an item selected.
If you no longer want to select anything, you must do so by code: `SelectedId = null`.

It is not possible to select multiple items at once. However, a custom development (see below) can be implemented to allow the selection of multiple items.

You can create a Tree manually by nesting `FluentTreeItem` components or by using the `Items` property of `FluentTreeView` to dynamically generate a tree from a list of objects.
If you use the `Items` property, you can also set the `ItemTemplate` property to specify how each item should be displayed.
With these two ways of creating a tree, using the `Text` parameter will display the text of the element, adding an ellipsis `...` if the text is too long.
Each element also has the properties `IconStart`, `IconEnd`, and `IconAside` to display an icon to the left, right, or at the end of the text.

> [!NOTE]
> When a user clicks on an item, it is selected and expanded/collapsed if children are present.
> It is not possible to open an item without selecting it (except by using the keyboard, pressing the `Enter` or `Space` key.).

## Manual TreeView

{{ TreeViewDefault }}

## Dynamic tree generation via Items

{{ TreeViewWithItems }}

## With Unlimited Items

{{ TreeViewWithUnlimitedItems }}

## API FluentTreeView

{{ API Type=FluentTreeView }}

## API FluentTreeItem

{{ API Type=FluentTreeItem }}

## Class TreeViewItem

{{ API Type=TreeViewItem Properties=All }}
