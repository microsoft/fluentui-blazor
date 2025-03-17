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

    if (element) {
      // Keep the original positioning value
      element.originalPositionning = element.getAttribute("positioning");

      element.addEventListener('toggle', (e: ToggleEvent) => {
        // Rollback the positionArea if the tooltip is closing
        if (e.newState != "open") {
          element.setAttribute("positioning", element.originalPositionning);
          return;
        }

        // Inverse the positionning if the tooltip is not in the viewport
        if (!isElementInViewport(element)) {
          const positionning = element.getAttribute("positioning");
          if (positionMap[positionning]) {
            element.setAttribute("positioning", positionMap[positionning]);
          }
        }
      });
    }
  }

  const positionMap: { [key: string]: string } = {
    "above-start": "below-end",
    "above": "below",
    "above-end": "below-start",
    "below-start": "above-end",
    "below": "above",
    "below-end": "above-start",
    "before-top": "after-bottom",
    "before": "after",
    "before-bottom": "after-top",
    "after-top": "before-bottom",
    "after": "before",
    "after-bottom": "before-top",
  };

  function isElementInViewport(element: HTMLElement) {
    const rect = element.getBoundingClientRect();
    return (
      rect.top >= 0 &&
      rect.left >= 0 &&
      rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
      rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
  }

}
