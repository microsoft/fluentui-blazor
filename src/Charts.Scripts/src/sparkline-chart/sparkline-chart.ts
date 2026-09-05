import { attr, nullableNumberConverter } from '@microsoft/fast-element';
import { scaleLinear, scaleTime } from 'd3-scale';
import { area, line } from 'd3-shape';
import { ChartBase } from '../utils/chart-base.js';
import { getColorFromToken, jsonConverter, SVG_NAMESPACE_URI } from '../utils/chart-helpers.js';
import type { SparklineChartData, SparklineDataPoint, SparklineVariant } from './sparkline-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const toNumber = (value: number | string | undefined, fallback: number): number => {
  if (value === undefined || value === null || value === '') {
    return fallback;
  }

  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
};

interface NormalizedPoint {
  x: number | Date;
  y: number;
}

/** @public */
export class SparklineChart extends ChartBase {
  @attr({ converter: jsonConverter })
  public data!: SparklineChartData;

  @attr
  public variant: SparklineVariant = 'area';

  @attr
  public color?: string;

  @attr({ attribute: 'show-legend', mode: 'boolean' })
  public showLegend: boolean = false;

  @attr({ attribute: 'value-text-width', converter: nullableNumberConverter })
  public valueTextWidth?: number;

  protected override _enableResizeObserver = true;

  constructor() {
    super();
    this.width = 80;
    this.height = 20;
  }

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'variant', 'color', 'showLegend', 'valueTextWidth'] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this._requestRender();
  }

  protected dataChanged() {
    this._requestRender();
  }

  protected variantChanged() {
    this._requestRender();
  }

  protected colorChanged() {
    this._requestRender();
  }

  protected showLegendChanged() {
    this._requestRender();
  }

  protected valueTextWidthChanged() {
    this._requestRender();
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    const series = this.data?.lineChartData[0];
    const points = series?.data ?? [];
    const legend = series?.legend;
    const chartWidth = toNumber(this.width, 80);
    const chartHeight = toNumber(this.height, 20);
    const legendWidth = this.showLegend && legend ? this.valueTextWidth ?? 80 : 0;
    this._applyHostDimensions(chartWidth + legendWidth, chartHeight);
    this._clearChart();
    this.legends = [];

    if (points.length === 0) {
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const width = chartWidth;
    const height = chartHeight;
    const margins = { top: 4, right: 4, bottom: 4, left: 4 };
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);
    const stroke = getColorFromToken(this.color ?? series?.color ?? '#637cef');

    const normalized = this._normalizePoints(points);
    const yMin = Math.min(...normalized.map(point => point.y));
    const yMax = Math.max(...normalized.map(point => point.y));
    const safeYMax = yMin === yMax ? yMax + 1 : yMax;
    const yScale = scaleLinear()
      .domain([Math.min(0, yMin), safeYMax])
      .range([innerHeight, 0]);

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const group = createSvgElement<SVGGElement>('g');
    group.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(group);

    const isTimeSeries = normalized[0]?.x instanceof Date;
    const xDomainStart = normalized[0]?.x ?? 0;
    const xDomainEnd = normalized[normalized.length - 1]?.x ?? 0;
    const safeEnd =
      isTimeSeries &&
      xDomainStart instanceof Date &&
      xDomainEnd instanceof Date &&
      xDomainStart.getTime() === xDomainEnd.getTime()
        ? new Date(xDomainEnd.getTime() + 1)
        : !isTimeSeries && xDomainStart === xDomainEnd
        ? Number(xDomainEnd) + 1
        : xDomainEnd;

    const xScale = isTimeSeries
      ? scaleTime()
          .domain([xDomainStart as Date, safeEnd as Date])
          .range([0, innerWidth])
      : scaleLinear()
          .domain([Number(xDomainStart), Number(safeEnd)])
          .range([0, innerWidth]);

    const lineGenerator = line<NormalizedPoint>()
      .x(point => (point.x instanceof Date ? xScale(point.x) : xScale(Number(point.x))))
      .y(point => yScale(point.y));

    if (this.variant === 'area') {
      const areaGenerator = area<NormalizedPoint>()
        .x(point => (point.x instanceof Date ? xScale(point.x) : xScale(Number(point.x))))
        .y0(innerHeight)
        .y1(point => yScale(point.y));

      const areaPath = createSvgElement<SVGPathElement>('path');
      areaPath.classList.add('sparkline-area');
      areaPath.setAttribute('fill', stroke);
      areaPath.setAttribute('d', areaGenerator(normalized) ?? '');
      group.appendChild(areaPath);
    }

    const linePath = createSvgElement<SVGPathElement>('path');
    linePath.classList.add('sparkline-line');
    linePath.setAttribute('stroke', stroke);
    linePath.setAttribute('d', lineGenerator(normalized) ?? '');
    group.appendChild(linePath);

    this.chartContainer.appendChild(svg);
    if (this.showLegend && legend) {
      this.chartContainer.appendChild(this._createLegendSvg(legend, legendWidth, height));
    }
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {}

  protected override _getHostAriaLabel(): string {
    const series = this.data?.lineChartData[0];
    const count = series?.data.length ?? 0;
    if (count === 0) {
      return 'Sparkline chart with no data.';
    }

    const label = series?.legend ?? this.chartTitle ?? this.data?.chartTitle;
    return label ? `Sparkline with label ${label}.` : `Sparkline chart with ${count} points.`;
  }

  private _clearChart(): void {
    while (this.chartContainer?.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }

  private _createLegendSvg(legend: string, width: number, height: number): SVGSVGElement {
    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('sparkline-legend');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));

    const text = createSvgElement<SVGTextElement>('text');
    text.classList.add('sparkline-legend-text');
    text.setAttribute('x', this._isRTL ? '100%' : '0%');
    text.setAttribute('text-anchor', this._isRTL ? 'end' : 'start');
    text.setAttribute('dx', this._isRTL ? '-8' : '8');
    text.setAttribute('y', '100%');
    text.setAttribute('dy', '-5');
    text.textContent = legend;
    svg.appendChild(text);
    return svg;
  }

  private _normalizePoints(points: SparklineDataPoint[]): NormalizedPoint[] {
    const isTimeSeries = points.some(
      point =>
        point.x instanceof Date ||
        (typeof point.x === 'string' && Number.isNaN(Number(point.x)) && !Number.isNaN(Date.parse(point.x))),
    );

    return points.map((point, index) => {
      if (isTimeSeries) {
        return {
          x: point.x instanceof Date ? point.x : new Date(point.x),
          y: point.y,
        };
      }
      return {
        x: Number(point.x ?? index),
        y: point.y,
      };
    });
  }
}
