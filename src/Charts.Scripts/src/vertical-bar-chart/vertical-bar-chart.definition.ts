import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './vertical-bar-chart.styles.js';
import { template } from './vertical-bar-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-vertical-bar-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-vertical-bar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
