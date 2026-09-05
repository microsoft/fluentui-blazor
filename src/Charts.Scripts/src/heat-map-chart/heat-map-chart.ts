import { attr } from '@microsoft/fast-element';
import { type Axis, axisBottom, axisLeft } from 'd3-axis';
import { type ScaleBand, scaleBand, scaleLinear } from 'd3-scale';
import { format as d3Format } from 'd3-format';
import { timeFormat as d3TimeFormat } from 'd3-time-format';
import { resolveChartMargins } from '../utils/cartesian-axis-helpers.js';
import { CartesianChartBase } from '../utils/cartesian-chart-base.js';
import {
  type AxisScaleLike,
  renderBandYAxisShared,
  renderBottomAxisShared,
  toOptionalAxisNumber as toOptionalNumber,
} from '../utils/cartesian-axis-shared.js';
import { getColorFromToken, jsonConverter, SVG_NAMESPACE_URI } from '../utils/chart-helpers.js';
import type { AxisCategoryOrder, Legend, TooltipProps } from '../utils/chart-options.js';
import type { HeatMapChartData, HeatMapChartDataPoint, HeatMapSortOrder } from './heat-map-chart.options.js';

// ── Internal types ────────────────────────────────────────────────────────────

type FlatPoint = HeatMapChartDataPoint & { legend: string };

type HeatMapTooltipProps = TooltipProps & {
  rectText: string;
  ratio?: [number, number];
  descriptionMessage?: string;
};

// ── Constants ─────────────────────────────────────────────────────────────────

const MARGIN_TOP = 20;
const MARGIN_BOTTOM = 35;
const MARGIN_LEFT_MIN = 40;
const MARGIN_LEFT_LABEL_GAP = 20;
const MARGIN_RIGHT = 20;
const DEFAULT_WIDTH = 640;
const DEFAULT_HEIGHT = 420;
const CELL_FONT_SIZE = 11;
/** Height reserved for the legend row: fluent-chart-legend (32 px) + its margin-top (spacingVerticalS ≈ 8 px). */
const LEGEND_HEIGHT = 40;
/** Height reserved for the chart title: body1Strong line-height (20 px) + margin-bottom (8 px). */
const TITLE_HEIGHT = 28;

// ── Helpers ───────────────────────────────────────────────────────────────────

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const detectAxisType = (value: string | Date | number): 'date' | 'number' | 'string' => {
  if (value instanceof Date) {
    return 'date';
  }
  if (typeof value === 'number') {
    return 'number';
  }
  // ISO date string?
  const d = new Date(value as string);
  if (!isNaN(d.getTime()) && /^\d{4}/.test(value as string)) {
    return 'date';
  }
  // Numeric string?
  if (value !== '' && !isNaN(Number(value))) {
    return 'number';
  }
  return 'string';
};

const axisValueToKey = (value: string | Date | number, type: 'date' | 'number' | 'string'): string => {
  if (type === 'date') {
    const d = value instanceof Date ? value : new Date(value as string);
    return String(d.getTime());
  }
  return String(value);
};

const formatAxisKey = (
  key: string,
  type: 'date' | 'number' | 'string',
  dateFormat: string,
  numberFormat: string,
): string => {
  if (type === 'date') {
    const date = new Date(Number(key));
    return d3TimeFormat(dateFormat)(date);
  }
  if (type === 'number') {
    return d3Format(numberFormat)(Number(key));
  }
  return key;
};

// Determine foreground text color with adequate contrast against `bgHex`.
const getTextColorForBg = (bgHex: string): string => {
  const clean = bgHex.replace('#', '');
  if (clean.length !== 6) {
    return '#000000';
  }
  const r = parseInt(clean.slice(0, 2), 16);
  const g = parseInt(clean.slice(2, 4), 16);
  const b = parseInt(clean.slice(4, 6), 16);
  // Relative luminance (WCAG 2.1)
  const toLinear = (c: number) => {
    const s = c / 255;
    return s <= 0.04045 ? s / 12.92 : Math.pow((s + 0.055) / 1.055, 2.4);
  };
  const L = 0.2126 * toLinear(r) + 0.7152 * toLinear(g) + 0.0722 * toLinear(b);
  // Contrast vs white (L=1) and black (L=0)
  const contrastWhite = (1 + 0.05) / (L + 0.05);
  const contrastBlack = (L + 0.05) / (0 + 0.05);
  return contrastWhite >= contrastBlack ? '#ffffff' : '#000000';
};

