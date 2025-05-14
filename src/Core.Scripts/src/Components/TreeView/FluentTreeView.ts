import { TreeItem } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.TreeView {

  /**
   * Switches the expanded state of a tree item.
   * @param event
   * @returns
   */
  export function ToggleItem(svg: SVGElement | null) {

    if (!svg) {
      return;
    }

    const treeItem = svg.closest('fluent-tree-item') as TreeItem;
    if (treeItem) {
      treeItem.expanded = !treeItem.expanded;
    }
  }
}
