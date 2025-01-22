export namespace Microsoft.FluentUI.Blazor.Utilities.Attributes {

  /**
  * Observe the changes in the `attributeName` attribute to update the element's `propertyName` property.
  */
  export function ObserveAttributeChanges(element: HTMLElement, attributeName: string, propertyName: string): void {
    const observer = new MutationObserver((mutationsList) => {
      for (let mutation of mutationsList) {
        if (mutation.type === 'attributes' && mutation.attributeName === attributeName) {
          const newValue = element.getAttribute(attributeName);
          const field = element as any;
          if (newValue !== field[propertyName]) {
            field[propertyName] = newValue;
          }
        }
      }
    });

    observer.observe(element, { attributes: true });
  }
}
