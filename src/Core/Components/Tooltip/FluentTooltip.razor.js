export function tooltipHideOnCursorLeave(anchorId) {
    const srcElement = document.getElementById(anchorId);
    const tooltipSelector = `fluent-tooltip[anchor="${anchorId}"]:not([visible])`;

    if (!!srcElement) {
        srcElement.addEventListener("mouseleave", function (event) {
            const tooltip = document.querySelector(tooltipSelector);
            if (!!tooltip) {
                tooltip.style.display = "none";
            }
        });

        srcElement.addEventListener("mouseenter", function (event) {
            const tooltip = document.querySelector(tooltipSelector);
            if (!!tooltip) {
                tooltip.style.display = null;
            }
        });

    }
}