/**
 * `<fluent-heat-map-chart>` – a grid-based heat map chart where each cell is
 * colored by a continuous color scale.
 *
 * Follows the same architecture as the other chart web components in this
 * package (extends CartesianChartBase, renders into `chartContainer`, uses
 * chart-base legend and tooltip systems).
 *
 * @public
 */
export class HeatMapChart extends CartesianChartBase {
  // ── Attrs ─────────────────────────────────────────────────────────────────

  @attr({ converter: jsonConverter })
  public data!: HeatMapChartData[];

  @attr
  public width?: number | string;

  @attr
  public height?: number | string;

  /** Control points for the color scale (one per color in `rangeValuesForColorScale`). */
  @attr({ attribute: 'domain-values-for-color-scale', converter: jsonConverter })
  public domainValuesForColorScale: number[] = [];

  /** Colors (hex strings or CSS tokens) mapped to domain control points. */
  @attr({ attribute: 'range-values-for-color-scale', converter: jsonConverter })
  public rangeValuesForColorScale: string[] = [];

  /** d3 time-format string for x-axis Date labels. Default: `'%b/%d'`. */
  @attr({ attribute: 'x-axis-date-format-string' })
  public xAxisDateFormatString?: string;

  /** d3 time-format string for y-axis Date labels. Default: `'%b/%d'`. */
  @attr({ attribute: 'y-axis-date-format-string' })
  public yAxisDateFormatString?: string;

  /** d3 number-format string for x-axis numeric labels. Default: `'.2~s'`. */
  @attr({ attribute: 'x-axis-number-format-string' })
  public xAxisNumberFormatString?: string;

  /** d3 number-format string for y-axis numeric labels. Default: `'.2~s'`. */
  @attr({ attribute: 'y-axis-number-format-string' })
  public yAxisNumberFormatString?: string;

  /** Max width in px for y-axis tick labels before truncating with ellipsis. */
  @attr({ attribute: 'y-axis-tick-label-max-width' })
  public yAxisTickLabelMaxWidth?: number | string;

  /** Sort order for string axis labels. Default: `'alphabetical'`. */
  @attr({ attribute: 'sort-order' })
  public sortOrder: HeatMapSortOrder = 'alphabetical';

  /**
   * Sort order for x-axis category labels.
   * Supports all 14 AxisCategoryOrder modes. Default: `'default'`.
   * When set, takes precedence over `sortOrder` for the x-axis.
   */
  @attr({ attribute: 'x-axis-category-order' })
  public override xAxisCategoryOrder: AxisCategoryOrder = 'default';

  /**
   * Sort order for y-axis category labels.
   * Supports all 14 AxisCategoryOrder modes. Default: `'default'`.
   * When set, takes precedence over `sortOrder` for the y-axis.
   */
  @attr({ attribute: 'y-axis-category-order' })
  public yAxisCategoryOrder: AxisCategoryOrder = 'default';

  /**
   * Optional JS function to map x-axis string keys to display labels.
   * Called for each unique x-axis string value. When set, overrides the raw string key.
   * Cannot be set via HTML attribute — assign directly on the element.
   */
  public xAxisStringFormatter?: (key: string) => string;

  /**
   * Optional JS function to map y-axis string keys to display labels.
   * Called for each unique y-axis string value. When set, overrides the raw string key.
   * Cannot be set via HTML attribute — assign directly on the element.
   */
  public yAxisStringFormatter?: (key: string) => string;

