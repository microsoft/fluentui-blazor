export function addThemeChangeEvent(dotNetHelper, id) {
    const element = document.getElementById(id);
   
    if (element) {
        element.addEventListener("onchange", (e) => {
            dotNetHelper.invokeMethodAsync("OnChangeRaisedAsync", e.detail.name, e.detail.olvValue, e.detail.newValue);
        });
    }
}
