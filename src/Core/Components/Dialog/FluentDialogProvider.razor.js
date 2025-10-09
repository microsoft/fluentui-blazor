export function getActiveElement() {
    return document.activeElement
}

export function focusElement(element) {
    if (!!element) {
        element.focus();
    }
}