  /**
   * JSON dictionary mapping x-axis string keys to display labels.
   * Used by Blazor (where JS functions cannot be passed as attributes).
   * Example: '\{"monday":"Mon","tuesday":"Tue"\}'
   */
  @attr({ attribute: 'x-axis-string-labels', converter: jsonConverter })
  public xAxisStringLabels?: Record<string, string>;

  /**
   * JSON dictionary mapping y-axis string keys to display labels.
   * Used by Blazor (where JS functions cannot be passed as attributes).
   * Example: '\{"q1":"Q1 2024","q2":"Q2 2024"\}'
   */
  @attr({ attribute: 'y-axis-string-labels', converter: jsonConverter })
  public yAxisStringLabels?: Record<string, string>;

  /** Narrows the inherited base tooltipProps type to include heat map fields. */
  public declare tooltipProps: HeatMapTooltipProps;

  protected override _enableResizeObserver = true;

  // ── Private state ─────────────────────────────────────────────────────────

  private _renderedCells: SVGGElement[] = [];
  /** Width of the last rendered SVG — used to clamp tooltip position. */
  private _lastSvgWidth: number = 0;
  /** Height of the last rendered SVG — used to clamp tooltip position. */
  private _lastSvgHeight: number = 0;

  connectedCallback(): void {
    const self = this as Record<string, unknown>;
    const attrFields = [
      'data',
      'width',
      'height',
      'domainValuesForColorScale',
      'rangeValuesForColorScale',
      'xAxisDateFormatString',
      'yAxisDateFormatString',
      'xAxisNumberFormatString',
      'yAxisNumberFormatString',
      'yAxisTickLabelMaxWidth',
      'sortOrder',
      'xAxisCategoryOrder',
      'yAxisCategoryOrder',
      'xAxisStringLabels',
      'yAxisStringLabels',
    ] as const;

    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    self['tooltipProps'] = {
      isVisible: false,
      legend: '',
      yValue: '',
      color: '',
      xPos: 0,
      yPos: 0,
      rectText: '',
      ratio: undefined,
      descriptionMessage: undefined,
    } satisfies HeatMapTooltipProps;

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this.addEventListener('mouseleave', this._handleMouseLeave);

    if (this.data) {
      this._requestRender();
    }
  }

  public disconnectedCallback(): void {
    this.removeEventListener('mouseleave', this._handleMouseLeave);
    super.disconnectedCallback();
  }

  // ── Changed callbacks ─────────────────────────────────────────────────────

  protected dataChanged(): void {
    this._requestRender();
  }

  protected widthChanged(): void {
    this._requestRender();
  }

  protected heightChanged(): void {
    this._requestRender();
  }

  protected domainValuesForColorScaleChanged(): void {
    this._requestRender();
  }

  protected rangeValuesForColorScaleChanged(): void {
    this._requestRender();
  }

  protected xAxisDateFormatStringChanged(): void {
    this._requestRender();
  }

  protected yAxisDateFormatStringChanged(): void {
    this._requestRender();
  }

  protected xAxisNumberFormatStringChanged(): void {
    this._requestRender();
  }

  protected yAxisNumberFormatStringChanged(): void {
    this._requestRender();
  }

  protected yAxisTickLabelMaxWidthChanged(): void {
    this._requestRender();
  }

  protected sortOrderChanged(): void {
    this._requestRender();
  }

  protected xAxisCategoryOrderChanged(): void {
    this._requestRender();
  }

  protected yAxisCategoryOrderChanged(): void {
    this._requestRender();
  }

  protected xAxisStringLabelsChanged(): void {
    this._requestRender();
  }

  protected yAxisStringLabelsChanged(): void {
    this._requestRender();
  }

  protected override tooltipPropsChanged(_old: TooltipProps, newValue: TooltipProps): void {
    const typed = newValue as HeatMapTooltipProps;
    if (typed.isVisible && !this.hideTooltip) {
      const parts: string[] = [];
      if (typed.legend) {
        parts.push(typed.legend);
      }
      if (typed.rectText) {
        parts.push(typed.rectText);
      }
      this.liveRegionText = parts.join(': ');
    } else {
      this.liveRegionText = '';
    }
  }

