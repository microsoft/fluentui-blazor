export function setFluentCheckBoxIndeterminate(id, value) {
    var item = document.getElementById(id);
    if (!!item) {
        item.indeterminate = value;

        if (value) {
            item.checked = false;
        }
    }
}
