import { ElementViewTemplate, html, ref, repeat, when } from '@microsoft/fast-element';
import type { GaugeChart } from './gauge-chart.js';
import type { GaugeTooltipRow } from './gauge-chart.js';

/**
 * Generates a template for the GaugeChart component.
 *
 * @public
 */
export function gaugeChartTemplate<T extends GaugeChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div class="chart-container" ${ref('chartContainer')}>
        <svg
          class="chart"
          role="none"
          width="${x => x._toSvgLength(x.width, 252)}"
          height="${x => x._toSvgLength(x.height, x.sublabel ? 116 : 96)}"
        >
          <defs ${ref('svgDefsEl')}></defs>
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
      <div class="live-region" role="status" aria-live="polite" aria-atomic="true">${x => x.liveRegionText}</div>
      ${when(
        x => !x.hideTooltip && x.gaugeTooltipProps.isVisible,
        html<T>`
          <div
            class="tooltip"
            style="inset-inline-start: ${x => x.gaugeTooltipProps.xPos}px; top: ${x => x.gaugeTooltipProps.yPos}px;"
          >
            <div class="tooltip-body">
              ${when(
                x => !x.tooltipRenderer,
                html<T>`
                  <div class="tooltip-header">${x => x.gaugeTooltipProps.headerValue}</div>
                  ${repeat(
                    x => x.gaugeTooltipProps.rows,
                    html<GaugeTooltipRow, T>`
                      <div class="tooltip-inner" style="border-color: ${x => x.color};">
                        <div class="tooltip-legend-text">${x => x.legend}</div>
                        <div class="tooltip-content-y" style="color: ${x => x.color};">${x => x.value}</div>
                      </div>
                    `,
                  )}
                `,
              )}
            </div>
          </div>
        `,
      )}
    </template>
  `;
}

/**
 * @internal
 */
export const template: ElementViewTemplate<GaugeChart> = gaugeChartTemplate();