  // ── Aria / accessibility ──────────────────────────────────────────────────

  protected _getHostAriaLabel(): string {
    const count = this.data?.reduce((n, d) => n + d.data.length, 0) ?? 0;
    return (this.chartTitle ? `${this.chartTitle}. ` : '') + `Heat map chart with ${count} data points.`;
  }

  // ── Render (called by ChartBase._requestRender) ───────────────────────────

  protected _performRender(): void {
    this._applyHostDimensions();
    this._renderChart();
  }

  protected _applyHostDimensions() {
    super._applyHostDimensions(this.width, this.height);
  }

  // ── Legend interaction ────────────────────────────────────────────────────

  protected _applyActiveLegendState(): void {
    if (!this._renderedCells) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    this._renderedCells.forEach(cell => {
      const legend = cell.dataset.legend ?? '';
      if (highlighted.length === 0 || highlighted.includes(legend)) {
        cell.style.opacity = '1';
        cell.classList.remove('inactive');
      } else {
        cell.style.opacity = '0.1';
        cell.classList.add('inactive');
      }
    });
  }

  // ── Private helpers ───────────────────────────────────────────────────────

  private readonly _handleMouseLeave = (): void => {
    this._clearHeatTooltip();
  };

  private _clearHeatTooltip(): void {
    this.tooltipProps = {
      ...(this.tooltipProps as HeatMapTooltipProps),
      isVisible: false,
    };
  }

  private _showHeatTooltip(event: MouseEvent | FocusEvent, point: FlatPoint, cellColor: string): void {
    if (this.hideTooltip) {
      return;
    }
    let clientX: number;
    let clientY: number;
    if (event instanceof MouseEvent) {
      clientX = event.clientX;
      clientY = event.clientY;
    } else {
      const rect = (event.currentTarget as Element).getBoundingClientRect();
      clientX = rect.left + rect.width / 2;
      clientY = rect.top + rect.height / 2;
    }
    const hostRect = this.getBoundingClientRect();
    const relX = clientX - hostRect.left;
    const relY = clientY - hostRect.top;

    // Clamp against the SVG dimensions — the visible chart boundary —
    // not the host width, which can be wider due to :host { width: 100% }.
    const chartW = this._lastSvgWidth || hostRect.width;
    const chartH = this._lastSvgHeight || hostRect.height;

    const rectText =
      point.rectText !== undefined
        ? String(point.rectText)
        : !isNaN(point.value)
        ? String(point.value)
        : 'No data available';

    this.tooltipProps = {
      isVisible: true,
      legend: point.legend,
      yValue: rectText,
      color: cellColor,
      xPos: relX,
      yPos: relY,
      rectText,
      ratio: point.ratio,
      descriptionMessage: point.descriptionMessage,
    } satisfies HeatMapTooltipProps;
    this._positionTooltipFromAnchor(relX, relY, {
      preferredVertical: 'below',
      horizontalAlign: 'center',
      gap: 12,
      estimatedWidth: 270,
      estimatedHeight: 130,
      boundsWidth: chartW,
      boundsHeight: chartH,
    });
  }

  private _buildColorScale() {
    const domain =
      Array.isArray(this.domainValuesForColorScale) && this.domainValuesForColorScale.length > 0
        ? this.domainValuesForColorScale
        : [0, 100];

    const resolvedRange =
      Array.isArray(this.rangeValuesForColorScale) && this.rangeValuesForColorScale.length > 0
        ? this.rangeValuesForColorScale.map(c => getColorFromToken(c))
        : ['#e6f2f8', '#004d8c'];

    return scaleLinear<string>()
      .domain(domain)
      .range(resolvedRange as string[]);
  }

