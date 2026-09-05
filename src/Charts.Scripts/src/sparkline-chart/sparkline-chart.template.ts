import { ElementViewTemplate, html, ref } from '@microsoft/fast-element';
import type { SparklineChart } from './sparkline-chart.js';

export function sparklineChartTemplate<T extends SparklineChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      <div class="chart-container" ${ref('chartContainer')}></div>
    </template>
  `;
}

export const template: ElementViewTemplate<SparklineChart> = sparklineChartTemplate();
