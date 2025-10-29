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

    const FocusableElement = anchoredRegionModule
        ? anchoredRegionModule.FocusableElement
        : null;
    if (!FocusableElement) {
        throw new Error("FocusableElement not available from AnchoredRegion module");
    }

    const menuElement = document.getElementById(menuId);
    const anchorElement = document.getElementById(anchorId);

    // Return if either the menu or anchor element have not rendered.
    if (!menuElement || !anchorElement) {
        return;
    }

    // We need to handle four cases to be fully accessible:
    // 1. When Tab is pressed on the anchor, focus must be moved to the first focusable element in the menu
    // 2. When Shift+Tab is pressed on any focusable element in the menu, focus must be moved back to the anchor. This will also close the menu.
    // 3. When Tab is pressed on any focusable element in the menu, focus should continue to the next focusable element in the element's root. This will also close the menu.
    // 4. When Escape is pressed on any focusable element in the menu, focus must be moved back to the anchor. This will also close the menu.
    const menuItemKeydownListener = function (ev) {
        if (ev.key === "Tab" || ev.key === "Escape") {
            try {
                ev.preventDefault && ev.preventDefault();
                ev.stopPropagation && ev.stopPropagation();

                if (!ev.shiftKey && ev.key === "Tab") {
                    // When Tab is pressed on a focusable element, we should continue to the next focusable element
                    // If this element is a fluent element, we should try to find the next focusable element within the shadow DOM of the fluent element if one exists,
                    // as that is the focusable element.
                    let element;
                    if (anchorElement.tagName.startsWith("FLUENT-") && anchorElement.shadowRoot && anchorElement.shadowRoot.children.length > 0) {
                        element = anchorElement.shadowRoot.children[0];
                    }
                    else {
                        element = anchorElement;
                    }

                    new FocusableElement(anchorElement.getRootNode()).findNextFocusableElement(element)?.focus();
                }
                else {
                    // When Shift+Tab is pressed on a focusable element, move focus back to the anchor
                    anchorElement.focus();
                }

                dotNetHelper.invokeMethodAsync('CloseAsync');
            } catch (ex) {
                console.error("Failed to focus anchor:", ex);
            }
        }
    };

    menuElement.addEventListener("keydown", menuItemKeydownListener);

    // Add keydown listener to the anchor for Tab (no shift) to focus first element
    const anchorKeyDownListener = function (ev) {
        if (ev.key === "Tab") {
            if (ev.shiftKey) {
                dotNetHelper.invokeMethodAsync('CloseAsync');
            }
            else {
                const focusableHelper = new FocusableElement(menuElement);
                const firstFocusable = focusableHelper.findNextFocusableElement();

                if (!firstFocusable) {
                    return; // no element to attach listener to
                }

                // When Tab is pressed on the anchor, move focus to the first focusable element
                try {
                    firstFocusable.focus();
                    ev.preventDefault && ev.preventDefault();
                    ev.stopPropagation && ev.stopPropagation();
                } catch (ex) {
                    console.error("Failed to focus first focusable element:", ex);
                }
            }
        }
    };

    anchorElement.addEventListener("keydown", anchorKeyDownListener);

    menuState.set(anchorId, {
        menuItemKeydownListener,
        anchorKeyDownListener,
        menuElement,
        anchorElement
    });
}

// Called to cleanup listeners when component is disposed
export function dispose(anchorId) {
    const state = menuState.get(anchorId);
    if (!state) {
        return;
    }

    const { menuItemKeydownListener, anchorKeyDownListener, menuElement, anchorElement } = state;
    menuElement.removeEventListener("keydown", menuItemKeydownListener);
    anchorElement.removeEventListener("keydown", anchorKeyDownListener);

    menuState.delete(anchorId);
}
