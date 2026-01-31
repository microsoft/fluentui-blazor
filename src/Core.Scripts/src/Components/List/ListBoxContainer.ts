import * as FluentUIComponents from '@fluentui/web-components'

/**
 * ListBoxContainer - Enhances FluentUI Listbox with keyboard navigation and multi-select support.
 * 
 * The `Init` function initializes a `ListBoxContainer` by attaching a `ListboxExtended` instance to a `fluent-listbox` element.
 * Call this from Blazor via JS interop after the component renders: 
 *   await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.ListBoxContainer.Init", "container-id");
 * 
 * The ListboxExtended class provides:
 * - Keyboard navigation (ArrowUp/Down to navigate, Space/Enter to select)
 * - Multi-select mode support (controlled by 'multiple' attribute on container)
 * - Tab index management for accessibility
 * - Automatic synchronization between DOM changes and Blazor state via MutationObserver
 * - Custom 'listboxchange' event dispatched when selections change (detail.selectedOptions contains semicolon-separated IDs)
 * 
 * Usage: Place a 'multiple' attribute on the container div to enable multi-select mode. 
 * Each fluent-option must have an 'id' attribute for tracking selections.
 */
export namespace Microsoft.FluentUI.Blazor.Components.ListBoxContainer {
  export function Init(id: string) {
    const container = document.getElementById(id) as HTMLElement;
    const listbox = container?.querySelector('fluent-listbox') as FluentUIComponents.Listbox;
    if (listbox) {
      (listbox as any).__fluentListbox = new ListboxExtended(listbox, container);
    }
  }

  export function refresh(id: string) {
    const container = document.getElementById(id) as HTMLElement;
    const listbox = container?.querySelector('fluent-listbox') as FluentUIComponents.Listbox;
    if (listbox) {
      const instance = (listbox as any).__fluentListbox as ListboxExtended;
      instance.refresh();
    }
  }

  /**
   * Extends the FluentUI Listbox component with additional functionality.
   */
  class ListboxExtended {

    private isInitialized: boolean = false;
    private container: HTMLElement;
    private listbox: FluentUIComponents.Listbox;

    /**
     * Initializes a new instance of the ListboxExtended class.
     * @param element The FluentUI Listbox element.
     */
    constructor(element: FluentUIComponents.Listbox, container: HTMLElement) {
      this.container = container;
      this.listbox = element;
      this.listbox.addEventListener("keydown", this.keydownHandler);

      // Set initial selected options based on the current state
      setTimeout(() => {
        this.refresh(true);
        this.setupListboxObserver();
        this.isInitialized = true;
      }, 0);

    }

    /**
     * Refresh the list items and default values
     * @param firstRendering 
     */
    public refresh(firstRendering: boolean = false) {
      this.listbox.multiple = (this.container.hasAttribute('multiple')) ?? false;

      // Set initial selected options based on the current state
      if (this.listbox.multiple) {
        const selectedIds = this.listbox.selectedOptions.map(option => option.id);
        for (let i = 0; i < this.listbox.options.length; i++) {
          const option = this.listbox.options[i];
          option.multiple = true;
          option.selected = selectedIds.find(id => id === option.id) !== undefined;
        }
      }

      // If disabled or readonly, do not set tab index
      if (this.container.hasAttribute('disabled') || this.container.hasAttribute('readonly')) {
        return;
      }

      // Set the tabIndex="0" to the first item, if not already yet on another item
      const existingTabItem = this.listbox.querySelector('fluent-option[tabindex="0"]');
      if (!existingTabItem) {
        const firstItem = this.listbox.querySelector('fluent-option') as FluentUIComponents.DropdownOption | null;
        if (firstItem) {
          firstItem.tabIndex = 0;
        }
      }
    }

