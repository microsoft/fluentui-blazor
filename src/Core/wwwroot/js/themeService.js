export function isMobile() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}

export function switchHighlightStyle(dark) {
    if (dark) {
        document
            .querySelector(`link[title="dark"]`)
            .removeAttribute("disabled");
        document
            .querySelector(`link[title="light"]`)
            .setAttribute("disabled", "disabled");
    }
    else {
        document
            .querySelector(`link[title="light"]`)
            .removeAttribute("disabled");
        document
            .querySelector(`link[title="dark"]`)
            .setAttribute("disabled", "disabled");
    }
}
export function isDarkMode() {
    let matched = window.matchMedia("(prefers-color-scheme: dark)").matches;

    if (matched)
        return true;
    else
        return false;
}

export function switchDirection(dir) {
    document.dir = dir;
    const container = document.getElementById('container');
    container.style.direction = dir;
}