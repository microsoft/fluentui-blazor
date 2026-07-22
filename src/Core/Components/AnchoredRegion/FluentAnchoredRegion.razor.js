export function goToNextFocusableElement(forContainer, toOriginal, delay) {
    const container = typeof forContainer === "string" ? document.getElementById(forContainer) : forContainer;

    if (container == null || container == undefined) {
        return;
    }

    if (!!!container.focusableElement) {
        container.focusableElement = new FocusableElement(container);
    }

    // Move the focus to the inital element
    if (toOriginal === true) {
        container.focusableElement.originalActiveElement.focus();
    }

    // Move to the next element
    else {
        // Find the next focusable element
        const nextElement = container.focusableElement.findNextFocusableElement();

        // Set focus on the next element,
        // after delay to give FluentUI web components time to build up
        if (nextElement) {
            if (delay > 0) {
                container.focusableElement.setFocusAfterDelay(nextElement, delay);
            }
            else {
                nextElement.focus();
            }
        }
    }
}


// ---------------------------------------------------------------------------
// Shared keyboard navigation for anchor+popup pairs (popovers, menus, etc.)
// Implements the standard 4-case ARIA keyboard pattern:
//   1. Tab on anchor          → focus first element in popup (stay open)
//   2. Shift+Tab on popup     → focus anchor, close popup
//   3. Tab on popup           → focus next page element after anchor, close popup
//   4. Escape on popup        → focus anchor, close popup
//   (Shift+Tab on anchor while popup open → close popup, browser handles focus)
// ---------------------------------------------------------------------------

const keyboardNavigationState = new Map();

function getDeepActiveElement(root = document) {
    let activeElement = root.activeElement;

    while (activeElement?.shadowRoot?.activeElement) {
        activeElement = activeElement.shadowRoot.activeElement;
    }

    return activeElement;
}

function containsComposedElement(container, element) {
    let current = element;

    while (current) {
        if (container?.contains?.(current)) {
            return true;
        }

        const root = current.getRootNode();
        current = root instanceof ShadowRoot ? root.host : null;
    }

    return false;
}

function focusElementOrDescendant(element) {
    if (element.tabIndex !== -1) {
        element.focus();

        const activeElement = getDeepActiveElement();
        if (activeElement !== element && containsComposedElement(element, activeElement)) {
            return;
        }
    }

    const focusTarget = new FocusableElement(element).findNextFocusableElement();
    (focusTarget ?? element).focus();
}

/**
 * Attaches keyboard navigation listeners to an anchor+popup pair.
 * @param {string} anchorId - Id of the anchor element.
 * @param {string} popupId - Id of the popup/overlay element.
 * @param {object} dotNetHelper - DotNetObjectReference; must expose CloseAsync().
 * @param {number[]} closeKeyCodes - Additional key codes (besides Tab) that close the popup. Defaults to [27] (Escape).
 */
export function initializeKeyboardNavigation(anchorId, popupId, dotNetHelper, closeKeyCodes = [27], tabExitsAlways = false) {
    disposeKeyboardNavigation(anchorId);

    const popupElement = document.getElementById(popupId);
    const anchorElement = document.getElementById(anchorId);

    if (!popupElement || !anchorElement) {
        return;
    }

    // Listeners on the popup content (cases 2, 3, 4)
    const popupKeydownListener = function (ev) {
        const keyCode = ev.which || ev.keyCode;
        const isCloseKey = closeKeyCodes.includes(keyCode);

        if (ev.key !== "Tab" && !isCloseKey) return;

        if (isCloseKey) {
            // Case 4: close key → return focus to anchor, close
            ev.preventDefault();
            ev.stopPropagation();
            focusElementOrDescendant(anchorElement);
            dotNetHelper.invokeMethodAsync('CloseAsync');
            return;
        }

        // Tab / Shift+Tab
        if (tabExitsAlways) {
            // Menu pattern: Tab on any element exits immediately
            ev.preventDefault();
            ev.stopPropagation();
            if (!ev.shiftKey) {
                // Case 3: move to element after anchor in page
                new FocusableElement(anchorElement.getRootNode()).findNextFocusableElement(anchorElement)?.focus();
            } else {
                // Case 2: Shift+Tab → focus anchor
                focusElementOrDescendant(anchorElement);
            }
            dotNetHelper.invokeMethodAsync('CloseAsync');
        } else {
            // Popover pattern: only intercept Tab at the first/last boundary;
            // let the browser handle Tab naturally for elements in between.
            const popupFocus = new FocusableElement(popupElement);
            const focusables = popupFocus.getFocusableElements();
            const activeIndex = popupFocus.getFocusableElementIndex(getDeepActiveElement(), focusables);

            if (!ev.shiftKey && (focusables.length === 0 || activeIndex === focusables.length - 1)) {
                // Case 3: Tab on last element → next page element after anchor, close
                ev.preventDefault();
                ev.stopPropagation();
                new FocusableElement(anchorElement.getRootNode()).findNextFocusableElement(anchorElement)?.focus();
                dotNetHelper.invokeMethodAsync('CloseAsync');
            } else if (ev.shiftKey && (focusables.length === 0 || activeIndex === 0)) {
                // Case 2: Shift+Tab on first element → focus anchor, close
                ev.preventDefault();
                ev.stopPropagation();
                focusElementOrDescendant(anchorElement);
                dotNetHelper.invokeMethodAsync('CloseAsync');
            }
            // Otherwise: middle element — let browser handle Tab/Shift+Tab naturally
        }
    };

    // Listener on the anchor (case 1 and Shift+Tab-while-open)
    const anchorKeydownListener = function (ev) {
        if (ev.key !== "Tab") return;

        if (!ev.shiftKey) {
            // Case 1: Tab on anchor → focus first focusable element in popup
            const firstFocusable = new FocusableElement(popupElement).findNextFocusableElement();
            if (!firstFocusable) return;

            firstFocusable.focus();
            ev.preventDefault();
            ev.stopPropagation();
        } else {
            // Shift+Tab on anchor while popup open → close, browser handles focus naturally
            dotNetHelper.invokeMethodAsync('CloseAsync');
        }
    };

    popupElement.addEventListener("keydown", popupKeydownListener);
    anchorElement.addEventListener("keydown", anchorKeydownListener);

    keyboardNavigationState.set(anchorId, {
        popupKeydownListener,
        anchorKeydownListener,
        popupElement,
        anchorElement
    });
}

