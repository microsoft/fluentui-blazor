import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.Layout {

  /**
   * Add an attribute to the layout element if the width is less than the maximum mobile width
   * @param id
   * @param maxMobileWidth
  */
  export function Initialize(dotNetHelper: DotNet.DotNetObject, id: string, maxMobileWidth: number) {

    const layoutElement = document.getElementById(id);

    if (layoutElement) {

      // Detect the layout size, and add a "mobile" attribute
      // if the width is less than the maximum mobile width
      const resizeObserver = new ResizeObserver(entries => {
        const hasMobileAttribute = layoutElement.hasAttribute('mobile');
        const isMobileSize = entries.some(entry => {
          return entry.borderBoxSize.length > 0
            ? entry.borderBoxSize[0].inlineSize <= maxMobileWidth
            : false;
        });

        if (!hasMobileAttribute && isMobileSize) {          
          layoutElement.setAttribute('mobile', '');
          dotNetHelper.invokeMethodAsync('FluentLayout_MediaChangedAsync', "mobile");
        }
        else if (hasMobileAttribute && !isMobileSize) {
          layoutElement.removeAttribute('mobile');
          dotNetHelper.invokeMethodAsync('FluentLayout_MediaChangedAsync', "desktop");
        }
      });

      resizeObserver.observe(layoutElement);
    }
  }
}
