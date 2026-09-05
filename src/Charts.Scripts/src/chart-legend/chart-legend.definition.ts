import { FluentDesignSystem, MenuButton, Menu, MenuItem, MenuList } from '@fluentui/web-components';
import { MenuButtonDefinition, MenuDefinition, MenuItemDefinition, MenuListDefinition } from '@fluentui/web-components';

import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './chart-legend.styles.js';
import { template } from './chart-legend.template.js';

MenuButton.define(MenuButtonDefinition);
MenuList.define(MenuListDefinition);
MenuItem.define(MenuItemDefinition);
Menu.define(MenuDefinition);

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-chart-legend>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-chart-legend`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
};
