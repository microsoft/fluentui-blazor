const menuButtonState = new Map();

export async function fluentMenuButtonOnRender(buttonId, menuId, anchoredRegionModule, dotNetHelper) {
    // Dispose existing listeners if any
    fluentMenuButtonDispose(buttonId);

    if (!menuId) {
        return;
    }

    const FocusableElement = anchoredRegionModule
        ? anchoredRegionModule.FocusableElement
        : null;
    if (!FocusableElement) {
        throw new Error("FocusableElement not available from AnchoredRegion module");
    }

    const menuElement = document.getElementById(menuId);
    const buttonElement = document.getElementById(buttonId);

    // Return if either the menu or button element have not rendered.
    if (!menuElement || !buttonElement) {
        return;
    }

    // We need to handle three cases to be fully accessible:
    // 1. When Tab is pressed on the button, focus must be moved to the first focusable element in the menu
    // 2. When Shift+Tab is pressed on any focusable element in the menu, focus must be moved back to the button. This will also close the menu.
    // 3. When Tab is pressed on any focusable element in the menu, focus should continue to the next focusable element in the element's root. This will also close the menu.

    const menuItemKeydownListener = function (ev) {
        if (ev.key === "Tab") {
            try {
                ev.preventDefault && ev.preventDefault();
                ev.stopPropagation && ev.stopPropagation();

                if (!ev.shiftKey) {
                    // When Tab is pressed on a focusable element, we should continue to the next focusable element
                    const innerHtmlButton = buttonElement.shadowRoot.querySelector('button');
                    new FocusableElement(buttonElement.getRootNode()).findNextFocusableElement(innerHtmlButton)?.focus();
                }
                else {
                    // When Shift+Tab is pressed on a focusable element, move focus back to the button
                    buttonElement.focus();
                }

                dotNetHelper.invokeMethodAsync('ToggleMenuAsync');
            } catch (ex) {
                console.error("Failed to focus button:", ex);
            }
        }
    };

    menuElement.addEventListener("keydown", menuItemKeydownListener);

    // Add keydown listener to the button for Tab (no shift) to focus first element
    const onButtonKeyDownListener = function (ev) {
        if (ev.key === "Tab") {
            if (ev.shiftKey) {
                dotNetHelper.invokeMethodAsync('ToggleMenuAsync');
            }
            else {
                const focusableHelper = new FocusableElement(menuElement);
                const firstFocusable = focusableHelper.findNextFocusableElement();

                if (!firstFocusable) {
                    return; // no element to attach listener to
                }

                // When Tab is pressed on the button, move focus to the first focusable element
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

    buttonElement.addEventListener("keydown", onButtonKeyDownListener);

    menuButtonState.set(buttonId, {
        menuItemKeydownListener,
        onButtonKeyDownListener,
        menuElement,
        buttonElement
    });
}

// Called to cleanup listeners when component is disposed
export function fluentMenuButtonDispose(buttonId) {
    const state = menuButtonState.get(buttonId);
    if (!state) {
        return;
    }

    const { menuItemKeydownListener, onButtonKeyDownListener, menuElement, buttonElement } = state;
    menuElement.removeEventListener("keydown", menuItemKeydownListener);
    buttonElement.removeEventListener("keydown", onButtonKeyDownListener);

    menuButtonState.delete(buttonId);
}
