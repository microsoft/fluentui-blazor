export namespace Microsoft.FluentUI.Blazor.Components.Layout {

  /**
   * Add an attribute to the layout element if the width is less than the maximum mobile width
   * @param id
   * @param maxMobileWidth
   */
  export function Initialize(id: string, maxMobileWidth: number) {

    const layoutElement = document.getElementById(id);

    if (layoutElement) {

      // Detect the layout size, and add a "mobile" attribute
      // if the width is less than the maximum mobile width
      const resizeObserver = new ResizeObserver(entries => {
        const hasMobileAttribute = layoutElement.hasAttribute('mobile');
        const isMobileSize = entries.some(entry => entry.contentRect.width <= maxMobileWidth);

        if (!hasMobileAttribute && isMobileSize) {
          console.log(isMobileSize);
          layoutElement.setAttribute('mobile', '');
        }
        else if (hasMobileAttribute && !isMobileSize) {
          console.log(isMobileSize);
          layoutElement.removeAttribute('mobile');
        }
      });

      resizeObserver.observe(layoutElement);
    }
  }
}
