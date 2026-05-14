import { attr, FASTElement, observable } from '@microsoft/fast-element';
import type { ChartLegend } from '../chart-legend/chart-legend.js';
import type { Legend, TooltipProps } from './chart.options.js';

/**
 * Abstract base class shared by all chart web components.
 *
 * Owns the legend interaction system, RAF-based render scheduling,
 * and the connectedCallback FAST field-shadow workaround for attrs
 * and observables common to every chart.
 *
 * @internal
 */
export abstract class ChartBase extends FASTElement {
  // ── Attrs shared across all charts ──────────────────────────────

  @attr({ attribute: 'chart-title' })
  public chartTitle?: string;

  @attr({ attribute: 'hide-legends', mode: 'boolean' })
  public hideLegends: boolean = false;

  @attr({ attribute: 'hide-tooltip', mode: 'boolean' })
  public hideTooltip: boolean = false;

  @attr({ attribute: 'hide-labels', mode: 'boolean' })
  public hideLabels: boolean = false;

  @attr({ attribute: 'round-corners', mode: 'boolean' })
  public roundCorners: boolean = false;

  @attr({ attribute: 'legend-list-label' })
  public legendListLabel?: string;

  @attr
  public culture?: string;

  @attr({ attribute: 'allow-multiple-legend-selection', mode: 'boolean' })
  public allowMultipleLegendSelection: boolean = false;

  // ── Observables shared across all charts ─────────────────────────

  @observable
  public activeLegend: string = '';
  protected activeLegendChanged(_oldValue: string, _newValue: string) {
    if (this._isSettingActiveLegend) {
      return;
    }
    this._updateLegendInteractionState();
  }

  @observable
  public isLegendSelected: boolean = false;

  @observable
  public selectedLegends: string[] = [];

  @observable
  public legends: Legend[] = [];

