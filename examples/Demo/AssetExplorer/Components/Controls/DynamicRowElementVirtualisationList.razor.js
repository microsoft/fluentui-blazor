export function getElementProperty(element, property) {
    if (element) {
        return element[property];
    }
    return null;
}
