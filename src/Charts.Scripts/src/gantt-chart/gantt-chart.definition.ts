import { FluentDesignSystem } from '@fluentui/web-components';
import { GanttChart } from './gantt-chart.js';
import { styles } from './gantt-chart.styles.js';
import { template } from './gantt-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-gantt-chart>`
 */
export const definition = GanttChart.compose({
  name: `${FluentDesignSystem.prefix}-gantt-chart`,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
});
