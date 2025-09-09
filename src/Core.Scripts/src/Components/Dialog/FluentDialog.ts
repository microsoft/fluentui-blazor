export namespace Microsoft.FluentUI.Blazor.Components.Dialog {

  /**
   * Display the fluent-dialog with the given id
   * @param id The id of the fluent-dialog to display
   */
  export function Show(id: string): HTMLElement | null {
    const previousElement = document.activeElement as HTMLElement;
    const dialog = document.getElementById(id) as any;
    dialog?.show();
    return previousElement;
  }

  /**
 * Hide the fluent-dialog with the given id
 * @param id The id of the fluent-dialog to hide
 */
  export function Hide(id: string): void {
    const dialog = document.getElementById(id) as any;
    dialog?.hide();
  }

  export function FocusPreviousElement(element: HTMLElement): void {
    if (element) {
      element.focus();
    }
  }
}
