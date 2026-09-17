export namespace Microsoft.FluentUI.Blazor.Grid {

  function GetMediaQueries() {
    return [
      { id: 'xs', items: (document as any)._fluentGrid.mediaXS, query: '(max-width: 599.98px)' },
      { id: 'sm', items: (document as any)._fluentGrid.mediaSM, query: '(min-width: 600px) and (max-width: 959.98px)' },
      { id: 'md', items: (document as any)._fluentGrid.mediaMD, query: '(min-width: 960px) and (max-width: 1279.98px)' },
      { id: 'lg', items: (document as any)._fluentGrid.mediaLG, query: '(min-width: 1280px) and (max-width: 1919.98px)' },
      { id: 'xl', items: (document as any)._fluentGrid.mediaXL, query: '(min-width: 1920px) and (max-width: 2559.98px)' },
      { id: 'xxl', items: (document as any)._fluentGrid.mediaXXL, query: '(min-width: 2560px)' },
    ];
  }

  export function FluentGridInitialize(id: string, dotNetHelper: any) {

    // Create a single instance of the media queries
    if (!(document as any)._fluentGrid) {
      (document as any)._fluentGrid = {
        mediaXS: [],
        mediaSM: [],
        mediaMD: [],
        mediaLG: [],
        mediaXL: [],
        mediaXXL: [],
      }

      // Add event listeners for each media query
      GetMediaQueries().forEach((mediaQuery) => {
        window.matchMedia(mediaQuery.query)
          .addEventListener('change', media => {
            if (media.matches) {
              mediaQuery.items.forEach((item: any) => {
                item.dotNetHelper.invokeMethodAsync('FluentGrid_MediaChangedAsync', mediaQuery.id);
              });
            }
          });
      });
    }

    // Add the item to each media query
    (document as any)._fluentGrid.mediaXS.push({ id: id, dotNetHelper: dotNetHelper });
    (document as any)._fluentGrid.mediaSM.push({ id: id, dotNetHelper: dotNetHelper });
    (document as any)._fluentGrid.mediaMD.push({ id: id, dotNetHelper: dotNetHelper });
    (document as any)._fluentGrid.mediaLG.push({ id: id, dotNetHelper: dotNetHelper });
    (document as any)._fluentGrid.mediaXL.push({ id: id, dotNetHelper: dotNetHelper });
    (document as any)._fluentGrid.mediaXXL.push({ id: id, dotNetHelper: dotNetHelper });

    // First check
    GetMediaQueries().forEach((mediaQuery) => {
      if (window.matchMedia(mediaQuery.query).matches) {
        dotNetHelper.invokeMethodAsync('FluentGrid_MediaChangedAsync', mediaQuery.id);
      }
    });
  }

  export function FluentGridCleanup(id: string) {
    if ((document as any)._fluentGrid) {
      RemoveItem((document as any)._fluentGrid.mediaXS, id);
      RemoveItem((document as any)._fluentGrid.mediaSM, id);
      RemoveItem((document as any)._fluentGrid.mediaMD, id);
      RemoveItem((document as any)._fluentGrid.mediaLG, id);
      RemoveItem((document as any)._fluentGrid.mediaXL, id);
      RemoveItem((document as any)._fluentGrid.mediaXXL, id);
    }
  }

  // Remove the Array item where item.id is found
  function RemoveItem(array: any, id: string) {
    for (var i = array.length - 1; i >= 0; i--) {
      if (array[i].id === id) {
        array.splice(i, 1);
      }
    }
  }

}
