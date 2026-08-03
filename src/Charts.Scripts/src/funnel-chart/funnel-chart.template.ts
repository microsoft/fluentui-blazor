import { ElementViewTemplate, html, ref, when } from '@microsoft/fast-element';
import type { FunnelChart } from './funnel-chart.js';
import { chartTooltipTemplate } from '../utils/chart-tooltip.template.js';

/**
 * Generates a template for the FunnelChart component.
 *
 * @public
 */
export function funnelChartTemplate<T extends FunnelChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div class="chart-container" ${ref('chartContainer')}>
        <svg
          ${ref('svgElement')}
          class="chart"
          width="${x => x._toSvgLength(x.width, 400)}"
          height="${x => x._toSvgLength(x.height, 400)}"
          role="none"
        >
          <g ${ref('group')}></g>
        </svg>
      </div>
      <fluent-chart-legend
        :items="${x => x.legends}"
        label="${x => x.legendListLabel}"
        position="${x => x.legendPosition}"
        ?hidden="${x => x.hideLegends}"
        :roundBoxes="${x => x.roundCorners}"
        @legend-click="${(x, c) => x.handleLegendClick((c.event as CustomEvent<string>).detail)}"
        @legend-mouseover="${(x, c) => x.handleLegendMouseoverAndFocus((c.event as CustomEvent<string>).detail)}"
        @legend-mouseout="${x => x.handleLegendMouseoutAndBlur()}"
        @legend-focus="${(x, c) => x.handleLegendFocus((c.event as CustomEvent<string>).detail)}"
        @legend-blur="${x => x.handleLegendMouseoutAndBlur()}"
      ></fluent-chart-legend>
      ${chartTooltipTemplate<T>()}
    </template>
  `;
}

/**
 * @internal
 */
export const template: ElementViewTemplate<FunnelChart> = funnelChartTemplate();
