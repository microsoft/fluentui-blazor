export function selectOnlyThisLink(navMenuId, selectedLinkId) {
    var allLinks = document.querySelectorAll("fluent-tree-view[id='" + navMenuId + "'] fluent-tree-item");    
    allLinks.forEach(item => {
        item.tabIndex = 0;
        if (item.id === selectedLinkId)
            item.selected = true;
        else
            item.selected = false;
    });
}