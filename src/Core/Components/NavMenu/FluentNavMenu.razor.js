export function onUpdate() {
    for (let element of document.getElementsByClassName("fluent-nav-group")) {
        attachEventHandlers(element);
    }
}
function attachEventHandlers(element) {
    let navlink = element.getElementsByClassName("fluent-nav-link")[0];
    if (!navlink.href) {
        navlink.addEventListener('click', () => toggleExpandedAsync(element));
    }
    navlink.addEventListener('keydown', (ev) => handleExpanderKeyDownAsync(element, ev));

    let expandCollapseButton = element.getElementsByClassName("expand-collapse-button")[0];
    expandCollapseButton.addEventListener('click', (ev) => toggleExpandedAsync(element, navlink, ev));
}

function toggleExpandedAsync(element, navlink, event) {
    if (navlink && navlink.href) {
        event.preventDefault();
    }
    setExpanded(element, !element.classList.contains('expanded'));
    if (event) {
        event.stopPropagation();
    }
}

function handleExpanderKeyDownAsync(element, event) {
    switch (event.code) {
        case "NumpadEnter":
        case "Enter":
            toggleExpandedAsync(element, null, event);
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