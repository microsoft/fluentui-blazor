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

let matched = window.matchMedia('(prefers-color-scheme: dark)').matches;

if (matched) {
    window.DefaultBaseLayerLuminance = 0.23;
    document.querySelector(`link[title="dark"]`).removeAttribute("disabled");
    document.querySelector(`link[title="light"]`).setAttribute("disabled", "disabled");
} else {
    window.DefaultBaseLayerLuminance = 1.0;
    document.querySelector(`link[title="light"]`).removeAttribute("disabled");
    document.querySelector(`link[title="dark"]`).setAttribute("disabled", "disabled");
}
