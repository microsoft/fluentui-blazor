import { type ElementViewTemplate, html, repeat, when } from '@microsoft/fast-element';
// NOTE: Item visibility is managed imperatively in ChartLegend._measure() — not
// through FAST style bindings — because repeat() does not propagate parent
// observable changes into inner bindings reactively.
import type { ChartLegend } from './chart-legend.js';
import type { ChartMarkerShape, Legend } from '../utils/chart-options.js';
import { getColorFromToken } from '../utils/chart-helpers.js';
import { getMarkerPath, markerShapeNames } from '../utils/marker-shapes.js';

const getLegendShapePath = (shape: ChartMarkerShape | undefined): string => {
  const shapeIndex = markerShapeNames.indexOf(shape ?? 'circle');
  return getMarkerPath(6, 6, 11, Math.max(shapeIndex, 0));
};

const getLegendRectStyle = (item: Legend): string => {
  const color = getColorFromToken(item.color);
  if (!item.isLineLegendInBarChart || item.lineStrokeDasharray === undefined) {
    return `background-color: ${color}; border-color: ${color};`;
  }

  const dashLength = Math.max(Number.parseFloat(String(item.lineStrokeDasharray)) || 1, 1);
  return (
    `background-color: transparent; border-color: transparent; ` +
    `background-image: repeating-linear-gradient(to right, ${color} 0 ${dashLength}px, ` +
    `transparent ${dashLength}px ${dashLength * 2}px);`
  );
};

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
            ${when(
              x => !!x.shape,
              html<Legend, T>`
                <svg
                  class="legend-shape"
                  data-shape="${x => x.shape}"
                  viewBox="0 0 12 12"
                  aria-hidden="true"
                  focusable="false"
                >
                  <path
                    d="${x => getLegendShapePath(x.shape)}"
                    fill="${x => getColorFromToken(x.color)}"
                    stroke="${x => getColorFromToken(x.color)}"
                    stroke-width="1"
                  ></path>
                </svg>
              `,
              html<Legend, T>`
                <div
                  class="${(x, c) =>
                    `legend-rect${x.isLineLegendInBarChart ? ' line' : ''}${c.parent.roundBoxes ? ' rounded' : ''}`}"
                  style="${x => getLegendRectStyle(x)}"
                ></div>
              `,
            )}
            <div class="legend-text">${x => x.legend}</div>
          </button>
        `,
      )}
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
                    ${when(
                      x => !!x.shape,
                      html<Legend, T>`
                        <svg
                          slot="start"
                          class="legend-shape"
                          data-shape="${x => x.shape}"
                          viewBox="0 0 12 12"
                          aria-hidden="true"
                          focusable="false"
                        >
                          <path
                            d="${x => getLegendShapePath(x.shape)}"
                            fill="${x => getColorFromToken(x.color)}"
                            stroke="${x => getColorFromToken(x.color)}"
                            stroke-width="1"
                          ></path>
                        </svg>
                      `,
                      html<Legend, T>`
                        <div
                          slot="start"
                          class="${(x, c) =>
                            `legend-rect${x.isLineLegendInBarChart ? ' line' : ''}${
                              c.parent.roundBoxes ? ' rounded' : ''
                            }`}"
                          style="${x => getLegendRectStyle(x)}"
                        ></div>
                      `,
                    )}
                    <span class="legend-text">${x => x.legend}</span>
                  </fluent-menu-item>
                `,
              )}
            </fluent-menu-list>
          </fluent-menu>
        `,
      )}
    </template>
  `;
}

/**
 * @internal
 */
export const template: ElementViewTemplate<ChartLegend> = chartLegendTemplate();
