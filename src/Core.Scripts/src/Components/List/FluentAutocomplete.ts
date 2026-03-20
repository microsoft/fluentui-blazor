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
      
      const popover = this.getPopover();
      if (!popover) throw new Error(`Popover not found for input with id ${inputId}`);
      this.Popover = popover;

      this.input.addEventListener('keydown', this.keydownHandler);
      this.input.addEventListener('input', this.inputChangeHandler);
    }

    private Popover: HTMLElement;

    /**
     * Handles keydown events on the autocomplete input to manage option hovering and selection.
     */
    private keydownHandler = (e: KeyboardEvent): void => {
      if (!this.Popover.hasAttribute('opened')) return;

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
      return Array.from(this.Popover.querySelectorAll('fluent-option:not([disabled])'));
    }

    /**
     * Sets the hovered attribute on the specified option.
     */
    private setHover(options: DropdownOption[], index: number): void {
      if (index < 0 || index >= options.length) return;
      this.clearAllHovers();
      options[index].setAttribute('hovered', '');
      options[index].tabIndex = 0;
    }

    /**
     * Sets the hover on the first option in the popover.
     */
    private setHoverFirstOption(): void {
      const options = this.getOptions();
      if (options.length === 0) return;
      this.setHover(options, 0);
    }

    /**
     * Clears the hovered attribute from all options in the popover.
     */
    private clearAllHovers(): void {
      this.Popover.querySelectorAll('fluent-option[hovered]').forEach((opt: Element) => {
        const option = opt as DropdownOption;
        option.removeAttribute('hovered');
          option.tabIndex = -1;
      });
    }

    /**
     * Clears all hovers when the input value changes to ensure the hover state is reset when the user types.
     */
    private inputChangeHandler = (): void => {
      this.setHoverFirstOption();
    }
   
  }
}
