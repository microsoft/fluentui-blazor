import {
    baseLayerLuminance,
    StandardLuminance
} from "/_content/Microsoft.FluentUI.AspNetCore.Components/js/web-components-v2.5.16.min.js";

const currentThemeCookieName = "currentTheme";
const themeSettingSystem = "System";
const themeSettingDark = "Dark";
const themeSettingLight = "Light";

/**
 * Returns the current system theme (Light or Dark)
 * @returns {string}
 */
export function getSystemTheme() {
    let matched = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (matched) {
        return themeSettingDark;
    } else {
        return themeSettingLight;
    }
}

/**
 * Sets the currentTheme cookie to the specified value.
 * @param {string} theme
 */
export function setThemeCookie(theme) {
    document.cookie = `${currentThemeCookieName}=${theme}`;
}

/**
 * Returns the value of the currentTheme cookie, or System if the cookie is not set.
 * @returns {string}
 */
export function getThemeCookieValue() {
    return getCookieValue(currentThemeCookieName) ?? themeSettingSystem;
}

export function switchDirection(dir) {
    document.dir = dir;
    const container = document.getElementById('container');
    container.style.direction = dir;
}

export function switchHighlightStyle(dark) {
    if (dark) {
        document.querySelector(`link[title="dark"]`).removeAttribute("disabled");
        document.querySelector(`link[title="light"]`).setAttribute("disabled", "disabled");
    }
    else {
        document.querySelector(`link[title="light"]`).removeAttribute("disabled");
        document.querySelector(`link[title="dark"]`).setAttribute("disabled", "disabled");
    }
}

/**
 * Returns the value of the specified cookie, or the empty string if the cookie is not present
 * @param {string} cookieName
 * @returns {string}
 */
function getCookieValue(cookieName) {
    const cookiePieces = document.cookie.split(';');
    for (let index = 0; index < cookiePieces.length; index++) {
        if (cookiePieces[index].trim().startsWith(cookieName)) {
            const cookieKeyValue = cookiePieces[index].split('=');
            if (cookieKeyValue.length > 1) {
                return cookieKeyValue[1];
            }
        }
    }

    return "";
}

function setInitialBaseLayerLuminance() {
    let theme = getThemeCookieValue();

    if (!theme || theme === themeSettingSystem) {
        theme = getSystemTheme();
    }

    if (theme === themeSettingDark) {
        baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
    } else /* Light */ {
        baseLayerLuminance.withDefault(StandardLuminance.LightMode);
    }
}

setInitialBaseLayerLuminance();