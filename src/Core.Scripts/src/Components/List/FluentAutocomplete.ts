import { DropdownOption } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.Autocomplete {

  export function initialize(id: string) {
    const input = document.getElementById(id) as HTMLElement;
    if (!input) return;

    new AutocompleteKeyboardNav(id, input);
  }

  /**
   * Handles keyboard navigation for the FluentAutocomplete component.
   * - ArrowDown: Moves hover to the next option.
   * - ArrowUp: Moves hover to the previous option.
   * - Enter: Selects the currently hovered option.
   */
  class AutocompleteKeyboardNav {

    private inputId: string;
    private input: HTMLElement;

    /**
     * Initializes the keyboard navigation for the autocomplete input.
     */
    constructor(inputId: string, input: HTMLElement) {
      this.inputId = inputId;
      this.input = input;
      this.input.addEventListener('keydown', this.keydownHandler);
      this.input.addEventListener('input', this.inputChangeHandler);
    }

    /**
     * Handles keydown events on the autocomplete input to manage option hovering and selection.
     */
    private keydownHandler = (e: KeyboardEvent): void => {
      const popover = this.getPopover();
      if (!popover || !popover.hasAttribute('opened')) return;

      const options = this.getOptions();
      if (options.length === 0) return;

      const currentIndex = options.findIndex(o => o.hasAttribute('hovered'));

      switch (e.key) {

        case 'ArrowDown': {
          e.preventDefault();
          const nextIndex = currentIndex < options.length - 1 ? currentIndex + 1 : options.length - 1;
          this.setHover(options, nextIndex);
          break;
        }

        case 'ArrowUp': {
          e.preventDefault();
          const prevIndex = currentIndex > 0 ? currentIndex - 1 : 0;
          this.setHover(options, prevIndex);
          break;
        }

        case 'Enter': {
          if (currentIndex >= 0) {
            e.preventDefault();
            options[currentIndex].click();
            this.clearAllHovers();
          }
          break;
        }
      }
    }

    /**
     * Returns the popover element associated with the autocomplete input.
     */
    private getPopover(): HTMLElement | null {
      return document.querySelector(`fluent-popover-b[anchor-id="${this.inputId}"]`);
    }

    /**
     * Returns an array of enabled options within the popover.
     */
    private getOptions(): DropdownOption[] {
      const popover = this.getPopover();
      if (!popover) return [];
      return Array.from(popover.querySelectorAll('fluent-option:not([disabled])'));
    }

    /**
     * Sets the hovered attribute on the specified option.
     */
    private setHover(options: DropdownOption[], index: number): void {
      if (index < 0 || index >= options.length) return;
      this.clearAllHovers();
      options[index].setAttribute('hovered', '');
    }

    /**
     * Clears the hovered attribute from all options in the popover.
     */
    private clearAllHovers(): void {
      const popover = this.getPopover();
      if (!popover) return;
      popover.querySelectorAll('fluent-option[hovered]').forEach((opt: Element) => {
        opt.removeAttribute('hovered');
      });
    }

    private inputChangeHandler = (): void => {
      this.clearAllHovers();
    }
   
  }
}
