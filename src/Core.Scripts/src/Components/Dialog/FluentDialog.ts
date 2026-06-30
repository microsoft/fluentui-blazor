export namespace Microsoft.FluentUI.Blazor.Components.Dialog {

  const getDeepActiveElement = (): HTMLElement | null => {
    let activeElement: Element | null = document.activeElement;
    while (activeElement instanceof HTMLElement && activeElement.shadowRoot?.activeElement) {
      activeElement = activeElement.shadowRoot.activeElement;
    }

    return activeElement instanceof HTMLElement ? activeElement : null;
  };

  /**
   * Tag names of non-modal, transient elements (e.g. toasts) that reuse the
   * dialog toggle plumbing but must never restore focus when they open or close.
   */
  const NonFocusRestoringTagNames: string[] = ['FLUENT-TOAST-B'];

  /**
   * Display the fluent-dialog with the given id
   * @param id The id of the fluent-dialog to display
   */
  export function Show(id: string): void {
    const dialog = document.getElementById(id) as any;
    dialog?.show();
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
      // Exclude non-modal, transient elements that reuse the dialog toggle plumbing 
      // but must never restore focus when they open or close.
      if (NonFocusRestoringTagNames.includes(dialog.tagName)) {
        return;
      }

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

  /**
   * Prevent or allow the ESC key from closing the dialog.
   * When prevent is true, a capture-phase keydown listener blocks the Escape key
   * before the web component can process it.
   * @param id The id of the fluent-dialog element
   * @param prevent Whether to prevent ESC from closing the dialog
   */
  export function SetPreventEscapeClose(id: string, prevent: boolean): void {
    const dialog = document.getElementById(id) as any;
    if (!dialog) {
      return;
    }

    // Remove any previously registered handler for this dialog.
    if (dialog._escapeHandler) {
      dialog.removeEventListener('keydown', dialog._escapeHandler, true);
      dialog._escapeHandler = null;
    }

    if (prevent) {
      const handler = (e: KeyboardEvent) => {
        if (e.key === 'Escape') {
          e.stopImmediatePropagation();
          e.preventDefault();
        }
      };
      dialog._escapeHandler = handler;
      dialog.addEventListener('keydown', handler, true);
    }
  }

  /**
   * Returns whether dialog keyboard shortcuts should be handled for the current focused element.
   * Shortcuts are limited to dialog action areas so interactive content inside drawers/dialogs
   * can use Enter/Space/Arrow keys without being intercepted.
   * @param id The id of the fluent-dialog/fluent-drawer element
   */
  export function ShouldHandleShortcut(id: string): boolean {
    const dialog = document.getElementById(id) as HTMLElement | null;
    if (!dialog) {
      return false;
    }

    const activeElement = getDeepActiveElement();
    if (!activeElement || !dialog.contains(activeElement)) {
      return false;
    }

    // Never steal keyboard handling from editable/menu-like controls in dialog content.
    if (
      activeElement.matches('input, textarea, select, fluent-data-grid, [contenteditable=""], [contenteditable="true"], [role="textbox"], [role="combobox"], [role="spinbutton"], [role="listbox"], [role="menu"], [role="menuitem"]') ||
      !!activeElement.closest('fluent-menu, fluent-menu-list, fluent-menu-item, .col-header-ui')
    ) {
      return false;
    }

    // Keep shortcuts active for explicit dialog action surfaces.
    return !!activeElement.closest('[slot="action"], [slot="footer"], [slot="close"], [slot="title-action"]');
  }
}
