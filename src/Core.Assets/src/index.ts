export * from '@fluentui/web-components/dist/web-components'
import { SplitPanels } from './SplitPanels'
import { DesignTheme } from './DesignTheme'
import { FluentPageScript, onEnhancedLoad } from './FluentPageScript'

interface Blazor {
  registerCustomEventType: (
    name: string,
    options: CustomEventTypeOptions) => void;

  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean
  }
  addEventListener: (name: string, callback: (event: any) => void) => void;
}

interface CustomEventTypeOptions {
  browserEventName: string;
  createEventArgs: (event: FluentUIEventType) => any;
}

interface FluentUIEventType {
  target: any;
  detail: any;
  _readOnly: any;
  type: string;
}


var styleSheet = new CSSStyleSheet();

const styles = `
body:has(.prevent-scroll) {
    overflow: hidden;
}
:root {
    --font-monospace: Consolas, "Courier New", "Liberation Mono", SFMono-Regular, Menlo, Monaco, monospace;
    --success: #0E700E;
    --warning: #E9835E;
    --error: #BC2F32;
    --info: #616161;
    --presence-available: #13a10e;
    --presence-away: #eaa300;
    --presence-busy: #d13438;
    --presence-dnd: #d13438;
    --presence-offline: #adadad;
    --presence-oof: #c239b3;
    --presence-unknown: #d13438;
    --highlight-bg: #fff3cd;
}
`;

styleSheet.replaceSync(styles);
document.adoptedStyleSheets.push(styleSheet);

var beforeStartCalled = false;
var afterStartedCalled = false;


export function afterWebStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor, 'web');
  }
}

export function beforeWebStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function beforeWebAssemblyStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterWebAssemblyStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor,'wasm');
  }
}

export function beforeServerStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterServerStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor,'server');
  }
}

export function afterStarted(blazor: Blazor, mode: string) {

  blazor.registerCustomEventType('radiogroupclick', {
    browserEventName: 'click',
    createEventArgs: event => {
      if (event.target!._readOnly || event.target!._disabled) {
        return null;
      }
      return {
        value: event.target!.value
      };
    }
  });

  blazor.registerCustomEventType('checkedchange', {
    browserEventName: 'change',
    createEventArgs: event => {

      // Hacking of a fake update
      if (event.target!.isUpdating) {
        return {
          checked: null,
          indeterminate: null
        }
      }

      return {
        checked: event.target!.currentChecked,
        indeterminate: event.target!.indeterminate
      };
    }
  });

  blazor.registerCustomEventType('switchcheckedchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      return {
        checked: event.target!.checked
      };
    }
  });

  blazor.registerCustomEventType('accordionchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-accordion-item') {
        return {
          activeId: event.target!.id,
        }
      };
      return null;
    }
  });

  blazor.registerCustomEventType('tabchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-tabs') {
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
      if (event.target!.localName == 'fluent-tree-item') {
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
      if (event.target!.localName == 'fluent-tooltip') {
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
      if (event.target!.localName == 'fluent-dialog') {
        return {
          id: event.target!.id,
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
        id: event.target!.id,
        value: event.target!.innerText
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

  blazor.registerCustomEventType('splitterresized', {
    browserEventName: 'splitterresized',
    createEventArgs: event => {
      return {
        panel1size: event.detail.panel1size,
        panel2size: event.detail.panel2size
      }
    }
  });
  blazor.registerCustomEventType('splittercollapsed', {
    browserEventName: 'splittercollapsed',
    createEventArgs: event => {
      return {
        collapsed: event.detail.collapsed
      }
    }
  });

  blazor.theme = {
    isSystemDark: () => {
      return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    },

    isDarkMode: () => {
      const luminance: string = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
      return parseFloat(luminance) < 0.5;
    }
  }


  if (typeof blazor.addEventListener === 'function' && mode === 'web') {
    blazor.addEventListener('enhancedload', onEnhancedLoad);
  }

  afterStartedCalled = true;
}

export function beforeStart(options: any) {
  customElements.define("fluent-design-theme", DesignTheme);
  customElements.define("split-panels", SplitPanels);
  customElements.define('fluent-page-script', FluentPageScript);

  beforeStartCalled = true;
}
