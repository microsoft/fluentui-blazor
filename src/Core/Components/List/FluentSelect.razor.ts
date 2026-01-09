export namespace Microsoft.FluentUI.Blazor.Select {

  /**
   * Clear the value of the select component with the specified ID.
  */
  export function ClearValue(id: string) {
    const element = document.getElementById(id) as any;
    if (element) {
      element.value = null;
    }
  }

  /**
   * For the combo box, set the value with the specified ID.
   * @param id
   * @param value
   */
  export function SetComboBoxValue(id: string, value: string) {
    const element = document.getElementById(id) as any;
    if (element && element.tagName === 'FLUENT-DROPDOWN' && element._control) {
      element._control.value = value;
    }
  }

  /**
   * Fluent-specific helper:
   * Retrieves the selected option value from a fluent-option.
   * @param id
   */
  export function GetSelectedValue(id: string): string | null {
    const element = document.getElementById(id) as any;

    if (!element) return null;

    if (element.tagName === 'FLUENT-OPTION' && element.value !== undefined) {
      return element.value?.toString() ?? null;
    }

    return null;
  }
}
