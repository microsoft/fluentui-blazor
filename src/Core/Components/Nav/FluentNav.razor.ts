export namespace Microsoft.FluentUI.Blazor.Nav {

  // Fluent motion tokens (matching React's motionTokens)
  const DURATION_FAST = 100;
  const DURATION_ULTRA_SLOW = 500;
  const CURVE_DECELERATE_MID = 'cubic-bezier(0, 0, 0, 1)';
  const CURVE_ACCELERATE_MIN = 'cubic-bezier(0.8, 0, 0.78, 1)';

  /**
   * Calculates animation duration based on item count (like React's NavGroupMotion)
   */
  function calculateDuration(itemCount: number, isSmallDensity: boolean): number {
    const durationPerItem = isSmallDensity ? 15 : 25;
    const baseDuration = DURATION_FAST + itemCount * durationPerItem;
    return Math.min(baseDuration, DURATION_ULTRA_SLOW);
  }

  /**
   * Creates keyframes for expand/collapse animation
   * Keyframes are defined once and can be reversed for collapse.
   */
  function createKeyframes(height: number): Keyframe[] {
    return [
      {
        minHeight: 0,
        height: 0
      },
      {
        minHeight: `${height}px`,
        height: `${height}px`
      }
    ];
  }


  /**
   * Animates expansion of a category group element using Web Animations API.
   */
  export function AnimateExpand(groupId: string, density: string = 'medium'): void {
    const group = document.getElementById(groupId) as HTMLElement;
    if (!group) return;

    group.getAnimations().forEach(anim => anim.cancel());

    const computedStyles = window.getComputedStyle(group);
    const isAlreadyVisible = computedStyles.overflow === 'visible';

    if (isAlreadyVisible) {
      group.style.height = 'auto';
      group.style.minHeight = 'auto';
      group.style.opacity = '1';
      group.style.overflow = 'visible';
      return;
    }

    const itemCount = group.children.length;
    const isSmallDensity = density === 'small';
    const targetHeight = group.scrollHeight;
    const duration = calculateDuration(itemCount, isSmallDensity);

    const keyframes = createKeyframes(targetHeight);

    group.style.overflowY = 'hidden';
    group.style.overflowX = 'visible';

    const animation = group.animate(keyframes, {
      duration: duration,
      easing: CURVE_DECELERATE_MID,
      fill: 'forwards'
    });

    animation.onfinish = () => {
      group.style.height = 'auto';
      group.style.minHeight = 'auto';
      group.style.opacity = '1';
      group.style.overflow = 'visible';

      const nav = group.closest('.fluent-nav') as HTMLElement;
      if (nav) UpdateTabIndices(nav);
    };
  }

  /**
   * Animates collapse of a category group element using Web Animations API.
   * Returns a Promise that resolves when the animation completes.
   */
  export function AnimateCollapse(groupId: string, density: string = 'medium'): Promise<void> {
    return new Promise((resolve) => {
      const group = document.getElementById(groupId) as HTMLElement;
      if (!group) {
        resolve();
        return;
      }

      group.getAnimations().forEach(anim => anim.cancel());

      const itemCount = group.children.length;
      const isSmallDensity = density === 'small';
      const currentHeight = group.scrollHeight;
      const duration = calculateDuration(itemCount, isSmallDensity);

      const keyframes = [...createKeyframes(currentHeight)].reverse();

      group.style.overflowY = 'hidden';
      group.style.overflowX = 'visible';

      const animation = group.animate(keyframes, {
        duration: duration,
        easing: CURVE_ACCELERATE_MIN,
        fill: 'forwards'
      });

      animation.onfinish = () => {
        group.style.height = '0px';
        group.style.minHeight = '0px';
        group.style.opacity = '0';

        const nav = group.closest('.fluent-nav') as HTMLElement;
        if (nav) UpdateTabIndices(nav);

        resolve();
      };
    });
  }

  type NavHandlers = { keydown: (e: KeyboardEvent) => void; focusin: (e: FocusEvent) => void };
  const _navHandlers = new Map<string, NavHandlers>();

  /**
   * Initializes keyboard navigation and roving tabindex for the nav menu.
   */
  export function Initialize(navId: string): void {
    const nav = document.getElementById(navId);
    if (!nav) return;

    UpdateTabIndices(nav);

    const keydownHandler = (e: KeyboardEvent) => {
      if (e.key === 'ArrowDown' || e.key === 'ArrowUp' || e.key === 'Home' || e.key === 'End') {
        const target = e.target as HTMLElement;
        if (!target || (!target.classList.contains('fluent-navitem') && !target.classList.contains('fluent-navcategoryitem'))) {
          return;
        }

        const items = getVisibleItems(nav);
        if (items.length === 0) return;

        const currentIndex = items.indexOf(target);
        if (currentIndex === -1) return;

        e.preventDefault();

        let nextIndex = currentIndex;
        if (e.key === 'ArrowDown') {
          nextIndex = (currentIndex + 1) % items.length;
        } else if (e.key === 'ArrowUp') {
          nextIndex = (currentIndex - 1 + items.length) % items.length;
        } else if (e.key === 'Home') {
          nextIndex = 0;
        } else if (e.key === 'End') {
          nextIndex = items.length - 1;
        }

        items[nextIndex].focus();
      }
    };

    const focusinHandler = (e: FocusEvent) => {
      const target = e.target as HTMLElement;
      if (target.classList.contains('fluent-navitem') || target.classList.contains('fluent-navcategoryitem')) {
        UpdateTabIndices(nav, target);
      }
    };

    nav.addEventListener('keydown', keydownHandler);
    nav.addEventListener('focusin', focusinHandler);

    _navHandlers.set(navId, { keydown: keydownHandler, focusin: focusinHandler });
  }

  /**
   * Removes event listeners added by Initialize and cleans up resources.
   */
  export function Dispose(navId: string): void {
    const nav = document.getElementById(navId);
    const handlers = _navHandlers.get(navId);
    if (nav && handlers) {
      nav.removeEventListener('keydown', handlers.keydown);
      nav.removeEventListener('focusin', handlers.focusin);
    }
    _navHandlers.delete(navId);
  }

  function getVisibleItems(nav: HTMLElement): HTMLElement[] {
    return Array.from(nav.querySelectorAll('.fluent-navitem, .fluent-navcategoryitem'))
      .filter(el => {
        if ((el as HTMLElement).classList.contains('disabled')) return false;
        const parentGroup = el.closest('.fluent-navsubitemgroup');
        return !parentGroup || parentGroup.classList.contains('expanded');
      }) as HTMLElement[];
  }

  function UpdateTabIndices(nav: HTMLElement, activeItem: HTMLElement | null = null): void {
    const items = Array.from(nav.querySelectorAll('.fluent-navitem, .fluent-navcategoryitem')) as HTMLElement[];
    const visibleItems = getVisibleItems(nav);

    if (!activeItem) {
      activeItem = visibleItems.find(el => el.classList.contains('active')) || visibleItems[0];
    }

    items.forEach(el => {
      if (el === activeItem) {
        el.setAttribute('tabindex', '0');
      } else {
        el.setAttribute('tabindex', '-1');
      }
    });
  }
}
