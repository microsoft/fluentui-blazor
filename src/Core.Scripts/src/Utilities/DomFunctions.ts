export namespace Microsoft.FluentUI.Blazor.Utilities.DomFunctions {

  // See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/DomWrapper.ts

  /**
   * Focuses an HTML or SVG element by reference or by ID.
   * @param elementOrId The element or its ID.
   * @param preventScroll Whether to prevent scrolling the element into view.
   */
  export function focus(elementOrId: HTMLElement | SVGElement | string, preventScroll: boolean = false): void {
    const element = typeof elementOrId === 'string'
      ? document.getElementById(elementOrId)
      : elementOrId;

    if (element instanceof HTMLElement) {
      element.focus({ preventScroll });
    }
    else if (element instanceof SVGElement) {
      if (element.hasAttribute('tabindex')) {
        element.focus({ preventScroll });
      } else {
        throw new Error('Unable to focus an SVG element that does not have a tabindex.');
      }
    } else {
      throw new Error('Unable to focus an invalid element.');
    }
  }

  /**
   * Focuses an element by its CSS selector.
   * @param selector The CSS selector of the element to focus.
   */
  export function focusBySelector(selector: string) {
    const element = document.querySelector(selector) as HTMLElement;
    if (element) {
      // If no explicit tabindex is defined, mark it as programmatically-focusable.
      // This does actually add a new HTML attribute, but it shouldn't interfere with
      // diffing because diffing only deals with the attributes you have in your code.
      if (!element.hasAttribute('tabindex')) {
        element.tabIndex = -1;
      }

      element.focus({ preventScroll: true });
    }
  }
}
