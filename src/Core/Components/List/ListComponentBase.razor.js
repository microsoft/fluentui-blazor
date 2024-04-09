export function getAriaActiveDescendant(id) {
    if (document.getElementById(id)) {
        return document.getElementById(id).getAttribute("aria-activedescendant")
    }
}