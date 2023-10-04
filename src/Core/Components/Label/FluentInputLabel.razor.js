export function setInputAriaLabel(id, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("input");

    console.log(id, value);
    console.log(fieldElement);

    if (!!fieldElement) {
        fieldElement?.setAttribute("aria-label", value);
    }
}