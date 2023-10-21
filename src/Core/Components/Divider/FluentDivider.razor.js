export function setDividerAriaOrientation() {

    const elements = document.querySelectorAll("fluent-divider[role='presentation']");

    if (!!elements) {
        elements.forEach((element, i, array) => {
            if (!!element && element.hasAttribute("aria-orientation")) {
                element.removeAttribute("aria-orientation");
            }
        });
    }
}