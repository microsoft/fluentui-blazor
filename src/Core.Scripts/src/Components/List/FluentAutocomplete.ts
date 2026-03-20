export namespace Microsoft.FluentUI.Blazor.Components.Autocomplete {

  export function initialize(id: string) {
    const input = document.getElementById(id) as HTMLElement;
    if (!input) return;

    new AutocompleteKeyboardNav(id, input);
  }

  class AutocompleteKeyboardNav {

    private hoveredIndex: number = -1;
    private inputId: string;
    private input: HTMLElement;

    constructor(inputId: string, input: HTMLElement) {
      this.inputId = inputId;
      this.input = input;
      this.input.addEventListener('keydown', this.keydownHandler);
      this.input.addEventListener('input', this.inputChangeHandler);
    }

    private getPopover(): HTMLElement | null {
      return document.querySelector(`fluent-popover-b[anchor-id="${this.inputId}"]`);
    }

    private getOptions(): HTMLElement[] {
      const popover = this.getPopover();
      if (!popover) return [];
      return Array.from(popover.querySelectorAll('fluent-option:not([disabled])'));
    }

    private applyHover(option: HTMLElement): void {
      option.style.backgroundColor = 'var(--colorNeutralBackground1Hover)';
      option.style.color = 'var(--colorNeutralForeground2Hover)';
    }

    private clearAllHovers(): void {
      const popover = this.getPopover();
      if (!popover) return;
      popover.querySelectorAll('fluent-option').forEach((opt: Element) => {
        (opt as HTMLElement).style.backgroundColor = '';
        (opt as HTMLElement).style.color = '';
      });
    }

    private inputChangeHandler = (): void => {
      this.clearAllHovers();
      this.hoveredIndex = -1;
    }

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
  }
}
