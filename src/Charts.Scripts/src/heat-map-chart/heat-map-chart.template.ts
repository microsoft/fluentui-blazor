import type { ElementViewTemplate } from '@microsoft/fast-element';
import { html, ref, when } from '@microsoft/fast-element';
import type { HeatMapChart } from './heat-map-chart.js';

export function heatMapChartTemplate<T extends HeatMapChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div class="chart-container" ${ref('chartContainer')}></div>
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
        x => !x.hideTooltip && x.tooltipProps.isVisible,
        html<T>`
          <div
            class="tooltip"
            style="inset-inline-start: ${x => x.tooltipProps.xPos}px; top: ${x => x.tooltipProps.yPos}px"
          >
            <div class="tooltip-body">
              ${when(
                x => !x.tooltipRenderer,
                html<T>`
                  <div class="tooltip-inner" style="border-color: ${x => x.tooltipProps.color};">
                    <div class="tooltip-block">
                      <div class="tooltip-legend-text">${x => x.tooltipProps.legend}</div>
                      <div class="tooltip-content-y" style="color: ${x => x.tooltipProps.color};">
                        ${x => x.tooltipProps.yValue}
                      </div>
                    </div>
                    ${when(
                      x => !!x.tooltipProps.ratio,
                      html<T>`<div class="tooltip-ratio">
                        <span class="tooltip-numerator">${x => x.tooltipProps.ratio![0]}</span>/<span
                          class="tooltip-denominator"
                          >${x => x.tooltipProps.ratio![1]}</span
                        >
                      </div>`,
                    )}
                  </div>
                  ${when(
                    x => !!x.tooltipProps.descriptionMessage,
                    html<T>`<div class="tooltip-description">${x => x.tooltipProps.descriptionMessage}</div>`,
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

export const template: ElementViewTemplate<HeatMapChart> = heatMapChartTemplate();
