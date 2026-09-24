import { ElementViewTemplate, html, ref, repeat, when } from '@microsoft/fast-element';
import type { PolarChart, PolarTooltipEntry } from './polar-chart.js';

export function polarChartTemplate<T extends PolarChart>(): ElementViewTemplate<T> {
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
            class="tooltip ${x => (x.isMeasuringTooltip ? 'measuring' : '')}"
            style="inset-inline-start: ${x => x.tooltipProps.xPos}px; top: ${x =>
              x.tooltipProps.yPos}px; transform: ${x => x.tooltipInlineTransform}"
          >
            <div class="tooltip-body">
              ${when(
                x => !x.tooltipRenderer,
                html<T>`
                  ${when(
                    x => x.enableMultiValueCallout,
                    html<T>`
                      <div class="tooltip-header">${x => x.tooltipProps.angularLabel}</div>
                      ${repeat(
                        x => x.tooltipProps.entries ?? [],
                        html<PolarTooltipEntry, T>`
                          <div class="tooltip-inner" style="border-color: ${x => x.color};">
                            <div class="tooltip-legend-text">${x => x.legend}</div>
                            <div class="tooltip-content-y" style="color: ${x => x.color};">${x => x.value}</div>
                          </div>
                        `,
                      )}
                    `,
                    html<T>`
                      <div class="tooltip-header">Value</div>
                      <div class="tooltip-inner" style="border-color: ${x => x.tooltipProps.color};">
                        <div class="tooltip-legend-text">${x => x.tooltipProps.legend}</div>
                        <div class="tooltip-content-y" style="color: ${x => x.tooltipProps.color};">
                          ${x => x.tooltipProps.yValue}
                        </div>
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

export const template: ElementViewTemplate<PolarChart> = polarChartTemplate();
