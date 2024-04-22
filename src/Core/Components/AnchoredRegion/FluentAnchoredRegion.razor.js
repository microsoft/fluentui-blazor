export function goToNextFocusableElement(forContainer, toOriginal, delay) {

    const container = typeof forContainer === "string" ? document.getElementById(forContainer) : forContainer;

    if (!!!container.focusableElement) {
        container.focusableElement = new FocusableElement(container);
    }

    // Move the focus to the inital element
    if (toOriginal === true) {
        container.focusableElement.originalActiveElement.focus();
    }

    // Move to the next element
    else {
        // Find the next focusable element
        const nextElement = container.focusableElement.findNextFocusableElement();

        // Set focus on the next element,
        // after delay to give FluentUI web components time to build up
        if (nextElement) {
            if (delay > 0) {
                container.focusableElement.setFocusAfterDelay(nextElement, delay);
            }
            else {
                nextElement.focus();
            }
        }
    }
}

/**
 * Focusable Element
 */
class FocusableElement {

    FOCUSABLE_SELECTORS = "input, select, textarea, button, object, a[href], area[href], [tabindex]";
    _originalActiveElement;
    _container;

    /**
     * Initializes a new instance of the FocusableElement class.
     */
    constructor(container) {
        this._originalActiveElement = document.activeElement;
        this._container = container;
    }

    /**
     * Gets the original document.activeElement before the focus was set to the current element.
     */
    get originalActiveElement() {
        return this._originalActiveElement;
    }

    /**
     * Find the next focusable element, after the optional current element, in the specified container.
     * @param container
     * @param currentElement
     * @returns
     */
    findNextFocusableElement(currentElement) {
        // Get all focusable elements
        const focusableElements = this._container.querySelectorAll(this.FOCUSABLE_SELECTORS);

        // Filter out elements with tabindex="-1"
        const filteredElements = Array.from(focusableElements).filter(el => el?.tabIndex !== -1);

        // Find the index of the current element
        const current = currentElement ?? document.activeElement;
        if (current != null) {
            const currentIndex = filteredElements.indexOf(current);

            // Calculate the index of the next element
            const nextIndex = (currentIndex + 1) % filteredElements.length;

            // Return the next focusable element
            return filteredElements[nextIndex];
        }

        // Not found
        return null;
    }

    /**
     * Set focus on the specified element after a delay.
     * @param element
     * @param delayInMilliseconds
     */
    setFocusAfterDelay(element, delayInMilliseconds) {
        setTimeout(() => {
            const elt = typeof element === "string" ? document.getElementById(element) : element;
            if (elt) {
                elt.focus();
            }
        }, delayInMilliseconds);
    }
}
