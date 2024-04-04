export function goToNextFocusableElement(id) {

    const container = document.getElementById(id);

    // Get the currently focused element
    const currentElement = document.activeElement;

    // Find the next focusable element
    const nextElement = findNextFocusableElement(container, currentElement);

    // Set focus on the next element
    if (nextElement) {
        nextElement.focus();
    }
}

function findNextFocusableElement(container, currentElement) {
    // Get all focusable elements
    const focusableElements = container.querySelectorAll(
        "input, select, textarea, button, object, a[href], area[href], [tabindex], fluent-checkbox"
    );

    // Filter out elements with tabindex="-1"
    const filteredElements = Array.from(focusableElements).filter(
        (el) => el.tabIndex !== -1
    );

    // Find the index of the current element
    const currentIndex = filteredElements.indexOf(currentElement);

    // Calculate the index of the next element
    const nextIndex = (currentIndex + 1) % filteredElements.length;

    // Return the next focusable element
    return filteredElements[nextIndex];
}

function setFocusAfterDelay(elementId, delayInMilliseconds) {
    setTimeout(() => {
        const element = document.getElementById(elementId);
        if (element) {
            element.focus();
        }
    }, delayInMilliseconds);
}
