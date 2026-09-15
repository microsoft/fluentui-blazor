import { FluentDesignSystem } from '@fluentui/web-components';
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
import { styles } from './tree-chart.styles.js';
import { template } from './tree-chart.template.js';

/** @public @remarks HTML Element: `<fluent-tree-chart>` */
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-tree-chart`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
  shadowOptions: { delegatesFocus: true },
};
