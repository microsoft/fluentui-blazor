import { type ElementViewTemplate, html, repeat } from '@microsoft/fast-element';
import type { ChartLegend } from './chart-legend.js';
import type { Legend } from '../utils/chart.options.js';
import { getColorFromToken } from '../utils/chart-helpers.js';

/**
 * Generates a template for the ChartLegend component.
 *
 * @public
 */
export function chartLegendTemplate<T extends ChartLegend>(): ElementViewTemplate<T> {
  return html<T>`
    <template role="listbox" aria-label="${x => x.label ?? 'Chart legend'}">
      ${repeat(
        x => x.items,
        html<Legend, T>`
          <button
            class="${(x, c) => {
              const inactive = c.parent.highlighted.length > 0 && !c.parent.highlighted.includes(x.legend);
              const selected = c.parent.selected.includes(x.legend);
              return `legend${inactive ? ' inactive' : ''}${selected ? ' selected' : ''}`;
            }}"
            role="option"
            tabindex="${(x, c) => (c.parent.items.indexOf(x) === 0 ? 0 : -1)}"
            aria-setsize="${(x, c) => c.parent.items.length}"
            aria-posinset="${(x, c) => c.parent.items.indexOf(x) + 1}"
            aria-selected="${(x, c) => c.parent.highlighted.includes(x.legend) || c.parent.selected.includes(x.legend)}"
            @mouseover="${(x, c) => c.parent.$emit('legend-mouseover', x.legend)}"
            @mouseout="${(x, c) => c.parent.$emit('legend-mouseout')}"
            @focus="${(x, c) => c.parent.$emit('legend-focus', x.legend)}"
            @blur="${(x, c) => c.parent.$emit('legend-blur')}"
            @click="${(x, c) => c.parent.$emit('legend-click', x.legend)}"
            @keydown="${(x, c) => c.parent._handleLegendKeydown(c.event as KeyboardEvent)}"
          >
            <div
              class="legend-rect"
              style="background-color: ${x => getColorFromToken(x.color)}; border-color: ${x =>
                getColorFromToken(x.color)};"
            ></div>
            <div class="legend-text">${x => x.legend}</div>
          </button>
        `,
      )}
    </template>
  `;
}

/**
 * @internal
 */
export const template: ElementViewTemplate<ChartLegend> = chartLegendTemplate();
