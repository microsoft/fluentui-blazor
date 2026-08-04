import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Components.Select {

  /**
   * Clear the value of the select component with the specified ID.
  */
  export function ClearValue(id: string) {
    const element = document.getElementById(id) as FluentUIComponents.Dropdown;
    if (element) {
      element.value = null;
    }
  }

  /**
   * Initializes the select component with the specified ID.   
   * @param id
   * @param defaultValue
   */
  export function Initialize(id: string, defaultValue: string) {
    const element = document.getElementById(id) as FluentDropdownControl;
    if (!element) {
      return;
    }

    // By default, the combobox text is not bound to the Value property.
    if (element.getAttribute('type') === 'combobox' && element.tagName === 'FLUENT-DROPDOWN' && element._control) {
      element._control.value = defaultValue;
    }

    // Accessibility: Set the aria-label and aria-expanded attributes for the button element if they are not already set.
    const controlElement = element.querySelector('button[slot=control], input[slot=control]') as HTMLButtonElement | HTMLInputElement | null;
    if (controlElement) {
      if (!controlElement.hasAttribute('aria-label')) {
        controlElement.setAttribute('aria-label', getAccessibleLabel(element));
      }
      if (!controlElement.hasAttribute('aria-expanded')) {
        controlElement.setAttribute('aria-expanded', 'false');
      }
    }
  }

  /**
   * Resolves the accessible label: the label of the parent fluent-field, then the placeholder, then a default text.
   */
  function getAccessibleLabel(element: HTMLElement): string {
    const label = element.closest('fluent-field')?.querySelector('label');
    if (label?.textContent?.trim()) {
      return label.textContent.trim();
    }

    const placeholder = element.getAttribute('placeholder');
    if (placeholder?.trim()) {
      return placeholder.trim();
    }

    return 'Select an option';
  }

  interface FluentDropdownControl extends FluentUIComponents.Dropdown {
    _control: any; // Access the internal control for combobox type
  }
}