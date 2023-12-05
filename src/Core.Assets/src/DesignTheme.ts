// ********************
// https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
// ********************


import {
    baseLayerLuminance,
    StandardLuminance,
    neutralBaseColor,
    accentBaseColor,
    SwatchRGB
} from "@fluentui/web-components/dist/web-components";

class DesignTheme extends HTMLElement {

    private _isColorSchemeChangedRegistered: boolean = false;
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

    constructor() {
        super();
    }

    /**
     * Gets the current mode (dark/light) attribute value.
     */
    get mode(): string | null {
        return this.getAttribute("mode");
    }

    /**
     * Sets the current mode (dark/light) attribute value.
     */
    set mode(value: string | null) {
        this.updateAttribute("mode", value);

        switch (value?.toLowerCase()) {
            // Dark mode - Luminance = 0.15
            case "dark":
                baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
                break;

            // Light mode - Luminance = 0.98
            case "light":
                baseLayerLuminance.withDefault(StandardLuminance.LightMode);
                break;

            // System mode
            default:
                const isDark = window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
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
    * Gets the current color or office name attribute value.
    * Access, Booking, Exchange, Excel, GroupMe, Office, OneDrive, OneNote, Outlook, 
    * Planner, PowerApps, PowerBI, PowerPoint, Project, Publisher, SharePoint, Skype, 
    * Stream, Sway, Teams, Visio, Windows, Word, Yammer
    */
    get primaryColor(): string | null {
        return this.getAttribute("primary-color");
    }

    /**
     * Sets the current color or office name attribute value.
     */
    set primaryColor(value: string | null) {
        this.updateAttribute("primary-color", value);

        if (value != null && value.startsWith("#")) {
            this.applyColor(value);
        }
        else {
            this.applyOffice(value);
        }
    }

     /**
     * Gets the current local-storage key, to persist the theme/color between sessions.
     */
    get localStorage(): string | null {
        return this.getAttribute("local-storage");
    }

    /**
     * Sets the current local-storage key, to persist the theme/color between sessions.
     */
    set localStorage(value: string | null) {
        this.updateAttribute("local-storage", value);
    }

    // Custom element added to page.
    connectedCallback() {
        DesignTheme._instanceCounter++;

        if (DesignTheme._instanceCounter > 1) {
            throw new Error("fluent-design-theme (DesignTheme) can only be used once.");
        }

        // Detect system theme changing
        if (!this._isColorSchemeChangedRegistered) {
            window.matchMedia("(prefers-color-scheme: dark)")
                .addEventListener("change", e => this.colorSchemeChanged(e));
        }
        
        // Load from LocalStorage
        this.readLocalStorage();

        this._isColorSchemeChangedRegistered = true;
    }

    // Custom element removed from page.
    disconnectedCallback() {
        this._isColorSchemeChangedRegistered = false;
        DesignTheme._instanceCounter--;
    }

    // Custom element moved to new page.
    adoptedCallback() {
    }

    // Attributes to observe
    static get observedAttributes() {
        return ["mode", "primary-color", "local-storage"];
    }

    // Attributes has changed.
    attributeChangedCallback(name: string, oldValue: string, newValue: string) {

        if (this._isInternalChange) {
            return;
        }

        switch (name) {
            case "mode":
                this.dispatchAttributeChanged(name, oldValue, newValue);
                this.mode = newValue;
                break;

            case "primary-color":
                this.primaryColor = newValue;
                break;

            case "local-storage":
                this.localStorage = newValue;
                break;
        }
    }

    private isSystemDark(): boolean {
        return (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches);
    }

    private colorSchemeChanged(e: MediaQueryListEvent) {
        if (!this._isColorSchemeChangedRegistered) {
            return;
        }

        const currentMode = this.getAttribute("mode");

        // Only if the DesignTheme.Mode = 'System' (null)
        // If not, the dev already "forced" the mode to "dark" or "light"
        if (currentMode == null) {

            // Dark
            if (e.matches) {
                this.dispatchAttributeChanged("mode", currentMode, "system-dark");
                baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
            }

            // Light
            else {
                this.dispatchAttributeChanged("mode", currentMode, "system-light");
                baseLayerLuminance.withDefault(StandardLuminance.LightMode);
            }
        }
    }

    private dispatchAttributeChanged(name: string, oldValue: string | null, newValue: string | null): void {
        if (oldValue !== newValue) {

            if (name === "mode") {
                newValue = newValue ?? (this.isSystemDark() ? "system-dark" : "system-light");
            }

            console.log(`dispatchAttributeChanged ${name}: ${oldValue} -> ${newValue}`);
            this.dispatchEvent(
                new CustomEvent("onchange", {
                    bubbles: true,
                    detail: {
                        name: name,
                        oldValue: oldValue,
                        newValue: newValue,
                    },
                }),
            );
        }
    }

    private applyOffice(name: string | null): void {
        const color = DesignTheme._COLORS.find(item => item.App.toLowerCase() === name?.toLowerCase())?.Color;
        this.applyColor(color ?? DesignTheme._DEFAULT_COLOR);
    }

    private applyColor(color: string | null): void {
        const rgb = this.parseColorHexRGB(color ?? DesignTheme._DEFAULT_COLOR);
        if (rgb != null) {
            const swatch = SwatchRGB.from(rgb);
            accentBaseColor.withDefault(swatch);
        }
    }

    private updateAttribute(name: string, value: string | null): void {
        this._isInternalChange = true;

        if (this.getAttribute(name) != value) {
            if (value) {
                this.setAttribute(name, value);
            } else {
                this.removeAttribute(name);
            }
        }

        this.updateLocalStorage();

        this._isInternalChange = false;
    }

    /**
     * See https://github.com/microsoft/fast -> packages/utilities/fast-colors/src/parse-color.ts
     */
    private parseColorHexRGB(raw: string): ColorRGB | null {
        // Matches #RGB and #RRGGBB, where R, G, and B are [0-9] or [A-F]
        const hexRGBRegex: RegExp = /^#((?:[0-9a-f]{6}|[0-9a-f]{3}))$/i;
        const result: string[] | null = hexRGBRegex.exec(raw);

        if (result === null) {
            return null;
        }

        let digits: string = result[1];

        if (digits.length === 3) {
            const r: string = digits.charAt(0);
            const g: string = digits.charAt(1);
            const b: string = digits.charAt(2);

            digits = r.concat(r, g, g, b, b);
        }

        const rawInt: number = parseInt(digits, 16);

        if (isNaN(rawInt)) {
            return null;
        }

        return new ColorRGB(
            this.normalized((rawInt & 0xff0000) >>> 16, 0, 255),
            this.normalized((rawInt & 0x00ff00) >>> 8, 0, 255),
            this.normalized(rawInt & 0x0000ff, 0, 255),
        );
    }

    /**
     * Scales an input to a number between 0 and 1
     */
    private normalized(i: number, min: number, max: number): number {
        if (isNaN(i) || i <= min) {
            return 0.0;
        } else if (i >= max) {
            return 1.0;
        }
        return i / (max - min);
    }

    private updateLocalStorage() {

        // connectedCallback not yet called
        if (!this._isColorSchemeChangedRegistered) {
            return;
        }

        // Save if localStorage is set
        if (this.localStorage != null) {
            const theme = {
                mode: this.mode,
                primaryColor: this.primaryColor,
            }
            localStorage.setItem(this.localStorage, JSON.stringify(theme));            
        }
    }

    public readLocalStorage() {
        if (this.localStorage != null) {
            const theme = JSON.parse(localStorage.getItem(this.localStorage));

            this.dispatchAttributeChanged("mode", this.mode, theme?.mode);

            this.mode = theme?.mode;
            this.primaryColor = theme?.primaryColor;

            return theme;
        }

        return null;
    }
}

class ColorRGB {
    constructor(red: number, green: number, blue: number) {
        this.r = red;
        this.g = green;
        this.b = blue;
    }

    public readonly r: number;
    public readonly g: number;
    public readonly b: number;
}

export { DesignTheme };