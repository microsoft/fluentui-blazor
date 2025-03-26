export namespace Microsoft.FluentUI.Blazor.List {

  const AttributeName = 'current-selected';

  export function Initialize(id: string, dotNetHelper: any) {
    const listbox = document.getElementById(id) as any;

    // Define a callback function to handle attribute changes
    const handleAttributeChange: MutationCallback = (mutationsList, observer) => {
      for (const mutation of mutationsList) {
        if (mutation.type === 'attributes' && mutation.attributeName === AttributeName) {
          const option = mutation.target as any;
          if (option.hasAttribute(AttributeName)) {
            dotNetHelper.invokeMethodAsync('FluentList_OptionChangedAsync', option.id, true);
          } else {
            dotNetHelper.invokeMethodAsync('FluentList_OptionChangedAsync', option.id, false);
          }
        }
      }
    };

    // Create a MutationObserver instance
    const observer = new MutationObserver(handleAttributeChange);

    // Start observing the fluent-listbox for attribute changes on its children
    observer.observe(listbox, {
      attributes: true,
      subtree: true, // Observe changes in the descendants
      attributeFilter: [AttributeName] // Only observe changes to the 'current-selected' attribute
    });
  }
}
