import { FluentDesignSystem } from '@fluentui/web-components';
<<<<<<< HEAD
import { ChartLegend } from './chart-legend.js';
=======
import { type PartialFASTElementDefinition } from '@microsoft/fast-element';
>>>>>>> users/vnbaaij/dev-v5/add-areachart
import { styles } from './chart-legend.styles.js';
import { template } from './chart-legend.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-chart-legend>`
 */
<<<<<<< HEAD
export const definition = ChartLegend.compose({
  name: `${FluentDesignSystem.prefix}-chart-legend`,
  template,
  styles,
});
=======
export const definition: PartialFASTElementDefinition = {
  name: `${FluentDesignSystem.prefix}-chart-legend`,
  registry: FluentDesignSystem.registry,
  template,
  styles,
};
>>>>>>> users/vnbaaij/dev-v5/add-areachart
