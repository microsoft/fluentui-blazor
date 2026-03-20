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
    
    private hoveredIndex: number = -1;
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

      switch (e.key) {

        case 'ArrowDown': {
          e.preventDefault();
          this.clearAllHovers();
          this.hoveredIndex = this.hoveredIndex < options.length - 1 ? this.hoveredIndex + 1 : options.length - 1;
          this.applyHover(options[this.hoveredIndex]);
          break;
        }

        case 'ArrowUp': {
          e.preventDefault();
          this.clearAllHovers();
          this.hoveredIndex = this.hoveredIndex > 0 ? this.hoveredIndex - 1 : 0;
          this.applyHover(options[this.hoveredIndex]);
          break;
        }

        case 'Enter': {
          if (this.hoveredIndex >= 0 && this.hoveredIndex < options.length) {
            e.preventDefault();
            options[this.hoveredIndex].click();
            this.clearAllHovers();
            this.hoveredIndex = -1;
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
     * Displays hover styles on the specified option.
     */
    private applyHover(option: DropdownOption): void {
      option.style.backgroundColor = 'var(--colorNeutralBackground1Hover)';
      option.style.color = 'var(--colorNeutralForeground2Hover)';
    }

    /**
     * Clears hover styles from all options in the popover.
     */
    private clearAllHovers(): void {
      const popover = this.getPopover();
      if (!popover) return;
      popover.querySelectorAll('fluent-option').forEach((opt: Element) => {
        (opt as DropdownOption).style.backgroundColor = '';
        (opt as DropdownOption).style.color = '';
      });
    }

    private inputChangeHandler = (): void => {
      this.clearAllHovers();
      this.hoveredIndex = -1;
    }
   
  }
}
