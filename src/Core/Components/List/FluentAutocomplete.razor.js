export function initialize(id) {
    var item = document.getElementById(id);
    if (!!item) {
        const input = item.shadowRoot?.querySelector('#control');

        // Observe attribute changes to remove 'aria-controls' if added by another process
        // This attribute cannot be set in fluent-autocomplete Shadow DOM
        if (input) {
            const observer = new MutationObserver((mutationsList) => {
            for (const mutation of mutationsList) {
                if (
                mutation.type === 'attributes' &&
                mutation.attributeName === 'aria-controls' &&
                input.hasAttribute('aria-controls')
                ) {
                input.removeAttribute('aria-controls');
                }
            }
            });
            observer.observe(input, { attributes: true, attributeFilter: ['aria-controls'] });
        }

        if (input && input.hasAttribute('aria-controls')) {
            input.removeAttribute('aria-controls');
        }
    }
}

export function displayLastSelectedItem(id) {
    var item = document.getElementById(id);
    var scroll = document.getElementById(id + "-scroll");
    if (!!item && !!scroll) {
        try {
            // To be optimized (how to detect the end of scroll container?)
            for (var i = 0; i < 10; i++) {
                scroll.scrollToNext();
                item.focus();
            }
        }
        // Sometimes fluent-horizontal-scroll.scrollToNext fails
        // (Cannot read properties of undefined - reading 'findIndex')
        catch (e) {
            console.warn("fluent-horizontal-scroll.scrollToNext fails.");
        }
    }
}

export function scrollToFirstSelectable(idPopup, goDown) {

    const popup = document.getElementById(idPopup);
    const item = popup?.querySelector("fluent-option[selectable]")
    const next = goDown ? item?.nextElementSibling : item?.previousElementSibling;

    if (!!next) {
        next.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
    }

}

export function focusOn(id) {
    var item = document.getElementById(id);
    if (!!item) {
        // Delay to let the UI refresh the control
        setTimeout(function () {
            item.focus();
        }, 200);
    }
}
