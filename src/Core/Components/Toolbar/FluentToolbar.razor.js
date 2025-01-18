// Prevent fluent-toolbar from overriding arrow key functionality in input fields
const handlers = new Map();

export function preventArrowKeyNavigation(id) {
    const toolbar = document.querySelector("#" + id);
    if (!toolbar) return;

    const arrowKeyListener = (event) => {
        if (event.key === 'ArrowLeft' || event.key === 'ArrowRight') {
            const activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === 'FLUENT-SEARCH'
                || activeElement.tagName === 'FLUENT-TEXT-FIELD'
                || activeElement.tagName === 'FLUENT-NUMBER-FIELD'
                || activeElement.tagName === 'FLUENT-TEXT-AREA')) {

                const textLength = activeElement.value?.length || 0;
                if (textLength > 0) {
                    event.stopPropagation();
                }
            }
        }
    };

    toolbar.addEventListener('keydown', arrowKeyListener, true);
    handlers.set(id, { toolbar, arrowKeyListener });
}

export function removePreventArrowKeyNavigation(id) {
    const handler = handlers.get(id);
    if (handler) {
        const { toolbar, arrowKeyListener } = handler;

        if (toolbar) {
            toolbar.removeEventListener('keydown', arrowKeyListener, true);
        }

        handlers.delete(id);
    }
}
