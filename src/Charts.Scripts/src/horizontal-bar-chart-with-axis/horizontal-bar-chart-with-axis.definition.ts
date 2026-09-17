import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './horizontal-bar-chart-with-axis.styles.js';
import { template } from './horizontal-bar-chart-with-axis.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-horizontal-bar-chart-with-axis>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-horizontal-bar-chart-with-axis`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
