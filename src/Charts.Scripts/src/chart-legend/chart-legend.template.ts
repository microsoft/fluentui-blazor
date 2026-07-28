<<<<<<< HEAD
import { type ElementViewTemplate, html, repeat } from '@microsoft/fast-element';
import type { ChartLegend } from './chart-legend.js';
import type { Legend } from '../utils/chart.options.js';
=======
import { type ElementViewTemplate, html, repeat, when } from '@microsoft/fast-element';
// NOTE: Item visibility is managed imperatively in ChartLegend._measure() — not
// through FAST style bindings — because repeat() does not propagate parent
// observable changes into inner bindings reactively.
import type { ChartLegend } from './chart-legend.js';
import type { Legend } from '../utils/chart-options.js';
>>>>>>> users/vnbaaij/dev-v5/add-areachart
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
<<<<<<< HEAD
              class="legend-rect"
=======
              class="${(x, c) => `legend-rect${c.parent.roundBoxes ? ' rounded' : ''}`}"
>>>>>>> users/vnbaaij/dev-v5/add-areachart
              style="background-color: ${x => getColorFromToken(x.color)}; border-color: ${x =>
                getColorFromToken(x.color)};"
            ></div>
            <div class="legend-text">${x => x.legend}</div>
          </button>
        `,
      )}
<<<<<<< HEAD
=======
      ${when(
        x => x._overflowCount > 0,
        html<T>`
          <fluent-menu close-on-scroll persist-on-item-click>
            <fluent-menu-button slot="trigger" size="small">
              +${x => x._overflowCount} ${x => x.overflowText ?? 'more'}
            </fluent-menu-button>
            <fluent-menu-list>
              ${repeat(
                x => x._overflowItems,
                html<Legend, T>`
                  <fluent-menu-item
                    role="menuitemcheckbox"
                    ?checked="${(x, c) => c.parent.selected.includes(x.legend)}"
                    class="${(x, c) => {
                      const inactive = c.parent.highlighted.length > 0 && !c.parent.highlighted.includes(x.legend);
                      const selected = c.parent.selected.includes(x.legend);
                      return `${inactive ? 'inactive' : ''}${selected ? ' selected' : ''}`.trim();
                    }}"
                    @click="${(x, c) => c.parent.$emit('legend-click', x.legend)}"
                    @mouseover="${(x, c) => c.parent.$emit('legend-mouseover', x.legend)}"
                    @mouseout="${(x, c) => c.parent.$emit('legend-mouseout')}"
                    @focus="${(x, c) => c.parent.$emit('legend-focus', x.legend)}"
                    @blur="${(x, c) => c.parent.$emit('legend-blur')}"
                  >
                    <span slot="indicator"></span>
                    <div
                      slot="start"
                      class="${(x, c) => `legend-rect${c.parent.roundBoxes ? ' rounded' : ''}`}"
                      style="background-color: ${x => getColorFromToken(x.color)}; border-color: ${x =>
                        getColorFromToken(x.color)};"
                    ></div>
                    <span class="legend-text">${x => x.legend}</span>
                  </fluent-menu-item>
                `,
              )}
            </fluent-menu-list>
          </fluent-menu>
        `,
      )}
>>>>>>> users/vnbaaij/dev-v5/add-areachart
    </template>
  `;
}

/**
 * @internal
 */
export const template: ElementViewTemplate<ChartLegend> = chartLegendTemplate();
