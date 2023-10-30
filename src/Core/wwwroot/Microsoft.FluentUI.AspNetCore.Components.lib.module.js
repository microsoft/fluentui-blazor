import { SplitPanels } from "./js/SplitPanels.js";

var beforeStartCalled = false;
var afterStartedCalled = false;

export function beforeWebStart(options, extensions) {
    if (!beforeStartCalled) {
        beforeStart(options, extensions);
    }
}

export function afterWebStarted(blazor) {
    if (!afterStartedCalled) {
        afterStarted(blazor);
    }
}


export function beforeServerStart(options, extensions) {
    if (!beforeStartCalled) {
        beforeStart(options, extensions);
    }
}

export function afterServerStarted(blazor) {
    if (!afterStartedCalled) {
        afterStarted(blazor);
    }
}


export function beforeWebAssemblyStart(options, extensions) {
    if (!beforeStartCalled) {
        beforeStart(options, extensions);
    }
}

export function afterWebAssemblyStarted(blazor) {
    if (!afterStartedCalled)
        afterStarted(blazor);
}


export function afterStarted(blazor) {

    customElements.define("split-panels", SplitPanels);

    blazor.registerCustomEventType('radiogroupclick', {
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
    blazor.registerCustomEventType('checkedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.currentChecked
            };
        }
    });
    blazor.registerCustomEventType('switchcheckedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.checked
            };
        }
    });
    blazor.registerCustomEventType('accordionchange', {
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
    blazor.registerCustomEventType('tabchange', {
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
    blazor.registerCustomEventType('selectedchange', {
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
    blazor.registerCustomEventType('expandedchange', {
        browserEventName: 'expanded-change',
        createEventArgs: event => {
            return {
                affectedId: event.detail.attributes['id'].value,
                selected: event.detail._selected,
                expanded: event.detail._expanded
            };
        }
    });
    blazor.registerCustomEventType('dateselected', {
        browserEventName: 'dateselected',
        createEventArgs: event => {
            return {
                calendarDateInfo: event.detail
            };
        }
    });
    blazor.registerCustomEventType('tooltipdismiss', {
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
    blazor.registerCustomEventType('dialogdismiss', {
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
    blazor.registerCustomEventType('menuchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                id: event.target.id,
                value: event.target.innerText
            };
        }
    });
    blazor.registerCustomEventType('scrollstart', {
        browserEventName: 'scrollstart',
        createEventArgs: event => {
            return {
                scroll: event.detail
            };
        }
    });
    blazor.registerCustomEventType('scrollend', {
        browserEventName: 'scrollend',
        createEventArgs: event => {
            return {
                scroll: event.detail
            };
        }
    });
    blazor.registerCustomEventType('cellfocus', {
        browserEventName: 'cell-focused',
        createEventArgs: event => {
            return {
                cellId: event.detail.attributes['cell-id'].value
            };
        }
    });
    blazor.registerCustomEventType('rowfocus', {
        browserEventName: 'row-focused',
        createEventArgs: event => {
            return {
                rowId: event.detail.attributes['row-id'].value
            };
        }
    });
    blazor.registerCustomEventType('sizechanged', {
        browserEventName: 'sizechanged',
        createEventArgs: event => {
            return event;
        }
    });

    afterStartedCalled = true;
}

export function beforeStart(options, extensions) {
    var wcScript = document.createElement('script');
    wcScript.type = 'module';
    wcScript.src = './_content/Microsoft.FluentUI.AspNetCore.Components/js/web-components-v2.5.16.min.js';
    wcScript.async = true;
    document.body.appendChild(wcScript);

    var libraryStyle = document.createElement('link');
    libraryStyle.rel = 'stylesheet';
    libraryStyle.type = 'text/css';
    libraryStyle.href = './_content/Microsoft.FluentUI.AspNetCore.Components/css/Microsoft.FluentUI.AspNetCore.Components.css';
    document.head.appendChild(libraryStyle);

    beforeStartCalled = true;
}

