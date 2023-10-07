export function setInputAriaLabel(id, value) {

    const element = document.getElementById(id);

    if (!!element) {
        const inputElement = element.shadowRoot?.querySelector("input");

        // input field included in Shadow DOM
        if (!!inputElement) {
            inputElement.setAttribute("aria-label", value);
        }

        // fluent-select
        else if (element.nodeName === "FLUENT-SELECT") {
            element.setAttribute("aria-label", value);
        }
    }
}