  private _flattenData(): FlatPoint[] {
    const flat: FlatPoint[] = [];
    (this.data || []).forEach(series => {
      series.data.forEach(point => {
        flat.push({ ...point, legend: series.legend });
      });
    });
    return flat;
  }

  private _detectTypes(flat: FlatPoint[]): {
    xType: 'date' | 'number' | 'string';
    yType: 'date' | 'number' | 'string';
  } {
    const first = flat[0];
    return {
      xType: first ? detectAxisType(first.x) : 'string',
      yType: first ? detectAxisType(first.y) : 'string',
    };
  }

  private _collectLabels(
    flat: FlatPoint[],
    xType: 'date' | 'number' | 'string',
    yType: 'date' | 'number' | 'string',
  ): { xLabels: string[]; yLabels: string[] } {
    const xKeySet = new Set<string>();
    const yKeySet = new Set<string>();
    const xDateFormat = this.xAxisDateFormatString ?? '%b/%d';
    const yDateFormat = this.yAxisDateFormatString ?? '%b/%d';
    const xNumFormat = this.xAxisNumberFormatString ?? '.2~s';
    const yNumFormat = this.yAxisNumberFormatString ?? '.2~s';

    flat.forEach(p => {
      xKeySet.add(axisValueToKey(p.x, xType));
      yKeySet.add(axisValueToKey(p.y, yType));
    });

    const sortKeys = (
      keys: Set<string>,
      type: 'date' | 'number' | 'string',
      order: AxisCategoryOrder | undefined,
      flat: FlatPoint[],
      axis: 'x' | 'y',
    ): string[] => {
      const arr = Array.from(keys);
      // Date/number axes: always sort numerically.
      if (type === 'date' || type === 'number') {
        return arr.sort((a, b) => Number(a) - Number(b));
      }
      // Determine effective order: axis-specific order wins, then fall back to sortOrder.
      const effectiveOrder: Exclude<AxisCategoryOrder, 'default'> | HeatMapSortOrder =
        !order || order === 'default' ? (this.sortOrder === 'none' ? 'none' : 'alphabetical') : order;

      if (effectiveOrder === 'none' || effectiveOrder === 'data') {
        return arr; // preserve insertion order
      }
      if (effectiveOrder === 'category ascending' || effectiveOrder === 'alphabetical') {
        return arr.sort((a, b) => a.toLowerCase().localeCompare(b.toLowerCase()));
      }
      if (effectiveOrder === 'category descending') {
        return arr.sort((a, b) => b.toLowerCase().localeCompare(a.toLowerCase()));
      }

      // Aggregate-based orders: need to compute per-key aggregates.
      const getValues = (key: string): number[] =>
        flat
          .filter(p => (axis === 'x' ? axisValueToKey(p.x, type) : axisValueToKey(p.y, type)) === key)
          .map(p => p.value);

      const aggregate = (key: string): number => {
        const values = getValues(key);
        if (values.length === 0) return 0;
        switch (effectiveOrder) {
          case 'total ascending':
          case 'total descending':
          case 'sum ascending':
          case 'sum descending':
            return values.reduce((s, v) => s + v, 0);
          case 'min ascending':
          case 'min descending':
            return Math.min(...values);
          case 'max ascending':
          case 'max descending':
            return Math.max(...values);
          case 'mean ascending':
          case 'mean descending':
            return values.reduce((s, v) => s + v, 0) / values.length;
          case 'median ascending':
          case 'median descending': {
            const sorted = [...values].sort((a, b) => a - b);
            const mid = Math.floor(sorted.length / 2);
            return sorted.length % 2 === 0 ? (sorted[mid - 1] + sorted[mid]) / 2 : sorted[mid];
          }
          default:
            return 0;
        }
      };

      const isDescending = effectiveOrder.endsWith('descending');
      return arr.sort((a, b) => {
        const diff = aggregate(a) - aggregate(b);
        return isDescending ? -diff : diff;
      });
    };

    const xKeys = sortKeys(xKeySet, xType, this.xAxisCategoryOrder, flat, 'x');
    const yKeys = sortKeys(yKeySet, yType, this.yAxisCategoryOrder, flat, 'y');

    const xLabels = xKeys.map(k => {
      if (xType === 'string') {
        if (this.xAxisStringFormatter) return this.xAxisStringFormatter(k);
        if (this.xAxisStringLabels?.[k] !== undefined) return this.xAxisStringLabels[k];
      }
      return formatAxisKey(k, xType, xDateFormat, xNumFormat);
    });
    const yLabels = yKeys.map(k => {
      if (yType === 'string') {
        if (this.yAxisStringFormatter) return this.yAxisStringFormatter(k);
        if (this.yAxisStringLabels?.[k] !== undefined) return this.yAxisStringLabels[k];
      }
      return formatAxisKey(k, yType, yDateFormat, yNumFormat);
    });

    return { xLabels, yLabels };
  }

