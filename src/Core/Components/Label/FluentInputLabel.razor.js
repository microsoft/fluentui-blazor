export function setInputAriaLabel(id, value) {

    const element = document.getElementById(id);

    if (!!element) {
        const inputElement = element.shadowRoot?.querySelector("input");

        // input field included in Shadow DOM
        if (!!inputElement) {
            inputElement.setAttribute("aria-label", value);
        }

        // Fluent element
        else {
            element.setAttribute("aria-label", value);
        }
    }
}