import { TreeItem } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.TreeView {

  /**
    * Toggle the expand/collapse tree item.
    * And selecte the item.
    */
  export function ToggleItem(element: HTMLElement | null) {

    if (!element) {
      return;
    }

    const treeItem = element.tagName.toLowerCase() === 'fluent-tree-item'
      ? element as TreeItem
      : element.closest('fluent-tree-item') as TreeItem;

    if (treeItem) {
      treeItem.expanded = !treeItem.expanded;
      treeItem.selected = true;
    }
  }
}
