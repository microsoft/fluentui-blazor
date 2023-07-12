export function raiseFluentInputFile(id) {
    var item = document.getElementById(id);
    if (!!item) {
        item.click();
    }
}

export function previewImage(inputElem, index, imgElem) {
    const url = URL.createObjectURL(inputElem.files[index]);
    imgElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    imgElem.src = url;
    imgElem.alt = inputElem.files[index].name;
}
