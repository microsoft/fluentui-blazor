import { FluentDesignSystem } from '@fluentui/web-components';
import { FunnelChart } from './funnel-chart.js';
import { styles } from './funnel-chart.styles.js';
import { template } from './funnel-chart.template.js';

/**
 * @public
 * @remarks
 * HTML Element: `<fluent-funnel-chart>`
 */
export const definition = FunnelChart.compose({
  name: `${FluentDesignSystem.prefix}-funnel-chart`,
  template,
  styles,
  shadowOptions: {
    delegatesFocus: true,
  },
});
