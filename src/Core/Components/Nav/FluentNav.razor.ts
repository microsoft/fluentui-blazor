export namespace Microsoft.FluentUI.Blazor.Nav {

  export function ToggleNav(id: string, expanded: boolean): void {
    const navContainer = document.getElementById(id)?.parentElement as HTMLElement;

    if (!navContainer)
      return;

    if (expanded) {
      collapse(navContainer);
    } else {
      expand(navContainer);
    }
  }


  // Animation helpers
  function getPositionTransform(position: 'start' | 'end', sizeVar: string, dir: 'ltr' | 'rtl'): string {
    const leftToRightTransform = `translate3d(var(${sizeVar}), 0, 0)`;
    const rightToLeftTransform = `translate3d(calc(var(${sizeVar}) * -1), 0, 0)`;

    if (position === 'start') {
      return dir === 'ltr' ? rightToLeftTransform : leftToRightTransform;
    }
    if (position === 'end') {
      return dir === 'ltr' ? leftToRightTransform : rightToLeftTransform;
    }
    return 'translate3d(0, 0, 0)';
  }

  function animateNav(element: HTMLElement, position: 'start' | 'end', sizeVar: string, dir: 'ltr' | 'rtl', show: boolean): Animation {
    const keyframes = show
      ? [
          { transform: getPositionTransform(position, sizeVar, dir), opacity: 0 },
          { transform: 'translate3d(0, 0, 0)', opacity: 1 }
        ]
      : [
          { transform: 'translate3d(0, 0, 0)', opacity: 1 },
          { transform: getPositionTransform(position, sizeVar, dir), opacity: 0 }
        ];
    return element.animate(keyframes, {
      duration: 300,
      easing: 'cubic-bezier(0.33, 0, 0.67, 1)'
    });
  }

  function expand(element: HTMLElement): void {
    // Make element visible before animating
    element.style.display = '';
    // Detect direction (LTR or RTL)
    const dir = getComputedStyle(element).direction || document.dir || "ltr";
    // Use CSS variable for nav size, fallback to 260px if not set
    const sizeVar = '--nav-width';
    const sizeValue = getComputedStyle(element).getPropertyValue(sizeVar) || '260px';
    // Assume position 'start' for nav, can be parameterized if needed
    const position: 'start' | 'end' = 'start';

    // Set the CSS variable for animation
    element.style.setProperty(sizeVar, sizeValue);

    // Animate in
    const animation = animateNav(element, position, sizeVar, dir as 'ltr' | 'rtl', true);
    animation.onfinish = () => {
      element.classList.add('expanded');
      element.style.transform = '';
      element.style.opacity = '1';
      //element.style.boxShadow = '0 4px 24px 0 rgba(0,0,0,0.12)';
    };
  }

  function collapse(element: HTMLElement): void {
    const dir = getComputedStyle(element).direction || document.dir || "ltr";
    const sizeVar = '--nav-width';
    const sizeValue = getComputedStyle(element).getPropertyValue(sizeVar) || '260px';
    const position: 'start' | 'end' = 'start';

    element.style.setProperty(sizeVar, sizeValue);

    // Animate out
    const animation = animateNav(element, position, sizeVar, dir as 'ltr' | 'rtl', false);
    animation.onfinish = () => {
      element.classList.remove('expanded');
      element.style.transform = '';
      element.style.opacity = '0';
      //element.style.boxShadow = '';
      element.style.display = 'none'; // Hide after animation
    };
  }

}
