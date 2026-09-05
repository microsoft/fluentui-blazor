import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './sparkline-chart.styles.js';
import { template } from './sparkline-chart.template.js';

/** @public @remarks HTML Element: `<fluent-sparkline-chart>` */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-sparkline-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: { delegatesFocus: true },
};
