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
}
