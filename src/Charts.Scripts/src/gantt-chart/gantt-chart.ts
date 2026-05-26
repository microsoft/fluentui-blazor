import { attr } from '@microsoft/fast-element';
import { scaleTime } from 'd3-scale';
import { timeFormat } from 'd3-time-format';
import { CartesianChartBase } from '../utils/cartesian-chart-base.js';
import {
  createNumberFormat,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  lightenColor,
  parseDateOrNumber,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { GanttChartDataPoint } from './gantt-chart.options.js';
import type { AxisCategoryOrder, Legend, TooltipProps, TooltipRenderer } from '../utils/chart.options.js';

type GanttTooltipProps = TooltipProps & {
  xLabel: string;
  xValue: string;
  yLabel: string;
};

type GroupedSeries = {
  key: string;
  rawY: number | string;
  points: GanttChartDataPoint[];
};

type RenderedBar = {
  legend?: string;
  element: SVGRectElement;
};

type PlotLayout = {
  barHeight: number;
  margins: {
    top: number;
    right: number;
    bottom: number;
    left: number;
  };
  innerHeight: number;
};

type XAxisType = 'date' | 'number';

const X_AXIS_LABEL = 'X';
const Y_AXIS_LABEL = 'Y';
const DEFAULT_HEIGHT = 320;
const DEFAULT_BAR_HEIGHT = 24;
const DEFAULT_X_TICK_COUNT = 6;
const DEFAULT_Y_TICK_COUNT = 4;
const DEFAULT_Y_AXIS_PADDING = 0.5;
const MIN_DOMAIN_MARGIN = 8;

const createSvgElement = <T extends SVGElement>(tag: string): T => {
  return document.createElementNS(SVG_NAMESPACE_URI, tag) as T;
};

const toNumber = (value: number | string | undefined, fallback: number): number => {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
};

const toOptionalNumber = (value: number | string | undefined): number | undefined => {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : undefined;
};

const formatCompactNumber = (value: number, culture?: string) => {
  return createNumberFormat(culture || undefined, {
    maximumFractionDigits: Math.abs(value) >= 1000 ? 1 : 2,
    notation: Math.abs(value) >= 1000 ? 'compact' : 'standard',
  }).format(value);
};

const formatAxisNumber = (value: number, culture?: string) => {
  return createNumberFormat(culture || undefined, {
    maximumFractionDigits: 2,
  }).format(value);
};

const clamp = (value: number, min: number, max: number) => Math.min(Math.max(value, min), max);

const getMedian = (values: number[]) => {
  if (values.length === 0) {
    return 0;
  }
  const sorted = [...values].sort((left, right) => left - right);
  const middle = Math.floor(sorted.length / 2);
  return sorted.length % 2 === 0 ? (sorted[middle - 1] + sorted[middle]) / 2 : sorted[middle];
};

const truncateText = (text: string, maxLength: number) => {
  return text.length > maxLength ? `${text.slice(0, maxLength - 1)}…` : text;
};

const getNiceStep = (start: number, stop: number, count: number) => {
  const safeCount = Math.max(count, 1);
  const rawStep = Math.abs(stop - start) / safeCount;
  const power = Math.floor(Math.log10(rawStep || 1));
  const base = Math.pow(10, power);
  const error = rawStep / base;

  if (error >= Math.sqrt(50)) {
    return base * 10;
  }
  if (error >= Math.sqrt(10)) {
    return base * 5;
  }
  if (error >= Math.sqrt(2)) {
    return base * 2;
  }
  return base;
};

const getNiceDomainAndTicks = (min: number, max: number, count: number) => {
  if (min === max) {
    return { domain: [min, max] as [number, number], ticks: [min] };
  }

  const step = getNiceStep(min, max, count);
  const domainStart = Math.floor(min / step) * step;
  const domainEnd = Math.ceil(max / step) * step;
  const ticks: number[] = [];

  for (let value = domainStart; value <= domainEnd + step / 2; value += step) {
    ticks.push(Number(value.toFixed(12)));
  }

  return {
    domain: [domainStart, domainEnd] as [number, number],
    ticks,
  };
};

const getClosestPairDiffAndRange = (values: number[]) => {
  if (values.length < 2) {
    return undefined;
  }

  const sorted = [...values].sort((left, right) => left - right);
  let closestPairDiff = Number.POSITIVE_INFINITY;

  for (let index = 1; index < sorted.length; index++) {
    closestPairDiff = Math.min(closestPairDiff, sorted[index] - sorted[index - 1]);
  }

  if (!Number.isFinite(closestPairDiff) || closestPairDiff <= 0) {
    return undefined;
  }

  return [closestPairDiff, sorted[sorted.length - 1] - sorted[0]] as const;
};

/** @see HBCA _applyFormat for documentation. */
const _applyFormat = (value: number, format: string): string => {
  const match = format.match(/^([+]?)\.?(\d*)([fFeEgG%])$/);
  if (!match) {
    return String(value);
  }
  const [, , fractionStr, type] = match;
  const fraction = fractionStr ? Number(fractionStr) : 0;
  switch (type.toLowerCase()) {
    case 'f':
      return value.toFixed(fraction);
    case 'e':
      return value.toExponential(fraction);
    case 'g':
      return value.toPrecision(fraction || 6);
    case '%':
      return `${(value * 100).toFixed(fraction)}%`;
    default:
      return String(value);
  }
};

export class GanttChart extends CartesianChartBase {
  @attr({ converter: jsonConverter })
  public data!: GanttChartDataPoint[];

  @attr({ attribute: 'show-y-axis-labels', mode: 'boolean' })
  public showYAxisLabels: boolean = false;

  @attr({ attribute: 'show-y-axis-labels-tooltip', mode: 'boolean' })
  public showYAxisLabelsTooltip: boolean = false;

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  @attr({ attribute: 'bar-height' })
  public barHeight?: number | string;

  @attr({ attribute: 'x-axis-tick-count' })
  public xAxisTickCount?: number | string;

  @attr({ attribute: 'y-axis-tick-count' })
  public yAxisTickCount?: number | string;

  @attr({ attribute: 'y-axis-padding' })
  public yAxisPadding?: number | string;

  @attr({ attribute: 'y-axis-category-order' })
  public yAxisCategoryOrder: AxisCategoryOrder = 'default';

  /** Narrows the inherited base tooltipProps type to include axis label fields. */
  public declare tooltipProps: GanttTooltipProps;

  /** Narrows the inherited base tooltipRenderer type to the GanttChart data point. */
  public declare tooltipRenderer: TooltipRenderer<GanttChartDataPoint> | undefined;

  protected override _enableResizeObserver = true;

  private _renderedBars: RenderedBar[] = [];
  private _xAxisType: XAxisType = 'number';

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'data',
      'showYAxisLabels',
      'showYAxisLabelsTooltip',
      'enableGradient',
      'barHeight',
      'xAxisTickCount',
      'yAxisTickCount',
      'yAxisPadding',
      'yAxisCategoryOrder',
    ] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    // Set the extended initial value before super so ChartBase picks it up
    // when processing tooltipProps in its observableFields.
    self['tooltipProps'] = {
      isVisible: false,
      legend: '',
      xLabel: X_AXIS_LABEL,
      xValue: '',
      yLabel: Y_AXIS_LABEL,
      yValue: '',
      color: '',
      xPos: 0,
      yPos: 0,
    } satisfies GanttTooltipProps;

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    if (this.data) {
      this._renderChart();
    }
  }

  protected dataChanged() {
    this._requestRender();
  }

  protected enableGradientChanged() {
    this._requestRender();
  }

  protected barHeightChanged() {
    this._requestRender();
  }

  protected xAxisTickCountChanged() {
    this._requestRender();
  }

  protected yAxisTickCountChanged() {
    this._requestRender();
  }

  protected yAxisPaddingChanged() {
    this._requestRender();
  }

  protected yAxisCategoryOrderChanged() {
    this._requestRender();
  }

  protected override tooltipPropsChanged(_old: TooltipProps, newValue: TooltipProps): void {
    const typed = newValue as GanttTooltipProps;
    if (typed.isVisible && !this.hideTooltip) {
      const parts: string[] = [];
      if (typed.yValue) parts.push(typed.yValue);
      if (typed.legend && typed.xValue) parts.push(`${typed.legend}: ${typed.xValue}`);
      else if (typed.legend) parts.push(typed.legend);
      this.liveRegionText = parts.join(', ');
    } else {
      this.liveRegionText = '';
    }
    super.tooltipPropsChanged(_old, newValue);
  }

  protected override _buildDefaultTooltipHTML(_dataPoint: unknown): string {
    const p = this.tooltipProps;
    const esc = (s: string) =>
      s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    return [
      `<div class="tooltip-header">${esc(p.yValue)}</div>`,
      `<div class="tooltip-info" style="border-color: ${esc(p.color)};">`,
      `<div class="tooltip-legend-text">${esc(p.legend)}</div>`,
      `<div class="tooltip-primary-value" style="color: ${esc(p.color)};">${esc(
        (p as GanttTooltipProps).xValue,
      )}</div>`,
      `</div>`,
    ].join('');
  }

  protected showYAxisLabelsChanged() {
    this._requestRender();
  }

  protected showYAxisLabelsTooltipChanged() {
    this._requestRender();
  }

  protected _getHostAriaLabel(): string {
    if (!Array.isArray(this.data) || this.data.length === 0) {
      return this.chartTitle || 'Gantt chart has no data.';
    }
    return (this.chartTitle ? `${this.chartTitle}. ` : '') + `Gantt chart with ${this.data.length} bars.`;
  }

  public get tooltipInlineTransform() {
    return this._isRTL ? 'translateX(50%)' : 'translateX(-50%)';
  }

  protected _performRender(): void {
    this._renderChart();
  }

  private _renderChart() {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._clearChart();

    if (!Array.isArray(this.data) || this.data.length === 0) {
      this.legends = [];
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    this._validateData(this.data);
    const firstStart = this.data[0].x.start;
    this._xAxisType = firstStart instanceof Date || typeof firstStart === 'string' ? 'date' : 'number';
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
    this._applyHostDimensions();

    const width = Math.max(
      this.chartContainer.getBoundingClientRect().width || this.getBoundingClientRect().width || 640,
      320,
    );
    const groups = this._getGroupedSeries();
    const numericYAxis = typeof groups[0]?.rawY === 'number';
    const yValues = groups.map(group => group.rawY).filter((value): value is number => typeof value === 'number');
    const height = this._getChartHeight(groups.length, numericYAxis, yValues);
    const yLabelWidth = this._getYAxisLabelWidth(groups, numericYAxis);
    const xAxisTitleOffset = this.xAxisTitle ? 20 : 0;
    const yAxisTitleOffset = this.yAxisTitle ? 16 : 0;
    const margins = this._isRTL
      ? {
          top: 20,
          right: yLabelWidth + yAxisTitleOffset,
          bottom: 35 + xAxisTitleOffset,
          left: 20,
        }
      : {
          top: 20,
          right: 20,
          bottom: 35 + xAxisTitleOffset,
          left: yLabelWidth + yAxisTitleOffset,
        };
    const innerWidth = width - margins.left - margins.right;
    const plotLayout = this._getPlotLayout(groups.length, numericYAxis, height, margins, yValues);
    const xAxisScale = this._getXScaleInfo();
    const yPositionForGroup = this._createYPositioner(
      groups,
      numericYAxis,
      plotLayout.margins,
      height,
      plotLayout.innerHeight,
      yValues,
    );
    const svg = createSvgElement<SVGSVGElement>('svg');

    svg.setAttribute('class', 'chart-svg');
    svg.setAttribute('role', 'none');
    svg.setAttribute('width', `${width}`);
    svg.setAttribute('height', `${height}`);
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const axisLayer = createSvgElement<SVGGElement>('g');
    const barsLayer = createSvgElement<SVGGElement>('g');
    svg.appendChild(axisLayer);
    svg.appendChild(barsLayer);

    this._renderXAxis(axisLayer, width, height, margins, xAxisScale.domain, xAxisScale.ticks);
    this._renderYAxis(axisLayer, groups, numericYAxis, width, height, plotLayout.margins, yPositionForGroup, yValues);

    this._renderedBars = [];
    const legendColorMap = this._buildLegendColorMap();
    let globalPointIndex = 0;
    const scaleX = (value: number) => {
      const [min, max] = xAxisScale.domain;
      const safeSpan = max - min || 1;
      const rangeStart = this._isRTL ? width - margins.right : margins.left;
      const rangeEnd = this._isRTL ? margins.left : width - margins.right;
      return rangeStart + ((value - min) / safeSpan) * (rangeEnd - rangeStart);
    };

    groups.forEach((group, groupIndex) => {
      const yPosition = yPositionForGroup(group, groupIndex);
      const resolvedBarHeight = plotLayout.barHeight;

      group.points.forEach((point, _pointIndex) => {
        const color = this._getPointColor(point, globalPointIndex++, legendColorMap);
        const gradientId = this._appendGradient(defs, groupIndex, globalPointIndex - 1, point, color);

        const xStart = scaleX(+parseDateOrNumber(point.x.start));
        const xEnd = scaleX(+parseDateOrNumber(point.x.end));
        const rectX = Math.min(xStart, xEnd);
        const barWidth = Math.max(Math.abs(xEnd - xStart), 1);

        const rect = createSvgElement<SVGRectElement>('rect');
        rect.setAttribute('class', 'bar');
        rect.setAttribute('x', `${rectX}`);
        rect.setAttribute('y', `${yPosition - resolvedBarHeight / 2}`);
        rect.setAttribute('width', `${barWidth}`);
        rect.setAttribute('height', `${resolvedBarHeight}`);
        rect.setAttribute('fill', gradientId ? `url(#${gradientId})` : color);
        if (this.strokeWidth !== undefined) {
          rect.setAttribute('stroke-width', `${this.strokeWidth}`);
          rect.setAttribute('stroke', color);
        }
        rect.setAttribute('role', 'img');
        rect.setAttribute('tabindex', this._renderedBars.length === 0 ? '0' : '-1');
        rect.setAttribute('aria-label', this._getAriaLabel(point));
        rect.setAttribute('rx', `${this.roundCorners ? 3 : 0}`);

        rect.addEventListener('mouseover', event => this._showTooltip(point, color, event, rect));
        rect.addEventListener('mouseout', () => this._clearTooltip());
        rect.addEventListener('focus', event => this._showTooltip(point, color, event, rect));
        rect.addEventListener('blur', () => this._clearTooltip());
        rect.addEventListener('click', () => point.onClick?.());
        rect.addEventListener('keydown', (e: KeyboardEvent) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            point.onClick?.();
          } else {
            this._rovingKeydown(
              this._renderedBars.map(b => b.element),
              e,
            );
          }
        });

        this._renderedBars.push({ legend: point.legend, element: rect });
        barsLayer.appendChild(rect);
      });
    });

    this.legends = Array.from(legendColorMap.entries()).map(([legend, color]) => ({ legend, color }));
    this.chartContainer.appendChild(svg);
    this._updateLegendInteractionState();

    void innerWidth; // suppress unused variable warning; kept for reference with other charts
  }

  private _clearChart() {
    if (!this.chartContainer) {
      return;
    }
    this._renderedBars = [];
    while (this.chartContainer.firstChild) {
      this.chartContainer.removeChild(this.chartContainer.firstChild);
    }
  }

  private _validateData(data: GanttChartDataPoint[]) {
    if (!Array.isArray(data)) {
      throw new TypeError('Invalid data: Expected an array.');
    }

    data.forEach((point, index) => {
      if (point === null || typeof point !== 'object' || Array.isArray(point)) {
        throw new TypeError(`Invalid data[${index}]: Expected an object.`);
      }
      if (point.x === null || typeof point.x !== 'object') {
        throw new TypeError(`Invalid data[${index}].x: Expected an object with start and end.`);
      }
      const start = point.x.start;
      const end = point.x.end;
      const isValidValue = (v: Date | number | string) => {
        if (typeof v === 'string') return !isNaN(new Date(v).getTime());
        return (v instanceof Date && !isNaN(v.getTime())) || typeof v === 'number';
      };
      if (!isValidValue(start) || !isValidValue(end)) {
        throw new TypeError(`Invalid data[${index}].x: start and end must be Date, number, or ISO 8601 string.`);
      }
      if (typeof point.y !== 'string' && typeof point.y !== 'number') {
        throw new TypeError(`Invalid data[${index}].y: Expected a string or number.`);
      }
    });
  }

  private _getGroupedSeries(): GroupedSeries[] {
    const groups = new Map<string, GroupedSeries>();
    this.data.forEach(point => {
      const numericCandidate = typeof point.y === 'string' ? Number(point.y) : NaN;
      const rawY: number | string =
        typeof point.y === 'number' ? point.y : isFinite(numericCandidate) ? numericCandidate : point.y;
      const key = String(rawY);
      const existing = groups.get(key);
      if (existing) {
        existing.points.push(point);
      } else {
        groups.set(key, { key, rawY, points: [point] });
      }
    });

    const groupList = Array.from(groups.values());
    const numericYAxis = typeof groupList[0]?.rawY === 'number';
    if (numericYAxis) {
      return groupList.sort((left, right) => Number(right.rawY) - Number(left.rawY));
    }

    return this._sortCategoricalGroups(groupList);
  }

  private _sortCategoricalGroups(groups: GroupedSeries[]): GroupedSeries[] {
    const order = this.yAxisCategoryOrder || 'default';
    if (order === 'default' || order === 'data') {
      // Collect first-appearance order (data order) — index 0 maps to the top of the chart.
      const seen = new Set<string>();
      const orderedKeys: string[] = [];
      for (const point of this.data) {
        const key = String(point.y);
        if (!seen.has(key)) {
          seen.add(key);
          orderedKeys.push(key);
        }
      }
      return orderedKeys.map(key => groups.find(group => group.key === key)!).filter(Boolean);
    }

    // Aggregate by total duration of bars in the group
    const aggregate = (group: GroupedSeries) => {
      const durations = group.points.map(point => +parseDateOrNumber(point.x.end) - +parseDateOrNumber(point.x.start));
      switch (order) {
        case 'category ascending':
        case 'category descending':
          return 0;
        case 'total ascending':
        case 'total descending':
        case 'sum ascending':
        case 'sum descending':
          return durations.reduce((sum, value) => sum + value, 0);
        case 'min ascending':
        case 'min descending':
          return Math.min(...durations);
        case 'max ascending':
        case 'max descending':
          return Math.max(...durations);
        case 'mean ascending':
        case 'mean descending':
          return durations.reduce((sum, value) => sum + value, 0) / durations.length;
        case 'median ascending':
        case 'median descending':
          return getMedian(durations);
        default:
          return 0;
      }
    };

    const sorted = [...groups];
    if (order.startsWith('category')) {
      sorted.sort((left, right) => left.key.localeCompare(right.key));
      if (order.endsWith('descending')) {
        sorted.reverse();
      }
      return sorted;
    }

    sorted.sort((left, right) => aggregate(left) - aggregate(right));
    if (order.endsWith('descending')) {
      sorted.reverse();
    }
    return sorted;
  }

  private _getChartHeight(groupCount: number, numericYAxis: boolean, yValues: number[]) {
    if (this.height !== undefined) {
      return Math.max(toNumber(this.height, DEFAULT_HEIGHT), 160);
    }

    if (numericYAxis && yValues.length > 1) {
      return DEFAULT_HEIGHT;
    }

    return Math.max(DEFAULT_HEIGHT, groupCount * 56 + 56);
  }

  private _getPlotLayout(
    groupCount: number,
    numericYAxis: boolean,
    height: number,
    baseMargins: { top: number; right: number; bottom: number; left: number },
    yValues: number[],
  ): PlotLayout {
    const padding = clamp(toNumber(this.yAxisPadding, DEFAULT_Y_AXIS_PADDING), 0, 0.99);
    const totalHeight = height - (baseMargins.top + MIN_DOMAIN_MARGIN) - (baseMargins.bottom + MIN_DOMAIN_MARGIN);
    let barHeight = this.barHeight !== undefined ? Math.max(toNumber(this.barHeight, DEFAULT_BAR_HEIGHT), 1) : 0;
    let domainMargin = MIN_DOMAIN_MARGIN;

    if (numericYAxis) {
      if (barHeight === 0) {
        barHeight = this._calculateAppropriateNumericBarHeight(yValues, totalHeight, padding);
      }

      barHeight = Math.max(barHeight, 1);
      domainMargin += barHeight / 2;
    } else {
      // Categorical: no extra domain margin — D3 scaleBand outer padding (via startOffset) handles spacing.
      domainMargin = 0;
      if (barHeight === 0) {
        barHeight = DEFAULT_BAR_HEIGHT;
      }
      barHeight = Math.max(barHeight, 1);
    }

    const margins = {
      ...baseMargins,
      top: baseMargins.top + domainMargin,
      bottom: baseMargins.bottom + domainMargin,
    };

    return {
      barHeight,
      margins,
      innerHeight: height - margins.top - margins.bottom,
    };
  }

  private _getYAxisLabelWidth(groups: GroupedSeries[], numericYAxis: boolean) {
    if (numericYAxis) {
      return 40;
    }

    const longest = groups.reduce((maxLength, group) => Math.max(maxLength, String(group.rawY).length), 0);
    const rawWidth = longest * 7 + 28;
    return clamp(rawWidth, 40, this.showYAxisLabels ? 240 : 160);
  }

  private _getXScaleInfo() {
    const starts = this.data.map(p => +parseDateOrNumber(p.x.start));
    const ends = this.data.map(p => +parseDateOrNumber(p.x.end));
    let rawMin = Math.min(...starts, ...ends);
    let rawMax = Math.max(...starts, ...ends);

    const userMin = toOptionalNumber(this.xMinValue);
    const userMax = toOptionalNumber(this.xMaxValue);
    if (userMin !== undefined) rawMin = Math.min(rawMin, userMin);
    if (userMax !== undefined) rawMax = Math.max(rawMax, userMax);

    if (rawMin === rawMax) {
      rawMin -= 1;
      rawMax += 1;
    }

    // Use D3's time scale to match React's behavior: calendar-aligned ticks + niced domain.
    const count = toNumber(this.xAxisTickCount, DEFAULT_X_TICK_COUNT);
    const scale = scaleTime()
      .domain([new Date(rawMin), new Date(rawMax)])
      .nice(count);
    const [niceMin, niceMax] = scale.domain() as [Date, Date];
    const ticks = this.tickValues
      ? (this.tickValues as Array<Date | number | string>).map(v => +v)
      : scale.ticks(count).map(d => +d);
    return { domain: [+niceMin, +niceMax] as [number, number], ticks };
  }

  private _getNumericYDomain(yValues: number[]) {
    const yMin = Math.min(...yValues);
    const yMax = Math.max(...yValues);
    const domainMin = Math.min(yMin, toOptionalNumber(this.yMinValue) ?? (this.supportNegativeData ? yMin : 0));
    const domainMax = Math.max(yMax, toOptionalNumber(this.yMaxValue) ?? 0);
    if (this.roundedTicks) {
      const niced = getNiceDomainAndTicks(domainMin, domainMax, toNumber(this.yAxisTickCount, DEFAULT_Y_TICK_COUNT));
      return niced.domain;
    }
    return [domainMin, domainMax] as [number, number];
  }

  private _createYPositioner(
    groups: GroupedSeries[],
    numericYAxis: boolean,
    margins: { top: number; bottom: number },
    height: number,
    innerHeight: number,
    yValues: number[],
  ) {
    if (!numericYAxis) {
      const padding = clamp(toNumber(this.yAxisPadding, DEFAULT_Y_AXIS_PADDING), 0, 0.95);
      const step = innerHeight / Math.max(groups.length + padding, 1);
      const barBand = step * (1 - padding);
      // Use 2 * padding * step to match D3 scaleBand's default align=0.5 outer-padding offset.
      const startOffset = 2 * padding * step;
      return (_group: GroupedSeries, index: number) => margins.top + startOffset + index * step + barBand / 2;
    }

    const [min, max] = this._getNumericYDomain(yValues);
    const safeSpan = max - min || 1;

    return (group: GroupedSeries) => {
      const ratio = (Number(group.rawY) - min) / safeSpan;
      return height - margins.bottom - ratio * innerHeight;
    };
  }

  private _calculateAppropriateNumericBarHeight(yValues: number[], totalHeight: number, innerPadding: number) {
    const result = getClosestPairDiffAndRange(yValues);
    if (!result || result[1] === 0) {
      return 16;
    }

    const [closestPairDiff, rawRange] = result;
    const yMax = Math.max(...yValues);
    const range = Math.max(rawRange, yMax);
    return Math.max(
      Math.floor((totalHeight * closestPairDiff * (1 - innerPadding)) / (range + closestPairDiff * (1 - innerPadding))),
      1,
    );
  }

  private _buildLegendColorMap(): Map<string, string> {
    const map = new Map<string, string>();
    let index = 0;
    this.data.forEach(point => {
      const legend = point.legend;
      if (legend && !map.has(legend)) {
        map.set(legend, point.color ? getColorFromToken(point.color) : getNextColor(index, 0));
        index++;
      }
    });
    return map;
  }

  private _getPointColor(point: GanttChartDataPoint, index: number, legendColorMap?: Map<string, string>) {
    if (point.color) {
      return getColorFromToken(point.color);
    }

    if (point.legend && legendColorMap?.has(point.legend)) {
      return legendColorMap.get(point.legend)!;
    }

    return getNextColor(index, 0);
  }

  protected _applyHostDimensions() {
    super._applyHostDimensions(this.width, this.height);
  }

  private _appendGradient(
    defs: SVGDefsElement,
    groupIndex: number,
    pointIndex: number,
    point: GanttChartDataPoint,
    color: string,
  ) {
    if (!this.enableGradient && !point.gradient) {
      return undefined;
    }

    const gradientId = `gantt-gradient-${groupIndex}-${pointIndex}`;
    const gradient = createSvgElement<SVGLinearGradientElement>('linearGradient');
    gradient.setAttribute('id', gradientId);
    gradient.setAttribute('x1', this._isRTL ? '100%' : '0%');
    gradient.setAttribute('x2', this._isRTL ? '0%' : '100%');
    gradient.setAttribute('y1', '0%');
    gradient.setAttribute('y2', '0%');

    const [from, to] = point.gradient ?? [lightenColor(color, 0.35), color];
    const start = createSvgElement<SVGStopElement>('stop');
    start.setAttribute('offset', '0%');
    start.setAttribute('stop-color', from);
    gradient.appendChild(start);

    const end = createSvgElement<SVGStopElement>('stop');
    end.setAttribute('offset', '100%');
    end.setAttribute('stop-color', to);
    gradient.appendChild(end);

    defs.appendChild(gradient);
    return gradientId;
  }

  private _formatDateTick(ms: number, rangeMs: number): string {
    const date = new Date(ms);
    if (this.tickFormat) {
      return timeFormat(this.tickFormat)(date);
    }
    const options: Intl.DateTimeFormatOptions =
      this.dateLocalizeOptions ??
      (rangeMs < 7 * 86_400_000
        ? { month: 'short', day: 'numeric', hour: '2-digit' }
        : rangeMs < 365 * 86_400_000
        ? { month: 'short', day: 'numeric' }
        : { year: 'numeric', month: 'short' });
    return new Intl.DateTimeFormat(this.culture || undefined, options).format(date);
  }

  private _formatXRange(point: GanttChartDataPoint): string {
    if (point.xAxisCalloutData) {
      return point.xAxisCalloutData;
    }
    if (this._xAxisType === 'date') {
      const fmt = new Intl.DateTimeFormat(this.culture || undefined, {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        timeZone: 'UTC',
      });
      return `${fmt.format(new Date(+parseDateOrNumber(point.x.start)))} - ${fmt.format(
        new Date(+parseDateOrNumber(point.x.end)),
      )}`;
    }
    return `${formatAxisNumber(+point.x.start, this.culture)} - ${formatAxisNumber(+point.x.end, this.culture)}`;
  }

  private _renderXAxis(
    axisLayer: SVGGElement,
    width: number,
    height: number,
    margins: { left: number; right: number; bottom: number },
    domain: [number, number],
    ticks: number[],
  ) {
    const axisY = height - margins.bottom;
    const min = domain[0];
    const max = domain[1];
    const rangeStart = this._isRTL ? width - margins.right : margins.left;
    const rangeEnd = this._isRTL ? margins.left : width - margins.right;
    const span = max - min || 1;
    const toX = (value: number) => rangeStart + ((value - min) / span) * (rangeEnd - rangeStart);
    const rangeMs = max - min;
    const tickGap = toNumber(this.tickPadding, 6);

    ticks.forEach(tick => {
      const x = toX(tick);
      const tickLine = createSvgElement<SVGLineElement>('line');
      tickLine.setAttribute('class', 'axis-tick-line');
      tickLine.setAttribute('x1', `${x}`);
      tickLine.setAttribute('x2', `${x}`);
      tickLine.setAttribute('y1', `${axisY}`);
      tickLine.setAttribute('y2', `${20}`);
      axisLayer.appendChild(tickLine);

      const labelY = axisY + tickGap + 12;
      const rawLabel =
        this.xAxisTickFormat && this._xAxisType !== 'date'
          ? _applyFormat(tick, this.xAxisTickFormat)
          : this._xAxisType === 'date'
          ? this._formatDateTick(tick, rangeMs)
          : formatAxisNumber(tick, this.culture);

      const MAX_LABEL_CHARS = 10;
      const displayLabel =
        this.showXAxisLabelsTooltip && rawLabel.length > MAX_LABEL_CHARS
          ? truncateText(rawLabel, MAX_LABEL_CHARS)
          : rawLabel;
      const isLabelTruncated = displayLabel !== rawLabel;

      const text = createSvgElement<SVGTextElement>('text');
      text.setAttribute('class', 'axis-text');
      text.setAttribute('x', `${x}`);
      text.setAttribute('y', `${labelY}`);

      if (this.rotateXAxisLabels) {
        text.setAttribute('text-anchor', this._isRTL ? 'start' : 'end');
        text.setAttribute('transform', `rotate(-45, ${x}, ${labelY})`);
        text.textContent = displayLabel;
      } else if (this.wrapXAxisLabels) {
        text.setAttribute('text-anchor', 'middle');
        const words = displayLabel.split(' ');
        if (words.length > 1) {
          words.forEach((word, i) => {
            const tspan = createSvgElement<SVGTSpanElement>('tspan');
            tspan.setAttribute('x', `${x}`);
            tspan.setAttribute('dy', i === 0 ? '0' : '1.2em');
            tspan.textContent = word;
            text.appendChild(tspan);
          });
        } else {
          text.textContent = displayLabel;
        }
      } else {
        text.setAttribute('text-anchor', 'middle');
        text.textContent = displayLabel;
      }

      // Prepend <title> after text content is set so it isn't wiped by textContent assignment.
      if (isLabelTruncated) {
        const title = createSvgElement<SVGTitleElement>('title');
        title.textContent = rawLabel;
        text.insertBefore(title, text.firstChild);
      }

      axisLayer.appendChild(text);
    });

    if (this.xAxisTitle) {
      const titleX = (rangeStart + rangeEnd) / 2;
      const titleY = height - 4;
      const titleText = createSvgElement<SVGTextElement>('text');
      titleText.setAttribute('class', 'axis-title');
      titleText.setAttribute('x', `${titleX}`);
      titleText.setAttribute('y', `${titleY}`);
      titleText.setAttribute('text-anchor', 'middle');
      titleText.textContent = this.xAxisTitle;
      axisLayer.appendChild(titleText);
    }
  }

  private _renderYAxis(
    axisLayer: SVGGElement,
    groups: GroupedSeries[],
    numericYAxis: boolean,
    width: number,
    height: number,
    margins: { top: number; left: number; bottom: number; right: number },
    yPositionForGroup: (group: GroupedSeries, index: number) => number,
    yValues: number[],
  ) {
    const axisX = this._isRTL ? width - margins.right : margins.left;
    if (numericYAxis) {
      const [min, max] = this._getNumericYDomain(yValues);
      const yAxisScale = getNiceDomainAndTicks(min, max, toNumber(this.yAxisTickCount, DEFAULT_Y_TICK_COUNT));
      const safeSpan = yAxisScale.domain[1] - yAxisScale.domain[0] || 1;
      yAxisScale.ticks.forEach(tick => {
        const ratio = (tick - yAxisScale.domain[0]) / safeSpan;
        const y = height - margins.bottom - ratio * (height - margins.top - margins.bottom);
        const label = this.yAxisTickFormat
          ? _applyFormat(tick, this.yAxisTickFormat)
          : formatCompactNumber(tick, this.culture).toLowerCase();
        this._appendYAxisTick(axisLayer, axisX, y, label);
      });
    } else {
      groups.forEach((group, index) => {
        const y = yPositionForGroup(group, index);
        const fullLabel = String(group.rawY);
        const label = this.showYAxisLabels ? fullLabel : truncateText(fullLabel, 18);
        this._appendYAxisTick(axisLayer, axisX, y, label, this.showYAxisLabelsTooltip ? fullLabel : undefined);
      });
    }

    if (this.yAxisTitle) {
      const midY = (margins.top + (height - margins.bottom)) / 2;
      const titleX = this._isRTL ? width - margins.right + 12 : 12;
      const titleText = createSvgElement<SVGTextElement>('text');
      titleText.setAttribute('class', 'axis-title');
      titleText.setAttribute('x', `${titleX}`);
      titleText.setAttribute('y', `${midY}`);
      titleText.setAttribute('text-anchor', 'middle');
      titleText.setAttribute('transform', `rotate(-90, ${titleX}, ${midY})`);
      titleText.textContent = this.yAxisTitle;
      axisLayer.appendChild(titleText);
    }
  }

  private _appendYAxisTick(axisLayer: SVGGElement, axisX: number, y: number, label: string, tooltipText?: string) {
    const tickGap = toNumber(this.tickPadding, 6);
    const tickLine = createSvgElement<SVGLineElement>('line');
    tickLine.setAttribute('class', 'axis-tick-line');
    tickLine.setAttribute('x1', `${axisX}`);
    tickLine.setAttribute('x2', `${axisX + tickGap}`);
    tickLine.setAttribute('y1', `${y}`);
    tickLine.setAttribute('y2', `${y}`);
    axisLayer.appendChild(tickLine);

    const text = createSvgElement<SVGTextElement>('text');
    text.setAttribute('class', 'y-axis-text');
    text.setAttribute('x', `${axisX + (this._isRTL ? tickGap + 6 : -(tickGap + 6))}`);
    text.setAttribute('y', `${y}`);
    text.setAttribute('dominant-baseline', 'central');
    text.setAttribute('text-anchor', this._isRTL ? 'start' : 'end');
    text.textContent = label;

    if (tooltipText) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = tooltipText;
      text.appendChild(title);
    }
    axisLayer.appendChild(text);
  }

  private _showTooltip(
    point: GanttChartDataPoint,
    color: string,
    event: MouseEvent | FocusEvent,
    target: SVGRectElement,
  ) {
    if (!this._shouldShowTooltip(point.legend || '')) {
      return;
    }

    const hostRect = this.getBoundingClientRect();
    const targetRect = target.getBoundingClientRect();
    const xReference = 'clientX' in event ? event.clientX : targetRect.left + targetRect.width / 2;
    const xPos = this._isRTL ? hostRect.right - xReference : xReference - hostRect.left;
    const yPos = ('clientY' in event ? event.clientY : targetRect.top) - hostRect.top - 44;
    this._currentTooltipDataPoint = point;
    this.tooltipProps = {
      isVisible: true,
      legend: point.legend || '',
      xLabel: X_AXIS_LABEL,
      xValue: this._formatXRange(point),
      yLabel: Y_AXIS_LABEL,
      yValue: point.yAxisCalloutData || String(point.y),
      color,
      xPos: Math.max(0, xPos),
      yPos: Math.max(0, yPos),
    };

    // After the tooltip renders, clamp its horizontal position so it stays within the host.
    requestAnimationFrame(() => {
      if (!this.tooltipProps?.isVisible) return;
      const tooltipEl = this.shadowRoot?.querySelector<HTMLElement>('.tooltip');
      if (!tooltipEl) return;
      const hostWidth = this.offsetWidth;
      const tooltipWidth = tooltipEl.offsetWidth;
      const clampedX = Math.max(tooltipWidth / 2, Math.min(hostWidth - tooltipWidth / 2, xPos));
      if (clampedX !== xPos) {
        this.tooltipProps = { ...this.tooltipProps, xPos: clampedX };
      }
    });
  }

  protected override _clearTooltip(): void {
    this.tooltipProps = {
      isVisible: false,
      legend: '',
      xLabel: X_AXIS_LABEL,
      xValue: '',
      yLabel: Y_AXIS_LABEL,
      yValue: '',
      color: '',
      xPos: 0,
      yPos: 0,
    };
  }

  private _getAriaLabel(point: GanttChartDataPoint) {
    const xValue = this._formatXRange(point);
    const legend = point.legend;
    const yValue = point.yAxisCalloutData || point.y;
    return point.callOutAccessibilityData?.ariaLabel || `${yValue}. ${legend ? `${legend}, ` : ''}${xValue}.`;
  }

  protected _applyActiveLegendState() {
    const highlighted = this._getHighlightedLegends();
    if (!Array.isArray(this._renderedBars)) {
      return;
    }

    this._renderedBars.forEach(({ legend, element }) => {
      const shouldHighlight = highlighted.length === 0 || (legend ? highlighted.includes(legend) : true);
      element.classList.toggle('inactive', !shouldHighlight);
      element.setAttribute('opacity', shouldHighlight ? '1' : '0.1');
      if (!shouldHighlight) {
        element.tabIndex = -1;
      }
    });

    const activeEls = this._renderedBars
      .filter(({ legend }) => highlighted.length === 0 || (legend ? highlighted.includes(legend) : true))
      .map(b => b.element);
    if (activeEls.length > 0 && !activeEls.some(el => el.tabIndex === 0)) {
      activeEls[0].tabIndex = 0;
    }
    // Pass ALL bars (including now-inactive ones) so that a bar which just lost its
    // tabIndex=0 is found as the focused candidate and focus is moved correctly.
    this._relocateFocusIfNeeded(this._renderedBars.map(b => b.element));
  }
}
