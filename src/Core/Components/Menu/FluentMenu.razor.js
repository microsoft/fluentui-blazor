// Add Left click event to open the FluentMenu
export function addEventLeftClick(id, dotNetHelper) {
    var item = document.getElementById(id);
    if (!!item) {
        item.addEventListener("click", function(e) {
            dotNetHelper.invokeMethodAsync('OpenAsync', window.innerWidth, window.innerHeight, e.clientX, e.clientY);
        });
    }
}

// Add Right click event to open the FluentMenu
export function addEventRightClick(id, dotNetHelper) {
    var item = document.getElementById(id);
    if (!!item) {
        item.addEventListener('contextmenu', function (e) {
            e.preventDefault();
            dotNetHelper.invokeMethodAsync('OpenAsync', window.innerWidth, window.innerHeight, e.clientX, e.clientY);
            return false;
        }, false);
    }
}

// Must use an animation frame to ensure the DOM is fully updated before checking the element's
// attributes to prevent stale or inconsistent reads.
export function isChecked(menuItemId) {
    return new Promise(resolve => {
        requestAnimationFrame(() => {
            const menuItemElement = document.getElementById(menuItemId);
            if (!menuItemElement) return resolve(false);
            resolve(menuItemElement.hasAttribute("checked") || menuItemElement.getAttribute("aria-checked") === "true");
        });
    });
}

const menuState = new Map();

export async function initialize(anchorId, menuId, menuOpen, anchoredRegionModule, dotNetHelper) {
    // Dispose existing listeners if any
    dispose(anchorId);

    if (!menuId || !menuOpen) {
        return;
    }

    if (!anchoredRegionModule) {
        throw new Error("AnchoredRegion module is required for keyboard navigation");
    }

    const menuElement = document.getElementById(menuId);
    const anchorElement = document.getElementById(anchorId);

    // Return if either the menu or anchor element have not rendered.
    if (!menuElement || !anchorElement) {
        return;
    }

    anchoredRegionModule.initializeKeyboardNavigation(anchorId, menuId, dotNetHelper, undefined, true);

    menuState.set(anchorId, { anchoredRegionModule });
}

// Called to cleanup listeners when component is disposed
export function dispose(anchorId) {
    const state = menuState.get(anchorId);
    if (!state) {
        return;
    }

    state.anchoredRegionModule?.disposeKeyboardNavigation(anchorId);
    menuState.delete(anchorId);
}
