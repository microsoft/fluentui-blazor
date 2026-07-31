import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Components.TextInput {

  /**
  * Attach 'immediate' event handling to the input element.
  * @param {string} id - The element ID
  * @param {number} delay - The delay in milliseconds before raising 'immediate' event
  * @returns {InputImmediate | null} The created InputImmediate instance, or null if element not found
  */
  export function attachImmediateEvent(id: string, delay: number): InputImmediate | null {
    const element = document.getElementById(id) as InputImmediateElement | null;
    if (!element) {
      return null;
    }

    // Dispose existing instance if any
    if (element.__immediate) {
      element.__immediate.dispose();
    }

    // Create new instance and attach to element
    element.__immediate = new InputImmediate(element, delay);

    return element.__immediate;
  };

  /**
   * Detach 'immediate' event handling from the input element.
   * @param {string} id - The element ID
   */
  export function detachImmediateEvent(id: string): void {
    const element = document.getElementById(id) as InputImmediateElement | null;
    if (element?.__immediate) {
      element.__immediate.dispose();
    }
  };

  /**
   * Type for elements that support delayed 'immediate' input events
   */
  type InputImmediateElement = (FluentUIComponents.TextInput | FluentUIComponents.TextArea) & {
    __immediate?: InputImmediate;
  };

  /**
   * Detail object for the 'immediate' custom event
   */
  interface InputImmediateEventDetail {
    value: string;
    originalEvent: Event;
  }

  /**
   * Class to manage delayed 'immediate' input events for a single input element.
   * Attach this to an element's __immediate property for automatic 
   * garbage collection when the element is removed from DOM.
   */
  class InputImmediate {
    private readonly element: InputImmediateElement;
    private readonly delay: number;
    private timerId: ReturnType<typeof setTimeout> | null;
    private readonly inputHandler: (event: Event) => void;

    /**
     * @param element - The input element
     * @param delay - The delay in milliseconds before raising 'immediate' event
     */
    constructor(element: InputImmediateElement, delay: number) {
      this.element = element;
      this.delay = delay;
      this.timerId = null;

      // Bind the handler to preserve 'this' context
      this.inputHandler = this.onInput.bind(this);

      // Add the input event listener
      this.element.addEventListener('input', this.inputHandler);
    }

    /**
     * Internal input event handler
     */
    private onInput(event: Event): void {

      // If delay is zero or negative, dispatch immediately
      if (this.delay <= 0) {
        this.element.dispatchEvent(this.createImmediateEvent(event));
        return;
      }

      // Reset any existing timer to delay the event
      if (this.timerId !== null) {
        clearTimeout(this.timerId);
      }

      // Set a new timer
      this.timerId = setTimeout(() => {
        // Create and dispatch the custom 'immediate' event
        this.element.dispatchEvent(this.createImmediateEvent(event));
        this.timerId = null;
      }, this.delay);
    }

    /**
     * Create the custom 'immediate' event
     * @param event 
     * @returns 
     */
    private createImmediateEvent(event: Event): CustomEvent<InputImmediateEventDetail> {
      return new CustomEvent<InputImmediateEventDetail>('immediate', {
        bubbles: true,
        cancelable: true,
        detail: {
          value: this.element.value,
          originalEvent: event
        }
      });
    }

    /**
     * Dispose and clean up the 'immediate' input event.
     */
    public dispose(): void {
      // Clear any pending timer
      if (this.timerId !== null) {
        clearTimeout(this.timerId);
        this.timerId = null;
      }

      // Remove the input event listener
      this.element.removeEventListener('input', this.inputHandler);

      // Remove reference from the element
      if (this.element.__immediate === this) {
        delete this.element.__immediate;
      }
    }
  }
}