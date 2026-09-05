import { ElementViewTemplate, html, ref, repeat, when } from '@microsoft/fast-element';
import type { LineChart } from './line-chart.js';

export function lineChartTemplate<T extends LineChart>(): ElementViewTemplate<T> {
  return html<T>`
    <template>
      ${when(x => !!x.chartTitle, html<T>`<div class="chart-title">${x => x.chartTitle}</div>`)}
      <div class="chart-container" ${ref('chartContainer')}></div>
      ${when(
        x => !!x.eventAnnotationCard,
        html<T>`
          <div
            class="event-annotation-card"
            role="dialog"
            aria-label="${x => x.eventAnnotationCard?.label} details"
            style="left: ${x => x.eventAnnotationCard?.x}px; top: ${x => x.eventAnnotationCard?.y}px;"
            tabindex="-1"
            @keydown="${(x, c) => x.handleEventAnnotationCardKeydown(c.event as KeyboardEvent)}"
          >
            <button
              class="event-annotation-card-close"
              type="button"
              aria-label="Close event details"
              @click="${x => x.dismissEventAnnotationCard(true)}"
            >
              &#x2715;
            </button>
            <div class="event-annotation-card-content" ${ref('eventAnnotationCardContent')}></div>
          </div>
        `,
      )}
      <fluent-chart-legend
        :items="${x => x.legends}"
        label="${x => x.legendListLabel}"
        position="${x => x.legendPosition}"
        ?hidden="${x => x.hideLegends}"
        :roundBoxes="${x => x.roundCorners && !x.allowMultipleShapesForPoints}"
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
            class="tooltip ${x => (x.isMeasuringTooltip ? 'measuring' : '')}"
            style="inset-inline-start: ${x => x.tooltipProps.xPos}px; top: ${x =>
              x.tooltipProps.yPos}px; transform: ${x => x.tooltipInlineTransform}"
          >
            <div class="tooltip-body preserve-default-content">
              <div class="tooltip-default-content" ?hidden="${x => !!x.tooltipRenderer}">
                <div class="tooltip-header">${x => x.tooltipProps.xValue}</div>
                ${repeat(
                  x => x.tooltipProps.entries,
                  html`
                    <div class="tooltip-info" style="border-color: ${x => x.color};">
                      <div class="tooltip-legend-text">${x => x.legend}</div>
                      <div class="tooltip-primary-value" style="color: ${x => x.color};">${x => x.value}</div>
                    </div>
                  `,
                )}
              </div>
              <div class="tooltip-custom-content" ?hidden="${x => !x.tooltipRenderer}"></div>
            </div>
          </div>
        `,
      )}
    </template>
  `;
}

export const template: ElementViewTemplate<LineChart> = lineChartTemplate();
