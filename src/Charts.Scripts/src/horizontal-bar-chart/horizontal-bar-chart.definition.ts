import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './horizontal-bar-chart.styles.js';
import { template } from './horizontal-bar-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-horizontal-bar-chart>`
 */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-horizontal-bar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
};
