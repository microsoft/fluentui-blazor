import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './gantt-chart.styles.js';
import { template } from './gantt-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-gantt-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-gantt-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