    /**
     * Handles keyboard events for the ListBoxContainer.
     * @param e The keyboard event.
     * @returns Whether the event was handled.
     */
    private keydownHandler = (e: KeyboardEvent): boolean | void => {

      if (!this.hasItems()) {
        return;
      }

      if (this.container.hasAttribute('disabled') || this.container.hasAttribute('readonly')) {
        return;
      }

      switch (e.key) {
        case 'ArrowUp': {
          e.preventDefault();
          this.activeIndex--;
          break;
        }

        case 'ArrowDown': {
          e.preventDefault();
          this.activeIndex++;
          break;
        }

        case ' ':
        case 'Enter': {
          e.preventDefault();
          const activeItem = this.activeItem;


          if (activeItem) {
            if (this.listbox.multiple) {
              activeItem.selected = !activeItem.selected;
            }
            else {
              activeItem.selected = true;
              this.unselectAllExcept(activeItem);
            }
          }
          break;
        }
      }

      return true;
    }

    /**
     * Unselects all options except the specified one.
     * @param selectedOption
     */
    private unselectAllExcept(selectedOption: FluentUIComponents.DropdownOption) {
      this.getItems().forEach(option => {
        if (option !== selectedOption) {
          option.selected = false;
        }
      });
    }

    /**
     * Gets the items of the ListBoxContainer.
     * @returns The list of items in the ListBoxContainer.
     */
    private getItems = (): FluentUIComponents.DropdownOption[] => {
      return Array.from(this.listbox.querySelectorAll('fluent-option'));
    }

    /**
     * Checks if the ListBoxContainer has any items.
     * @returns True if the ListBoxContainer has items, false otherwise.
     */
    private hasItems = (): boolean => {
      return this.listbox.querySelectorAll('fluent-option').length > 0;
    }

    /** 
     * Gets the active index of the ListBoxContainer. 
    */
    private get activeIndex(): number {
      const items = this.getItems();
      const activeIndex = items.findIndex(i => i.active);

      if (activeIndex >= 0) {
        return activeIndex;
      }

      return items.findIndex(i => i.tabIndex === 0);
    }

    /**
     * Sets the active index of the ListBoxContainer.
     */
    private set activeIndex(index: number) {
      const items = this.getItems();

      if (index < 0) {
        index = 0;
      }

      if (index >= items.length) {
        index = items.length - 1;
      }

      for (let i = 0; i < items.length; i++) {
        items[i].tabIndex = (i === index) ? 0 : -1;
        if (i === index) {
          items[i].focus();
        }
      }

    }

    /**
     * Gets the active item of the ListBoxContainer.
     * @returns The active item in the ListBoxContainer, or null if there is no active item.
     */
    private get activeItem(): FluentUIComponents.DropdownOption | null {
      const items = this.getItems();
      return items[this.activeIndex] || null;
    }

    /**
     * Sets up a single MutationObserver on the listbox to detect changes to child fluent-option elements.
     */
    private setupListboxObserver = (): void => {

      const observer = new MutationObserver((mutations) => {
        let hasSelectedOptionsChanged = false;
        let hasNewRemovedOptions = false;

        mutations.forEach(mutation => {

          // Detect attribute changes on child nodes (fluent-option elements)
          if (mutation.type === 'attributes' && mutation.target !== this.listbox) {
            const target = mutation.target as FluentUIComponents.DropdownOption;
            const isSelectedAttribute = mutation.attributeName === 'current-selected' || mutation.attributeName === 'selected';
            if (target.tagName === 'FLUENT-OPTION' && isSelectedAttribute) {

              hasSelectedOptionsChanged = true;
            }
          }

          // Detect when child nodes are added or removed
          if (mutation.type === 'childList') {
            hasNewRemovedOptions = true;
          }
        });

        if (hasNewRemovedOptions) {
          // Defer to allow FluentUI component to update its internal options array
          queueMicrotask(() => {
            this.refresh(false);
          });
        }

        if (hasSelectedOptionsChanged) {
          this.raiseSelectedOptionsChangeEvent();
        }
      });

      observer.observe(this.listbox, {
        attributes: true,
        attributeFilter: ['current-selected', 'selected'],
        subtree: true,
        childList: true
      });
    }

    /**
     * Raises the onselectedoptionschange event when selected options change.
     */
    private raiseSelectedOptionsChangeEvent = (): void => {

      if (!this.isInitialized) {
        return;
      }

      const event = new CustomEvent('listboxchange', {
        bubbles: true,
        composed: true,
        detail: {
          selectedOptions: this.listbox.selectedOptions.map(option => option.id).join(';')
        }
      });

      this.container.dispatchEvent(event);
    }

  }
}
