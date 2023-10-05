export function raiseFluentInputFile(fileInputId) {
    var item = document.getElementById(fileInputId);
    if (!!item) {
        item.click();
    }
}

export function attachClickHandler(buttonId, fileInputId) {
    var button = document.getElementById(buttonId);
    var fileInput = document.getElementById(fileInputId);
    if (button && fileInput) {
        button.addEventListener("click", function (e) {
            fileInput.click();
        });
    }
}

export function previewImage(inputElem, index, imgElem) {
    const url = URL.createObjectURL(inputElem.files[index]);
    imgElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    imgElem.src = url;
    imgElem.alt = inputElem.files[index].name;
}
