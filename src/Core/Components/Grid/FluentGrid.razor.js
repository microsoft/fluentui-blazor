function GetMediaQueries() {
    return [
        { id: 'xs', items: document._fluentGrid.mediaXS, query: '(max-width: 599px)' },
        { id: 'sm', items: document._fluentGrid.mediaSM, query: '(min-width: 600px) and (max-width: 959px)' },
        { id: 'md', items: document._fluentGrid.mediaMD, query: '(min-width: 960px) and (max-width: 1279px)' },
        { id: 'lg', items: document._fluentGrid.mediaLG, query: '(min-width: 1280px) and (max-width: 1919px)' },
        { id: 'xl', items: document._fluentGrid.mediaXL, query: '(min-width: 1920px) and (max-width: 2559px)' },
        { id: 'xxl', items: document._fluentGrid.mediaXXL, query: '(min-width: 2560px)' },
    ];
}

export function FluentGridInitialize(id, dotNetHelper) {

    // Create a single instance of the media queries
    if (!document._fluentGrid) {
        document._fluentGrid = {
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
                          mediaQuery.items.forEach((item) => {
                              item.dotNetHelper.invokeMethodAsync('FluentGrid_MediaChangedAsync', mediaQuery.id);
                          });
                      }
                  });
        });
    }

    // Add the item to each media query
    document._fluentGrid.mediaXS.push({ id: id, dotNetHelper: dotNetHelper });
    document._fluentGrid.mediaSM.push({ id: id, dotNetHelper: dotNetHelper });
    document._fluentGrid.mediaMD.push({ id: id, dotNetHelper: dotNetHelper });
    document._fluentGrid.mediaLG.push({ id: id, dotNetHelper: dotNetHelper });
    document._fluentGrid.mediaXL.push({ id: id, dotNetHelper: dotNetHelper });
    document._fluentGrid.mediaXXL.push({ id: id, dotNetHelper: dotNetHelper });

    // First check
    GetMediaQueries().forEach((mediaQuery) => {
        if (window.matchMedia(mediaQuery.query).matches) {
            dotNetHelper.invokeMethodAsync('FluentGrid_MediaChangedAsync', mediaQuery.id);
        }
    });
}

export function FluentGridCleanup(id, dotNetHelper) {
    if (document._fluentGrid) {
        RemoveItem(document._fluentGrid.mediaXS, id);
        RemoveItem(document._fluentGrid.mediaSM, id);
        RemoveItem(document._fluentGrid.mediaMD, id);
        RemoveItem(document._fluentGrid.mediaLG, id);
        RemoveItem(document._fluentGrid.mediaXL, id);
        RemoveItem(document._fluentGrid.mediaXXL, id);
    }
}

// Remove the Array item where item.id is found
function RemoveItem(array, id) {
    for (var i = array.length - 1; i >= 0; i--) {
        if (array[i].id === id) {
            array.splice(i, 1);
        }
    }
}