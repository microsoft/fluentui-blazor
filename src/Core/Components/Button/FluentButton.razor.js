// workaround for https://github.com/microsoft/fast/issues/5675
export function updateProxy(id) {
    if (!id) {
        return;
    }
    const element = document.getElementById(id);

    if (!!element && !!element.form) {
        element.proxy.setAttribute("form", element.form.id)
    }
}