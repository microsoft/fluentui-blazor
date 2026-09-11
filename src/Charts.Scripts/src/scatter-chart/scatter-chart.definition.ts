import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './scatter-chart.styles.js';
import { template } from './scatter-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-scatter-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-scatter-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
