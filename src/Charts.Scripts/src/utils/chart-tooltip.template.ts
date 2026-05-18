import { html, when } from '@microsoft/fast-element';
import type { ChartBase } from './chart-base.js';

/**
 * Standard tooltip template fragment for charts that use the base 6-field TooltipProps.
 *
 * Includes the `when` guard so callers simply interpolate the result:
 * ```ts
 * ${chartTooltipTemplate<T>()}
 * ```
 *
 * The rendered markup uses the `.tooltip-inner` and `.tooltip-content-y` class names.
 * Each chart stylesheet should style those classes to match its visual design.
 *
 * Charts with extended TooltipProps (e.g. HorizontalBarChartWithAxis) define
 * their own tooltip template inline.
 *
 * @internal
 */
export function chartTooltipTemplate<T extends ChartBase>() {
  return html<T>`
    <div class="live-region" role="status" aria-live="polite" aria-atomic="true">${(x: T) => x.liveRegionText}</div>
    ${when(
      (x: T) => !x.hideTooltip && x.tooltipProps.isVisible,
      html<T>`
        <div
          class="tooltip"
          style="inset-inline-start: ${(x: T) => x.tooltipProps.xPos}px; top: ${(x: T) => x.tooltipProps.yPos}px"
        >
          <div class="tooltip-inner" style="border-color: ${(x: T) => x.tooltipProps.color};">
            <div class="tooltip-legend-text">${(x: T) => x.tooltipProps.legend}</div>
            <div class="tooltip-content-y" style="color: ${(x: T) => x.tooltipProps.color};">
              ${(x: T) => x.tooltipProps.yValue}
            </div>
          </div>
        </div>
      `,
    )}
  `;
}
