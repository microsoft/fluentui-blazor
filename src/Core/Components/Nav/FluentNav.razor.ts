export namespace Microsoft.FluentUI.Blazor.Nav {

  /**
   * Animates the nav panel open.
   */
  export function AnimateNavOpen(navContainerId: string): void {
    const navContainer = document.getElementById(navContainerId)?.parentElement as HTMLElement;
    if (!navContainer) return;

    navContainer.style.display = '';
    void navContainer.offsetHeight;
  }

  /**
   * Animates the nav panel closed.
   */
  export function AnimateNavClose(navContainerId: string): void {
    const navContainer = document.getElementById(navContainerId)?.parentElement as HTMLElement;
    if (!navContainer) return;

    void navContainer.offsetHeight;

    navContainer.addEventListener('transitionend', (e) => {
      if (e.propertyName === 'transform') {
        navContainer.style.display = 'none';
      }
    }, { once: true });
  }

  /**
   * Animates expansion of a category group element.
   */
  export function AnimateExpand(groupId: string): void {
    const group = document.getElementById(groupId) as HTMLElement;
    if (!group) return;

    const height = group.scrollHeight;

    group.style.maxHeight = '0px';
    group.style.opacity = '0';

    void group.offsetHeight;

    requestAnimationFrame(() => {
      group.style.maxHeight = `${height}px`;
      group.style.opacity = '1';
    });

    group.addEventListener('transitionend', (e) => {
      if (e.propertyName === 'max-height' && group.classList.contains('expanded')) {
        group.style.maxHeight = 'none';
      }
    }, { once: true });
  }

  /**
   * Animates collapse of a category group element.
   */
  export function AnimateCollapse(groupId: string): void {
    const group = document.getElementById(groupId) as HTMLElement;
    if (!group) return;

    const height = group.scrollHeight;

    group.style.maxHeight = `${height}px`;
    group.style.opacity = '1';

    void group.offsetHeight;

    requestAnimationFrame(() => {
      group.style.maxHeight = '0px';
      group.style.opacity = '0';
    });
  }
}
