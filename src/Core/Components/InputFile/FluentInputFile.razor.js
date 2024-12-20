export function raiseFluentInputFile(fileInputId) {
    var item = document.getElementById(fileInputId);
    if (!!item) {
        item.click();
    }
}

export function attachClickHandler(buttonId, fileInputId) {
    var button = document.getElementById(buttonId);
    var fileInput = document.getElementById(fileInputId);

    if (button && fileInput && !button.fluentuiBlazorFileInputHandlerAttached) {

        button.addEventListener("click", function (e) {
            fileInput.click();
        });

        button.fluentuiBlazorFileInputHandlerAttached = true;
    }
}

export function previewImage(inputElem, index, imgElem) {
    const url = URL.createObjectURL(inputElem.files[index]);
    imgElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    imgElem.src = url;
    imgElem.alt = inputElem.files[index].name;
}

export function initializeFileDropZone(containerElement, inputFile) {
    function onDragHover(e) {
        e.preventDefault();
        containerElement.setAttribute("drop-files", "true");
    }

    function onDragLeave(e) {
        e.preventDefault();
        containerElement.removeAttribute("drop-files");
    }

    // Handle the paste and drop events
    function onDrop(e) {
        e.preventDefault();
        containerElement.removeAttribute("drop-files");

        // Set the files property of the input element and raise the change event
        inputFile.files = e.dataTransfer.files;
        const event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);
    }

    // We'll implement this later
    //function onPaste(e) {
    //    // Set the files property of the input element and raise the change event
    //    inputFile.files = e.clipboardData.files;
    //    const event = new Event('change', { bubbles: true });
    //    inputFile.dispatchEvent(event);
    //}

    // Register all events
    containerElement?.addEventListener("dragenter", onDragHover);
    containerElement?.addEventListener("dragover", onDragHover);
    containerElement?.addEventListener("dragleave", onDragLeave);
    containerElement?.addEventListener("drop", onDrop);
    //containerElement?.addEventListener('paste', onPaste);

    // The returned object allows to unregister the events when the Blazor component is destroyed
    return {
        dispose: () => {
            containerElement?.removeEventListener('dragenter', onDragHover);
            containerElement?.removeEventListener('dragover', onDragHover);
            containerElement?.removeEventListener('dragleave', onDragLeave);
            containerElement?.removeEventListener("drop", onDrop);
            //containerElement?.removeEventListener('paste', onPaste);
        }
    }
}
