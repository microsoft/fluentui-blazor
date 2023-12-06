// ********************
// https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
// ********************

import { ColorsUtils } from "./Design/ColorsUtils";
import {
    baseLayerLuminance,
    StandardLuminance,
    neutralBaseColor,
    accentBaseColor,
    SwatchRGB
} from "@fluentui/web-components/dist/web-components";
import { ThemeStorage } from "./Design/ThemeStorage";


class DesignTheme extends HTMLElement {

    public _isInitialized: boolean = false;
    private _themeStorage: ThemeStorage;

    _isInternalChange = false;

    constructor() {
        super();
        this._themeStorage = new ThemeStorage(this);
    }

    /**
     * Gets the ThemeStorage
     */
    get themeStorage(): ThemeStorage {
        return this._themeStorage;
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

        console.log(` ** Set "mode" = "${value}"`)

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

        // Convert the OfficeColor to an HEX color
        const color = value == null || !value.startsWith("#")
            ? ColorsUtils.getHexColor(value)
            : value;

        // Apply the color
        const rgb = ColorsUtils.parseColorHexRGB(color);
        if (rgb != null) {
            const swatch = SwatchRGB.from(rgb);
            accentBaseColor.withDefault(swatch);
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
        // Detect system theme changing
        window.matchMedia("(prefers-color-scheme: dark)")
            .addEventListener("change", this.colorSchemeListener);

        // Load from LocalStorage
        if (this.localStorage != null) {
            const theme = this._themeStorage.readLocalStorage();

            if (theme != null) {
                this.attributeChangedCallback("mode", this.mode, theme.mode);
                this.attributeChangedCallback("primary-color", this.primaryColor, theme.primaryColor);
            }
        }

        this._isInitialized = true;

        // Default System mode
        if (this.mode == null) {
            this.colorSchemeListener(new MediaQueryListEvent("change", { matches: ColorsUtils.isSystemDark() }));
        }
    }

    // Custom element removed from page.
    disconnectedCallback() {
        this._isInitialized = false;

        window.matchMedia("(prefers-color-scheme: dark)")
            .removeEventListener("change", this.colorSchemeListener);
    }

    // Custom element moved to new page.
    adoptedCallback() {
    }

    // Attributes to observe
    static get observedAttributes() {
        return ["mode", "primary-color", "local-storage"];
    }

    // Attributes has changed.
    attributeChangedCallback(name: string, oldValue: string | null, newValue: string | null) {

        if (this._isInternalChange) {
            return;
        }

        if (oldValue === newValue) {
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

    private colorSchemeListener = (e: MediaQueryListEvent) => {
        if (!this._isInitialized) {
            return;
        }

        const currentMode = this.getAttribute("mode");

        // Only if the DesignTheme.Mode = 'System' (null)
        // If not, the dev already "forced" the mode to "dark" or "light"
        if (currentMode == null) {

            console.log(` ** colorSchemeListener = "${currentMode}"`)

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

    public dispatchAttributeChanged(name: string, oldValue: string | null, newValue: string | null): void {
        if (oldValue !== newValue) {

            if (name === "mode") {
                newValue = newValue ?? (ColorsUtils.isSystemDark() ? "system-dark" : "system-light");
            }

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

    private updateAttribute(name: string, value: string | null): void {
        this._isInternalChange = true;

        if (this.getAttribute(name) != value) {
            if (value) {
                this.setAttribute(name, value);
            } else {
                this.removeAttribute(name);
            }
        }

        if (this.localStorage != null) {
            this._themeStorage.updateLocalStorage(this.mode, this.primaryColor);
        }

        this._isInternalChange = false;
    }
}

export { DesignTheme };