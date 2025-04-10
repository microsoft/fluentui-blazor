export namespace Microsoft.FluentUI.Blazor.Components.Tabs {

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
  }
}
