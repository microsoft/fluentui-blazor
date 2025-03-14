export namespace Microsoft.FluentUI.Blazor.Slider {

  // fluent-slider sets negative tabindexes to 0!
  export function tabindexFixer(ref: HTMLElement): void {
    // If the reference element is null or undefined, exit the function
    if (!ref) return;

    // Update the tabindex attribute based on the 'readonly' attribute
    const updateTabindex = () => {
      // Check if the element has the 'readonly' attribute and it is not set to 'false'
      if (ref.hasAttribute('readonly') && ref.getAttribute('readonly') !== 'false') {
        // Set tabindex to -1 to make the element non-focusable
        ref.setAttribute("tabindex", "-1");
      } else {
        // Set tabindex to 0 to make the element focusable
        ref.setAttribute("tabindex", "0");
      }
    };

    // Check the initial state of the element
    updateTabindex();

    // Create a MutationObserver to watch for attribute changes
    const observer = new MutationObserver(() => updateTabindex());
    // Start observing the element for changes to the 'readonly' and 'tabindex' attributes
    observer.observe(ref, { attributes: true, attributeFilter: ["readonly", "tabindex"] });
  }

}
