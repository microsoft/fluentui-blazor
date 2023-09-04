export function addAriaHidden(id) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".clear-button");

    if (!!fieldElement) {
        fieldElement?.setAttribute("aria-hidden", "true");
    }
}