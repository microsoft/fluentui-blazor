

import {
  FluentDesignSystem,
  setTheme,
  accordionItemDefinition,
  accordionDefinition,
  AnchorButtonDefinition,
  AvatarDefinition,
  BadgeDefinition,
  ButtonDefinition,
  CheckboxDefinition,
  CompoundButtonDefinition,
  CounterBadgeDefinition,
  DialogDefinition,
  DialogBodyDefinition,
  DividerDefinition,
  FieldDefinition,
  ImageDefinition,
  LabelDefinition,
  MenuButtonDefinition,
  MenuItemDefinition,
  MenuListDefinition,
  MenuDefinition,
  ProgressBarDefinition,
  RadioGroupDefinition,
  RadioDefinition,
  SliderDefinition,
  SpinnerDefinition,
  SwitchDefinition,
  TabPanelDefinition,
  TabDefinition,
  TabsDefinition,
  TextInputDefinition,
  TextDefinition,
  ToggleButtonDefinition
} from '@fluentui/web-components'

import { webLightTheme, webDarkTheme, BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { SplitPanels } from './SplitPanels'
import { FluentPageScript, onEnhancedLoad } from './FluentPageScript'
import { DesignTheme } from './DesignTheme'



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
    --font-monospace: var(--fontFamilyMonospace);
    --success: var(--colorStatusSuccessForeground1);
    --warning: var(--colorStatusWarningForeground1);
    --error: var(--colorStatusDangerForeground1);
    --info: var(--colorPalettePlatinumForeground2);
    --presence-available: var(--colorPaletteLightGreenForeground3);
    --presence-away: var(--colorPaletteMarigoldBackground3);
    --presence-busy: var(--colorPaletteRedBackground3);
    --presence-dnd: var(--colorPaletteRedBackground3);
    --presence-offline: var(--colorNeutralForeground3);
    --presence-oof: var(--colorPaletteBerryForeground3);
    --presence-blocked: var(--colorPaletteRedBackground3);
    --presence-unknown: var(--colorNeutralForeground3);
    --highlight-bg: #fff3cd;
    --design-unit: 4px;

}


[role='checkbox'].invalid::part(control),
[role='combobox'].invalid::part(control),
fluent-combobox.invalid::part(control),
fluent-text-area.invalid::part(control),
fluent-text-field.invalid::part(root)
{
    outline: calc(var(--stroke-width) * 1px)  solid var(--error);
}

`;

styleSheet.replaceSync(styles);
// document.adoptedStyleSheets.push(styleSheet);
document.adoptedStyleSheets = [...document.adoptedStyleSheets, styleSheet];

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
    afterStarted(blazor, 'wasm');
  }
}

export function beforeServerStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterServerStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor, 'server');
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
          expanded: event.target!._expanded
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
    customElements.define('fluent-page-script', FluentPageScript);
    blazor.addEventListener('enhancedload', onEnhancedLoad);
  }

  afterStartedCalled = true;
}

export function beforeStart(options: any) {



  const myNewTheme: BrandVariants = {
    10: "#050205",
    20: "#231121",
    30: "#3C183A",
    40: "#511C4E",
    50: "#661F63",
    60: "#7D2279",
    70: "#94248F",
    80: "#AA28A5",
    90: "#B443AE",
    100: "#BD59B6",
    110: "#C66EBF",
    120: "#CF82C7",
    130: "#D795D0",
    140: "#DFA8D9",
    150: "#E7BBE1",
    160: "#EECEEA"
  };

  const lightTheme: Theme = {
    ...createLightTheme(myNewTheme),
  };

  const darkTheme: Theme = {
    ...createDarkTheme(myNewTheme),
  };


  darkTheme.colorBrandForeground1 = myNewTheme[110];
  darkTheme.colorBrandForeground2 = myNewTheme[120];

  setTheme(webLightTheme);

  accordionItemDefinition.define(FluentDesignSystem.registry);
  accordionDefinition.define(FluentDesignSystem.registry);
  AnchorButtonDefinition.define(FluentDesignSystem.registry);
  AvatarDefinition.define(FluentDesignSystem.registry);
  BadgeDefinition.define(FluentDesignSystem.registry);
  ButtonDefinition.define(FluentDesignSystem.registry);
  CheckboxDefinition.define(FluentDesignSystem.registry);
  CompoundButtonDefinition.define(FluentDesignSystem.registry);
  CounterBadgeDefinition.define(FluentDesignSystem.registry);
  DialogDefinition.define(FluentDesignSystem.registry);
  DialogBodyDefinition.define(FluentDesignSystem.registry);
  DividerDefinition.define(FluentDesignSystem.registry);
  FieldDefinition.define(FluentDesignSystem.registry);
  ImageDefinition.define(FluentDesignSystem.registry);
  LabelDefinition.define(FluentDesignSystem.registry);
  MenuButtonDefinition.define(FluentDesignSystem.registry);
  MenuItemDefinition.define(FluentDesignSystem.registry);
  MenuListDefinition.define(FluentDesignSystem.registry);
  MenuDefinition.define(FluentDesignSystem.registry);
  ProgressBarDefinition.define(FluentDesignSystem.registry);
  RadioGroupDefinition.define(FluentDesignSystem.registry);
  RadioDefinition.define(FluentDesignSystem.registry);
  SliderDefinition.define(FluentDesignSystem.registry);
  SpinnerDefinition.define(FluentDesignSystem.registry);
  SwitchDefinition.define(FluentDesignSystem.registry);
  TabPanelDefinition.define(FluentDesignSystem.registry);
  TabDefinition.define(FluentDesignSystem.registry);
  TabsDefinition.define(FluentDesignSystem.registry);
  TextInputDefinition.define(FluentDesignSystem.registry);
  TextDefinition.define(FluentDesignSystem.registry);
  ToggleButtonDefinition.define(FluentDesignSystem.registry);

  customElements.define("fluent-design-theme", DesignTheme);
  customElements.define("split-panels", SplitPanels);

  beforeStartCalled = true;
}
