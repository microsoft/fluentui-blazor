import { ElementViewTemplate, html, ref, repeat, when } from '@microsoft/fast-element';
import type { HorizontalBarChartWithAxis } from './horizontal-bar-chart-with-axis.js';
import type { HorizontalBarChartWithAxisLegend } from './horizontal-bar-chart-with-axis.options.js';

export function horizontalBarChartWithAxisTemplate<T extends HorizontalBarChartWithAxis>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div ${ref('chartContainer')}></div>
      ${when(
        x => !x.hideLegends,
        html<T>`
          <div class="legend-container" role="listbox" aria-label="${x => x.legendListLabel}">
            ${repeat(
              x => x.uniqueLegends,
              html<HorizontalBarChartWithAxisLegend, T>`
                <button
                  class="legend${(x, c) => (c.parent.isLegendDimmed(x.legend) ? ' inactive' : '')}"
                  role="option"
                  aria-setsize="${(x, c) => c.length}"
                  aria-posinset="${(x, c) => c.index + 1}"
                  aria-selected="${(x, c) => c.parent.isLegendSelected(x.legend)}"
                  @mouseover="${(x, c) => c.parent.handleLegendMouseoverAndFocus(x.legend)}"
                  @mouseout="${(x, c) => c.parent.handleLegendMouseoutAndBlur()}"
                  @focus="${(x, c) => c.parent.handleLegendMouseoverAndFocus(x.legend)}"
                  @blur="${(x, c) => c.parent.handleLegendMouseoutAndBlur()}"
                  @click="${(x, c) => c.parent.handleLegendClick(x.legend)}"
                >
                  <div
                    class="legend-rect"
                    style="background-color: ${x => x.color}; border-color: ${x => x.color};"
                  ></div>
                  <div class="legend-text">${x => x.legend}</div>
                </button>
              `,
            )}
          </div>
        `,
      )}
      ${when(
        x => !x.hideTooltip && x.tooltipProps.isVisible,
        html<T>`
          <div
            class="tooltip"
            style="inset-inline-start: ${x => x.tooltipProps.xPos}px; top: ${x =>
              x.tooltipProps.yPos}px; transform: ${x => x.tooltipInlineTransform}"
          >
            <div class="tooltip-header">${x => x.tooltipProps.yValue}</div>
            <div class="tooltip-info" style="border-color: ${x => x.tooltipProps.color};">
              <div class="tooltip-legend-text">${x => x.tooltipProps.legend}</div>
              <div class="tooltip-primary-value" style="color: ${x => x.tooltipProps.color};">
                ${x => x.tooltipProps.xValue}
              </div>
            </div>
          </div>
        `,
      )}
    </template>
  `;
}

export const template: ElementViewTemplate<HorizontalBarChartWithAxis> = horizontalBarChartWithAxisTemplate();
