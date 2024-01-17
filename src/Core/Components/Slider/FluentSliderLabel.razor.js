export function updateSliderLabel(id) {

    const element = document.getElementById(id);

    if (!!element) {
        const labelElement = element.shadowRoot?.querySelector(".label");

        // label included in Shadow DOM
        if (!!labelElement) {
            labelElement.style.maxWidth = "unset";
        }
    }
}