// ********************
// https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
// ********************

import {
    baseLayerLuminance,
    StandardLuminance,
    neutralBaseColor,
    accentBaseColor,
    SwatchRGB
} from "./web-components-v2.5.16.min.js";

class DesignTheme extends HTMLElement {

    static observedAttributes = ["theme", "color", "appName"];

    static _DEFAULT_COLOR = "#0078D4";
    static _instanceCounter = 0;
    static _COLORS = [
        { App: "Access", Color: "#a4373a" },
        { App: "Booking", Color: "#00a99d" },
        { App: "Exchange", Color: "#0078d4" },
        { App: "Excel", Color: "#217346" },
        { App: "GroupMe", Color: "#00bcf2" },
        { App: "Office", Color: "#d83b01" },
        { App: "OneDrive", Color: "#0078d4" },
        { App: "OneNote", Color: "#7719aa" },
        { App: "Outlook", Color: "#0f6cbd" },
        { App: "Planner", Color: "#31752f" },
        { App: "PowerApps", Color: "#742774" },
        { App: "PowerBI", Color: "#f2c811" },
        { App: "PowerPoint", Color: "#b7472a" },
        { App: "Project", Color: "#31752f" },
        { App: "Publisher", Color: "#077568" },
        { App: "SharePoint", Color: "#0078d4" },
        { App: "Skype", Color: "#0078d4" },
        { App: "Stream", Color: "#bc1948" },
        { App: "Sway", Color: "#008272" },
        { App: "Teams", Color: "#6264a7" },
        { App: "Visio", Color: "#3955a3" },
        { App: "Windows", Color: "#0078d4" },
        { App: "Word", Color: "#2b579a" },
        { App: "Yamme", Color: "#106ebe" },
        { App: "Word", Color: "" },
    ];

    _isInternalChange = false;

    BaseLayerLuminance = baseLayerLuminance;
    StandardLuminance = StandardLuminance;
    NeutralBaseColor = neutralBaseColor;
    AccentBaseColor = accentBaseColor;

    constructor() {
        super();
    }

    /**
     * Gets the current theme attribute value.
     */
    get theme() {
        return this.getAttribute('theme');
    }

    /**
     * Sets the current theme attribute value.
     */
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

    /**
    * Gets the current color attribute value.
    */
    get color() {
        return this.getAttribute('color');
    }

    /**
     * Sets the current color attribute value.
     */
    set color(value) {
        this.updateAttribute("color", value);
        this.applyColor(value);

        const appName = DesignTheme._COLORS.find(item => item.Color.toLowerCase() === value.toLowerCase())?.App;
        this.updateAttribute("appName", appName);
    }

    /**
    * Gets the current Microsoft application name
    */
    get appName() {
        return this.getAttribute('appName');
    }

    /**
     * Sets the current color based on a Microsoft Application name.
     * Access, Booking, Exchange, Excel, GroupMe, Office, OneDrive, OneNote, Outlook, 
     * Planner, PowerApps, PowerBI, PowerPoint, Project, Publisher, SharePoint, Skype, 
     * Stream, Sway, Teams, Visio, Windows, Word, Yammer
     */
    set appName(value) {
        this.updateAttribute("appName", value);

        const color = DesignTheme._COLORS.find(item => item.App.toLowerCase() === value.toLowerCase())?.Color ?? DesignTheme._DEFAULT_COLOR;
        this.color = color;
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

            case "color":
                this.color = newValue;
                break;

            case "appName":
                this.appName = newValue;
                break;
        }
    }

    applyColor(color) {
        const rgb = this.parseColorHexRGB(color ?? DesignTheme._DEFAULT_COLOR);
        const swatch = SwatchRGB.from(rgb);
        this.AccentBaseColor.withDefault(swatch);
    }

    updateAttribute(name, value) {
        this._isInternalChange = true;

        if (name != value) {
            if (value) {
                this.setAttribute(name, value);
            } else {
                this.removeAttribute(name);
            }
        }

        this._isInternalChange = false;
    }

    /**
     * See https://github.com/microsoft/fast -> packages/utilities/fast-colors/src/parse-color.ts
     */
    parseColorHexRGB(raw) {
        // Matches #RGB and #RRGGBB, where R, G, and B are [0-9] or [A-F]
        const hexRGBRegex = /^#((?:[0-9a-f]{6}|[0-9a-f]{3}))$/i;
        const result = hexRGBRegex.exec(raw);

        if (result === null) {
            return null;
        }

        let digits = result[1];

        if (digits.length === 3) {
            const r = digits.charAt(0);
            const g = digits.charAt(1);
            const b = digits.charAt(2);

            digits = r.concat(r, g, g, b, b);
        }

        const rawInt = parseInt(digits, 16);

        if (isNaN(rawInt)) {
            return null;
        }

        return {
            r: this.normalize((rawInt & 0xff0000) >>> 16, 0, 255),
            g: this.normalize((rawInt & 0x00ff00) >>> 8, 0, 255),
            b: this.normalize(rawInt & 0x0000ff, 0, 255),
        }
    }

    /**
     * Scales an input to a number between 0 and 1
     */
    normalize(i, min, max) {
        if (isNaN(i) || i <= min) {
            return 0.0;
        } else if (i >= max) {
            return 1.0;
        }
        return i / (max - min);
    }
}

export { DesignTheme };