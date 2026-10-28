import { FASTElement, Updates } from '@microsoft/fast-element';
import { RadioGroup } from '@fluentui/web-components';

export namespace Microsoft.FluentUI.Blazor.Utilities.DomFunctions {

  // See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/DomWrapper.ts

  /**
   * Focuses an HTML or SVG element by reference or by ID.
   * @param elementOrId The element or its ID.
   * @param preventScroll Whether to prevent scrolling the element into view.
   * @param selector An optional CSS selector identifying the focus target instead of the root element.
   */
  export async function focus(elementOrId: HTMLElement | SVGElement | string, preventScroll: boolean = false, selector?: string): Promise<void> {
    if (selector) {
      await focusBySelector(elementOrId, selector, preventScroll);
      return;
    }

    const element = typeof elementOrId === 'string'
      ? document.getElementById(elementOrId)
      : elementOrId;

    // If the element is a FASTElement, wait for the next update cycle to ensure it is fully initialized.
    if (element instanceof FASTElement) {
      await Updates.next();
    }

    // The radio group focus override does not forward FocusOptions to its radio.
    if (element instanceof RadioGroup) {
      const radios = element.enabledRadios;
      const radio = radios.find(radio => radio.checked) ?? radios[0];
      radio?.focus({ preventScroll });
      return;
    }

    // If the element is an HTML element, it can be focused directly.
    if (element instanceof HTMLElement) {
      element.focus({ preventScroll });
    }

    // If the element is an SVG element, it must have a tabindex to be focusable.
    else if (element instanceof SVGElement) {
      if (element.hasAttribute('tabindex')) {
        element.focus({ preventScroll });
      } else {
        throw new Error('Unable to focus an SVG element that does not have a tabindex.');
      }
    }

    // If the element is neither an HTML nor an SVG element, it is considered invalid for focusing.
    else {
      throw new Error('Unable to focus an invalid element.');
    }
  }

  /**
   * Focuses an element by its CSS selector.
   * @param elementOrId The element or its ID. When null or undefined, the selector is used instead.
   * @param selector The CSS selector of the element to focus.
   * @param preventScroll Whether to prevent scrolling the element into view.
   */
  export async function focusBySelector(elementOrId: HTMLElement | SVGElement | string | null | undefined, selector: string, preventScroll: boolean = false): Promise<void> {
    
    // Determine the element to focus based on the provided elementOrId or selector.
    const element = elementOrId === null || elementOrId === undefined
      ? document.querySelector<HTMLElement | SVGElement>(selector)
      : typeof elementOrId === 'string'
        ? document.getElementById(elementOrId)?.querySelector<HTMLElement | SVGElement>(selector)
        : elementOrId.querySelector<HTMLElement | SVGElement>(selector);

    if (element) {

      // If the element is a FASTElement, wait for the next update cycle to ensure it is fully initialized.
      if (element instanceof FASTElement) {
        await Updates.next();
      }
      // If no explicit tabindex is defined, mark it as programmatically-focusable.
      // This does actually add a new HTML attribute, but it shouldn't interfere with
      // diffing because diffing only deals with the attributes you have in your code.
      if (!element.hasAttribute('tabindex')) {
        element.tabIndex = -1;
      }

      element.focus({ preventScroll });
    }
  }
}
