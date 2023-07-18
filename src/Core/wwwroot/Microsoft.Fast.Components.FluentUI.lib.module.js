import { SplitPanels } from "./js/SplitPanels.js";

export function afterStarted(Blazor) {

    customElements.define("split-panels", SplitPanels);

    Blazor.registerCustomEventType('customclick', {
        browserEventName: 'click',
        createEventArgs: event => {
            if (event.target._readOnly || event.target._disabled) {
                return null;
            }
            return {
                value: event.target.value
            };
        }
    });
    Blazor.registerCustomEventType('checkedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.currentChecked
            };
        }
    });
    Blazor.registerCustomEventType('switchcheckedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.checked
            };
        }
    });
    Blazor.registerCustomEventType('accordionchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            if (event.target.localName == 'fluent-accordion-item') {
                return {
                    activeId: event.target.id,
                }
            };
            return null;
        }
    });
    Blazor.registerCustomEventType('tabchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            if (event.target.localName == 'fluent-tabs') {
                return {
                    activeId: event.detail.id,
                }
            };
            return null;
        }
    });
    Blazor.registerCustomEventType('selectedchange', {
        browserEventName: 'selected-change',
        createEventArgs: event => {
            if (event.target.localName == 'fluent-tree-item') {
                return {
                    affectedId: event.detail.attributes['id'].value,
                    selected: event.detail._selected,
                    expanded: event.detail._expanded
                }
            };
            return null;
        }
    });
    Blazor.registerCustomEventType('expandedchange', {
        browserEventName: 'expanded-change',
        createEventArgs: event => {
            return {
                affectedId: event.detail.attributes['id'].value,
                selected: event.detail._selected,
                expanded: event.detail._expanded
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
    Blazor.registerCustomEventType('tooltipdismiss', {
        browserEventName: 'dismiss',
        createEventArgs: event => {
            if (event.target.localName == 'fluent-tooltip') {
                return {
                    reason: event.type
                };
            };
            return null;
        }
    });
    Blazor.registerCustomEventType('dialogdismiss', {
        browserEventName: 'dismiss',
        createEventArgs: event => {
            if (event.target.localName == 'fluent-dialog') {
                return {
                    id: event.target.id,
                    reason: event.type
                };
            };
            return null;
        }
    });
    Blazor.registerCustomEventType('menuchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                id: event.target.id,
                value: event.target.innerText
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
    Blazor.registerCustomEventType('sizechanged', {
        browserEventName: 'sizechanged',
        createEventArgs: event => {
            return event;
        }
    });
}

export function beforeStart(options, extensions) {
    var wcScript = document.createElement('script');
    wcScript.type = 'module';
    wcScript.src = './_content/Microsoft.Fast.Components.FluentUI/js/web-components.min.js';
    wcScript.async = true;
    document.head.appendChild(wcScript);
}

