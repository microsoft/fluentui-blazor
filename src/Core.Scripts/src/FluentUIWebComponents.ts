import * as FluentUIComponents from '@fluentui/web-components'
import * as Theme from './Utilities/Theme'

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
  }
}
