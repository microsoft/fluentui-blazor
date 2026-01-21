import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Components.TextInput {

  /**
  * Attach 'input-delayed' event handling to the input element.
  * @param {string} id - The element ID
  * @param {number} delay - The delay in milliseconds before raising input-delayed
  * @returns {InputDelayed | null} The created InputDelayed instance, or null if element not found
  */
  export function attachDelayedEvent(id: string, delay: number): InputDelayed | null {
    const element = document.getElementById(id) as InputDelayedElement | null;
    if (!element || delay <= 0) {
      return null;
    }

    // Dispose existing instance if any
    if (element.__inputDelayed) {
      element.__inputDelayed.dispose();
    }

    // Create new instance and attach to element
    element.__inputDelayed = new InputDelayed(element, delay);

    return element.__inputDelayed;
  };

  /**
   * Detach 'input-delayed' event handling from the input element.
   * @param {string} id - The element ID
   */
  export function detachDelayedEvent(id: string): void {
    const element = document.getElementById(id) as InputDelayedElement | null;
    if (element?.__inputDelayed) {
      element.__inputDelayed.dispose();
    }
  };

  /**
   * Type for elements that support delayed input events
   */
  type InputDelayedElement = (FluentUIComponents.TextInput | FluentUIComponents.TextArea) & {
    __inputDelayed?: InputDelayed;
  };

  /**
   * Detail object for the input-delayed custom event
   */
  interface InputDelayedEventDetail {
    value: string;
    originalEvent: Event;
  }

  /**
   * Class to manage delayed input events for a single input element.
   * Attach this to an element's __inputDelayed property for automatic 
   * garbage collection when the element is removed from DOM.
   */
  class InputDelayed {
    private readonly element: InputDelayedElement;
    private readonly delay: number;
    private timerId: ReturnType<typeof setTimeout> | null;
    private readonly inputHandler: (event: Event) => void;

    /**
     * @param element - The input element
     * @param delay - The delay in milliseconds before raising input-delayed
     */
    constructor(element: InputDelayedElement, delay: number) {
      this.element = element;
      this.delay = delay;
      this.timerId = null;

      // Bind the handler to preserve 'this' context
      this.inputHandler = this.onInput.bind(this);

      // Add the input event listener in capture phase to run before other listeners
      // Use capture: true to ensure we run first, then stopImmediatePropagation prevents others
      this.element.addEventListener('input', this.inputHandler, { capture: true });
    }

    /**
     * Internal input event handler
     */
    private onInput(event: Event): void {
      // Stop other input event listeners from firing
      event.stopImmediatePropagation();

      // Clear any existing timer
      if (this.timerId !== null) {
        clearTimeout(this.timerId);
      }

      // Set a new timer
      this.timerId = setTimeout(() => {
        // Create and dispatch the custom input-delayed event
        const delayedEvent = new CustomEvent<InputDelayedEventDetail>('input-delayed', {
          bubbles: true,
          cancelable: true,
          detail: {
            value: this.element.value,
            originalEvent: event
          }
        });

        this.element.dispatchEvent(delayedEvent);
        this.timerId = null;
      }, this.delay);
    }

    /**
     * Dispose and clean up the delayed input event.
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
      if (this.element.__inputDelayed === this) {
        delete this.element.__inputDelayed;
      }
    }
  }
}