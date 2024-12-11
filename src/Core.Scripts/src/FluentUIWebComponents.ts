import * as FluentUIComponents from '@fluentui/web-components'
import { webLightTheme, webDarkTheme, BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { setTheme } from '@fluentui/web-components';

export namespace Microsoft.FluentUI.Blazor.FluentUIWebComponents {

  /**
   * Initialize and define all the FluentUI WebComponents
   */
  export function defineComponents() {
    const registry = FluentUIComponents.FluentDesignSystem.registry;

    FluentUIComponents.accordionItemDefinition.define(registry);
    FluentUIComponents.accordionDefinition.define(registry);
    FluentUIComponents.AnchorButtonDefinition.define(registry);
    FluentUIComponents.AvatarDefinition.define(registry);
    FluentUIComponents.BadgeDefinition.define(registry);
    FluentUIComponents.ButtonDefinition.define(registry);
    FluentUIComponents.CheckboxDefinition.define(registry);
    FluentUIComponents.CompoundButtonDefinition.define(registry);
    FluentUIComponents.CounterBadgeDefinition.define(registry);
    FluentUIComponents.DialogDefinition.define(registry);
    FluentUIComponents.DialogBodyDefinition.define(registry);
    FluentUIComponents.DividerDefinition.define(registry);
    FluentUIComponents.FieldDefinition.define(registry);
    FluentUIComponents.ImageDefinition.define(registry);
    FluentUIComponents.LabelDefinition.define(registry);
    FluentUIComponents.MenuButtonDefinition.define(registry);
    FluentUIComponents.MenuItemDefinition.define(registry);
    FluentUIComponents.MenuListDefinition.define(registry);
    FluentUIComponents.MenuDefinition.define(registry);
    FluentUIComponents.ProgressBarDefinition.define(registry);
    FluentUIComponents.RadioGroupDefinition.define(registry);
    FluentUIComponents.RadioDefinition.define(registry);
    FluentUIComponents.SliderDefinition.define(registry);
    FluentUIComponents.SpinnerDefinition.define(registry);
    FluentUIComponents.SwitchDefinition.define(registry);
    FluentUIComponents.TabPanelDefinition.define(registry);
    FluentUIComponents.TabDefinition.define(registry);
    FluentUIComponents.TabsDefinition.define(registry);
    FluentUIComponents.TextInputDefinition.define(registry);
    FluentUIComponents.TextDefinition.define(registry);
    FluentUIComponents.ToggleButtonDefinition.define(registry);
  }

  /**
   * Initialize the FluentUI Theme
   */
  export function initializeTheme(blazor: Blazor) {

    const themeColorVariants: BrandVariants = {
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
      ...createLightTheme(themeColorVariants),
    };

    const darkTheme: Theme = {
      ...createDarkTheme(themeColorVariants),
    };


    darkTheme.colorBrandForeground1 = themeColorVariants[110];
    darkTheme.colorBrandForeground2 = themeColorVariants[120];

    setTheme(webLightTheme);


    blazor.theme = {
      isSystemDark: () => {
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
      },

      isDarkMode: () => {
        const luminance: string = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
        return parseFloat(luminance) < 0.5;
      }
    }
  }
}
