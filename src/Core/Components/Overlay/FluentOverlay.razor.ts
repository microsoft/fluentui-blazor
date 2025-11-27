export namespace Microsoft.FluentUI.Blazor.Overlay {
  export function Initialize(dotNetHelper: any, containerId: string, id: string) {
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
        const isInsideContainer = isClickInsideContainer(event, document.getElementById(containerId));
        const isInsideExcludedElement = !!document.getElementById(id) && isClickInsideContainer(event, document.getElementById(id));

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
  function isClickInsideContainer(event: MouseEvent, container: any) {
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
