export function onLoad() {
    const mql = window.matchMedia("(max-width: 600px)");

    for (let expander of document.getElementsByClassName("expander")) {
        if (expander) {
            const origStyle = expander.parentElement.style.cssText;
            expander.addEventListener('click', (ev) => toggleMenuExpandedAsync(expander, origStyle, ev));
            expander.addEventListener('keydown', (ev) => handleMenuExpanderKeyDownAsync(expander, origStyle, ev));

            mql.onchange = (e) => {
                if (e.matches) {
                    setMenuExpanded(expander, origStyle, true)
                }
            };
        }
    }
    for (let element of document.getElementsByClassName("fluent-nav-group")) {
        attachEventHandlers(element);
    }
}
export function onUpdate() {
    
}

export function onDispose() {
    for (let expander of document.getElementsByClassName("expander")) {
        if (expander) {
            expander.removeEventListener('click', toggleMenuExpandedAsync);
            expander.removeEventListener('keydown', handleMenuExpanderKeyDownAsync);
        }
    }
    for (let element of document.getElementsByClassName("fluent-nav-group")) {
        detachEventHandlers(element);
    }
}

function attachEventHandlers(element) {
    let navlink = element.getElementsByClassName("fluent-nav-link")[0];
    if (!navlink) {
        return;
    }
    if (!navlink.href) {
        navlink.addEventListener('click', () => toggleGroupExpandedAsync(element));
    }
    navlink.addEventListener('keydown', (ev) => handleExpanderKeyDownAsync(element, ev));

    let expandCollapseButton = element.getElementsByClassName("expand-collapse-button")[0];
    if (!expandCollapseButton) {
        return;
    }
    expandCollapseButton.addEventListener('click', (ev) => toggleGroupExpandedAsync(element, navlink, ev));
}

function detachEventHandlers(element) {
    let navlink = element.getElementsByClassName("fluent-nav-link")[0];
    if (!navlink) {
        return;
    }
    if (!navlink.href) {
        navlink.removeEventListener('click', toggleGroupExpandedAsync);
    }
    navlink.removeEventListener('keydown', handleExpanderKeyDownAsync);

    let expandCollapseButton = element.getElementsByClassName("expand-collapse-button")[0];
    expandCollapseButton.removeEventListener('click', toggleGroupExpandedAsync);
}

function toggleMenuExpandedAsync(element, orig, event) {

    let parent = element.parentElement;
    if (!parent.classList.contains('collapsed')) {
        parent.classList.add('collapsed');
        parent.style.width = '40px';
        parent.style.minWidth = '40px';
        parent.ariaExpanded = 'false';
        element.ariaExpanded = 'false';
    }
    else {
        parent.classList.remove('collapsed');
        parent.style.cssText = orig;
        parent.ariaExpanded = 'true';
        element.ariaExpanded = 'true';
    }
    event?.stopPropagation();
}

function toggleGroupExpandedAsync(element, navlink, event) {
    if (navlink && navlink.href) {
        event.preventDefault();
    }
    setExpanded(element, !element.classList.contains('expanded'));
    event?.stopPropagation();
}

function handleExpanderKeyDownAsync(element, event) {
    switch (event.code) {
        case "NumpadEnter":
        case "Enter":
            toggleGroupExpandedAsync(element, null, event);
            break;
        case "NumpadArrowRight":
        case "ArrowRight":
            setExpanded(element, true);
            break;
        case "NumpadArrowLeft":
        case "ArrowLeft":
            setExpanded(element, false);
            break;
    }
    event.stopPropagation();
}

function handleMenuExpanderKeyDownAsync(element, origStyle, event) {
    switch (event.code) {
        case "NumpadEnter":
        case "Enter":
            toggleMenuExpandedAsync(element, origStyle, event);
            break;
        case "NumpadArrowRight":
        case "ArrowRight":
            setMenuExpanded(element, origStyle, true);
            break;
        case "NumpadArrowLeft":
        case "ArrowLeft":
            setMenuExpanded(element, origStyle, false);
            break;
    }
    event.stopPropagation();
}

function setExpanded(element, expand) {
    var collapsibleRegion = element.getElementsByClassName("items")[0];
    var button = element.getElementsByClassName("expand-collapse-button")[0];
    if (expand) {
        button.classList.add("rotate");
        collapsibleRegion.style.height = 'auto';
        element.classList.add('expanded');

    } else {
        button.classList.remove("rotate");
        collapsibleRegion.style.height = '0px';
        element.classList.remove('expanded');
    }
}

function setMenuExpanded(element, origStyle, expand) {
    let parent = element.parentElement;
    if (expand) {
        parent.classList.remove('collapsed');
        parent.style.cssText = origStyle;
        parent.ariaExpanded = 'true';
        element.ariaExpanded = 'true';
    }
    else {
        parent.classList.add('collapsed');
        parent.style.width = '40px';
        parent.style.minWidth = '40px';
        parent.ariaExpanded = 'false';
        element.ariaExpanded = 'false';
    }
}
