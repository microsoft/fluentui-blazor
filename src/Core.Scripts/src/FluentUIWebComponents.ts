import * as FluentUIComponents from '@fluentui/web-components'
import { css, ViewTemplate } from '@microsoft/fast-element';
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
      // Keep this inside the shadow root to target [open]: external ::part styles cannot filter that attribute,
      // and :open is unsupported in WebKit 26. Unconditional display: flex would also reveal closed dialogs.
      const dialogDefinition: typeof FluentUIComponents.DialogDefinition = {
        ...FluentUIComponents.DialogDefinition,
        styles: [FluentUIComponents.DialogDefinition.styles ?? [], css`
          :host([fuib]:not([position])) dialog[open] {
            display: flex;
            flex-direction: column;
          }
        `].flat(),
      };
      FluentUIComponents.Dialog.define(dialogDefinition);
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
      // Expose the internal `.control` element as `part="control"`,
      // so it can be styled from outside the shadow DOM (e.g. `fluent-dropdown::part(control) { min-width: 0; }`).
      const controlTag = '<div class="control">';
      const template = FluentUIComponents.DropdownDefinition.template as unknown as ViewTemplate;
      const canExposeControl = template instanceof ViewTemplate
        && typeof template.html === 'string'
        && template.html.includes(controlTag);

      const dropdownDefinition: typeof FluentUIComponents.DropdownDefinition = {
        // Spread the original dropdown definition first, so we can override the template and styles below.
        ...FluentUIComponents.DropdownDefinition,

        // Expose the internal `.control` element as `part="control"` if possible.
        template: canExposeControl
          ? new ViewTemplate((template.html as string).replace(controlTag, '<div class="control" part="control">'), template.factories) as unknown as typeof FluentUIComponents.DropdownDefinition.template
          : FluentUIComponents.DropdownDefinition.template,
        
        // Allow grid/flex shrinkage and keep the indicator visible with long selected text.
        styles: [FluentUIComponents.DropdownDefinition.styles ?? [], css`
          :host {
            min-width: 0;
          }

          .control {
            min-width: 0;
            grid-template-columns: minmax(0, 1fr) var(--_indicator-size);
          }
        `].flat(),
      };

      FluentUIComponents.Dropdown.define(dropdownDefinition);
    });
    defineOnce('fluentui:web-components:dropdown-option', () => {
      FluentUIComponents.DropdownOption.define(FluentUIComponents.DropdownOptionDefinition);
    });
    defineOnce('fluentui:web-components:field', () => {
      FluentUIComponents.Field.define(FluentUIComponents.FieldDefinition);

      // Width is applied to the field wrapper; keep this default overridable by component and user styles.
      // Because of the dropdown and listbox whose styles we've overridden in this file
      const listWidthStyles = new CSSStyleSheet();
      listWidthStyles.replaceSync(`
        :where(fluent-field:has(> fluent-dropdown, > .fluent-listbox)) {
          width: 160px;
        }

        :where(fluent-field > .fluent-listbox) {
          min-width: 0;
        }
      `);
      document.adoptedStyleSheets = [...document.adoptedStyleSheets, listWidthStyles];
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
      const listboxDefinition: typeof FluentUIComponents.ListboxDefinition = {
        ...FluentUIComponents.ListboxDefinition,
        // Remove the `min-inline-size: 160px` imposed by the web component.
        styles: [FluentUIComponents.ListboxDefinition.styles ?? [], css`
          :host {
            min-inline-size: 0;
          }
        `].flat(),
      };
      FluentUIComponents.Listbox.define(listboxDefinition);
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
