export function afterStarted(Blazor) {
    Blazor.registerCustomEventType('checkedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.currentChecked
            };
        }
    });
    Blazor.registerCustomEventType('tabchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                activeId: event.detail.id,
                affectedId: event.detail.attributes['tab-id'].value
            };
        }
    });
    Blazor.registerCustomEventType('selectedchange', {
        browserEventName: 'selected-change',
        createEventArgs: event => {
            return {
                affectedId: event.detail.attributes['tree-item-id'].value
            };
        }
    });
    Blazor.registerCustomEventType('expandedchange', {
        browserEventName: 'expanded-change',
        createEventArgs: event => {
            return {
                affectedId: event.detail.attributes['tree-item-id'].value
            };
        }
    });
    Blazor.registerCustomEventType('dateselected', {
        browserEventName: 'dateselected',
        createEventArgs: event => {
            return {
                calendarDateInfo: event.detail
            };
        }
    });
    //Blazor.registerCustomEventType('accordionchange', {
    //    browserEventName: 'change',
    //    createEventArgs: event => {
    //        return {
    //            activeId: event.detail.id,
    //            affectedId: event.detail.attributes['accordion-item-id'].value
    //        };
    //    }
    //});
    Blazor.registerCustomEventType('dismiss', {
        browserEventName: 'dismiss',
        createEventArgs: event => {
            return {
                event: event
            };
        }
    });
    Blazor.registerCustomEventType('menuchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                event: event
            };
        }
    });
    Blazor.registerCustomEventType('scrollstart', {
        browserEventName: 'scrollstart',
        createEventArgs: event => {
            return {
                scroll: event.detail
            };
        }
    });
    Blazor.registerCustomEventType('scrollend', {
        browserEventName: 'scrollend',
        createEventArgs: event => {
            return {
                scroll: event.detail
            };
        }
    });
    Blazor.registerCustomEventType('cellfocus', {
        browserEventName: 'cell-focused',
        createEventArgs: event => {
            return {
                cellId: event.detail.attributes['cell-id'].value
            };
        }
    });
    Blazor.registerCustomEventType('rowfocus', {
        browserEventName: 'row-focused',
        createEventArgs: event => {
            return {
                rowId: event.detail.attributes['row-id'].value
            };
        }
    });
}