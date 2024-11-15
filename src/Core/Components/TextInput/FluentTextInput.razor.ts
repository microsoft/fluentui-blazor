export namespace Microsoft.FluentUI.Blazor.TextInput {

  /**
   * Observe the changes in the ‘value’ attribute to update the element's ‘value’ property.
   * Wait for this PR to delete the code.
   * https://github.com/microsoft/fluentui/pull/33144
   */
  export function ObserveAttributeChanges(element: HTMLElement): void {
    const observer = new MutationObserver((mutationsList) => {
      for (let mutation of mutationsList) {
        if (mutation.type === "attributes" && mutation.attributeName === "value") {
          const newValue = element.getAttribute("value");
          const field = element as any;
          if (newValue !== field.value) {
            field.value = newValue;
          }
        }
      }
    });

    observer.observe(element, { attributes: true });
  }

}
