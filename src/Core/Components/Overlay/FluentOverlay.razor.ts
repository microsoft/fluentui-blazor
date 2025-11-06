/**
 * Initialize the global click event handler
 * @param {any} dotNetHelper
 * @param {any} id
 */
export function overlayInitialize(dotNetHelper, containerId, id) {

    if (!document.fluentOverlayData) {
        document.fluentOverlayData = {};
    }

    if (document.fluentOverlayData[id]) {
        return;
    }

    // Store the data
    document.fluentOverlayData[id] = {

        // Click event handler
        clickHandler: async function (event) {
            const isInsideContainer = isClickInsideContainer(event, document.getElementById(containerId));
            const isInsideExcludedElement = !!document.getElementById(id) && isClickInsideContainer(event, document.getElementById(id));

            if (isInsideContainer && !isInsideExcludedElement) {
                dotNetHelper.invokeMethodAsync('OnCloseInteractiveAsync', event); 
            }
        }
    };

    // Let the user click on the container (containerId or the entire document)
    document.addEventListener('click', document.fluentOverlayData[id].clickHandler);
}

/**
 * Dispose the global click event handler
 */
export function overlayDispose(id) {
    if (document.fluentOverlayData[id]) {

        // Remove the event listener
        document.removeEventListener('click', document.fluentOverlayData[id].clickHandler);

        // Remove the data
        document.fluentOverlayData[id] = null;
        delete document.fluentOverlayData[id];
    }
}

/**
 * Determines whether a mouse click event occurred inside a specific HTML element.
 */
function isClickInsideContainer(event, container) {
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
