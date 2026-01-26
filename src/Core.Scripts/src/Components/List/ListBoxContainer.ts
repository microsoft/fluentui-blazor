import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Components.ListBoxContainer {
  export function Init(id: string) {
    const container = document.getElementById(id) as HTMLElement;
    const listbox = container?.querySelector('fluent-listbox') as FluentUIComponents.Listbox;
    if (listbox) {
      (listbox as any).__fluentListbox = new ListboxExtended(listbox, container);
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
      this.listbox.multiple = (this.container.hasAttribute('multiple')) ?? false;
      this.listbox.addEventListener("keydown", this.keydownHandler);

      const firstItem = this.firstItem();
      if (firstItem) {
        firstItem.tabIndex = 0;
      }

      // Set initial selected options based on the current state
      const selectedIds = this.listbox.selectedOptions.map(option => option.id);     
      setTimeout(() => {
        if (this.listbox.multiple) {
          for (let i = 0; i < this.listbox.options.length; i++) {
            const option = this.listbox.options[i];
            option.selected = selectedIds.find(id => id === option.id) !== undefined;
          }
        }
        this.isInitialized = true;
      }, 0);

      this.setupListboxObserver();

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
            activeItem.selected = !activeItem.selected;
          }
          break;
        }
      }

      return true;
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
      return this.listbox.querySelectorAll('fluent-option') !== null;
    }

    /**
     * Gets the first item of the ListBoxContainer.
     * @returns The first item in the ListBoxContainer, or null if there are no items.
     */
    private firstItem = (): FluentUIComponents.DropdownOption | null => {
      return this.listbox.querySelector('fluent-option');
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
        mutations.forEach(mutation => {

          // Detect attribute changes on child nodes (fluent-option elements)
          if (mutation.type === 'attributes' && mutation.target !== this.listbox) {
            const target = mutation.target as FluentUIComponents.DropdownOption;
            if (target.tagName === 'FLUENT-OPTION' &&
              (mutation.attributeName === 'current-selected' || mutation.attributeName === 'selected')) {
              this.raiseSelectedOptionsChangeEvent();
            }
          }

          // Detect when child nodes are added or removed
          if (mutation.type === 'childList') {
            this.raiseSelectedOptionsChangeEvent();
          }
        });
      });

      observer.observe(this.listbox, {
        attributes: true,
        attributeFilter: ['current-selected'],
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
