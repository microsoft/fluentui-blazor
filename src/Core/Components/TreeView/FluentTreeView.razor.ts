export namespace Microsoft.FluentUI.Blazor.TreeView {

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
