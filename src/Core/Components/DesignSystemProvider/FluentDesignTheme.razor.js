export function addThemeChangeEvent(dotNetHelper, id) {
    const element = document.getElementById(id);
   
    if (element) {
        element.addEventListener("onchange", (e) => {
            dotNetHelper.invokeMethodAsync("OnChangeRaisedAsync", e.detail.name, e.detail.newValue ?? "system");
        });

        const theme = element.themeStorage.readLocalStorage()
        return theme == null ? theme : JSON.stringify(theme);
    }

    return null;
}
