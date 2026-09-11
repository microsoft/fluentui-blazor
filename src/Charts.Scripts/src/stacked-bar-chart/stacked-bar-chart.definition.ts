import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './stacked-bar-chart.styles.js';
import { template } from './stacked-bar-chart.template.js';

/** @public @remarks HTML Element: `<fluent-stacked-bar-chart>` */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-stacked-bar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: { delegatesFocus: true },
};