  /**
   * Build a lookup map from \{xLabel, yLabel\} → FlatPoint.
   * This enables O(1) cell lookups when rendering the grid.
   */
  private _buildPointMap(
    flat: FlatPoint[],
    xType: 'date' | 'number' | 'string',
    yType: 'date' | 'number' | 'string',
  ): Map<string, FlatPoint> {
    const xDateFormat = this.xAxisDateFormatString ?? '%b/%d';
    const yDateFormat = this.yAxisDateFormatString ?? '%b/%d';
    const xNumFormat = this.xAxisNumberFormatString ?? '.2~s';
    const yNumFormat = this.yAxisNumberFormatString ?? '.2~s';

    const map = new Map<string, FlatPoint>();
    flat.forEach(p => {
      const xKey = axisValueToKey(p.x, xType);
      const yKey = axisValueToKey(p.y, yType);
      const xLabel =
        xType === 'string'
          ? this.xAxisStringFormatter?.(xKey) ??
            this.xAxisStringLabels?.[xKey] ??
            formatAxisKey(xKey, xType, xDateFormat, xNumFormat)
          : formatAxisKey(xKey, xType, xDateFormat, xNumFormat);
      const yLabel =
        yType === 'string'
          ? this.yAxisStringFormatter?.(yKey) ??
            this.yAxisStringLabels?.[yKey] ??
            formatAxisKey(yKey, yType, yDateFormat, yNumFormat)
          : formatAxisKey(yKey, yType, yDateFormat, yNumFormat);
      map.set(`${xLabel}|${yLabel}`, p);
    });
    return map;
  }

  private _getAriaLabel(point: FlatPoint | null, xLabel: string, yLabel: string): string {
    if (!point) {
      return `${xLabel}, ${yLabel}. No data available.`;
    }
    const zValue = point.ratio ? `${point.ratio[0]}/${point.ratio[1]}` : point.rectText ?? point.value;
    const base = `${xLabel}, ${yLabel}. ${point.legend}, ${zValue}.`;
    return (
      point.callOutAccessibilityData?.ariaLabel ??
      base + (point.descriptionMessage ? ` ${point.descriptionMessage}.` : '')
    );
  }

