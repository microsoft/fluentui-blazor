export function addAriaHidden(id) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".clear-button");

    if (!!fieldElement) {
        fieldElement?.setAttribute("aria-hidden", "true");
    }
}

export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("#control");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}
