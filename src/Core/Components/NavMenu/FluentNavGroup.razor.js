function attachEventHandlers(id){
    var element = document.getElementById(id);
    var positioningRegion = element.getElementsByClassName("positioning-region")[0];
    positioningRegion.addEventListener('click', () => toggleExpandedAsync(element));
    positioningRegion.addEventListener('keydown', (ev) => handleExpanderKeyDownAsync(element,ev));
}

function toggleExpandedAsync(element) {
    setExpanded(element, !element.classList.contains('expanded'));
}

function handleExpanderKeyDownAsync(element, event){
    switch (event.code){
        case "NumpadEnter":
        case "Enter":
            toggleExpandedAsync(element);
            break;
        case "NumpadArrowRight":
        case "ArrowRight":
            setExpanded(element, true);
            break;
        case "NumpadArrowLeft":
        case "ArrowLeft":
            setExpanded(element,false);
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