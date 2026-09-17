import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './donut-chart.styles.js';
import { template } from './donut-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-donut-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-donut-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
