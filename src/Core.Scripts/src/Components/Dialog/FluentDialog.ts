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
    FocusOnPreviousActiveElement(id);
  }

  /**
    * Save the element that was active before the dialog was opened
    * @param id
    */
  export function DialogToggle_PreviousActiveElement(id: string, newState: string): void {
    const dialog = document.getElementById(id) as any;
    if (dialog) {
      if (newState === 'open') {
        dialog.previousActiveElement = document.activeElement;
      }
      else if (newState === 'closed') {
        FocusOnPreviousActiveElement(id);
      }
    }
  }

  /**
   * Focus on the element that was active before the dialog was opened
   * @param id
   */
  export function FocusOnPreviousActiveElement(id: string): void {
    const dialog = document.getElementById(id) as any;
    if (dialog) {
      setTimeout(() => {
        dialog.previousActiveElement?.focus();
      }, 25);
    }
  }
}
