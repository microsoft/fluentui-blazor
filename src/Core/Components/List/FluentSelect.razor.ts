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
}
