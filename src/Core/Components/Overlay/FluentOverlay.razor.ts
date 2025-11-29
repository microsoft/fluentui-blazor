import { DotNet } from "../../../Core.Scripts/src/d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Overlay {
  export function Initialize(dotNetHelper: DotNet.DotNetObject, containerId: string, id: string) {
    const _document = document as any;

    if (!_document.fluentOverlayData) {
      _document.fluentOverlayData = {};
    }

    if (_document.fluentOverlayData[id]) {
      return;
    }

    // Store the data
    _document.fluentOverlayData[id] = {

      // Click event handler
      clickHandler: async function (event: MouseEvent) {
        const containerElement = document.getElementById(containerId) as HTMLElement;
        const isInsideContainer = isClickInsideContainer(event, containerElement);
        const isInsideExcludedElement = !!containerElement && isClickInsideContainer(event, containerElement);

        if (isInsideContainer && !isInsideExcludedElement) {
          dotNetHelper.invokeMethodAsync('OnCloseInteractiveAsync', event);
        }
      }
    };

    // Let the user click on the container (containerId or the entire document)
    document.addEventListener('click', _document.fluentOverlayData[id].clickHandler);
  }
  export function overlayDispose(id: string) {
    const _document = document as any;
    if (_document.fluentOverlayData[id]) {

      // Remove the event listener
      document.removeEventListener('click', _document.fluentOverlayData[id].clickHandler);

      // Remove the data
      _document.fluentOverlayData[id] = null;
      delete _document.fluentOverlayData[id];
    }
  }
  function isClickInsideContainer(event: MouseEvent, container: HTMLElement) {
    if (!!container) {
      const rect = container.getBoundingClientRect();

      return (
        event.clientX >= rect.left &&
        event.clientX <= rect.right &&
        event.clientY >= rect.top &&
        event.clientY <= rect.bottom
      );
    }

    // Default is true
    return true;
  }
}
