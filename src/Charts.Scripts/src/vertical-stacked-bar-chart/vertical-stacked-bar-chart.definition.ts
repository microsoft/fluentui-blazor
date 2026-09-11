import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './vertical-stacked-bar-chart.styles.js';
import { template } from './vertical-stacked-bar-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-vertical-stacked-bar-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-vertical-stacked-bar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