  private _renderChart(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._clearChart();

    if (!Array.isArray(this.data) || this.data.length === 0) {
      this.legends = [];
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    this.elementInternals.ariaLabel = this._getHostAriaLabel();

    const colorScale = this._buildColorScale();
    const flat = this._flattenData();
    const { xType, yType } = this._detectTypes(flat);
    const { xLabels, yLabels } = this._collectLabels(flat, xType, yType);
    const pointMap = this._buildPointMap(flat, xType, yType);

    const containerWidth =
      this.chartContainer.getBoundingClientRect().width || this.getBoundingClientRect().width || DEFAULT_WIDTH;

    const w = Math.max(parseFloat(String(this.width)) || containerWidth, 200);
    const legendOffset = this.hideLegends ? 0 : LEGEND_HEIGHT;
    const titleOffset = this.chartTitle ? TITLE_HEIGHT : 0;
    const h = Math.max((parseFloat(String(this.height)) || DEFAULT_HEIGHT) - legendOffset - titleOffset, 100);

    const isRTL = this._isRTL;
    const yLabelMargin = this._measureLongestYLabel(yLabels);
    const margins = resolveChartMargins(
      { top: MARGIN_TOP, right: MARGIN_RIGHT, bottom: MARGIN_BOTTOM, left: yLabelMargin },
      this.margins,
      isRTL,
    );
    const innerWidth = w - margins.left - margins.right;
    const innerHeight = h - margins.top - margins.bottom;

    // ── Scales ───────────────────────────────────────────────────────────────

    const xScale = scaleBand()
      .domain(isRTL ? [...xLabels].reverse() : xLabels)
      .range([0, innerWidth])
      .padding(0.02);

    // y-axis: first label at top → range [0, innerHeight] with first label at 0
    const yScale = scaleBand()
      .domain([...yLabels].reverse())
      .range([0, innerHeight])
      .padding(0.02);

    // ── SVG structure ─────────────────────────────────────────────────────────

    const svg = this._createChartSvg(w, h, { role: 'none' });

    this._lastSvgWidth = w;
    this._lastSvgHeight = h;

    const g = createSvgElement<SVGGElement>('g');
    g.setAttribute('transform', `translate(${margins.left},${margins.top})`);
    svg.appendChild(g);

    // ── Axes ──────────────────────────────────────────────────────────────────

    const xAxis = axisBottom(xScale).tickPadding(8);
    renderBottomAxisShared({
      svg,
      scale: xScale as AxisScaleLike<string>,
      axis: xAxis as Axis<string>,
      formatter: value => value,
      axisLeft: margins.left,
      axisTop: margins.top,
      innerWidth,
      innerHeight,
      tickPadding: 8,
      isRTL,
      showTickLines: false,
      xAxisTitle: this.xAxisTitle,
      titleClassName: 'axis-title',
    });

    const yAxis = axisLeft(yScale).tickPadding(0);
    renderBandYAxisShared({
      svg,
      scale: yScale as AxisScaleLike<string>,
      axis: yAxis as Axis<string>,
      formatter: value => value,
      axisX: margins.left,
      axisTop: margins.top,
      innerHeight,
      isRTL,
      tickPadding: 0,
      showTickLines: false,
      ltrLabelX: -6,
      rtlLabelX: innerWidth + 6,
      yAxisTitle: this.yAxisTitle,
      titleClassName: 'axis-title',
      tickLabelMaxWidth: toOptionalNumber(this.yAxisTickLabelMaxWidth),
    });

    // ── Grid cells ────────────────────────────────────────────────────────────

    this._renderedCells = [];
    let firstCell = true;

    yLabels.forEach(yLabel => {
      const yscaledY = yScale(yLabel);
      if (yscaledY === undefined) {
        return;
      }

      xLabels.forEach(xLabel => {
        const scaledX = xScale(xLabel);
        if (scaledX === undefined) {
          return;
        }

        const key = `${xLabel}|${yLabel}`;
        const point = pointMap.get(key) ?? null;
        const hasData = point !== null && !isNaN(point.value);
        const fillColor = hasData ? colorScale(point!.value) : 'transparent';
        const textColor = hasData ? getTextColorForBg(fillColor) : 'transparent';
        const ariaLabel = this._getAriaLabel(point, xLabel, yLabel);
        const legend = point?.legend ?? '';

        const cell = createSvgElement<SVGGElement>('g');
        cell.setAttribute('class', 'heat-cell');
        cell.setAttribute('role', 'img');
        cell.setAttribute('aria-label', ariaLabel);
        cell.setAttribute('data-legend', legend);
        cell.setAttribute('data-x', xLabel);
        cell.setAttribute('data-y', yLabel);
        cell.setAttribute('fill-opacity', legend && !this._getHighlightedLegends().length ? '1' : '1');
        cell.setAttribute('tabindex', firstCell ? '0' : '-1');
        cell.setAttribute('transform', `translate(${scaledX},${yscaledY})`);
        if (firstCell) {
          firstCell = false;
        }

        const rect = createSvgElement<SVGRectElement>('rect');
        rect.setAttribute('width', `${xScale.bandwidth()}`);
        rect.setAttribute('height', `${yScale.bandwidth()}`);
        rect.setAttribute('fill', fillColor);
        rect.setAttribute('class', 'heat-rect');
        cell.appendChild(rect);

        if (hasData && point!.rectText !== undefined) {
          const text = createSvgElement<SVGTextElement>('text');
          text.setAttribute('class', 'cell-text');
          text.setAttribute('x', `${xScale.bandwidth() / 2}`);
          text.setAttribute('y', `${yScale.bandwidth() / 2}`);
          text.setAttribute('dominant-baseline', 'middle');
          text.setAttribute('text-anchor', 'middle');
          text.setAttribute('fill', textColor);
          text.setAttribute('font-size', `${CELL_FONT_SIZE}`);
          const displayValue =
            this.culture && typeof point!.rectText === 'number'
              ? point!.rectText.toLocaleString(this.culture)
              : String(point!.rectText);
          text.textContent = displayValue;
          cell.appendChild(text);
        }

        // ── Events ────────────────────────────────────────────────────────────

        const handleInteraction = (e: MouseEvent | FocusEvent): void => {
          if (!point || isNaN(point.value)) {
            return;
          }
          const highlighted = this._getHighlightedLegends();
          if (highlighted.length === 0 || highlighted.includes(point.legend)) {
            this._showHeatTooltip(e, point, fillColor);
          }
        };

        cell.addEventListener('mouseover', handleInteraction);
        cell.addEventListener('mousemove', handleInteraction);
        cell.addEventListener('focus', handleInteraction);
        cell.addEventListener('mouseleave', () => this._clearHeatTooltip());
        cell.addEventListener('blur', () => this._clearHeatTooltip());
        cell.addEventListener('click', () => {
          this._focusRovingElement(this._renderedCells, cell);
          point?.onClick?.();
        });
        cell.addEventListener('keydown', (e: KeyboardEvent) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            point?.onClick?.();
          } else {
            this._rovingKeydown(this._renderedCells, e);
          }
        });

        this._renderedCells.push(cell);
        g.appendChild(cell);
      });
    });

