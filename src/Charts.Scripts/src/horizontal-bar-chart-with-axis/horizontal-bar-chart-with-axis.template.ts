import { ElementViewTemplate, html, ref, when } from '@microsoft/fast-element';
import type { HorizontalBarChartWithAxis } from './horizontal-bar-chart-with-axis.js';

export function horizontalBarChartWithAxisTemplate<T extends HorizontalBarChartWithAxis>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div class="chart-container" ${ref('chartContainer')}></div>
      <fluent-chart-legend
        :items="${x => x.legends}"
        label="${x => x.legendListLabel}"
        position="${x => x.legendPosition}"
        ?hidden="${x => x.hideLegends}"
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
