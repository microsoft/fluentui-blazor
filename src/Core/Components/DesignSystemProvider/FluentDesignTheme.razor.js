export function addThemeChangeEvent(id) {
    const element = document.getElementById(id);
   
    if (element) {
        element.addEventListener("onchange", (e) => console.log(e.detail));
    }
}
