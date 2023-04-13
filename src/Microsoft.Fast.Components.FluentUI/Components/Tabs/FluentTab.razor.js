export function TabEditable_Changed(dotNetHelper, cssSelector, id) {
    var item = document.querySelector(cssSelector);
    if (!!item) {

        item.addEventListener('keydown', (e) => {
            if (e.keyCode == 13) {
                e.preventDefault();
                document.activeElement.blur();
            }
        });        

        item.addEventListener('blur', (e) => {
            dotNetHelper.invokeMethodAsync("UpdateTabLabelAsync", id, item.innerText);
        });        
    }    
}