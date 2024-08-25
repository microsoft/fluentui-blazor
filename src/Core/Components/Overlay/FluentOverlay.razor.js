let clickHandler;

/**
 * Initialize the global click event handler
 * @param {any} dotNetHelper
 * @param {any} id
 */
export function overlayInitialize(dotNetHelper, id) {

    if (!document.fluentOverlayData) {
        document.fluentOverlayData = {};
    }

    document.fluentOverlayData[id] = {

        // Click event handler
        clickHandler: async function (event) {
            var excludeElement = document.getElementById(id);
            if (excludeElement && !excludeElement.contains(event.target)) {
                dotNetHelper.invokeMethodAsync('OnCloseHandlerAsync', event);
            }
        }
    };

    document.addEventListener('click', document.fluentOverlayData[id].clickHandler);
}

/**
 * Dispose the global click event handler
 */
export function overlayDispose(id) {
    if (document.fluentOverlayData[id]) {
        document.removeEventListener('click', document.fluentOverlayData[id].clickHandler);
        document.fluentOverlayData[id] = null;
    }
}
