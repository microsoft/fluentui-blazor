import * as FluentUIComponents from '@fluentui/web-components'
import { webLightTheme, webDarkTheme, BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { setTheme } from '@fluentui/web-components';

export namespace Microsoft.FluentUI.Blazor.FluentUIWebComponents {

  /**
   * Initialize and define all the FluentUI WebComponents
   */
  export function defineComponents() {
    const registry = FluentUIComponents.FluentDesignSystem.registry;

    // To generate these definitions, run the `_ExtractWebComponents.ps1` file
    // and paste the output here.

    FluentUIComponents.accordionDefinition.define(registry);
    FluentUIComponents.accordionItemDefinition.define(registry);
    FluentUIComponents.AnchorButtonDefinition.define(registry);
    FluentUIComponents.AvatarDefinition.define(registry);
    FluentUIComponents.BadgeDefinition.define(registry);
    FluentUIComponents.ButtonDefinition.define(registry);
    FluentUIComponents.CheckboxDefinition.define(registry);
    FluentUIComponents.CompoundButtonDefinition.define(registry);
    FluentUIComponents.CounterBadgeDefinition.define(registry);
    FluentUIComponents.DialogBodyDefinition.define(registry);
    FluentUIComponents.DialogDefinition.define(registry);
    FluentUIComponents.DividerDefinition.define(registry);
    FluentUIComponents.DrawerBodyDefinition.define(registry);
    FluentUIComponents.DrawerDefinition.define(registry);
    FluentUIComponents.DropdownDefinition.define(registry);
    FluentUIComponents.DropdownOptionDefinition.define(registry);
    FluentUIComponents.FieldDefinition.define(registry);
    FluentUIComponents.ImageDefinition.define(registry);
    FluentUIComponents.LabelDefinition.define(registry);
    FluentUIComponents.LinkDefinition.define(registry);
    FluentUIComponents.ListboxDefinition.define(registry);
    FluentUIComponents.MenuButtonDefinition.define(registry);
    FluentUIComponents.MenuDefinition.define(registry);
    FluentUIComponents.MenuItemDefinition.define(registry);
    FluentUIComponents.MenuListDefinition.define(registry);
    FluentUIComponents.MessageBarDefinition.define(registry);
    FluentUIComponents.ProgressBarDefinition.define(registry);
    FluentUIComponents.RadioDefinition.define(registry);
    FluentUIComponents.RadioGroupDefinition.define(registry);
    FluentUIComponents.RatingDisplayDefinition.define(registry);
    FluentUIComponents.SliderDefinition.define(registry);
    FluentUIComponents.SpinnerDefinition.define(registry);
    FluentUIComponents.SwitchDefinition.define(registry);
    FluentUIComponents.TabDefinition.define(registry);
    FluentUIComponents.TablistDefinition.define(registry);
    FluentUIComponents.TabPanelDefinition.define(registry);
    FluentUIComponents.TabsDefinition.define(registry);
    FluentUIComponents.TextAreaDefinition.define(registry);
    FluentUIComponents.TextDefinition.define(registry);
    FluentUIComponents.TextInputDefinition.define(registry);
    FluentUIComponents.ToggleButtonDefinition.define(registry);
    FluentUIComponents.TooltipDefinition.define(registry);
    FluentUIComponents.TreeItemDefinition.define(registry);
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

    setTheme(webLightTheme);

    blazor.theme = {
      isSystemDark: () => {
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
      },

      isDarkMode: () => {
        const luminance: string = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
        return parseFloat(luminance) < 0.5;
      },

      setLightTheme: () => {
        setTheme(webLightTheme);
      },

      setDarkTheme: () => {
        setTheme(webDarkTheme);
      }
    }
  }
}
