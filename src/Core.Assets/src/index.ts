export * from '@fluentui/web-components/dist/web-components'
import { SplitPanels } from './SplitPanels'
import { DesignTheme } from './DesignTheme'

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

const pageScriptInfoBySrc = new Map();

function registerPageScriptElement(src: any) {
  if (!src) {
    throw new Error('Must provide a non-empty value for the "src" attribute.');
  }

  let pageScriptInfo = pageScriptInfoBySrc.get(src);

  if (pageScriptInfo) {
    pageScriptInfo.referenceCount++;
  } else {
    pageScriptInfo = { referenceCount: 1, module: null };
    pageScriptInfoBySrc.set(src, pageScriptInfo);
    initializePageScriptModule(src, pageScriptInfo);
  }
}

function unregisterPageScriptElement(src: any) {
  if (!src) {
    return;
  }

  const pageScriptInfo = pageScriptInfoBySrc.get(src);
  if (!pageScriptInfo) {
    return;
  }

  pageScriptInfo.referenceCount--;
}

async function initializePageScriptModule(src: any, pageScriptInfo: any) {
  if (src.startsWith("./")) {
    src = new URL(src.substr(2), document.baseURI).toString();
  }

  const module = await import(src);

  if (pageScriptInfo.referenceCount <= 0) {
    return;
  }

  pageScriptInfo.module = module;
  module.onLoad?.();
  module.onUpdate?.();
}

function onEnhancedLoad() {
  for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
    if (referenceCount <= 0) {
      module?.onDispose?.();
      pageScriptInfoBySrc.delete(src);
    }
  }

  for (const { module } of pageScriptInfoBySrc.values()) {
    module?.onUpdate?.();
  }
}

export function afterWebStarted(blazor: any) {
  customElements.define('page-script', class extends HTMLElement {
    static observedAttributes = ['src'];
    src: string | null = null;

    attributeChangedCallback(name: any, oldValue: any, newValue: any) {
      if (name !== 'src') {
        return;
      }

      this.src = newValue;
      unregisterPageScriptElement(oldValue);
      registerPageScriptElement(newValue);
    }

    disconnectedCallback() {
      unregisterPageScriptElement(this.src);
    }
  });

  blazor.addEventListener('enhancedload', onEnhancedLoad);
  if (!afterStartedCalled) {
    afterStarted(blazor);
  }
}


interface Blazor {
  registerCustomEventType: (
    name: string,
    options: CustomeventTypeOptions) => void;

  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean
  }
}

interface CustomeventTypeOptions {
  browserEventName: string;
  createEventArgs: (event: FluentUIEventType) => any;
}

interface FluentUIEventType {
  target: any;
  detail: any;
  _readOnly: any;
  type: string;
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
    afterStarted(blazor);
  }
}

export function beforeServerStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterServerStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor);
  }
}

export function afterStarted(blazor: Blazor) {

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

  afterStartedCalled = true;
}

export function beforeStart(options: any) {

  customElements.define("fluent-design-theme", DesignTheme);
  customElements.define("split-panels", SplitPanels);

  beforeStartCalled = true;
}
