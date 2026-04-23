import * as FluentUIComponents from '@fluentui/web-components'
import { defineOnce } from './RegistrationState';


export namespace Microsoft.FluentUI.Blazor.FluentUIWebComponents {

  /**
   * Initialize and define all the FluentUI WebComponents
   */
  export function defineComponents() {
    const registry = FluentUIComponents.FluentDesignSystem.registry;

    // To generate these definitions, run the `_ExtractWebComponents.ps1` file
    // and paste the output here.
    defineOnce('fluentui:web-components:accordion', () => {
      FluentUIComponents.accordionDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:accordion-item', () => {
      FluentUIComponents.accordionItemDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:anchor-button', () => {
      FluentUIComponents.AnchorButtonDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:avatar', () => {
      FluentUIComponents.AvatarDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:badge', () => {
      FluentUIComponents.BadgeDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:button', () => {
      FluentUIComponents.ButtonDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:checkbox', () => {
      FluentUIComponents.CheckboxDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:compound-button', () => {
      FluentUIComponents.CompoundButtonDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:counter-badge', () => {
      FluentUIComponents.CounterBadgeDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:dialog-body', () => {
      FluentUIComponents.DialogBodyDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:dialog', () => {
      FluentUIComponents.DialogDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:divider', () => {
      FluentUIComponents.DividerDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:drawer-body', () => {
      FluentUIComponents.DrawerBodyDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:drawer', () => {
      FluentUIComponents.DrawerDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:dropdown', () => {
      FluentUIComponents.DropdownDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:dropdown-option', () => {
      FluentUIComponents.DropdownOptionDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:field', () => {
      FluentUIComponents.FieldDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:image', () => {
      FluentUIComponents.ImageDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:label', () => {
      FluentUIComponents.LabelDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:link', () => {
      FluentUIComponents.LinkDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:listbox', () => {
      FluentUIComponents.ListboxDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:menu-button', () => {
      FluentUIComponents.MenuButtonDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:menu', () => {
      FluentUIComponents.MenuDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:menu-item', () => {
      FluentUIComponents.MenuItemDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:menu-list', () => {
      FluentUIComponents.MenuListDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:message-bar', () => {
      FluentUIComponents.MessageBarDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:progress-bar', () => {
      FluentUIComponents.ProgressBarDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:radio', () => {
      FluentUIComponents.RadioDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:radio-group', () => {
      FluentUIComponents.RadioGroupDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:rating-display', () => {
      FluentUIComponents.RatingDisplayDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:slider', () => {
      FluentUIComponents.SliderDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:spinner', () => {
      FluentUIComponents.SpinnerDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:switch', () => {
      FluentUIComponents.SwitchDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:tab', () => {
      FluentUIComponents.TabDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:tablist', () => {
      FluentUIComponents.TablistDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:text-area', () => {
      FluentUIComponents.TextAreaDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:text', () => {
      FluentUIComponents.TextDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:text-input', () => {
      FluentUIComponents.TextInputDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:toggle-button', () => {
      FluentUIComponents.ToggleButtonDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:tooltip', () => {
      FluentUIComponents.TooltipDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:tree', () => {
      FluentUIComponents.TreeDefinition.define(registry);
    });
    defineOnce('fluentui:web-components:tree-item', () => {
      FluentUIComponents.TreeItemDefinition.define(registry);
    });
  }
}
