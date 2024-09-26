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
