const handlers = new Map();

export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".selected-value");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}

export function attachClickHandlers(id) {
    const combobox = document.querySelector("#" + id);
    if (!combobox) return;

    const indicator = combobox.shadowRoot?.querySelector('[part="indicator"]');
    const inputbox = combobox.shadowRoot?.querySelector('[part="selected-value"]');

    // Ensure at least one element exists
    if (!indicator && !inputbox) return;

    const clickHandler = (event) => {
        if (combobox.hasAttribute('open')) {
            event.preventDefault();
            event.stopImmediatePropagation();

            const escEvent = new KeyboardEvent('keydown', {
                key: 'Escape',
                code: 'Escape',
                keyCode: 27,
                bubbles: true,
                cancelable: true
            });

            combobox.dispatchEvent(escEvent);
        }
    };

    // Attach listeners if elements are available
    if (indicator) {
        indicator.addEventListener('click', clickHandler);
    }
    if (inputbox) {
        inputbox.addEventListener('click', clickHandler);
    }

    // Store handlers for cleanup
    handlers.set(id, { indicator, inputbox, clickHandler });
}

export function detachHandlers(id) {
    const handler = handlers.get(id);
    if (handler) {
        const { indicator, inputbox, clickHandler } = handler;

        if (indicator) {
            indicator.removeEventListener('click', clickHandler);
        }
        if (inputbox) {
            inputbox.removeEventListener('click', clickHandler);
        }

        handlers.delete(id);
    }
}
