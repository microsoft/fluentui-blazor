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
                activeId: event.detail.id
            };
        }
    });
    Blazor.registerCustomEventType('selectedchange', {
        browserEventName: 'selected-change',
        createEventArgs: event => {
            return {
                affectedItem: event.detail.id
            };
        }
    });
    Blazor.registerCustomEventType('expandedchange', {
        browserEventName: 'expanded-change',
        createEventArgs: event => {
            return {
                affectedItem: event.detail.id
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
    Blazor.registerCustomEventType('accordionchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                activeId: event.detail.id
            };
        }
    });
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
    Blazor.registerCustomEventType('cellfocused', {
        browserEventName: 'cell-focused',
        createEventArgs: event => {
            return {
                cell: event.detail
            };
        }
    });
    Blazor.registerCustomEventType('rowfocused', {
        browserEventName: 'row-focused',
        createEventArgs: event => {
            return {
                row: event.detail
            };
        }
    });
}