export namespace Microsoft.FluentUI.Blazor.Nav {

  // Fluent motion tokens (matching React's motionTokens)
  const DURATION_FAST = 150;
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
        opacity: 0,
        minHeight: 0,
        height: 0
      },
      {
        opacity: 1,
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

    group.style.overflow = 'hidden';

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

      group.style.overflow = 'hidden';

      const animation = group.animate(keyframes, {
        duration: duration,
        easing: CURVE_ACCELERATE_MIN,
        fill: 'forwards'
      });

      animation.onfinish = () => {
        group.style.height = '0px';
        group.style.minHeight = '0px';
        group.style.opacity = '0';
        resolve();
      };
    });
  }
}
