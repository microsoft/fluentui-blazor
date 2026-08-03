import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.Layout {

  function isMobile(element: HTMLElement): boolean {
    const maxWidth = parseInt(element.getAttribute('breakdown-width') ?? '768', 10);
    return element.offsetWidth <= maxWidth;
  }

  /**
   * Add an attribute to the layout element if the width is less than the maximum mobile width
   * @param dotNetHelper DotNet helper to call back to the Blazor component
   * @param id Identifier of the layout element
   * @param maxMobileWidth Maximum width for mobile layout (pixels)
  */
  export function Initialize(dotNetHelper: DotNet.DotNetObject | null, id: string) {

    const layoutElement = document.getElementById(id) as any;

    if (!layoutElement) {
      return;
    }

    // Allow upgrading a static (null) initialization to an interactive one.
    if (layoutElement.fluentLayoutInitialized) {
      if (dotNetHelper !== null) {
        layoutElement._fluentDotNetHelper = dotNetHelper;
      }
      return;
    }

    layoutElement.fluentLayoutInitialized = true;
    layoutElement._fluentDotNetHelper = dotNetHelper;

    if (layoutElement) {

      // Detect the layout size, and add a "mobile" attribute
      // if the width is less than the maximum mobile width
      const resizeObserver = new ResizeObserver(_entries => {
        const hasMobileAttribute = layoutElement.hasAttribute('mobile');
        const isMobileSize = isMobile(layoutElement);

        if (!hasMobileAttribute && isMobileSize) {
          layoutElement.setAttribute('mobile', '');
          try {
            layoutElement._fluentDotNetHelper?.invokeMethodAsync('FluentLayout_MediaChangedAsync', 'mobile');
          }
          catch (error) {
          }
        }

        else if (hasMobileAttribute && !isMobileSize) {
          layoutElement.removeAttribute('mobile');
          try {
            layoutElement._fluentDotNetHelper?.invokeMethodAsync('FluentLayout_MediaChangedAsync', 'desktop');
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
  export function HamburgerInitialize(dotNetHelper: DotNet.DotNetObject | null, id: string, containerId: string | null | undefined) {
    const element = document.getElementById(id) as any;

    if (!element) {
      return;
    }

    // Allow upgrading a static (null) initialization to an interactive one.
    if (element.fluentHamburgerInitialized) {
      if (dotNetHelper !== null) {
        element._fluentHamburgerDotNetHelper = dotNetHelper;
      }
      return;
    }

    element.fluentHamburgerInitialized = true;
    element._fluentHamburgerDotNetHelper = dotNetHelper;

    const layoutContainer = containerId ? document.getElementById(containerId) : null;
    const dialog = document.getElementById(id + '-drawer') as any;
    const closeButton = document.getElementById(id + '-drawer-close-button');

    if (element) {

      element.addEventListener('click', (event: MouseEvent) => {
        const layoutNav = layoutContainer ? layoutContainer.querySelector('.fluent-layout-item[area="nav"]') : null;
        const isExpanded = element.getAttribute('aria-expanded') === 'true';
        const newValue = !isExpanded;
        const isMobileSize = isMobile(layoutContainer ?? document.body);

        // Show or hide the nav area
        if (layoutNav && !isMobileSize) {
          element.setAttribute('aria-expanded', newValue ? 'true' : 'false');
          layoutNav.toggleAttribute('hidden', newValue);
        }

        // Show or hide the fluent-drawer
        else if (isMobileSize && dialog) {

          // Add a Toggle event
          if (!dialog.toggleRegistered) {
            dialog.toggleRegistered = true;
            dialog.addEventListener('toggle', (e: any) => {
              // Toggle the aria-expanded attribute

              const newState = (e.detail?.newState ?? e.newState) === `open`;

              element.setAttribute('aria-expanded', newState ? 'true' : 'false');
              try {
                element._fluentHamburgerDotNetHelper?.invokeMethodAsync('FluentLayout_HamburgerClickAsync', newState);
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

  /**
   * Scans the document for layouts and hamburger menus that aren't wired up yet, and initializes them
   * without a .NET reference. This lets the hamburger open/close the fluent-drawer even when
   * rendered statically, since `HamburgerInitialize` is otherwise only called from
   * `OnAfterRenderAsync`, which never runs without an interactive render mode.
   */
  export function LayoutAutoInitialize() {

    document.querySelectorAll<HTMLElement>('.fluent-layout').forEach(element => {
      if (!element.id || (element as any).fluentLayoutInitialized) {
        return;
      }

      Initialize(null, element.id);

      element.querySelectorAll<HTMLElement>('.fluent-layout-hamburger').forEach(hamburger => {
        if (!hamburger.id || (hamburger as any).fluentHamburgerInitialized) {
          return;
        }

        HamburgerInitialize(null, hamburger.id, element.id);
      });

    });
  }
}
