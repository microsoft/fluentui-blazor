import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './chart-legend.styles.js';
import { template } from './chart-legend.template.js';

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
