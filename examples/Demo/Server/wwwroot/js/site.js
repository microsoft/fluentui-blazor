// To avoid Flash of Unstyled Content, the body is hidden by default with
// the before-started CSS class. Here we'll find the first web component
// and wait for it to be upgraded. When it is, we'll remove that class
// from the body. 

const firstUndefinedElement = document.body.querySelector(":not(:defined)");

if (firstUndefinedElement) {
    customElements.whenDefined(firstUndefinedElement.localName).then(() => {
        document.body.classList.remove("before-started");
    });
}
var DefaultBaseLayerLuminance = function () {
    let matched = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (matched) {
        return 0.23;
    } else {
        return 1.0;
    }
}