  @observable
  public tooltipProps: TooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };

  // ── Public refs ──────────────────────────────────────────────────

  public chartContainer!: HTMLDivElement;
  public elementInternals: ElementInternals = this.attachInternals();

  // ── Protected shared state ───────────────────────────────────────

  protected _isRTL: boolean = false;

  // ── Private state ────────────────────────────────────────────────

  private _isSettingActiveLegend: boolean = false;
  private _renderDirty = false;
  private _frameHandle: number | null = null;

  constructor() {
    super();
    this.elementInternals.role = 'region';
  }

  /**
   * Implemented by each chart to perform one full render.
   * Invoked by the RAF-based scheduler when the chart is dirty.
   */
  protected abstract _performRender(): void;

  /**
   * Implemented by each chart to apply active/inactive CSS state to its
   * rendered elements (bars, arcs, etc.) when the legend selection changes.
   */
  protected abstract _applyActiveLegendState(): void;

  /**
   * Returns the accessible label for the host element.
   * Used both on initial render and when `chartTitle` changes at runtime.
   */
  protected abstract _getHostAriaLabel(): string;

  // ── Lifecycle ────────────────────────────────────────────────────

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    //
    // Subclasses call super.connectedCallback() after removing their own specific
    // fields, ensuring all own properties are gone before FASTElement renders the template.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'chartTitle',
      'hideLegends',
      'hideTooltip',
      'hideLabels',
      'roundCorners',
      'legendListLabel',
      'culture',
      'allowMultipleLegendSelection',
    ] as const;
    const observableFields = [
      'activeLegend',
      'isLegendSelected',
      'selectedLegends',
      'legends',
      'tooltipProps',
    ] as const;

    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    const savedObservables: Partial<Record<(typeof observableFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    for (const field of observableFields) {
      savedObservables[field] = self[field];
      delete self[field];
      if (savedObservables[field] !== undefined) {
        self[field] = savedObservables[field];
      }
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }
  }

  public disconnectedCallback() {
    this._cancelScheduledRender();
    super.disconnectedCallback();
  }

  // ── Attr change handlers ─────────────────────────────────────────

  protected chartTitleChanged() {
    if (this.$fastController.isConnected) {
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
    }
  }

  protected cultureChanged() {
    this._requestRender();
  }

  protected hideLabelsChanged() {
    this._requestRender();
  }

  protected roundCornersChanged() {
    this._requestRender();
  }

  protected allowMultipleLegendSelectionChanged() {
    if (!this.allowMultipleLegendSelection) {
      this.selectedLegends = [];
      this._setActiveLegend('');
      this.isLegendSelected = false;
      return;
    }
    this._updateLegendInteractionState();
  }

  protected selectedLegendsChanged() {
    this._updateLegendInteractionState();
  }

  // ── Public legend interaction API ────────────────────────────────

  public handleLegendMouseoverAndFocus(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else if (this.isLegendSelected) {
      return;
    }
    this._setActiveLegend(legendTitle);
  }

  public handleLegendMouseoutAndBlur() {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else if (this.isLegendSelected) {
      return;
    }
    this._setActiveLegend('');
  }

  public handleLegendClick(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      const nextSelection = this.selectedLegends.includes(legendTitle)
        ? this.selectedLegends.filter(legend => legend !== legendTitle)
        : [...this.selectedLegends, legendTitle];

      this.selectedLegends = nextSelection;

      if (nextSelection.length === 0) {
        this._setActiveLegend('');
      } else if (!nextSelection.includes(this.activeLegend)) {
        this._setActiveLegend(nextSelection[nextSelection.length - 1]);
      } else {
        this._updateLegendInteractionState();
      }

      return;
    }

    if (this.isLegendSelected && this.activeLegend === legendTitle) {
      this._setActiveLegend('');
      this.isLegendSelected = false;
    } else {
      this._setActiveLegend(legendTitle);
      this.isLegendSelected = true;
    }
  }

  // ── Protected legend helpers ─────────────────────────────────────

  protected _getHighlightedLegends(): string[] {
    if (this.allowMultipleLegendSelection && this.selectedLegends.length > 0) {
      return this.selectedLegends;
    }
    return this.activeLegend ? [this.activeLegend] : [];
  }

  protected _updateLegendInteractionState() {
    this._applyActiveLegendState();
    this._applyLegendButtonState();
  }

  protected _setActiveLegend(value: string) {
    this._isSettingActiveLegend = true;
    this.activeLegend = value;
    this._isSettingActiveLegend = false;
    this._updateLegendInteractionState();
  }

  protected _applyLegendButtonState() {
    const el = this.shadowRoot?.querySelector<ChartLegend>('fluent-chart-legend');
    if (el) {
      el.highlighted = this._getHighlightedLegends();
    }
  }

  // ── Tooltip helpers ──────────────────────────────────────────────

  /**
   * Returns true when the tooltip should be shown for the given legend title.
   * Returns false when legend highlighting is active and excludes this legend.
   */
  protected _shouldShowTooltip(legendTitle: string): boolean {
    const highlighted = this._getHighlightedLegends();
    return highlighted.length === 0 || highlighted.includes(legendTitle);
  }

  /**
   * Resets the base tooltip fields to their hidden/empty defaults.
   * Subclasses with extended tooltip shapes (e.g. extra axis labels) should override this.
   */
  protected _clearTooltip(): void {
    this.tooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };
  }

  // ── Render scheduling ────────────────────────────────────────────

  protected _requestRender(): void {
    this._renderDirty = true;

    if (this._frameHandle !== null) {
      return;
    }

    this._frameHandle = requestAnimationFrame(() => {
      this._frameHandle = null;

      if (!this._renderDirty) {
        return;
      }

      this._renderDirty = false;
      this._performRender();
    });
  }

  protected _cancelScheduledRender(): void {
    if (this._frameHandle !== null) {
      cancelAnimationFrame(this._frameHandle);
      this._frameHandle = null;
    }

    this._renderDirty = false;
  }

  // ── Host dimension helpers (used by both bar charts) ─────────────

  protected _applyHostDimensions(width: number | string | undefined, height: number | string | undefined): void {
    if (width === undefined || width === null || width === '') {
      this.style.removeProperty('width');
    } else {
      this.style.width = this._toCssLength(width as number | string);
    }

    if (height === undefined || height === null || height === '') {
      this.style.removeProperty('height');
    } else {
      this.style.height = this._toCssLength(height as number | string);
    }
  }

  protected _toCssLength(value: number | string): string {
    return typeof value === 'number' || /^\d+(\.\d+)?$/.test(value as string) ? `${value}px` : `${value}`;
  }
}
