import { FluentDesignSystem } from '@fluentui/web-components';
import { HorizontalBarChart } from './horizontal-bar-chart.js';
import { styles } from './horizontal-bar-chart.styles.js';
import { template } from './horizontal-bar-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-horizontal-bar-chart>`
 */
export const definition = HorizontalBarChart.compose({
  name: `${FluentDesignSystem.prefix}-horizontal-bar-chart`,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
});
