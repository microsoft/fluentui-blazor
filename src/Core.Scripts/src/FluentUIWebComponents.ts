import * as FluentUIComponents from '@fluentui/web-components'
import { defineOnce } from './RegistrationState';


export namespace Microsoft.FluentUI.Blazor.FluentUIWebComponents {

  /**
   * Initialize and define all the FluentUI WebComponents
   */
  export function defineComponents() {
    // To generate these definitions, run the `_ExtractWebComponents.ps1` file
    // and paste the output here.
    defineOnce('fluentui:web-components:accordion', () => {
      FluentUIComponents.Accordion.define(FluentUIComponents.AccordionDefinition);
    });
    defineOnce('fluentui:web-components:accordion-item', () => {
      FluentUIComponents.AccordionItem.define(FluentUIComponents.AccordionItemDefinition);
    });
    defineOnce('fluentui:web-components:anchor-button', () => {
      FluentUIComponents.AnchorButton.define(FluentUIComponents.AnchorButtonDefinition);
    });
    defineOnce('fluentui:web-components:avatar', () => {
      FluentUIComponents.Avatar.define(FluentUIComponents.AvatarDefinition);
    });
    defineOnce('fluentui:web-components:badge', () => {
      FluentUIComponents.Badge.define(FluentUIComponents.BadgeDefinition);
    });
    defineOnce('fluentui:web-components:button', () => {
      FluentUIComponents.Button.define(FluentUIComponents.ButtonDefinition);
    });
    defineOnce('fluentui:web-components:checkbox', () => {
      FluentUIComponents.Checkbox.define(FluentUIComponents.CheckboxDefinition);
    });
    defineOnce('fluentui:web-components:compound-button', () => {
      FluentUIComponents.CompoundButton.define(FluentUIComponents.CompoundButtonDefinition);
    });
    defineOnce('fluentui:web-components:counter-badge', () => {
      FluentUIComponents.CounterBadge.define(FluentUIComponents.CounterBadgeDefinition);
    });
    defineOnce('fluentui:web-components:dialog-body', () => {
      FluentUIComponents.DialogBody.define(FluentUIComponents.DialogBodyDefinition);
    });
    defineOnce('fluentui:web-components:dialog', () => {
      FluentUIComponents.Dialog.define(FluentUIComponents.DialogDefinition);
    });
    defineOnce('fluentui:web-components:divider', () => {
      FluentUIComponents.Divider.define(FluentUIComponents.DividerDefinition);
    });
    defineOnce('fluentui:web-components:drawer-body', () => {
      FluentUIComponents.DrawerBody.define(FluentUIComponents.DrawerBodyDefinition);
    });
    defineOnce('fluentui:web-components:drawer', () => {
      FluentUIComponents.Drawer.define(FluentUIComponents.DrawerDefinition);
    });
    defineOnce('fluentui:web-components:dropdown', () => {
      FluentUIComponents.Dropdown.define(FluentUIComponents.DropdownDefinition);
    });
    defineOnce('fluentui:web-components:dropdown-option', () => {
      FluentUIComponents.DropdownOption.define(FluentUIComponents.DropdownOptionDefinition);
    });
    defineOnce('fluentui:web-components:field', () => {
      FluentUIComponents.Field.define(FluentUIComponents.FieldDefinition);
    });
    defineOnce('fluentui:web-components:image', () => {
      FluentUIComponents.Image.define(FluentUIComponents.ImageDefinition);
    });
    defineOnce('fluentui:web-components:label', () => {
      FluentUIComponents.Label.define(FluentUIComponents.LabelDefinition);
    });
    defineOnce('fluentui:web-components:link', () => {
      FluentUIComponents.Link.define(FluentUIComponents.LinkDefinition);
    });
    defineOnce('fluentui:web-components:listbox', () => {
      FluentUIComponents.Listbox.define(FluentUIComponents.ListboxDefinition);
    });
    defineOnce('fluentui:web-components:menu-button', () => {
      FluentUIComponents.MenuButton.define(FluentUIComponents.MenuButtonDefinition);
    });
    defineOnce('fluentui:web-components:menu', () => {
      FluentUIComponents.Menu.define(FluentUIComponents.MenuDefinition);
    });
    defineOnce('fluentui:web-components:menu-item', () => {
      FluentUIComponents.MenuItem.define(FluentUIComponents.MenuItemDefinition);
    });
    defineOnce('fluentui:web-components:menu-list', () => {
      FluentUIComponents.MenuList.define(FluentUIComponents.MenuListDefinition);
    });
    defineOnce('fluentui:web-components:message-bar', () => {
      FluentUIComponents.MessageBar.define(FluentUIComponents.MessageBarDefinition);
    });
    defineOnce('fluentui:web-components:progress-bar', () => {
      FluentUIComponents.ProgressBar.define(FluentUIComponents.ProgressBarDefinition);
    });
    defineOnce('fluentui:web-components:radio', () => {
      FluentUIComponents.Radio.define(FluentUIComponents.RadioDefinition);
    });
    defineOnce('fluentui:web-components:radio-group', () => {
      FluentUIComponents.RadioGroup.define(FluentUIComponents.RadioGroupDefinition);
    });
    defineOnce('fluentui:web-components:rating-display', () => {
      FluentUIComponents.RatingDisplay.define(FluentUIComponents.RatingDisplayDefinition);
    });
    defineOnce('fluentui:web-components:slider', () => {
      FluentUIComponents.Slider.define(FluentUIComponents.SliderDefinition);
    });
    defineOnce('fluentui:web-components:spinner', () => {
      FluentUIComponents.Spinner.define(FluentUIComponents.SpinnerDefinition);
    });
    defineOnce('fluentui:web-components:switch', () => {
      FluentUIComponents.Switch.define(FluentUIComponents.SwitchDefinition);
    });
    defineOnce('fluentui:web-components:tab', () => {
      FluentUIComponents.Tab.define(FluentUIComponents.TabDefinition);
    });
    defineOnce('fluentui:web-components:tablist', () => {
      FluentUIComponents.Tablist.define(FluentUIComponents.TablistDefinition);
    });
    defineOnce('fluentui:web-components:text-area', () => {
      FluentUIComponents.TextArea.define(FluentUIComponents.TextAreaDefinition);
    });
    defineOnce('fluentui:web-components:text', () => {
      FluentUIComponents.Text.define(FluentUIComponents.TextDefinition);
    });
    defineOnce('fluentui:web-components:text-input', () => {
      FluentUIComponents.TextInput.define(FluentUIComponents.TextInputDefinition);
    });
    defineOnce('fluentui:web-components:toggle-button', () => {
      FluentUIComponents.ToggleButton.define(FluentUIComponents.ToggleButtonDefinition);
    });
    defineOnce('fluentui:web-components:tooltip', () => {
      FluentUIComponents.Tooltip.define(FluentUIComponents.TooltipDefinition);
    });
    defineOnce('fluentui:web-components:tree', () => {
      FluentUIComponents.Tree.define(FluentUIComponents.TreeDefinition);
    });
    defineOnce('fluentui:web-components:tree-item', () => {
      FluentUIComponents.TreeItem.define(FluentUIComponents.TreeItemDefinition);
    });
  }
}
