import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.Layout {

  /**
   * Add an attribute to the layout element if the width is less than the maximum mobile width
   * @param dotNetHelper DotNet helper to call back to the Blazor component
   * @param id Identifier of the layout element
   * @param maxMobileWidth Maximum width for mobile layout (pixels)
  */
  export function Initialize(dotNetHelper: DotNet.DotNetObject, id: string, maxMobileWidth: number) {

    const layoutElement = document.getElementById(id);

    if (layoutElement) {

      // Detect the layout size, and add a "mobile" attribute
      // if the width is less than the maximum mobile width
      const resizeObserver = new ResizeObserver(entries => {
        const hasMobileAttribute = layoutElement.hasAttribute('mobile');
        const isMobileSize = entries.some(entry => {
          return entry.borderBoxSize.length > 0
            ? entry.borderBoxSize[0].inlineSize <= maxMobileWidth
            : false;
        });

        if (!hasMobileAttribute && isMobileSize) {
          layoutElement.setAttribute('mobile', '');
          try {
            dotNetHelper.invokeMethodAsync('FluentLayout_MediaChangedAsync', 'mobile');
          }
          catch (error) {
          }
        }

        else if (hasMobileAttribute && !isMobileSize) {
          layoutElement.removeAttribute('mobile');
          try {
            dotNetHelper.invokeMethodAsync('FluentLayout_MediaChangedAsync', 'desktop');
          }
          catch (error) {
          }
        }
      });

      resizeObserver.observe(layoutElement);
    }
  }

  /**
   * Initialize the hamburger menu, to show or hide the fluent-drawer
   * @param dotNetHelper DotNet helper to call back to the Blazor component
   * @param id Identifier of the hamburger menu
   */
  export function HamburgerInitialize(dotNetHelper: DotNet.DotNetObject, id: string) {
    const element = document.getElementById(id);
    const dialog = document.getElementById(id + '-drawer') as any;
    const closeButton = document.getElementById(id + '-drawer-close-button');

    if (element) {

      element.addEventListener('click', (event: MouseEvent) => {
        const isExpanded = element.getAttribute('aria-expanded') === 'true';
        const newValue = !isExpanded;

        // Show or hide the fluent-drawer
        if (dialog) {

          // Add a Toggle event
          if (!dialog.toggleRegistered) {
            dialog.toggleRegistered = true;
            dialog.addEventListener('toggle', (e: any) => {
              // Toggle the aria-expanded attribute

              const newState = (e.detail?.newState ?? e.newState) === `open`;

              element.setAttribute('aria-expanded', newState ? 'true' : 'false');
              try {
                dotNetHelper.invokeMethodAsync('FluentLayout_HamburgerClickAsync', newState);
              }
              catch (error) {
              }

            });
          }

          // Show or hide
          if (newValue) {
            dialog.show();
          }
          else {
            dialog.hide();
          }
        }
      });

      // Add a keydown event to handle Enter and Space keys
      element.addEventListener('keydown', (event: KeyboardEvent) => {
        if (event.key === 'Enter' || event.key === ' ') {
          event.preventDefault(); // Prevent the default action for space key
          element.click();
        }
      });

      // Hide the drawer when the close button is clicked
      if (closeButton) {
        closeButton.addEventListener('click', (event: MouseEvent) => {
          dialog.hide();
        });
      }
    }
  }

}