    this._renderAnnotations({
      svg,
      margins,
      innerWidth,
      innerHeight,
      mapDataX: value => {
        const x = xScale(String(value));
        return x === undefined ? undefined : x + xScale.bandwidth() / 2;
      },
      mapDataY: value => {
        const y = yScale(String(value));
        return y === undefined ? undefined : y + yScale.bandwidth() / 2;
      },
    });

    // ── Legends ───────────────────────────────────────────────────────────────

    this.legends = (this.data || []).map(series => ({
      legend: series.legend,
      color: colorScale(series.value),
    })) as Legend[];

    this.chartContainer.appendChild(svg);
    this._applyActiveLegendState();
    this._applyLegendButtonState();
  }

  private _measureLongestYLabel(labels: string[]): number {
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    if (!ctx) {
      return MARGIN_LEFT_MIN + MARGIN_LEFT_LABEL_GAP;
    }
    ctx.font =
      '600 10px "Segoe UI", "Segoe UI Web (West European)", -apple-system, BlinkMacSystemFont, Roboto, "Helvetica Neue", sans-serif';
    let max = 0;
    for (const label of labels) {
      max = Math.max(max, ctx.measureText(label).width);
    }
    return Math.max(MARGIN_LEFT_MIN, Math.ceil(max) + MARGIN_LEFT_LABEL_GAP);
  }

  private _clearChart(): void {
    if (!this.chartContainer) {
      return;
    }
    this._renderedCells = [];
    while (this.chartContainer.firstChild) {
      this.chartContainer.removeChild(this.chartContainer.firstChild);
    }
  }
}
