export namespace Microsoft.FluentUI.Blazor.InputFile {

  /**
    * Initializes a drag-and-drop file upload zone.
    * This function sets up event listeners on a container element to handle drag-and-drop file uploads.
    * When files are dropped, they are assigned to the input element, and a change event is triggered.
    *
    * @param containerElement - The HTML element that acts as the drag-and-drop zone.
    * @param inputFile - The HTML input element where the dropped files will be assigned.
    * @returns An object with a `dispose` method to unregister the event listeners.
    */
  export function InitializeFileDropZone(containerElement: HTMLElement, inputFile: HTMLElement) {

    function onDragHover(e: DragEvent) {
      e.preventDefault();
      containerElement.setAttribute("drop-files", "true");
    }

    function onDragLeave(e: DragEvent) {
      e.preventDefault();
      containerElement.removeAttribute("drop-files");
    }

    function onDrop(e: DragEvent) {
      e.preventDefault();
      containerElement.removeAttribute("drop-files");

      // Set the files property of the input element and raise the change event
      (inputFile as any).files = e.dataTransfer?.files;
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

  /**
   * Open the dialog-box to select files.
   * 
   * @param fileInputId
   */
  export function RaiseFluentInputFile(fileInputId: string) {
    var item = document.getElementById(fileInputId);
    if (!!item) {
      item.click();
    }
  }

  /**
   * Attaches a click event handler to a button element that triggers a file input click.
   * This allows the file input dialog to open when the button is clicked.
   *
   * @param buttonId - The ID of the button element to attach the click handler to.
   * @param fileInputId - The ID of the file input element to trigger on button click.
   */
  export function AttachClickHandler(buttonId: string, fileInputId: string) {
    var button = document.getElementById(buttonId);
    var fileInput = document.getElementById(fileInputId) as HTMLInputElement;

    if (button && fileInput && !button.hasAttribute("fluentuiBlazorFileInputHandlerAttached")) {

      button.addEventListener("click", e => {
        fileInput.click();
      });

      button.setAttribute("fluentuiBlazorFileInputHandlerAttached", "true");
    }
  }
}
