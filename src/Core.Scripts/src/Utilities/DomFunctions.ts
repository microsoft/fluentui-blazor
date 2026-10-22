import { FASTElement, Updates } from '@microsoft/fast-element';

export namespace Microsoft.FluentUI.Blazor.Utilities.DomFunctions {

  // See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/DomWrapper.ts

  /**
   * Focuses an HTML or SVG element by reference or by ID.
   * @param elementOrId The element or its ID.
   * @param preventScroll Whether to prevent scrolling the element into view.
   */
  export async function focus(elementOrId: HTMLElement | SVGElement | string, preventScroll: boolean = false): Promise<void> {
    const element = typeof elementOrId === 'string'
      ? document.getElementById(elementOrId)
      : elementOrId;

    // If the element is a FASTElement, wait for the next update cycle to ensure it is fully initialized.
    if (element instanceof FASTElement) {
      await Updates.next();
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
   * @param selector The CSS selector of the element to focus.
   * @param preventScroll Whether to prevent scrolling the element into view.
   */
  export async function focusBySelector(selector: string, preventScroll: boolean = false): Promise<void> {
    const element = document.querySelector(selector) as HTMLElement;

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
