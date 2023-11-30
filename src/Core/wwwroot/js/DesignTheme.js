// ********************
// https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
// ********************

import {
    baseLayerLuminance,
    StandardLuminance,
    neutralBaseColor,
    accentBaseColor,
} from "./web-components-v2.5.16.min.js";

class DesignTheme extends HTMLElement {

    static observedAttributes = ["theme"];
    static _instanceCounter = 0;

    _isInternalChange = false;

    BaseLayerLuminance = baseLayerLuminance;
    StandardLuminance = StandardLuminance;
    NeutralBaseColor = neutralBaseColor;
    AccentBaseColor = accentBaseColor;

    constructor() {
        super();
    }

    get theme() {
        return this.getAttribute('theme');
    }

    set theme(value) {
        this.updateAttribute("theme", value);

        switch (value) {
            case "dark":
                baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
                break;

            case "light":
                baseLayerLuminance.withDefault(StandardLuminance.LightMode);
                break;

            default:
                const isDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
                if (isDark) {
                    baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
                }
                else {
                    baseLayerLuminance.withDefault(StandardLuminance.LightMode);
                }
                break;
        }
    }

    // Custom element added to page.
    connectedCallback() {
        DesignTheme._instanceCounter++;

        if (DesignTheme._instanceCounter > 1) {
            throw new Error("fluent-design-theme (DesignTheme) can only be used once.");
        }

        // Detect system theme changing
        window.matchMedia("(prefers-color-scheme: dark)")
            .addEventListener("change", ({ matches }) => {
                if (matches) {
                    this.theme = "dark";
                } else {
                    this.theme = "light";
                }
            })
    }

    // Custom element removed from page.
    disconnectedCallback() {
        DesignTheme._instanceCounter--;
    }

    // Custom element moved to new page.
    adoptedCallback() {
    }

    // Attribute "name" has changed.
    attributeChangedCallback(name, oldValue, newValue) {

        if (this._isInternalChange) {
            return;
        }

        switch (name) {
            case "theme":
                this.theme = newValue;
                break;
        }
    }

    updateAttribute(name, value) {
        this._isInternalChange = true;

        if (value) {
            this.setAttribute(name, value);
        } else {
            this.removeAttribute(name);
        }

        this._isInternalChange = false;
    }
}

export { DesignTheme };