import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.Layout {

  interface LayoutElement extends HTMLElement {
    fluentLayoutInitialized?: boolean;
    _fluentDotNetHelper?: DotNet.DotNetObject | null;
  }

  let reconnectInitialized = false;

  /**
   * Resynchronizes mounted layouts after the reconnect UI has finished updating.
   */
  function refreshLayouts(): void {
    requestAnimationFrame(() => {
      document.querySelectorAll<LayoutElement>('.fluent-layout').forEach(element => {
        void updateLayoutAsync(element, true);
      });
    });
  }

  // Blazor's reconnect event does not bubble. Wait until the application's UI has handled it.
  document.addEventListener('components-reconnect-state-changed', event => {
    if (event instanceof CustomEvent && event.detail?.state === 'hide') {
      refreshLayouts();
    }
  }, { capture: true });

  /**
   * Hooks the default Blazor reconnect handler once, preserving its original behavior.
   */
  function initializeReconnect(): void {
    const handler = (window as Window & {
      Blazor?: { defaultReconnectionHandler?: { onConnectionUp(): void } };
    }).Blazor?.defaultReconnectionHandler;
    if (!handler || reconnectInitialized) {
      return;
    }

    reconnectInitialized = true;
    const onConnectionUp = handler.onConnectionUp.bind(handler);

    // The fallback reconnect UI does not emit components-reconnect-state-changed.
    handler.onConnectionUp = () => {
      onConnectionUp();
      refreshLayouts();
    };
  }

  /**
   * Updates the mobile attribute and notifies .NET when the breakpoint changes.
   * @param element Layout whose current width determines the breakpoint
   * @param force Resends the breakpoint after reconnection, even if the DOM attribute is unchanged
   */
  async function updateLayoutAsync(element: LayoutElement, force = false): Promise<void> {
    if (!element.isConnected || element.offsetWidth === 0) {
      return;
    }

    const mobile = isMobile(element);
    const changed = element.hasAttribute('mobile') !== mobile;
    element.toggleAttribute('mobile', mobile);

    if (changed || force) {
      try {
        await element._fluentDotNetHelper?.invokeMethodAsync('FluentLayout_MediaChangedAsync', mobile ? 'mobile' : 'desktop');
      }
      catch (error) {
        console.error('FluentLayout: unable to synchronize the breakpoint with .NET.', error);
      }
    }
  }

  /**
   * Checks the layout width against its configured mobile breakpoint, defaulting to 768px.
   */
  function isMobile(element: HTMLElement): boolean {
    const maxWidth = parseInt(element.getAttribute('breakdown-width') ?? '768', 10);
    return element.offsetWidth <= maxWidth;
  }

  /**
   * Add an attribute to the layout element if the width is less than the maximum mobile width
   * @param dotNetHelper DotNet helper to call back to the Blazor component
   * @param id Identifier of the layout element
  */
  export function Initialize(dotNetHelper: DotNet.DotNetObject | null, id: string) {

    initializeReconnect();
    const layoutElement = document.getElementById(id) as LayoutElement | null;

    if (!layoutElement) {
      return;
    }

    // Allow upgrading a static (null) initialization to an interactive one.
    if (layoutElement.fluentLayoutInitialized) {
      if (dotNetHelper !== null) {
        layoutElement._fluentDotNetHelper = dotNetHelper;
        void updateLayoutAsync(layoutElement, true);
      }
      return;
    }

    layoutElement.fluentLayoutInitialized = true;
    layoutElement._fluentDotNetHelper = dotNetHelper;

    const resizeObserver = new ResizeObserver(() => void updateLayoutAsync(layoutElement));
    resizeObserver.observe(layoutElement);
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

      dialog?.addEventListener('click', (event: MouseEvent) => {
        const target = event.target;

        if (target instanceof Element && target.closest('.fluent-navitem')) {
          dialog.hide();
        }
      });

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
