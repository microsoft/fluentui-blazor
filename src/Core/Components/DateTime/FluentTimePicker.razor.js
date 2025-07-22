export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.getElementById(id)?.shadowRoot?.querySelector(".control");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}
