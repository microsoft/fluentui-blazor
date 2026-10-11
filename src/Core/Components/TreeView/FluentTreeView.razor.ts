export namespace Microsoft.FluentUI.Blazor.TreeView {

  const controlledCheckboxes = new WeakSet<HTMLElement>();

  interface TreeCheckbox extends HTMLElement {
    checked: boolean;
    indeterminate: boolean;
  }

  /**
   * Synchronizes live properties, including after user interaction or removing CheckState.
   * The web component does not reflect its indeterminate property to an attribute.
   */
  export function UpdateCheckStates(id: string) {
    const treeView = document.getElementById(id);
    if (!treeView) {
      return;
    }

    for (const checkbox of treeView.querySelectorAll<TreeCheckbox>('fluent-tree-item > fluent-checkbox')) {
      if (checkbox.closest('fluent-tree') !== treeView) {
        continue;
      }

      const controlled = checkbox.hasAttribute('check-state');
      if (controlled || controlledCheckboxes.has(checkbox)) {
        checkbox.checked = checkbox.hasAttribute('checked');
        checkbox.indeterminate = checkbox.hasAttribute('indeterminate');

        if (controlled) {
          controlledCheckboxes.add(checkbox);
        } else {
          controlledCheckboxes.delete(checkbox);
        }
      }
    }
  }

  /**
   * Initializes the Fluent TreeView component.
   * @param id
   * @param multiple
   */
  export function Initialize(id: string, multiple: boolean) {
    const treeView = document.getElementById(id);

    if (treeView && multiple) {

      treeView.addEventListener('keydown', (event: KeyboardEvent) => {

        if (event.code === 'Space' && event.target instanceof HTMLElement) {

          const checkbox = event.target.querySelector('fluent-checkbox') as HTMLElement;
          if (checkbox) {
            checkbox.click();
          }
        }
      });
    }
  }
}
