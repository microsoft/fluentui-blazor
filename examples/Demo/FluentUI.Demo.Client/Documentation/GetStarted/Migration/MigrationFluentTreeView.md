---
title: Migration FluentTreeView
route: /Migration/TreeView
hidden: true
---

### New properties
  `Appearance`,  `HideSelection`, `MultipleSelectionVisibility`,
  `SelectedId`, `SelectedItems`, `SelectionMode`, `Size` are new properties.

### Removed properties💥
  - The `RenderCollapsedNodes` property has been removed.
  Use `LazyLoadItems` instead.

  - The `FluentTreeItem.InitiallyExpanded` property has been removed. You can use the `FluentTreeItem.Expanded` parameter instead.
    This parameter is now a two-way binding, so you can use it to control the expanded state of the item.

  - The `FluentTreeItem.InitiallySelected` property has been removed. You can use the `FluentTreeView.SelectedId` or `FluentTreeView.SelectedItem`parameter instead.
    These parametere are now a two-way binding.
    The main reason is than the FluentTreeView supports only one selected item at a time (except using the `SelectionMode.Multiple` mode).

  - The `FluentTreeItem.Disabled` property has been removed. The underline webcomponent does not support this property.
