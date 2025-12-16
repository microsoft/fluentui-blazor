export namespace Microsoft.FluentUI.Blazor.Nav {

  // FluentNav panel toggle
  export function ToggleNav(id: string, expanded: boolean): void {
    const navContainer = document.getElementById(id)?.parentElement as HTMLElement;

    if (!navContainer)
      return;

    if (expanded) {
      collapseNav(navContainer);
    } else {
      expandNav(navContainer);
    }
  }

  // Pure animation functions - Blazor owns all state
  // These functions only animate based on the element ID passed from Blazor
  
  /**
   * Animates expansion of a category group element.
   * Blazor has already set the 'expanded' class - this just animates the transition.
   */
  export function AnimateExpand(groupId: string): void {
    const group = document.getElementById(groupId) as HTMLElement;
    if (!group) return;

    const height = group.scrollHeight;
    
    // Start from collapsed state
    group.style.maxHeight = '0px';
    group.style.opacity = '0';

    // Force a reflow to ensure the starting state is applied
    void group.offsetHeight;

    // Animate to expanded state
    requestAnimationFrame(() => {
      group.style.maxHeight = `${height}px`;
      group.style.opacity = '1';
    });

    // Remove maxHeight constraint after animation completes to allow dynamic content
    group.addEventListener('transitionend', (e) => {
      if (e.propertyName === 'max-height' && group.classList.contains('expanded')) {
        group.style.maxHeight = 'none';
      }
    }, { once: true });
  }

  /**
   * Animates collapse of a category group element.
   * Blazor has already removed the 'expanded' class - this just animates the transition.
   */
  export function AnimateCollapse(groupId: string): void {
    const group = document.getElementById(groupId) as HTMLElement;
    if (!group) return;

    const height = group.scrollHeight;

    // Set to actual height first to enable transition
    group.style.maxHeight = `${height}px`;
    group.style.opacity = '1';

    // Force a reflow to ensure the starting state is applied
    void group.offsetHeight;

    // Animate to collapsed state
    requestAnimationFrame(() => {
      group.style.maxHeight = '0px';
      group.style.opacity = '0';
    });
  }

  // Nav panel animation helpers using CSS transitions
  function expandNav(element: HTMLElement): void {
    // Make element visible before animating
    element.style.display = '';

    element.classList.remove('collapsed');

    // Force a reflow to ensure the starting state is applied
    void element.offsetHeight;
  }

  function collapseNav(element: HTMLElement): void {
    element.classList.add('collapsed');

    // Force a reflow to ensure the starting state is applied
    void element.offsetHeight;

    // Hide after animation completes
    // Only respond to transform to avoid double-firing (opacity also transitions)
    element.addEventListener('transitionend', (e) => {
      if (e.propertyName === 'transform') {
        element.style.display = 'none';
      }
    }, { once: true });
  }

}
