import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './funnel-chart.styles.js';
import { template } from './funnel-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-funnel-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-funnel-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
