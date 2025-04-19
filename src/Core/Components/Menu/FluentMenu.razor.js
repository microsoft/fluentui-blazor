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
