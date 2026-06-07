import { FASTElement, attr, observable } from '@microsoft/fast-element';
import type { ChartLegendPosition, Legend } from '../utils/chart.options.js';
import { jsonConverter } from '../utils/chart-helpers.js';

/**
 * A reusable legend list used by all chart components.
 *
 * Renders a listbox of colour-coded legend buttons and emits custom events
 * (`legend-click`, `legend-mouseover`, `legend-mouseout`, `legend-focus`,
 * `legend-blur`) so the parent chart can update its interaction state.
 *
 * The parent is responsible for keeping `highlighted` up to date by calling
 * `_applyLegendButtonState()` on ChartBase whenever interaction state changes.
 *
 * @public
 */
export class ChartLegend extends FASTElement {
  /**
   * The legend items to render.
   *
   * When set from HTML (e.g. from a Blazor wrapper), pass a JSON array string:
   * `<fluent-chart-legend items='[{"legend":"Apples","color":"#637cef"}]'>`
   *
   * When set from JavaScript, assign the array directly:
   * `element.items = [{ legend: 'Apples', color: '#637cef' }]`
   */
  @attr({ converter: jsonConverter })
  public items: Legend[] = [];

  /**
   * Legend titles that are currently active (hovered, focused, or selected).
   * When non-empty, all other items are dimmed.
   */
  @observable
  public highlighted: string[] = [];

  /**
   * Legend titles that are persistently selected (by click or Space/Enter).
   * Used to apply `aria-selected` and the bold `.selected` style.
   */
  @observable
  public selected: string[] = [];

  /** Accessible label for the legend listbox (`aria-label`). */
  @attr
  public label?: string;

  /** Position of the legend relative to the chart body. Defaults to `'bottom'`. */
  @attr
  public position?: ChartLegendPosition;

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST
    // @attr and @observable reactive getter/setters on the prototype. Delete them
    // so that attribute changes go through the FAST reactive system and trigger
    // template bindings and *Changed() callbacks.
    // Save defaults first so we can restore them for fields that have no
    // corresponding HTML attribute (FAST won't call the setter in that case).
    const self = this as Record<string, unknown>;
    const attrFields = ['items', 'position'] as const;
    const observableFields = ['highlighted', 'selected'] as const;

    const savedAttr: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    const savedObs: Partial<Record<(typeof observableFields)[number], unknown>> = {};

    for (const field of attrFields) {
      savedAttr[field] = self[field];
      delete self[field];
    }

    for (const field of observableFields) {
      savedObs[field] = self[field];
      delete self[field];
      // Restore observable defaults through the prototype's FAST reactive setter
      // BEFORE super.connectedCallback() renders the template.
      if (savedObs[field] !== undefined) {
        self[field] = savedObs[field];
      }
    }

    super.connectedCallback();

    // Restore attr-field defaults when no HTML attribute was provided.
    for (const field of attrFields) {
      if (self[field] === undefined && savedAttr[field] !== undefined) {
        self[field] = savedAttr[field];
      }
    }
  }

  /**
   * Roving tabindex handler for legend button keyboard navigation.
   * Arrow keys move focus between legend items; all other keys are ignored.
   */
  public _handleLegendKeydown(e: KeyboardEvent): boolean {
    const forward = e.key === 'ArrowRight' || e.key === 'ArrowDown';
    const backward = e.key === 'ArrowLeft' || e.key === 'ArrowUp';
    if (!forward && !backward) {
      return true; // Don't prevent default for Space, Enter, Tab, etc.
    }
    e.preventDefault();
    const buttons = Array.from(this.shadowRoot?.querySelectorAll<HTMLButtonElement>('button.legend') ?? []);
    const count = buttons.length;
    if (count === 0) {
      return false;
    }
    const index = buttons.indexOf(e.currentTarget as HTMLButtonElement);
    if (index === -1) {
      return false;
    }
    const nextIndex = forward ? (index + 1) % count : (index - 1 + count) % count;
    buttons[index].tabIndex = -1;
    buttons[nextIndex].tabIndex = 0;
    buttons[nextIndex].focus();
    return false;
  }
}
