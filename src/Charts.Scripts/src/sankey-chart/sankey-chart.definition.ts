import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './sankey-chart.styles.js';
import { template } from './sankey-chart.template.js';

/** @public @remarks HTML Element: `<fluent-sankey-chart>` */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-sankey-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: { delegatesFocus: true },
};
