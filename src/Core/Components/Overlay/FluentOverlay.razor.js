/**
 * Initialize the global click event handler
 * @param {any} dotNetHelper
 * @param {any} id
 */
export function overlayInitialize(dotNetHelper, id) {

    if (!document.fluentOverlayData) {
        document.fluentOverlayData = {};
    }

    if (document.fluentOverlayData[id]) {
        return;
    }

    document.fluentOverlayData[id] = {

        // Click event handler
        clickHandler: async function (event) {
            const excludeElement = document.getElementById(id);
            const isExcludeElement = excludeElement && excludeElement.contains(event.target);

            if (!isExcludeElement) {
                dotNetHelper.invokeMethodAsync('OnCloseInteractiveAsync', event);
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
        delete document.fluentOverlayData[id];
    }
}
