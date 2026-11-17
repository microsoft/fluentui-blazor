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

    // The hamburger controls the nav area on desktop and the drawer on mobile.
    if (changed) {
      updateLayoutHamburgers(element);
    }

    if (changed || force) {
      try {
        await element._fluentDotNetHelper?.invokeMethodAsync('FluentLayout_MediaChangedAsync', mobile ? 'mobile' : 'desktop');
      }
      catch (error) {
        console.error('FluentLayout: unable to synchronize the breakpoint with .NET.', error);
      }

      // With NavigationDeferredLoading, .NET renders the nav area only now: sync again.
      if (changed) {
        updateLayoutHamburgers(element);
      }
    }
  }

  /**
   * Updates every hamburger of this layout, but not those of a nested layout.
   * They all share the same nav area on desktop.
   */
  function updateLayoutHamburgers(layoutContainer: HTMLElement): void {
    layoutContainer.querySelectorAll<HTMLElement>('.fluent-layout-hamburger')
      .forEach(hamburger => {
        if (hamburger.closest('.fluent-layout') === layoutContainer) {
          updateHamburgerExpanded(hamburger, layoutContainer);
        }
      });
  }

  /**
   * Checks the layout width against its configured mobile breakpoint, defaulting to 768px.
   */
  function isMobile(element: HTMLElement): boolean {
    const maxWidth = parseInt(element.getAttribute('breakdown-width') ?? '768', 10);
    return element.offsetWidth <= maxWidth;
  }

  /**
   * Sets aria-expanded and aria-controls on the hamburger button, not on the role-less wrapper div.
   * On desktop they refer to the nav area (aria-controls only when the nav area has an id),
   * on mobile to the drawer.
   * The state is always read from the DOM (nav [hidden], drawer dialog open): aria-expanded is output only.
   * @param hamburger The hamburger wrapper element
   * @param layoutContainer The parent layout, if any
   */
  function updateHamburgerExpanded(hamburger: HTMLElement, layoutContainer: HTMLElement | null): void {
    const button = hamburger.querySelector('fluent-button');
    if (!button) {
      return;
    }

    const layoutNav = layoutContainer?.querySelector('.fluent-layout-item[area="nav"]') ?? null;
    const drawerId = hamburger.id + '-drawer';
    const drawer = document.getElementById(drawerId) as any;
    const controlsNav = !!(layoutContainer && layoutNav && !isMobile(layoutContainer));
    const isExpanded = controlsNav
      ? !layoutNav!.hasAttribute('hidden')
      : drawer?.dialog?.open === true;
    const controls = controlsNav ? layoutNav!.id : drawerId;

    button.setAttribute('aria-expanded', isExpanded ? 'true' : 'false');
    if (controls) {
      button.setAttribute('aria-controls', controls);
    }
    else {
      button.removeAttribute('aria-controls');
    }
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

    // Initial state (e.g. the nav area is visible on desktop)
    updateHamburgerExpanded(element, layoutContainer);

    if (element) {

      dialog?.addEventListener('click', (event: MouseEvent) => {
        const target = event.target;

        if (target instanceof Element && target.closest('.fluent-navitem')) {
          dialog.hide();
        }
      });

      // Registered once, so the state stays right however the drawer opens or closes
      // (hamburger click, ShowAsync/HideAsync, Escape, close button, nav item click).
      dialog?.addEventListener('toggle', (e: any) => {
        if (e.target !== dialog) {
          return;
        }

        const newState = (e.detail?.newState ?? e.newState) === 'open';

        updateHamburgerExpanded(element, layoutContainer);
        try {
          element._fluentHamburgerDotNetHelper?.invokeMethodAsync('FluentLayout_HamburgerClickAsync', newState);
        }
        catch (error) {
        }
      });

      element.addEventListener('click', (event: MouseEvent) => {
        const layoutNav = layoutContainer?.querySelector('.fluent-layout-item[area="nav"]') ?? null;
        const isMobileSize = isMobile(layoutContainer ?? document.body);

        // Show or hide the nav area (aria-expanded="true" when the nav is visible)
        if (layoutNav && !isMobileSize) {
          layoutNav.toggleAttribute('hidden', !layoutNav.hasAttribute('hidden'));
          updateLayoutHamburgers(layoutContainer!);
          updateHamburgerExpanded(element, layoutContainer);
        }

        // Show or hide the fluent-drawer: the 'toggle' event above updates aria-expanded
        else if (isMobileSize && dialog) {
          if (dialog.dialog?.open) {
            dialog.hide();
          }
          else {
            dialog.show();
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
   * Also called after each enhanced navigation, which merges the server HTML into the DOM.
   */
  export function LayoutAutoInitialize() {

    document.querySelectorAll<HTMLElement>('.fluent-layout').forEach(element => {
      if (!element.id) {
        return;
      }

      if (!(element as any).fluentLayoutInitialized) {
        Initialize(null, element.id);
      }

      element.querySelectorAll<HTMLElement>('.fluent-layout-hamburger').forEach(hamburger => {
        if (!hamburger.id) {
          return;
        }

        // Enhanced navigation keeps this element but resets its button to the server-rendered
        // aria-expanded="false" and removes the nav area's [hidden]: re-sync the state.
        if ((hamburger as any).fluentHamburgerInitialized) {
          updateHamburgerExpanded(hamburger, element);
          return;
        }

        HamburgerInitialize(null, hamburger.id, element.id);
      });

    });
  }
}
