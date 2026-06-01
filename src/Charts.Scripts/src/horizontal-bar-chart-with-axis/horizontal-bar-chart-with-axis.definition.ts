import { FluentDesignSystem } from '@fluentui/web-components';
import { HorizontalBarChartWithAxis } from './horizontal-bar-chart-with-axis.js';
import { styles } from './horizontal-bar-chart-with-axis.styles.js';
import { template } from './horizontal-bar-chart-with-axis.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-horizontal-bar-chart-with-axis>`
 */
export const definition = HorizontalBarChartWithAxis.compose({
  name: `${FluentDesignSystem.prefix}-horizontal-bar-chart-with-axis`,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
});
