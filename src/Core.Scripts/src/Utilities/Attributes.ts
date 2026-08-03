export namespace Microsoft.FluentUI.Blazor.Utilities.Attributes {

  /**
   * Copies an attribute from a regular element to a shadow DOM element.
   * @param elementOrId The element or its ID to copy the attribute from.
   * @param shadowSelector The selector for the shadow DOM element to copy the attribute to. E.g `[part='control']`
   * @param attributeName The name of the attribute to copy.
   * @param attributeValue The value of the attribute to copy.
   * @returns
   */
  export function copyToShadow(
    elementOrId: HTMLElement | string,
    shadowSelector: string,
    attributeName: string,
    attributeValue: string) {

    // Get the element reference
    let element: HTMLElement | null;
    if (typeof elementOrId === 'string') {
      element = document.getElementById(elementOrId);
    } else {
      element = elementOrId;
    }

    if (!element) {
      return;
    }

    // Find the shadow element with the specified selector
    const shadowElement = element.shadowRoot?.querySelector(shadowSelector);

    if (shadowElement) {
      // Add the attribute to the found shadow element
      shadowElement.setAttribute(attributeName, attributeValue);
    }
  }

  /**
   * Applies custom CSS to an element inside a shadow DOM by injecting a scoped <style> element into
   * the shadow root. Unlike a `::part()` selector applied from outside, this can also target
   * pseudo-elements (e.g. '::-ms-reveal') since the style lives in the same shadow tree as the target.
   * @param elementOrId The host element (or its ID) whose shadow root will receive the <style> element.
   * @param shadowSelector The selector (scoped to the shadow root) that plain CSS declarations apply to, e.g. '.control'.
   * @param style CSS text. Plain declarations without a selector (e.g. 'color: red;') are wrapped with
   * `shadowSelector`; text containing a selector (e.g. '::-ms-reveal { display: none; }') is injected as-is.
   */
  export function applyShadowStyle(elementOrId: HTMLElement | string, shadowSelector: string, style: string | null): void {
    let element: HTMLElement | null;
    if (typeof elementOrId === 'string') {
      element = document.getElementById(elementOrId);
    } else {
      element = elementOrId;
    }

    const shadowRoot = element?.shadowRoot;
    if (!shadowRoot) {
      return;
    }

    const styleElement = shadowRoot.querySelector<HTMLStyleElement>('style[data-fluent-shadow-style]');

    if (!style) {
      styleElement?.remove();
      return;
    }

    const cssText = style.includes('{') ? style : `${shadowSelector} { ${style} }`;

    if (styleElement) {
      styleElement.textContent = cssText;
      return;
    }

    const newStyleElement = document.createElement('style');
    newStyleElement.setAttribute('data-fluent-shadow-style', '');
    newStyleElement.textContent = cssText;
    shadowRoot.appendChild(newStyleElement);
  }

  /**
   * Calls reportValidity on a custom element by reference or by id.
   * @param elementOrId The element or its ID.
   * @returns True when reportValidity is available and was called.
   */
  export function reportValidity(elementOrId: HTMLElement | string): boolean {
    let element: HTMLElement | null;
    if (typeof elementOrId === 'string') {
      element = document.getElementById(elementOrId);
    } else {
      element = elementOrId;
    }

    if (!element) {
      return false;
    }

    const reportValidityMethod = (element as any).reportValidity;
    if (typeof reportValidityMethod !== 'function') {
      return false;
    }

    return reportValidityMethod.call(element);
  }

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

      // The TextArea component uses a preConnectControlEl to set the value before the control is fully connected
      // so we need to update that as well to avoid issues with the value not being set correctly on initial render
      const preConnect = (element as any).preConnectControlEl;
      if (preConnect) {
        preConnect.value = newValue;
      }
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
