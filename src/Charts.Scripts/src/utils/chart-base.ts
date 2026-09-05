import { attr, FASTElement, observable } from '@microsoft/fast-element';
import type { ChartLegend } from '../chart-legend/chart-legend.js';
import type {
  ChartLegendPosition,
  ChartTitleAlign,
  ChartTitlePosition,
  Legend,
  TooltipProps,
  TooltipRenderer,
} from './chart-options.js';
import { escapeHtml, getRTL } from './chart-helpers.js';

type TooltipVerticalPlacement = 'above' | 'below';
type TooltipHorizontalAlign = 'start' | 'center' | 'end';

interface TooltipPositionOptions {
  preferredVertical?: TooltipVerticalPlacement;
  horizontalAlign?: TooltipHorizontalAlign;
  gap?: number;
  padding?: number;
  estimatedWidth?: number;
  estimatedHeight?: number;
  outputAnchorX?: boolean;
  boundsWidth?: number;
  boundsHeight?: number;
  /**
   * When the preferred side doesn't have room, clamp to the bounds instead of
   * flipping to the opposite side of the anchor — flipping would place the
   * tooltip directly on top of (and obscure) the hovered element.
   */
  preventAnchorOverlap?: boolean;
}

interface TooltipOverlapPositionOptions {
  horizontalPlacement?: 'center' | 'side';
  preferredHorizontalSide?: 'left' | 'right';
  preferredVerticalSide?: TooltipVerticalPlacement;
  gap?: number;
}

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

  /** Horizontal alignment of the chart title. Defaults to `'start'`. */
  @attr({ attribute: 'title-align' })
  public titleAlign?: ChartTitleAlign;

  /** Vertical position of the chart title. Defaults to `'top'`. */
  @attr({ attribute: 'title-position' })
  public titlePosition?: ChartTitlePosition;

  /** Position of the legend relative to the chart. Defaults to `'bottom'`. */
  @attr({ attribute: 'legend-position' })
  public legendPosition?: ChartLegendPosition;

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

  /** Width of the chart. Accepts pixels (number) or any valid CSS length string (e.g. `'50%'`). */
  @attr
  public width?: number | string;

  /** Height of the chart. Accepts pixels (number) or any valid CSS length string (e.g. `'50%'`). */
  @attr
  public height?: number | string;

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

  /** Text announced to screen readers each time a tooltip becomes visible. */
  @observable
  public liveRegionText: string = '';

  /**
   * Optional function to customize the tooltip content.
   *
   * When set, the function receives the current data point and a `defaultRender` callback.
   * Return either an HTML string or a DOM Node to replace the default tooltip body.
   *
   * @example
   * ```ts
   * chart.tooltipRenderer = (point, defaultRender) =>
   *   `<strong>${point.legend}</strong><br>${defaultRender(point)}`;
   * ```
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public tooltipRenderer?: TooltipRenderer<any>;

  /** The data point for the tooltip that is currently visible (or was last visible). */
  protected _currentTooltipDataPoint: unknown = null;

  /**
   * Tracks which data point was last passed to tooltipRenderer.
   * Reset to `undefined` when the tooltip is hidden so a re-hover on the same
   * point triggers a fresh render.
   */
  private _lastRenderedTooltipDataPoint: unknown = undefined;

  /** Updates the custom tooltip renderer and refreshes a currently visible tooltip. */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public setTooltipRenderer(value: TooltipRenderer<any> | undefined): void {
    this.tooltipRenderer = value;
    this.tooltipRendererChanged();
  }

  protected tooltipRendererChanged(): void {
    this._lastRenderedTooltipDataPoint = undefined;
    if (this.tooltipProps.isVisible) {
      this.tooltipProps = { ...this.tooltipProps };
      this._syncTooltipRendererContent();
    }
  }

  private _syncTooltipRendererContent(): void {
    requestAnimationFrame(() => {
      const tooltipBody = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-body');
      const defaultContent = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-default-content');
      const customContent = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-custom-content');
      if (!this.tooltipRenderer && tooltipBody && !defaultContent) {
        tooltipBody.innerHTML = this._buildDefaultTooltipHTML(this._currentTooltipDataPoint);
        return;
      }
      if (defaultContent) {
        defaultContent.hidden = !!this.tooltipRenderer;
      }
      if (customContent) {
        customContent.hidden = !this.tooltipRenderer;
        if (!this.tooltipRenderer) {
          customContent.innerHTML = '';
        }
      }
    });
  }

  protected tooltipPropsChanged(_old: TooltipProps, newValue: TooltipProps): void {
    if (newValue.isVisible && !this.hideTooltip) {
      this.liveRegionText = [newValue.legend, newValue.yValue].filter(Boolean).join(': ');
      this._syncTooltipRendererContent();
      // Only invoke the renderer when the hovered data point has changed.
      //
      // This intentionally allows re-rendering on true→true isVisible transitions
      // (e.g. DonutChart / HBCWA moving between segments or bars without hiding
      // the tooltip in between) while still skipping redundant calls caused by
      // GanttChart's position-clamping RAF, which updates only xPos and leaves
      // _currentTooltipDataPoint unchanged.
      if (this.tooltipRenderer && this._currentTooltipDataPoint !== this._lastRenderedTooltipDataPoint) {
        const renderer = this.tooltipRenderer;
        this._lastRenderedTooltipDataPoint = this._currentTooltipDataPoint;
        requestAnimationFrame(() => {
          if (this.tooltipRenderer !== renderer) {
            return;
          }
          // Call the renderer BEFORE querying .tooltip-body.
          //
          // On the very first hover, FAST's when() directive hasn't yet run its own
          // rAF to insert .tooltip-body into the shadow DOM (it was queued after ours).
          // We still need to invoke the renderer so that the host (e.g. Blazor) is
          // notified and can re-render its portal.  The bridge's MutationObserver will
          // push the portal content once Blazor renders AND FAST has inserted the body.
          const result = renderer(this._currentTooltipDataPoint, (p: unknown) => this._buildDefaultTooltipHTML(p));

          const getTarget = () =>
            this.shadowRoot?.querySelector<HTMLElement>('.tooltip-custom-content') ??
            this.shadowRoot?.querySelector<HTMLElement>('.tooltip-body');
          const renderResult = (el: HTMLElement) => {
            el.innerHTML = '';
            if (result instanceof Promise) {
              result.then(r => {
                if (!this.tooltipProps?.isVisible || this.tooltipRenderer !== renderer) return;
                const body = getTarget();
                if (!body) return;
                if (typeof r === 'string') {
                  body.innerHTML = r;
                } else {
                  body.appendChild(r);
                }
              });
            } else if (typeof result === 'string') {
              el.innerHTML = result;
            } else {
              el.appendChild(result);
            }
          };

          const tooltipBody = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-body');
          const customContent = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-custom-content');
          if (tooltipBody?.classList.contains('preserve-default-content') && !customContent) {
            requestAnimationFrame(() => {
              if (this.tooltipRenderer !== renderer) return;
              const deferredTarget = this.shadowRoot?.querySelector<HTMLElement>('.tooltip-custom-content');
              if (deferredTarget) {
                renderResult(deferredTarget);
              }
            });
            return;
          }

          const el = getTarget();
          if (!el) {
            // .tooltip-body is not in the shadow DOM yet — FAST will insert it in the
            // next rAF.  The bridge MutationObserver handles populating it once ready.
            return;
          }
          renderResult(el);
        });
      }
    } else {
      this.liveRegionText = '';
      // Reset so re-hovering the same point after a hide triggers a fresh render.
      this._lastRenderedTooltipDataPoint = undefined;
    }
  }

  /**
   * Builds the default HTML string for the tooltip body.
   * Subclasses with a richer tooltip structure should override this.
   */
  protected _buildDefaultTooltipHTML(_dataPoint: unknown): string {
    const p = this.tooltipProps;
    return [
      `<div class="tooltip-inner" style="border-color: ${escapeHtml(p.color)};">`,
      `<div class="tooltip-legend-text">${escapeHtml(p.legend)}</div>`,
      `<div class="tooltip-content-y" style="color: ${escapeHtml(p.color)};">${escapeHtml(p.yValue)}</div>`,
      `</div>`,
    ].join('');
  }

  // ── Public refs ──────────────────────────────────────────────────

  public chartContainer!: HTMLDivElement;
  public elementInternals: ElementInternals = this.attachInternals();

  // ── Protected shared state ───────────────────────────────────────

  protected _isRTL: boolean = false;

  /** CSS transform used by centered tooltip templates, adjusted for RTL inline positioning. */
  @observable
  protected _tooltipTransform: string = 'translateX(-50%)';

  /** Keeps a freshly rendered tooltip hidden until its actual dimensions are measured. */
  @observable
  protected _isMeasuringTooltip: boolean = false;

  private _lastTooltipHeight: number = 64;
  private _lastTooltipWidth: number = 176;

  public get tooltipInlineTransform(): string {
    return this._tooltipTransform;
  }

  public get isMeasuringTooltip(): boolean {
    return this._isMeasuringTooltip;
  }

  /** Set to true in a subclass to automatically observe host resize and re-render. */
  protected _enableResizeObserver: boolean = false;

  // ── Private state ────────────────────────────────────────────────

  private _isSettingActiveLegend: boolean = false;
  private _mouseClickPending: boolean = false;
  private readonly _onShadowPointerDown = () => {
    this._mouseClickPending = true;
  };
  private readonly _onShadowPointerUp = () => {
    this._mouseClickPending = false;
  };
  private _renderDirty = false;
  private _frameHandle: number | null = null;
  private _resizeObserver?: ResizeObserver;
  private _axisLabelTooltipEl?: HTMLDivElement;

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
      'titleAlign',
      'titlePosition',
      'legendPosition',
      'hideLegends',
      'hideTooltip',
      'hideLabels',
      'roundCorners',
      'legendListLabel',
      'culture',
      'allowMultipleLegendSelection',
      'width',
      'height',
    ] as const;
    const observableFields = [
      'activeLegend',
      'isLegendSelected',
      'selectedLegends',
      'legends',
      'tooltipProps',
      'liveRegionText',
      '_tooltipTransform',
      '_isMeasuringTooltip',
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

    if (this._enableResizeObserver) {
      this._resizeObserver = new ResizeObserver(() => this._requestRender());
      this._resizeObserver.observe(this.chartContainer);
    }

    // Track mouse-click-initiated focus so handleLegendFocus can ignore it.
    // pointerdown fires before focus; pointerup fires before click.
    this.shadowRoot?.removeEventListener('pointerdown', this._onShadowPointerDown);
    this.shadowRoot?.removeEventListener('pointerup', this._onShadowPointerUp);
    this.shadowRoot?.addEventListener('pointerdown', this._onShadowPointerDown);
    this.shadowRoot?.addEventListener('pointerup', this._onShadowPointerUp);
  }

  public disconnectedCallback() {
    this.shadowRoot?.removeEventListener('pointerdown', this._onShadowPointerDown);
    this.shadowRoot?.removeEventListener('pointerup', this._onShadowPointerUp);
    this._resizeObserver?.disconnect();
    this._cancelScheduledRender();
    this._hideAxisLabelTooltip();
    this._axisLabelTooltipEl?.remove();
    this._axisLabelTooltipEl = undefined;
    super.disconnectedCallback();
  }

  // ── Attr change handlers ─────────────────────────────────────────

  protected chartTitleChanged() {
    if (this.$fastController.isConnected) {
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
    }
  }

  protected legendPositionChanged() {
    // no-op: CSS handles layout via attr selectors
  }

  protected hideLegendsChanged() {
    // no-op for base class: subclass logic handled by render
  }

  protected cultureChanged() {
    this._requestRender();
  }

  protected widthChanged() {
    this._requestRender();
  }

  protected heightChanged() {
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

  protected hideTooltipChanged() {
    if (this.hideTooltip) {
      this._clearTooltip();
    }
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

  public handleLegendFocus(legendTitle: string) {
    // When focus is triggered by a mouse click (pointerdown precedes focus),
    // skip state changes here — handleLegendClick will manage everything.
    if (this._mouseClickPending) {
      return;
    }
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else if (this.isLegendSelected) {
      // An explicit selection is active; arrow-key navigation should move the
      // keyboard cursor without disturbing the persistent filter.
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
      let nextSelection = this.selectedLegends.includes(legendTitle)
        ? this.selectedLegends.filter(legend => legend !== legendTitle)
        : [...this.selectedLegends, legendTitle];

      const legendCount = new Set(this.legends.map(legend => legend.legend)).size;
      if (nextSelection.length === legendCount) {
        nextSelection = [];
      }

      this.selectedLegends = nextSelection;

      if (nextSelection.length === 0) {
        this._setActiveLegend('');
      } else if (!nextSelection.includes(this.activeLegend)) {
        this._setActiveLegend(nextSelection[nextSelection.length - 1]);
      } else {
        this._updateLegendInteractionState();
      }

      this._announceLegendFilter();
      return;
    }

    if (this.isLegendSelected && this.activeLegend === legendTitle) {
      this.isLegendSelected = false;
      this._setActiveLegend('');
    } else {
      this.isLegendSelected = true;
      this._setActiveLegend(legendTitle);
    }
    this._announceLegendFilter();
  }

  private _announceLegendFilter(): void {
    const highlighted = this._getHighlightedLegends();
    if (highlighted.length === 0) {
      this.liveRegionText = 'Filter cleared.';
    } else if (highlighted.length === 1) {
      this.liveRegionText = `Filtered to ${highlighted[0]}.`;
    } else {
      this.liveRegionText = `Filtered to ${highlighted.join(', ')}.`;
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
      el.selected = this._getSelectedLegends();
    }
  }

  private _getSelectedLegends(): string[] {
    if (this.allowMultipleLegendSelection) {
      return this.selectedLegends;
    }
    return this.isLegendSelected && this.activeLegend ? [this.activeLegend] : [];
  }

  // ── Tooltip helpers ──────────────────────────────────────────────

  private static _clamp(value: number, min: number, max: number): number {
    if (max < min) {
      return min;
    }
    return Math.min(Math.max(value, min), max);
  }

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

  /**
   * Shows a shared HTML tooltip for a truncated axis label.
   * This mirrors React chart behavior more closely than native <title>.
   */
  protected _showAxisLabelTooltip(target: SVGTextElement, fullLabel: string): void {
    if (!this.shadowRoot || !fullLabel) {
      return;
    }

    const tooltip = this._getOrCreateAxisLabelTooltipElement();
    tooltip.textContent = fullLabel;

    const hostRect = this.getBoundingClientRect();
    const labelRect = target.getBoundingClientRect();
    const left = (labelRect.left + labelRect.right) / 2 - hostRect.left;
    const bottom = hostRect.bottom - (labelRect.top - 4);

    tooltip.style.left = `${Math.max(0, left)}px`;
    tooltip.style.bottom = `${Math.max(0, bottom)}px`;
    tooltip.style.transform = 'translateX(-50%)';
    tooltip.style.opacity = '0.9';
  }

  /** Hides the shared axis-label tooltip overlay. */
  protected _hideAxisLabelTooltip(): void {
    if (this._axisLabelTooltipEl) {
      this._axisLabelTooltipEl.style.opacity = '0';
    }
  }

  private _getOrCreateAxisLabelTooltipElement(): HTMLDivElement {
    if (this._axisLabelTooltipEl && this._axisLabelTooltipEl.isConnected) {
      return this._axisLabelTooltipEl;
    }

    const tooltip = document.createElement('div');
    tooltip.className = 'axis-label-tooltip';
    tooltip.setAttribute('role', 'tooltip');
    tooltip.style.opacity = '0';
    this.shadowRoot!.appendChild(tooltip);
    this._axisLabelTooltipEl = tooltip;
    return tooltip;
  }

  /**
   * Positions the tooltip around an anchor point and keeps it within the host bounds.
   * `anchorX` / `anchorY` are physical host-relative coordinates (LTR geometry).
   */
  protected _positionTooltipFromAnchor(anchorX: number, anchorY: number, options: TooltipPositionOptions = {}): void {
    if (!this.tooltipProps?.isVisible || this.hideTooltip) {
      return;
    }

    this.tooltipProps = { ...this.tooltipProps, ...this._resolveTooltipPositionFromAnchor(anchorX, anchorY, options) };
  }

  /**
   * Computes clamped tooltip position around an anchor point.
   * `anchorX` / `anchorY` are physical host-relative coordinates (LTR geometry).
   */
  protected _resolveTooltipPositionFromAnchor(
    anchorX: number,
    anchorY: number,
    options: TooltipPositionOptions = {},
  ): Pick<TooltipProps, 'xPos' | 'yPos'> {
    const hostWidth = this.offsetWidth;
    const hostHeight = this.offsetHeight;
    if (hostWidth <= 0 || hostHeight <= 0) {
      return { xPos: 0, yPos: 0 };
    }

    const preferredVertical = options.preferredVertical ?? 'above';
    const horizontalAlign = options.horizontalAlign ?? 'center';
    const gap = options.gap ?? 8;
    const padding = options.padding ?? 8;
    const estimatedWidth = options.estimatedWidth ?? 176;
    const estimatedHeight = options.estimatedHeight ?? 64;
    const outputAnchorX = options.outputAnchorX ?? false;
    const boundsWidth = Math.max(0, Math.min(options.boundsWidth ?? hostWidth, hostWidth));
    const boundsHeight = Math.max(0, Math.min(options.boundsHeight ?? hostHeight, hostHeight));
    const widthForClamp = boundsWidth || hostWidth;
    const heightForClamp = boundsHeight || hostHeight;

    let left = anchorX;
    if (horizontalAlign === 'center') {
      left = anchorX - estimatedWidth / 2;
    } else if (horizontalAlign === 'end') {
      left = anchorX - estimatedWidth;
    }

    const maxLeft = widthForClamp - estimatedWidth - padding;
    left = ChartBase._clamp(left, padding, maxLeft);

    const topAbove = anchorY - estimatedHeight - gap;
    const topBelow = anchorY + gap;
    let top = preferredVertical === 'below' ? topBelow : topAbove;

    if (preferredVertical === 'above' && top < padding) {
      top = options.preventAnchorOverlap ? padding : topBelow;
    } else if (preferredVertical === 'below' && top + estimatedHeight > heightForClamp - padding) {
      top = options.preventAnchorOverlap ? heightForClamp - estimatedHeight - padding : topAbove;
    }

    const maxTop = heightForClamp - estimatedHeight - padding;
    top = ChartBase._clamp(top, padding, maxTop);

    if (outputAnchorX) {
      const minAnchorX = padding + estimatedWidth / 2;
      const maxAnchorX = widthForClamp - padding - estimatedWidth / 2;
      const clampedAnchorX = ChartBase._clamp(anchorX, minAnchorX, maxAnchorX);
      return {
        xPos: this._isRTL ? Math.max(0, widthForClamp - clampedAnchorX) : Math.max(0, clampedAnchorX),
        yPos: top,
      };
    }

    const inlineStart = this._isRTL ? widthForClamp - left - estimatedWidth : left;
    return { xPos: Math.max(0, inlineStart), yPos: top };
  }

  /**
   * Positions a tooltip outside the active datum, then corrects that position after
   * measuring the rendered tooltip. Subclasses provide the datum's vertical bounds.
   */
  protected _positionTooltipAvoidingOverlap(
    anchorX: number,
    topY: number,
    bottomY: number = topY,
    isFreshShow: boolean = true,
    options: TooltipOverlapPositionOptions = {},
  ): void {
    const gap = options.gap ?? 16;
    const padding = 8;
    const useSidePlacement = options.horizontalPlacement === 'side';

    this._tooltipTransform = useSidePlacement ? 'none' : this._isRTL ? 'translateX(50%)' : 'translateX(-50%)';

    const applyPosition = (estimatedHeight: number, estimatedWidth: number): void => {
      const hostHeight = this.offsetHeight;
      const hostWidth = this.offsetWidth;
      const roomAbove = topY - padding;
      const roomBelow = hostHeight - bottomY - padding;
      const preferBelow = options.preferredVerticalSide === 'below';
      const preferredVertical = preferBelow
        ? roomBelow >= estimatedHeight + gap || roomBelow >= roomAbove
          ? 'below'
          : 'above'
        : roomAbove >= estimatedHeight + gap || roomAbove >= roomBelow
        ? 'above'
        : 'below';
      const anchorY = preferredVertical === 'above' ? topY : bottomY;

      if (useSidePlacement) {
        const { yPos } = this._resolveTooltipPositionFromAnchor(anchorX, anchorY, {
          preferredVertical,
          preventAnchorOverlap: true,
          estimatedHeight,
          estimatedWidth,
          gap,
        });
        const preferLeft = options.preferredHorizontalSide ? options.preferredHorizontalSide === 'left' : this._isRTL;
        const preferredLeft = preferLeft ? anchorX - gap - estimatedWidth : anchorX + gap;
        const fitsPreferredSide = preferredLeft >= padding && preferredLeft + estimatedWidth <= hostWidth - padding;
        const physicalLeft = fitsPreferredSide
          ? preferredLeft
          : preferLeft
          ? anchorX + gap
          : anchorX - gap - estimatedWidth;
        const clampedLeft = ChartBase._clamp(physicalLeft, padding, hostWidth - estimatedWidth - padding);
        const inlineStart = this._isRTL ? hostWidth - clampedLeft - estimatedWidth : clampedLeft;
        this.tooltipProps = { ...this.tooltipProps, xPos: Math.max(0, inlineStart), yPos };
        return;
      }

      this._positionTooltipFromAnchor(anchorX, anchorY, {
        outputAnchorX: true,
        preferredVertical,
        preventAnchorOverlap: true,
        estimatedHeight,
        estimatedWidth,
        gap,
      });
    };

    applyPosition(this._lastTooltipHeight, this._lastTooltipWidth);

    if (!isFreshShow) {
      return;
    }

    this._isMeasuringTooltip = true;
    const measure = (retriesLeft: number): void => {
      if (!this.tooltipProps.isVisible) {
        this._isMeasuringTooltip = false;
        return;
      }

      const rect = this.shadowRoot?.querySelector<HTMLElement>('.tooltip')?.getBoundingClientRect();
      if (rect && rect.height > 0 && rect.width > 0) {
        this._lastTooltipHeight = rect.height;
        this._lastTooltipWidth = rect.width;
        applyPosition(rect.height, rect.width);
        this._isMeasuringTooltip = false;
      } else if (retriesLeft > 0) {
        requestAnimationFrame(() => measure(retriesLeft - 1));
      } else {
        this._isMeasuringTooltip = false;
      }
    };

    requestAnimationFrame(() => measure(2));
  }
  /**
   * Implements the roving tabindex keyboard pattern for a focusable group.
   * Arrow keys move focus within `elements` without adding extra Tab stops.
   */
  protected _rovingKeydown(elements: Element[], e: KeyboardEvent): void {
    const forward = e.key === 'ArrowRight' || e.key === 'ArrowDown';
    const backward = e.key === 'ArrowLeft' || e.key === 'ArrowUp';
    if (!forward && !backward) {
      return;
    }
    e.preventDefault();
    const currentIndex = elements.findIndex(el => (el as HTMLElement).tabIndex === 0);
    if (currentIndex === -1) {
      return;
    }
    const nextIndex = forward
      ? (currentIndex + 1) % elements.length
      : (currentIndex - 1 + elements.length) % elements.length;
    (elements[currentIndex] as HTMLElement).tabIndex = -1;
    (elements[nextIndex] as HTMLElement).tabIndex = 0;
    (elements[nextIndex] as HTMLElement).focus();
  }

  /** Promotes a pointer-selected element to the active member of a roving tabindex group. */
  protected _focusRovingElement(elements: HTMLOrSVGElement[], target: HTMLOrSVGElement): void {
    elements.forEach(element => {
      element.tabIndex = element === target ? 0 : -1;
    });
    target.focus();
  }

  /**
   * If one of the `candidates` data elements currently has focus but has just been
   * set to tabIndex -1 (i.e. it became inactive), move focus to the first candidate
   * that still has tabIndex 0.
   *
   * We explicitly require the focused element to be in `candidates` so that nested
   * elements (e.g. the fluent-chart-legend host, whose tabIndex is -1 by default)
   * do not accidentally trigger a focus relocation when the user tabs into the legend.
   */
  protected _relocateFocusIfNeeded(candidates: HTMLOrSVGElement[]): void {
    const focused = this.shadowRoot?.activeElement;
    if (
      focused &&
      candidates.includes(focused as unknown as HTMLOrSVGElement) &&
      (focused as unknown as HTMLOrSVGElement).tabIndex === -1
    ) {
      candidates.find(el => el.tabIndex === 0)?.focus();
    }
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
      this._isRTL = getRTL(this);
      this._hideAxisLabelTooltip();
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

  /**
   * Returns a safe SVG width/height attribute value when host dimensions are also applied in CSS.
   * Percentages are normalized to `100%` to avoid percentage-of-percentage double scaling.
   */
  public _toSvgLength(value: number | string | undefined, fallback: number | string): number | string {
    if (value === undefined || value === null || value === '') {
      return fallback;
    }

    if (typeof value === 'string' && value.trim().endsWith('%')) {
      return '100%';
    }

    return value;
  }
}
