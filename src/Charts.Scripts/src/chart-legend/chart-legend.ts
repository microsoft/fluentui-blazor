import { FASTElement, attr, observable } from '@microsoft/fast-element';
import type { ChartLegendPosition, Legend } from '../utils/chart-options.js';
import { jsonConverter } from '../utils/chart-helpers.js';

// Keep in sync with the `gap` in chart-legend.styles.ts.
const LEGEND_ITEM_GAP_PX = 8;

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

  /** Label appended to the overflow count. Defaults to `'more'` → "+3 more". */
  @attr({ attribute: 'overflow-text' })
  public overflowText?: string;

  /** Centers the legend row horizontally within its host. */
  @attr({ attribute: 'center', mode: 'boolean' })
  public center = false;

  /**
   * Enable rounded corners on legend colored boxes.
   * When undefined, inherits from the parent chart's rounded-corners setting.
   * When explicitly set, overrides the chart setting.
   */
  @attr({ attribute: 'round-boxes', mode: 'boolean' })
  public roundBoxes?: boolean;

  /**
   * Number of items that overflow the row. Drives the `when()` that renders
   * the "+N more" button and the overflow popup.
   */
  @observable
  public _overflowCount: number = 0;

  /**
   * The subset of `items` that are hidden in the row and shown in the popup.
   * Updated imperatively by `_measure()` alongside DOM visibility changes.
   */
  @observable
  public _overflowItems: Legend[] = [];

  /** Index boundary used for imperative DOM hiding — not an observable. */
  private _visibleCount: number = Number.MAX_SAFE_INTEGER;
  /** Cached offsetWidths so hidden items (offsetWidth=0) can still be measured. */
  private _itemWidths: number[] = [];
  private _resizeObserver?: ResizeObserver;

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST
    // @attr and @observable reactive getter/setters on the prototype. Delete them
    // so that attribute changes go through the FAST reactive system and trigger
    // template bindings and *Changed() callbacks.
    // Save defaults first so we can restore them for fields that have no
    // corresponding HTML attribute (FAST won't call the setter in that case).
    const self = this as Record<string, unknown>;
    const attrFields = ['items', 'position', 'overflowText', 'center', 'roundBoxes'] as const;
    const observableFields = ['highlighted', 'selected', '_overflowCount', '_overflowItems'] as const;

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

    // Attach ResizeObserver — fires on every layout change of the host element.
    this._resizeObserver = new ResizeObserver(() => this._measure());
    this._resizeObserver.observe(this);

    // Initial measurement after the first render frame so all item buttons
    // are in the DOM and have layout (offsetWidth > 0).
    requestAnimationFrame(() => this._measure());
  }

  disconnectedCallback() {
    super.disconnectedCallback();
    this._resizeObserver?.disconnect();
    this._resizeObserver = undefined;
  }

  /** Called by FAST when `items` changes — reset state and re-measure. */
  itemsChanged() {
    this._itemWidths = [];
    this._visibleCount = Number.MAX_SAFE_INTEGER;
    this._overflowCount = 0;
    this._overflowItems = [];
    requestAnimationFrame(() => this._measure());
  }

  /**
   * Measures item widths, imperatively shows/hides individual buttons, and
   * updates the `_overflowCount` / `_overflowItems` observables which control
   * the "+N more" button and popup via FAST `when()` directives.
   *
   * This is deliberately imperative: FAST's `repeat()` does not propagate
   * parent-observable changes down to inner style bindings reactively, so
   * visibility is managed through direct `element.style.display` assignments.
   */
  _measure() {
    // Vertical positions (start / end) always stack — no overflow needed.
    if (this.position === 'start' || this.position === 'end') {
      this._visibleCount = Number.MAX_SAFE_INTEGER;
      this._overflowCount = 0;
      this._overflowItems = [];

      this.shadowRoot
        ?.querySelectorAll<HTMLElement>('.legend:not(.overflow-button)')
        ?.forEach(btn => (btn.style.display = ''));
      return;
    }

    const root = this.shadowRoot;
    if (!root) return;

    const hostWidth = this.clientWidth;
    if (hostWidth === 0) return;

    // Keep a tiny safety margin so borders/anti-aliasing do not get clipped.
    const overflowSafetyPx = 2;
    const hostStyles = getComputedStyle(this);
    const horizontalPadding =
      (Number.parseFloat(hostStyles.paddingLeft || '0') || 0) +
      (Number.parseFloat(hostStyles.paddingRight || '0') || 0);
    const availableWidth = Math.max(hostWidth - horizontalPadding - overflowSafetyPx, 0);

    const buttons = Array.from(root.querySelectorAll<HTMLButtonElement>('.legend:not(.overflow-button)'));
    if (buttons.length === 0) return;

    // Make all buttons visible first so we can measure their natural widths.
    for (const btn of buttons) {
      btn.style.display = '';
    }

    // Build / refresh the width cache.
    for (let i = 0; i < buttons.length; i++) {
      const w = buttons[i].getBoundingClientRect().width || buttons[i].offsetWidth;
      if (w > 0) this._itemWidths[i] = w;
    }

    const totalWidth =
      this._itemWidths.slice(0, buttons.length).reduce((s, w) => s + (w ?? 0), 0) +
      LEGEND_ITEM_GAP_PX * Math.max(buttons.length - 1, 0);

    if (totalWidth <= availableWidth) {
      // Everything fits — clear any previous overflow state.
      this._visibleCount = buttons.length;
      this._overflowCount = 0;
      this._overflowItems = [];
      return;
    }

    // Reserve space for the overflow button.
    // If it is already in the DOM we can measure it; otherwise use 80 px as a
    // conservative estimate (the ResizeObserver will fire again after FAST adds
    // the button to the DOM and the measurement will self-correct).
    const overflowMenu = root.querySelector<HTMLElement>('fluent-menu');
    const measuredOverflowBtnWidth = overflowMenu?.getBoundingClientRect().width ?? 0;
    const overflowBtnWidth = measuredOverflowBtnWidth > 0 ? measuredOverflowBtnWidth : 80;

    // Find how many items fit alongside the overflow button.
    let used = 0;
    let count = 0;
    for (let i = 0; i < buttons.length; i++) {
      const w = this._itemWidths[i] ?? 0;
      const gapBeforeItem = i > 0 ? LEGEND_ITEM_GAP_PX : 0;
      if (used + gapBeforeItem + w + LEGEND_ITEM_GAP_PX + overflowBtnWidth <= availableWidth) {
        used += gapBeforeItem + w;
        count = i + 1;
      } else {
        break;
      }
    }

    // Always keep at least one item visible.
    count = Math.max(count, 1);
    this._visibleCount = count;

    // Imperatively hide overflow items — this is not driven through FAST bindings.
    for (let i = 0; i < buttons.length; i++) {
      buttons[i].style.display = i >= count ? 'none' : '';
    }

    this._overflowCount = buttons.length - count;
    this._overflowItems = this.items.slice(count);

    // If we had to estimate trigger width, run one more pass after the trigger
    // has rendered so the final visible/overflow split uses exact width.
    if (!overflowMenu && this._overflowCount > 0) {
      requestAnimationFrame(() => this._measure());
    }
  }

  /**
   * Roving tabindex handler for visible legend button keyboard navigation.
   * Arrow keys move focus between legend items; all other keys are ignored.
   */
  public _handleLegendKeydown(e: KeyboardEvent): boolean {
    const forward = e.key === 'ArrowRight' || e.key === 'ArrowDown';
    const backward = e.key === 'ArrowLeft' || e.key === 'ArrowUp';
    if (!forward && !backward) {
      return true; // Don't prevent default for Space, Enter, Tab, etc.
    }
    e.preventDefault();
    // Navigate only among visible legend buttons (not the overflow trigger).
    const buttons = Array.from(
      this.shadowRoot?.querySelectorAll<HTMLButtonElement>('.legend:not(.overflow-button)') ?? [],
    ).filter(b => b.style.display !== 'none');
    const count = buttons.length;
    if (count === 0) return false;
    const index = buttons.indexOf(e.currentTarget as HTMLButtonElement);
    if (index === -1) return false;
    const nextIndex = forward ? (index + 1) % count : (index - 1 + count) % count;
    buttons[index].tabIndex = -1;
    buttons[nextIndex].tabIndex = 0;
    buttons[nextIndex].focus();
    return false;
  }
}
