import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './grouped-vertical-bar-chart.styles.js';
import { template } from './grouped-vertical-bar-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-grouped-vertical-bar-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-grouped-vertical-bar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
