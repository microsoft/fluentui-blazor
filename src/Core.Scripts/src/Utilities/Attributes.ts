export namespace Microsoft.FluentUI.Blazor.Utilities.Attributes {

  /**
  * Observe the change in the HTML `attributeName` attribute to update the element's `propertyName` JavaScript property.
  * @param element The element to observe.
  * @param attributeName The name of the attribute to observe.
  * @param propertyType Optional. The type of the property to update (default is 'string').
  * @param propertyName Optional. The name of the property to update (default is the attributeName).
  * @param forceRefresh Optional. If true, all properties will be refreshed when the attribute changes (default is false).
  * @returns True if the observer was added, false if the observer was already added.
  *
  * Example:
  *   const element = document.getElementById('myCheckbox');
  *   observeAttributeChange(element, 'checked', 'boolean')                    // Observe the 'checked' HTML attribute to update the 'checked' JavaScript property.
  *   observeAttributeChange(element, 'indeterminate', 'boolean', '', true)    // Observe the 'indeterminate' HTML attribute to update all registered JavaScript property (forceRefresh=true).
  */
  export function observeAttributeChange(element: HTMLElement, attributeName: string, propertyType: 'number' | 'string' | 'boolean' = 'string', propertyName: string = '', forceRefresh: boolean = false): boolean {

    if (element == null || element == undefined) {
      return false;
    }

    const fuibName = `attr-${attributeName}`;

    // Check if an Observer is already defined for this element.attributeName
    const fuib = getInternalData(element);
    if (fuib[fuibName]) {
      return false;
    }

    // Set the default propertyName if not provided
    if (propertyName === '') {
      propertyName = attributeName;
    }

    // Create and add an observer on the element.attributeName
    const observer = new MutationObserver((mutationsList) => {
      for (let mutation of mutationsList) {
        if (mutation.type === 'attributes' && mutation.attributeName === attributeName) {

          // Refresh all properties if forceRefresh is true
          if (forceRefresh) {
            for (const key in fuib) {
              if (fuib.hasOwnProperty(key) && key.startsWith('attr-')) {
                const attr = fuib[key];
                updateJavaScriptProperty(element, attr.attributeName, attr.propertyType, attr.propertyName);
              }
            }
          }

          // Refresh only the changed property
          else {
            updateJavaScriptProperty(element, attributeName, propertyType, propertyName);
          }
        }
      }
    });

    // Add an observer and keep the parameters in the element's internal data
    observer.observe(element, { attributes: true });
    fuib[fuibName] = {
      attributeName: attributeName,
      propertyType: propertyType,
      propertyName: propertyName,
    };

    // Update the JavaScript property with the current attribute value
    updateJavaScriptProperty(element, attributeName, propertyType, propertyName);

    return true;
  }

  function updateJavaScriptProperty(element: HTMLElement, attributeName: string, propertyType: 'number' | 'string' | 'boolean', propertyName: string): void {
    const newValue = convertToType(element.getAttribute(attributeName), propertyType);
    const field = element as any;
    if (newValue !== field[propertyName]) {
      field[propertyName] = newValue;
    }
  }

  /**
   * Convert a string value to a typed value.
   * @param value
   * @param type
   * @returns
   */
  function convertToType(value: string | null, type: 'number' | 'string' | 'boolean'): number | string | boolean | null {
    switch (type) {
      case 'number':
        return value ? parseFloat(value) : null;
      case 'boolean':
        return value === 'true' || value === '';
      default:
        return value;
    }
  }

  /**
   * Create or get the internal data object for the element.
   * @param element
   * @returns
   */
  function getInternalData(element: HTMLElement): any {
    if ((element as any)['__fuib'] == undefined) {
      (element as any)['__fuib'] = {};
    }
    return (element as any)['__fuib'];
  }
}
