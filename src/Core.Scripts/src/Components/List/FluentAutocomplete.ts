import { DropdownOption, TextInput } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.Autocomplete {

  /**
   * Initializes the FluentAutocomplete component by attaching keyboard navigation event listeners 
   * to the input element and managing the popover state for option selection.
   * @param id The ID of the input element to initialize.
   */
  export function initialize(id: string) {
    const input = document.getElementById(id) as TextInput;
    if (!input) return;

    detectWrappedItems(input);
    new AutocompleteKeyboardNav(id, input);
  }

  /**
   * Sets focus to the input element of the FluentAutocomplete component, allowing users to start typing immediately.
   * @param id The ID of the input element to focus.
   */
  export function setFocus(id: string) {
    const input = document.getElementById(id) as TextInput;
    if (!input) return;

    input.focus();

    // Move the cursor to the end of the input value
    const control = (input as any).control as HTMLInputElement;
    if (control) {
      const len = control.value.length;
      control.setSelectionRange(len, len);
    }
  }

  /**
   * Detects if the items in the start slot of the autocomplete input are wrapped to multiple lines and sets an attribute accordingly.
   */
  function detectWrappedItems(input: TextInput): void {

    const startSlot = input.querySelector("div[slot='start'] > div") as HTMLElement;
    if (!startSlot) return;

    const observer = new ResizeObserver(entries => {
      for (const entry of entries) {
        if (isFlexWrapped(input, startSlot)) {
          input.setAttribute("items-wrapped", "true");
        } else {
          input.removeAttribute("items-wrapped");
        }
      }
    });

    observer.observe(startSlot);
  }

  /**
   * Returns true if the items in the container are wrapped to multiple lines.
   */
  function isFlexWrapped(input: TextInput, container: HTMLElement): boolean {
    const items = Array.from(container.children) as HTMLElement[];
    if (items.length < 2) return false;
    
    if (input.getAttribute("dismiss") === "hidden") return false;
    if (input.hasAttribute("items-wrapped")) return true;

    const firstTop = items[0].offsetTop;
    return items.some(item => item.offsetTop !== firstTop);
  }

  /**
   * Handles keyboard navigation for the FluentAutocomplete component.
   * - ArrowDown: Moves hover to the next option.
   * - ArrowUp: Moves hover to the previous option.
   * - Enter: Selects the currently hovered option.
   */
  class AutocompleteKeyboardNav {

    private inputId: string;
    private input: TextInput;

    /**
     * Initializes the keyboard navigation for the autocomplete input.
     */
    constructor(inputId: string, input: TextInput) {
      this.inputId = inputId;
      this.input = input;

      const popover = this.getPopover();
      if (!popover) throw new Error(`Popover not found for input with id ${inputId}`);
      this.Popover = popover as IFluentPopover;

      this.input.addEventListener('keydown', this.keydownHandler);
      this.input.addEventListener('input', this.inputChangeHandler);
      this.Popover.addEventListener('toggle', this.popoverToggleHandler);
    }

    private Popover: IFluentPopover;

    /**
     * Handles keydown events on the autocomplete input to manage option hovering and selection.
     */
    private keydownHandler = (e: KeyboardEvent): void => {

      const options = this.getOptions();
      const currentIndex = options.findIndex(o => o.hasAttribute('hovered'));

      switch (e.key) {

        case 'ArrowDown': {
          e.preventDefault();

          if (!this.isPopoverOpen()) this.Popover.showPopover();

          const nextIndex = currentIndex < options.length - 1 ? currentIndex + 1 : options.length - 1;
          this.setHover(options, nextIndex);

          break;
        }

        case 'ArrowUp': {
          e.preventDefault();

          if (!this.isPopoverOpen()) this.Popover.showPopover();

          const prevIndex = currentIndex > 0 ? currentIndex - 1 : 0;
          this.setHover(options, prevIndex);

          break;
        }

        case 'Enter': {
          if (currentIndex >= 0 && this.isPopoverOpen()) {
            e.preventDefault();
            options[currentIndex].click();

            // Close the popover after selection
            this.Popover.closePopover();
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
     * Returns true if the popover is currently open, false otherwise.
     */
    private isPopoverOpen(): boolean {
      return this.Popover.hasAttribute('opened') && (this.Popover.getAttribute('opened') === 'true' || this.Popover.getAttribute('opened') === '' || this.Popover.getAttribute('opened') === null);
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
      options[index].scrollIntoView({ block: 'nearest', inline: 'nearest' });
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

    /**
     * Hovers the first option when the popover is opened.
     */
    private popoverToggleHandler = (e: Event): void => {
      if ((e as CustomEvent).detail?.newState === 'open') {
        this.setHoverFirstOption();
      }
    }

  }

  interface IFluentPopover extends HTMLElement {
    showPopover(): void;
    closePopover(): void;
  }
}
