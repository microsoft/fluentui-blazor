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
  }

  interface FluentDropdownControl extends FluentUIComponents.Dropdown {
    _control: any; // Access the internal control for combobox type
  }
}