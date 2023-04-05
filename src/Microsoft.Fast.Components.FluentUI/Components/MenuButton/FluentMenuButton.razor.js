export function clickOutsideHandler(buttonId, menuId, dotnetHelper) {
    const button = document.getElementById(buttonId);
    const menu = document.getElementById(menuId);
    
    document.addEventListener('click', (e) => {
        if (e && e.target !== button && button.hasAttribute('aria-expanded') && !menu.contains(e.target))
        {
            dotnetHelper.invokeMethodAsync("HideMenu");
        }
    });
}
