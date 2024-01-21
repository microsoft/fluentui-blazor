// workaround for https://github.com/microsoft/fast/issues/5675
export function updateProxy(id) {
    if (!id) {
        return;
    }
    const element = document.getElementById(id);

    if (element && element.form) {
        if (element.form.id !== '') {
            element.proxy.setAttribute("form", element.form.id)
        }
        else {
            console.warn("When the submit button is placed outside of the EditForm, make sure to supply an id attribute to the EditForm.");
        }
    }
}