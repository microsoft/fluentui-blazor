import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './polar-chart.styles.js';
import { template } from './polar-chart.template.js';

/** @public @remarks HTML Element: `<fluent-polar-chart>` */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-polar-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: { delegatesFocus: true },
};
