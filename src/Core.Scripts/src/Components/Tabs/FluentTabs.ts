export namespace Microsoft.FluentUI.Blazor.Components.Tabs {
  const observers = new Map<string, MutationObserver>();
  const keyboardControllers = new Map<string, AbortController>();

  /**
   * Initiates the list of tabs when a tab is added or removed
   * @param id The id of the fluent-tablist container to refresh
   */
  export function ObserveTabsChanged(id: string): void {
    const tabsContainer = document.getElementById(id) as HTMLElement | null;
    const tabsList = tabsContainer?.querySelector('fluent-tablist') as HTMLElement | null;

    if (!tabsContainer || !tabsList) {
      return;
    }

    Dispose(id);

    // Listen for keydown events on the tabs container to handle overflow navigation
    const keyboardController = new AbortController();
    tabsContainer.addEventListener('keydown',
      (event: KeyboardEvent) => handleOverflowKeyDown(event, id, tabsContainer, tabsList),
      { capture: true, signal: keyboardController.signal });
    keyboardControllers.set(id, keyboardController);

    // Observe for childList mutations (tab-panel additions/removals) in the tabs container
    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.type === 'childList') {
          const addedNodes = Array.from(mutation.addedNodes).filter(
            (node) => node instanceof HTMLElement && node.classList.contains('fluent-tab-panel')
          );
          const removedNodes = Array.from(mutation.removedNodes).filter(
            (node) => node instanceof HTMLElement && node.classList.contains('fluent-tab-panel')
          );

          if (addedNodes.length > 0 || removedNodes.length > 0) {
            // Call the tabsChanged method on the fluent-tablist element
            (tabsList as any).tabsChanged();
          }
        }
      });
    });

    observer.observe(tabsContainer, { childList: true });
    observers.set(id, observer);
  }

  /** Stops observing tab-panel additions and removals for a FluentTabs instance. */
  export function Dispose(id: string): void {
    observers.get(id)?.disconnect();
    observers.delete(id);
    keyboardControllers.get(id)?.abort();
    keyboardControllers.delete(id);
  }

  /** Handles keydown events for the overflow menu navigation. */
  function handleOverflowKeyDown(
    event: KeyboardEvent,
    id: string,
    tabsContainer: HTMLElement,
    tabsList: HTMLElement
  ): void {
    if (event.defaultPrevented
      || tabsList.getAttribute('orientation') === 'vertical'
      || (event.key !== 'ArrowRight' && event.key !== 'ArrowLeft')
      || !tabsList.querySelector('fluent-tab[overflow]')) {
      return;
    }

    const menuButton = document.getElementById(`${id}-more`) as HTMLElement | null;
    if (!menuButton || !tabsContainer.contains(menuButton) || menuButton.hasAttribute('disabled')) {
      return;
    }

    const visibleTabs = Array.from(
      tabsList.querySelectorAll<HTMLElement>('fluent-tab:not([hidden]):not([disabled])')
    );
    const lastVisibleTab = visibleTabs.at(-1);
    if (!lastVisibleTab) {
      return;
    }

    const source = event.composedPath().find(node => node instanceof HTMLElement);
    if (event.key === 'ArrowRight' && source === lastVisibleTab) {
      event.preventDefault();
      menuButton.focus();
    } else if (event.key === 'ArrowLeft' && source === menuButton) {
      event.preventDefault();
      lastVisibleTab.focus();
    }
  }
}
