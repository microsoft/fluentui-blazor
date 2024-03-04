export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".selected-value");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}
