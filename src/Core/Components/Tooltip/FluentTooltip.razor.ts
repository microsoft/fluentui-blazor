export namespace Microsoft.FluentUI.Blazor.Tooltip {

  /**
   * To update when the FluentUI Web Component team will fix this issue:
   * 
   * This method is called when the component is rendered.
   * This bypasses the problem of creating the web component before the DOM is ready.
   */

  export function FluentTooltipInitialize(id: string) {
    const element = document.getElementById(id) as any;
    if (element && element.anchorElement && typeof element.connectedCallback === "function") {
      element.connectedCallback();
    }
  }
}
