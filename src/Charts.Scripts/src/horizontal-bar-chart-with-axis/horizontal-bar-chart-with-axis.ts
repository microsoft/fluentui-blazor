import { attr, observable } from '@microsoft/fast-element';
import { ChartBase } from '../utils/chart-base.js';
import {
  getColorFromToken,
  getNextColor,
  getRTL,
  jsonConverter,
  lightenColor,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type {
  AxisCategoryOrder,
  HorizontalBarChartWithAxisDataPoint,
} from './horizontal-bar-chart-with-axis.options.js';
import type { Legend } from '../utils/chart.options.js';

type TooltipProps = {
  isVisible: boolean;
  legend: string;
  xLabel: string;
  xValue: string;
  yLabel: string;
  yValue: string;
  color: string;
  xPos: number;
  yPos: number;
};

type GroupedSeries = {
  key: string;
  rawY: number | string;
  points: HorizontalBarChartWithAxisDataPoint[];
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

const X_AXIS_LABEL = 'X';
const Y_AXIS_LABEL = 'Y';
const DEFAULT_HEIGHT = 320;
const DEFAULT_BAR_HEIGHT = 32;
const DEFAULT_X_TICK_COUNT = 6;
const DEFAULT_Y_TICK_COUNT = 4;
const DEFAULT_Y_AXIS_PADDING = 0.5;

const MIN_DOMAIN_MARGIN = 8;
const STACKED_SEGMENT_GAP = 2;
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
  return new Intl.NumberFormat(culture || undefined, {
    maximumFractionDigits: Math.abs(value) >= 1000 ? 1 : 2,
    notation: Math.abs(value) >= 1000 ? 'compact' : 'standard',
  }).format(value);
};

const formatAxisNumber = (value: number, culture?: string) => {
  return new Intl.NumberFormat(culture || undefined, {
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

export class HorizontalBarChartWithAxis extends ChartBase {
  @attr({ converter: jsonConverter })
  public data!: HorizontalBarChartWithAxisDataPoint[];

  @attr
  public width?: number | string;

  @attr({ attribute: 'show-y-axis-labels', mode: 'boolean' })
  public showYAxisLabels: boolean = false;

  @attr({ attribute: 'show-y-axis-labels-tooltip', mode: 'boolean' })
  public showYAxisLabelsTooltip: boolean = false;

  @attr({ attribute: 'use-single-color', mode: 'boolean' })
  public useSingleColor: boolean = false;

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  @attr({ attribute: 'bar-height' })
  public barHeight?: number | string;

  @attr({ attribute: 'height' })
  public height?: number | string;

  @attr({ attribute: 'x-axis-tick-count' })
  public xAxisTickCount?: number | string;

  @attr({ attribute: 'y-axis-tick-count' })
  public yAxisTickCount?: number | string;

  @attr({ attribute: 'y-axis-padding' })
  public yAxisPadding?: number | string;

  @attr({ attribute: 'x-min-value' })
  public xMinValue?: number | string;

  @attr({ attribute: 'x-max-value' })
  public xMaxValue?: number | string;

  @attr({ attribute: 'y-min-value' })
  public yMinValue?: number | string;

  @attr({ attribute: 'y-max-value' })
  public yMaxValue?: number | string;

  @attr({ attribute: 'y-axis-category-order' })
  public yAxisCategoryOrder: AxisCategoryOrder = 'default';

  @observable
  public tooltipProps: TooltipProps = {
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

  private _renderedBars: RenderedBar[] = [];
  private _resizeObserver?: ResizeObserver;

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'data',
      'width',
      'showYAxisLabels',
      'showYAxisLabelsTooltip',
      'useSingleColor',
      'enableGradient',
      'barHeight',
      'height',
      'xAxisTickCount',
      'yAxisTickCount',
      'yAxisPadding',
      'xMinValue',
      'xMaxValue',
      'yMinValue',
      'yMaxValue',
      'yAxisCategoryOrder',
    ] as const;
    const observableFields = ['tooltipProps'] as const;
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

    this._resizeObserver = new ResizeObserver(() => this._requestRender());
    this._resizeObserver.observe(this);
    if (this.data) {
      this._renderChart();
    }
  }

  public disconnectedCallback() {
    this._resizeObserver?.disconnect();
    super.disconnectedCallback();
  }

  protected dataChanged() {
    this._requestRender();
  }

  protected widthChanged() {
    this._requestRender();
  }

  protected heightChanged() {
    this._requestRender();
  }

  protected useSingleColorChanged() {
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

  protected xMinValueChanged() {
    this._requestRender();
  }

  protected xMaxValueChanged() {
    this._requestRender();
  }

  protected yMinValueChanged() {
    this._requestRender();
  }

  protected yMaxValueChanged() {
    this._requestRender();
  }

  protected yAxisCategoryOrderChanged() {
    this._requestRender();
  }

  protected showYAxisLabelsChanged() {
    this._requestRender();
  }

  protected showYAxisLabelsTooltipChanged() {
    this._requestRender();
  }

  protected _getHostAriaLabel(): string {
    if (!Array.isArray(this.data) || this.data.length === 0) {
      return this.chartTitle || 'Horizontal bar chart with axis has no data.';
    }
    return (
      (this.chartTitle ? `${this.chartTitle}. ` : '') + `Horizontal bar chart with axis with ${this.data.length} bars.`
    );
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
    this._isRTL = getRTL(this);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
    this._applyHostDimensions();

    const width = Math.max(this.getBoundingClientRect().width || 640, 320);
    const groups = this._getGroupedSeries();
    const numericYAxis = typeof groups[0]?.rawY === 'number';
    const yValues = groups.map(group => group.rawY).filter((value): value is number => typeof value === 'number');
    const height = this._getChartHeight(groups.length, numericYAxis, yValues);
    const yLabelWidth = this._getYAxisLabelWidth(groups, numericYAxis);
    const margins = this._isRTL
      ? {
          top: 20,
          right: yLabelWidth,
          bottom: 35,
          left: 20,
        }
      : {
          top: 20,
          right: 20,
          bottom: 35,
          left: yLabelWidth,
        };
    const innerWidth = width - margins.left - margins.right;
    const plotLayout = this._getPlotLayout(groups.length, numericYAxis, height, margins, yValues);
    const xAxisScale = this._getXScaleInfo(groups);
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
    this._renderOriginLine(axisLayer, plotLayout.margins, height, xAxisScale.domain, innerWidth);

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
      let positiveTotal = 0;
      let negativeTotal = 0;
      const positivePointCount = group.points.filter(point => point.x >= 0).length;
      const negativePointCount = group.points.length - positivePointCount;
      let positivePointIndex = 0;
      let negativePointIndex = 0;

      group.points.forEach((point, _pointIndex) => {
        const color = this._getPointColor(point, globalPointIndex++, legendColorMap);
        const gradientId = this._appendGradient(defs, groupIndex, globalPointIndex - 1, point, color);

        const startValue = point.x >= 0 ? positiveTotal : negativeTotal;
        const endValue = startValue + point.x;
        if (point.x >= 0) {
          positiveTotal = endValue;
          positivePointIndex += 1;
        } else {
          negativeTotal = endValue;
          negativePointIndex += 1;
        }

        const xStart = scaleX(startValue);
        const xEnd = scaleX(endValue);
        const rawWidth = Math.max(Math.abs(xEnd - xStart), 1);
        const shouldApplyGap =
          rawWidth > STACKED_SEGMENT_GAP &&
          ((point.x >= 0 && positivePointIndex !== positivePointCount) ||
            (point.x < 0 && negativePointIndex !== negativePointCount));
        const barWidth = rawWidth - (shouldApplyGap ? STACKED_SEGMENT_GAP : 0);
        const rectX = Math.min(xStart, xEnd);
        const rect = createSvgElement<SVGRectElement>('rect');
        rect.setAttribute('class', 'bar');
        rect.setAttribute('x', `${rectX}`);
        rect.setAttribute('y', `${yPosition - resolvedBarHeight / 2}`);
        rect.setAttribute('width', `${barWidth}`);
        rect.setAttribute('height', `${resolvedBarHeight}`);
        rect.setAttribute('fill', gradientId ? `url(#${gradientId})` : color);
        rect.setAttribute('role', 'img');
        rect.setAttribute('tabindex', '0');
        rect.setAttribute('aria-label', this._getAriaLabel(point));
        rect.setAttribute('rx', `${this.roundCorners ? 3 : 0}`);

        rect.addEventListener('mouseover', event => this._showTooltip(point, color, event, rect));
        rect.addEventListener('mouseout', () => this._clearTooltipState());
        rect.addEventListener('focus', event => this._showTooltip(point, color, event, rect));
        rect.addEventListener('blur', () => this._clearTooltipState());
        rect.addEventListener('click', () => point.onClick?.());

        this._renderedBars.push({ legend: point.legend, element: rect });
        barsLayer.appendChild(rect);
      });

      const totalValue = positiveTotal + negativeTotal;
      if (!this.hideLabels && resolvedBarHeight >= 16) {
        const label = createSvgElement<SVGTextElement>('text');
        const anchorValue = totalValue >= 0 ? positiveTotal : negativeTotal;
        const x = scaleX(anchorValue);
        label.setAttribute('class', 'bar-label');
        label.setAttribute('x', `${x}`);
        label.setAttribute('y', `${yPosition}`);
        label.setAttribute('dominant-baseline', 'central');
        label.setAttribute(
          'text-anchor',
          this._isRTL ? (totalValue >= 0 ? 'end' : 'start') : totalValue >= 0 ? 'start' : 'end',
        );
        label.setAttribute(
          'transform',
          `translate(${totalValue >= 0 ? (this._isRTL ? -4 : 4) : this._isRTL ? 4 : -4}, 0)`,
        );
        label.textContent = formatCompactNumber(totalValue, this.culture);
        barsLayer.appendChild(label);
      }
    });

    this.legends = Array.from(legendColorMap.entries()).map(([legend, color]) => ({ legend, color }));
    this.chartContainer.appendChild(svg);
    this._updateLegendInteractionState();
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

  private _validateData(data: HorizontalBarChartWithAxisDataPoint[]) {
    if (!Array.isArray(data)) {
      throw new TypeError('Invalid data: Expected an array.');
    }

    data.forEach((point, index) => {
      if (point === null || typeof point !== 'object' || Array.isArray(point)) {
        throw new TypeError(`Invalid data[${index}]: Expected an object.`);
      }
      if (typeof point.x !== 'number') {
        throw new TypeError(`Invalid data[${index}].x: Expected a number.`);
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
      const reversed = [...this.data].reverse();
      const orderedKeys = new Set(reversed.map(point => String(point.y)));
      return Array.from(orderedKeys)
        .map(key => groups.find(group => group.key === key)!)
        .filter(Boolean);
    }

    const aggregate = (group: GroupedSeries) => {
      const values = group.points.map(point => point.x);
      switch (order) {
        case 'category ascending':
        case 'category descending':
          return 0;
        case 'total ascending':
        case 'total descending':
        case 'sum ascending':
        case 'sum descending':
          return values.reduce((sum, value) => sum + value, 0);
        case 'min ascending':
        case 'min descending':
          return Math.min(...values);
        case 'max ascending':
        case 'max descending':
          return Math.max(...values);
        case 'mean ascending':
        case 'mean descending':
          return values.reduce((sum, value) => sum + value, 0) / values.length;
        case 'median ascending':
        case 'median descending':
          return getMedian(values);
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
      const barGapRate = padding / (1 - padding);
      const totalBands = groupCount + Math.max(groupCount - 1, 0) * barGapRate;
      if (barHeight === 0) {
        barHeight = totalHeight / Math.max(totalBands, 1);
      }

      const requiredHeight = totalBands * barHeight;
      if (totalHeight >= requiredHeight) {
        domainMargin = MIN_DOMAIN_MARGIN + (totalHeight - requiredHeight) / 2;
      }
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

  private _getXScaleInfo(groups: GroupedSeries[]) {
    let min = 0;
    let max = 0;

    groups.forEach(group => {
      const positive = group.points.filter(point => point.x >= 0).reduce((sum, point) => sum + point.x, 0);
      const negative = group.points.filter(point => point.x < 0).reduce((sum, point) => sum + point.x, 0);
      max = Math.max(max, positive);
      min = Math.min(min, negative);
    });

    min = Math.min(min, toOptionalNumber(this.xMinValue) ?? min);
    max = Math.max(max, toOptionalNumber(this.xMaxValue) ?? max);

    if (min === max) {
      if (max === 0) {
        max = 1;
      } else {
        min = Math.min(0, min);
      }
    }

    return getNiceDomainAndTicks(min, max, toNumber(this.xAxisTickCount, DEFAULT_X_TICK_COUNT));
  }

  private _getNumericYDomain(yValues: number[]) {
    const yMin = Math.min(...yValues);
    const yMax = Math.max(...yValues);
    const domainMin = Math.min(yMin, toOptionalNumber(this.yMinValue) ?? 0);
    const domainMax = Math.max(yMax, toOptionalNumber(this.yMaxValue) ?? 0);
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
      const startOffset = padding * step;
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

  private _getPointColor(
    point: HorizontalBarChartWithAxisDataPoint,
    index: number,
    legendColorMap?: Map<string, string>,
  ) {
    if (this.useSingleColor) {
      const singleColorPoint = this.data.find(
        candidate => typeof candidate.color === 'string' && candidate.color.length > 0,
      );
      return singleColorPoint?.color ? getColorFromToken(singleColorPoint.color) : getColorFromToken('qualitative.2');
    }

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
    point: HorizontalBarChartWithAxisDataPoint,
    color: string,
  ) {
    if (!this.enableGradient && !point.gradient) {
      return undefined;
    }

    const gradientId = `hbcwa-gradient-${groupIndex}-${pointIndex}`;
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

    ticks.forEach(tick => {
      const x = toX(tick);
      const tickLine = createSvgElement<SVGLineElement>('line');
      tickLine.setAttribute('class', 'axis-tick-line');
      tickLine.setAttribute('x1', `${x}`);
      tickLine.setAttribute('x2', `${x}`);
      tickLine.setAttribute('y1', `${axisY}`);
      tickLine.setAttribute('y2', `${20}`);
      axisLayer.appendChild(tickLine);

      const text = createSvgElement<SVGTextElement>('text');
      text.setAttribute('class', 'axis-text');
      text.setAttribute('x', `${x}`);
      text.setAttribute('y', `${axisY + 18}`);
      text.setAttribute('text-anchor', 'middle');
      text.textContent = formatAxisNumber(tick, this.culture);
      axisLayer.appendChild(text);
    });
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
        this._appendYAxisTick(axisLayer, axisX, y, formatCompactNumber(tick, this.culture).toLowerCase());
      });
      return;
    }

    groups.forEach((group, index) => {
      const y = yPositionForGroup(group, index);
      const fullLabel = String(group.rawY);
      const label = this.showYAxisLabels ? fullLabel : truncateText(fullLabel, 18);
      this._appendYAxisTick(axisLayer, axisX, y, label, this.showYAxisLabelsTooltip ? fullLabel : undefined);
    });
  }

  private _appendYAxisTick(axisLayer: SVGGElement, axisX: number, y: number, label: string, tooltipText?: string) {
    const tickLine = createSvgElement<SVGLineElement>('line');
    tickLine.setAttribute('class', 'axis-tick-line');
    tickLine.setAttribute('x1', `${axisX}`);
    tickLine.setAttribute('x2', `${axisX + 6}`);
    tickLine.setAttribute('y1', `${y}`);
    tickLine.setAttribute('y2', `${y}`);
    axisLayer.appendChild(tickLine);

    const text = createSvgElement<SVGTextElement>('text');
    text.setAttribute('class', 'y-axis-text');
    text.setAttribute('x', `${axisX + (this._isRTL ? 12 : -12)}`);
    text.setAttribute('y', `${y}`);
    text.setAttribute('dominant-baseline', 'central');
    text.setAttribute('text-anchor', 'end');
    text.textContent = label;

    if (tooltipText) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = tooltipText;
      text.appendChild(title);
    }
    axisLayer.appendChild(text);
  }

  private _renderOriginLine(
    axisLayer: SVGGElement,
    margins: { top: number; right: number; left: number; bottom: number },
    height: number,
    domain: [number, number],
    innerWidth: number,
  ) {
    if (!(domain[0] < 0 && domain[1] > 0)) {
      return;
    }

    const span = domain[1] - domain[0] || 1;
    const rangeStart = this._isRTL
      ? this.getBoundingClientRect().width - margins.right || margins.left + innerWidth
      : margins.left;
    const rangeEnd = this._isRTL
      ? margins.left
      : this.getBoundingClientRect().width - margins.right || margins.left + innerWidth;
    const originX = rangeStart + ((0 - domain[0]) / span) * (rangeEnd - rangeStart);
    const line = createSvgElement<SVGLineElement>('line');
    line.setAttribute('class', 'origin-line');
    line.setAttribute('x1', `${originX}`);
    line.setAttribute('x2', `${originX}`);
    line.setAttribute('y1', `${margins.top}`);
    line.setAttribute('y2', `${height - margins.bottom}`);
    axisLayer.appendChild(line);
  }

  private _showTooltip(
    point: HorizontalBarChartWithAxisDataPoint,
    color: string,
    event: MouseEvent | FocusEvent,
    target: SVGRectElement,
  ) {
    const highlighted = this._getHighlightedLegends();
    if (highlighted.length > 0 && point.legend && !highlighted.includes(point.legend)) {
      return;
    }

    const hostRect = this.getBoundingClientRect();
    const targetRect = target.getBoundingClientRect();
    const xReference = 'clientX' in event ? event.clientX : targetRect.left + targetRect.width / 2;
    const xPos = this._isRTL ? hostRect.right - xReference : xReference - hostRect.left;
    const yPos = ('clientY' in event ? event.clientY : targetRect.top) - hostRect.top - 44;
    this.tooltipProps = {
      isVisible: true,
      legend: point.legend || '',
      xLabel: X_AXIS_LABEL,
      xValue: point.xAxisCalloutData || formatAxisNumber(point.x, this.culture),
      yLabel: Y_AXIS_LABEL,
      yValue: point.yAxisCalloutData || String(point.y),
      color,
      xPos: Math.max(0, xPos),
      yPos: Math.max(0, yPos),
    };
  }

  private _clearTooltipState() {
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

  private _getAriaLabel(point: HorizontalBarChartWithAxisDataPoint) {
    const xValue = point.xAxisCalloutData || point.x;
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
      element.setAttribute('tabindex', shouldHighlight ? '0' : '-1');
    });
  }
}
