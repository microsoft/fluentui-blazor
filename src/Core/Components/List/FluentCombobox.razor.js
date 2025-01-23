const handlers = new Map();

export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".selected-value");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}

export function attachIndicatorClickHandler(id) {
    const combobox = document.querySelector("#" + id);
    if (!combobox) return;

    const indicator = combobox.shadowRoot?.querySelector('[part="indicator"]');

    if (!indicator) return;

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

    if (indicator) {
        indicator.addEventListener('click', clickHandler);
    }

    handlers.set(id, { indicator, clickHandler });
}

export function detachIndicatorClickHandler(id) {
    const handler = handlers.get(id);
    if (handler) {
        const { indicator, clickHandler } = handler;

        if (indicator) {
            indicator.removeEventListener('click', clickHandler);
        }

        handlers.delete(id);
    }
}
