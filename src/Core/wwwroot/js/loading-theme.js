// To avoid Flash of Unstyled Content, the body is hidden.
// Here we'll find the first web component and wait for it to be upgraded.
// When it is, we'll remove this visibility from the body.

class LoadingTheme extends HTMLElement {

    className = "hidden-body";
    defaultDarkColor = "#272727";
    defaultLightColor = "#fbfbfb";

    constructor() {
        super();
    }

    // Attributes to observe
    static observedAttributes = ["mode", "storage-name"];

    // Custom element added to page.
    connectedCallback() {

        // Attributes
        const storageName = this.getAttribute("storage-name");
        const mode = this.getAttribute("mode");

        // Compute the saved or the system theme (dark/light).
        const isDarkSaved = (mode ?? JSON.parse(localStorage.getItem(storageName))?.mode) === "dark";
        const isSystemDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        const bgColor = isDarkSaved || isSystemDark ? this.defaultDarkColor : this.defaultLightColor;

        console.log("LoadingTheme", { storageName, mode, isDarkSaved, isSystemDark, bgColor });

        // Create a ".hidden-unstyled-body" class
        // where the background-color is dark or light.
        var css = `.${this.className} { visibility: hidden; background-color: ${bgColor}; }`;

        // Add a <style> element to the <head> element
        const head = document.head || document.getElementsByTagName('head')[0];
        const style = document.createElement('style');

        head.appendChild(style);
        style.appendChild(document.createTextNode(css));

        document.body.classList.add(this.className);

        // Add a <fluent-design-theme storage-name="{<theme>}" /> element
        const designTheme = document.createElement("fluent-design-theme");
        if (mode) designTheme.setAttribute("mode", mode);
        if (storageName) designTheme.setAttribute("storage-name", storageName);
        this.appendChild(designTheme);

        // Wait for the fluentui web components to be loaded
        // and to remove the className to show the <body> content.
        customElements.whenDefined("fluent-button").then(() => {
            document.body.classList.remove(this.className);
        });
    }

    // Attributes has changed.
    attributeChangedCallback(name, oldValue, newValue) {

    }
}

customElements.define("loading-theme", LoadingTheme);