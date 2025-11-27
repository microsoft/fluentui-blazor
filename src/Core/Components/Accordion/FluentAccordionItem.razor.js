export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("[part='button']");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}
