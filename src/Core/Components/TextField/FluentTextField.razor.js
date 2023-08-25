export function setAutocomplete(id, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("#control");

    if (!!fieldElement) {
        fieldElement?.setAttribute("autocomplete", value);
    }
}