/**
 * Removes keyboard navigation listeners registered by initializeKeyboardNavigation.
 * @param {string} anchorId
 */
export function disposeKeyboardNavigation(anchorId) {
    const state = keyboardNavigationState.get(anchorId);
    if (!state) return;

    const { popupKeydownListener, anchorKeydownListener, popupElement, anchorElement } = state;
    popupElement.removeEventListener("keydown", popupKeydownListener);
    anchorElement.removeEventListener("keydown", anchorKeydownListener);

    keyboardNavigationState.delete(anchorId);
}

/**
 * Focusable Element
 */
export class FocusableElement {
    FOCUSABLE_SELECTORS = "input, select, textarea, button, object, a[href], area[href], [tabindex]";
    _originalActiveElement;
    _container;

    /**
     * Initializes a new instance of the FocusableElement class.
     */
    constructor(container) {
        this._originalActiveElement = document.activeElement;
        this._container = container;
    }

    /**
     * Gets the original document.activeElement before the focus was set to the current element.
     */
    get originalActiveElement() {
        return this._originalActiveElement;
    }

    /**
     * Returns all focusable elements within the container, resolving Fluent web component shadow roots.
     * @returns {Element[]}
     */
    getFocusableElements() {
        const focusableElements = [];

        const collectFocusableElements = container => {
            Array.from(container.children).forEach(element => {
                const isFocusable = element.matches(this.FOCUSABLE_SELECTORS)
                    && element.tabIndex !== -1
                    && element.checkVisibility();

                if (isFocusable) {
                    focusableElements.push(element);
                } else if (element.shadowRoot) {
                    collectFocusableElements(element.shadowRoot);
                }

                collectFocusableElements(element);
            });
        };

        if (this._container.shadowRoot) {
            collectFocusableElements(this._container.shadowRoot);
        }

        collectFocusableElements(this._container);
        return focusableElements;
    }

    /**
     * Gets the position of an element or its shadow/composite representation in a focusable element list.
     * @param currentElement
     * @param focusableElements
     * @param reverse - If true, use the first focusable descendant instead of the last.
     * @returns
     */
    getFocusableElementIndex(currentElement, focusableElements = this.getFocusableElements(), reverse = false) {
        let current = currentElement;

        while (current) {
            const currentIndex = focusableElements.indexOf(current);
            if (currentIndex !== -1) {
                return currentIndex;
            }

            const root = current.getRootNode();
            current = root instanceof ShadowRoot ? root.host : null;
        }

        const descendantIndexes = focusableElements
            .map((element, index) => ({ element, index }))
            .filter(({ element }) => containsComposedElement(currentElement, element))
            .map(({ index }) => index);

        if (descendantIndexes.length === 0) {
            return -1;
        }

        return reverse ? descendantIndexes[0] : descendantIndexes[descendantIndexes.length - 1];
    }

    /**
     * Find the next focusable element, after the optional current element, in the specified container.
     * @param currentElement
     * @param reverse - If true, find the previous focusable element instead of the next.
     * @returns
     */
    findNextFocusableElement(currentElement, reverse = false) {
        const filteredElements = this.getFocusableElements();

        if (filteredElements.length === 0) {
            return null;
        }

        // Find the index of the current element
        const current = currentElement ?? document.activeElement;
        if (current != null) {
            const currentIndex = this.getFocusableElementIndex(current, filteredElements, reverse);

            if (currentIndex === -1) {
                return currentElement === undefined && !reverse ? filteredElements[0] : null;
            }

            // Calculate the index of the next (or previous) element
            const nextIndex = reverse
                ? (currentIndex - 1 + filteredElements.length) % filteredElements.length
                : (currentIndex + 1) % filteredElements.length;

            // Return the next focusable element
            return filteredElements[nextIndex];
        }

        // Not found
        return null;
    }

    /**
     * Set focus on the specified element after a delay.
     * @param element
     * @param delayInMilliseconds
     */
    setFocusAfterDelay(element, delayInMilliseconds) {
        setTimeout(() => {
            const elt = typeof element === "string" ? document.getElementById(element) : element;
            if (elt) {
                elt.focus();
            }
        }, delayInMilliseconds);
    }
}
