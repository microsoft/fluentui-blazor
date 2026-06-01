import { FluentDesignSystem } from '@fluentui/web-components';
import { ChartLegend } from './chart-legend.js';
import { styles } from './chart-legend.styles.js';
import { template } from './chart-legend.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-chart-legend>`
 */
export const definition = ChartLegend.compose({
  name: `${FluentDesignSystem.prefix}-chart-legend`,
  template,
  styles,
});
