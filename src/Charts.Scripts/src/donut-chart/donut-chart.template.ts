import { ElementViewTemplate, html, ref, repeat, when } from '@microsoft/fast-element';
import type { DonutChart } from './donut-chart.js';
import type { Legend } from './donut-chart.options.js';

/**
 * Generates a template for the DonutChart component.
 *
 * @public
 */
export function donutChartTemplate<T extends DonutChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div ${ref('chartContainer')}>
        <svg class="chart" width="${x => x.width}" height="${x => x.height}">
          <g ${ref('group')} transform="translate(${x => x.width / 2}, ${x => x.height / 2})"></g>
        </svg>
      </div>
      <div class="legend-container" ?hidden="${x => x.hideLegends}" role="listbox" aria-label="${x => x.legendListLabel}">
        ${repeat(
          x => x.legends,
          html<Legend, T>` <button
            class="legend${(x, c) => (c.parent.isLegendItemDimmed(x.title) ? ' inactive' : '')}"
            role="option"
            aria-setsize="${(x, c) => c.length}"
            aria-posinset="${(x, c) => c.index + 1}"
            aria-selected="${(x, c) => c.parent.isLegendItemSelected(x.title) || x.title === c.parent.activeLegend}"
            @mouseover="${(x, c) => c.parent.handleLegendMouseoverAndFocus(x.title)}"
            @mouseout="${(x, c) => c.parent.handleLegendMouseoutAndBlur()}"
            @focus="${(x, c) => c.parent.handleLegendMouseoverAndFocus(x.title)}"
            @blur="${(x, c) => c.parent.handleLegendMouseoutAndBlur()}"
            @click="${(x, c) => c.parent.handleLegendClick(x.title)}"
          >
            <div
              class="legend-rect"
              style="background-color: ${x => x.color}; border-color: ${x => x.color};"
            ></div>
            <div class="legend-text">${x => x.title}</div>
          </button>`,
        )}
      </div>
      ${when(
        x => !x.hideTooltip && x.tooltipProps.isVisible,
        html<T>`
          <div
            class="tooltip"
            style="inset-inline-start: ${x => x.tooltipProps.xPos}px; top: ${x => x.tooltipProps.yPos}px"
          >
            <div class="tooltip-body" style="border-color: ${x => x.tooltipProps.color};">
              <div class="tooltip-legend-text">${x => x.tooltipProps.legend}</div>
              <div class="tooltip-content-y" style="color: ${x => x.tooltipProps.color};">
                ${x => x.tooltipProps.yValue}
              </div>
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
export const template: ElementViewTemplate<DonutChart> = donutChartTemplate();
