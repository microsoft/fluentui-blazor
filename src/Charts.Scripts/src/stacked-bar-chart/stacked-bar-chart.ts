import { attr } from '@microsoft/fast-element';
import { format } from 'd3-format';
import { scaleLinear } from 'd3-scale';
import type { Legend } from '../utils/chart-options.js';
import { ChartBase } from '../utils/chart-base.js';
import {
  getColorFromToken,
  getNextColor,
  jsonConverter,
  lightenColor,
  parseNumber as toNumber,
  resolvePixelDimension,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { StackedBarChartData, StackedBarChartDataPoint } from './stacked-bar-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const defaultNumberFormatter = format(',.2~f');

const getSegmentColor = (point: StackedBarChartDataPoint, index: number): string => {
  if (point.placeHolder) {
    return '#d1d1d1';
  }

  if (point.color) {
    return getColorFromToken(point.color);
  }

  return getNextColor(index, 0);
};

interface RenderedSegment {
  legend: string;
  bar: SVGRectElement;
  label?: SVGTextElement;
}

/** @public */
export class StackedBarChart extends ChartBase {
  @attr({ converter: jsonConverter })
  public data!: StackedBarChartData;

  @attr({ attribute: 'bar-height' })
  public barHeight?: number | string;

  @attr({ attribute: 'hide-number-display', mode: 'boolean' })
  public hideNumberDisplay: boolean = false;

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  protected override _enableResizeObserver = true;

  private _segments: RenderedSegment[] = [];

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'barHeight', 'hideNumberDisplay', 'enableGradient'] as const;
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
    if (!this.chartTitle && this.data?.chartTitle) {
      this.chartTitle = this.data.chartTitle;
    }
    this._requestRender();
  }

  protected barHeightChanged() {
    this._requestRender();
  }

  protected hideNumberDisplayChanged() {
    this._requestRender();
  }

  protected enableGradientChanged() {
    this._requestRender();
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();

    const chartData = this.data?.chartData ?? [];
    const title = this.chartTitle || this.data?.chartTitle;
    if (!this.chartTitle && title) {
      this.chartTitle = title;
      return;
    }

    if (chartData.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const margins = { top: 32, right: 20, bottom: 20, left: 20 };
    const barHeight = toNumber(this.barHeight, 16);
    const width = resolvePixelDimension(this.width, this.chartContainer.getBoundingClientRect().width, 600);
    const height = resolvePixelDimension(
      this.height,
      this.chartContainer.getBoundingClientRect().height,
      margins.top + barHeight + margins.bottom,
    );
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const total = chartData.reduce((sum, point) => sum + Math.max(point.data, 0), 0);
    const scale = scaleLinear()
      .domain([0, total || 1])
      .range([0, innerWidth]);

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const group = createSvgElement<SVGGElement>('g');
    group.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(group);

    const legendMap = new Map<string, string>();
    chartData.forEach((point, index) => {
      if (!point.placeHolder) {
        legendMap.set(point.legend, getSegmentColor(point, index));
      }
    });
    this.legends = Array.from(legendMap.entries()).map(([legend, color]) => ({ legend, color }));
    this._updateLegendInteractionState();

    let start = 0;
    this._segments = [];

    chartData.forEach((point, index) => {
      const segmentWidth = Math.max(scale(Math.max(point.data, 0)), total === 0 ? innerWidth / chartData.length : 0);
      const x = this._isRTL ? innerWidth - scale(start + Math.max(point.data, 0)) : scale(start);
      const color = getSegmentColor(point, index);
      const fill = this._createFill(defs, color, point, index);

      const rect = createSvgElement<SVGRectElement>('rect');
      rect.classList.add('bar');
      rect.dataset.legend = point.legend;
      rect.setAttribute('x', String(x));
      rect.setAttribute('y', '0');
      rect.setAttribute('width', String(Math.max(segmentWidth, 0)));
      rect.setAttribute('height', String(barHeight));
      rect.setAttribute('rx', this.roundCorners ? '4' : '0');
      rect.setAttribute('fill', fill);
      rect.setAttribute('role', 'img');
      rect.setAttribute('tabindex', index === 0 ? '0' : '-1');
      rect.setAttribute('aria-label', `${point.legend}: ${defaultNumberFormatter(point.data)}`);
      if (point.onClick) {
        rect.classList.add('interactive');
      }

      const showTooltip = (event: MouseEvent | FocusEvent) => {
        if (!this._shouldShowTooltip(point.legend) || this.hideTooltip) {
          return;
        }
        this._showTooltip(point, color, event, rect);
      };

      rect.addEventListener('mouseenter', showTooltip);
      rect.addEventListener('focus', showTooltip);
      rect.addEventListener('mouseleave', () => this._clearTooltip());
      rect.addEventListener('blur', () => this._clearTooltip());
      rect.addEventListener('keydown', event => {
        if (event.key === 'Enter' || event.key === ' ') {
          event.preventDefault();
          point.onClick?.();
        }
        this._rovingKeydown(
          this._segments.map(segment => segment.bar),
          event,
        );
      });
      rect.addEventListener('click', () => {
        this._focusRovingElement(
          this._segments.map(segment => segment.bar),
          rect,
        );
        point.onClick?.();
      });

      group.appendChild(rect);

      let label: SVGTextElement | undefined;
      if (!this.hideNumberDisplay) {
        label = createSvgElement<SVGTextElement>('text');
        label.classList.add('bar-value');
        label.dataset.legend = point.legend;
        label.setAttribute('x', String(x + segmentWidth / 2));
        label.setAttribute('y', '-8');
        label.setAttribute('text-anchor', 'middle');
        label.textContent = defaultNumberFormatter(point.data);
        group.appendChild(label);
      }

      this._segments.push({ legend: point.legend, bar: rect, label });
      start += Math.max(point.data, 0);
    });

    this.chartContainer.appendChild(svg);
    this._applyActiveLegendState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this._segments) {
      return;
    }
    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;

    for (const segment of this._segments) {
      const isActive = !hasSelection || highlighted.includes(segment.legend);
      segment.bar.classList.toggle('inactive', !isActive);
      segment.bar.setAttribute('opacity', isActive ? '1' : '0.1');
      if (segment.label) {
        segment.label.classList.toggle('inactive', !isActive);
        segment.label.setAttribute('opacity', isActive ? '1' : '0.1');
      }
    }

    this._relocateFocusIfNeeded(this._segments.map(segment => segment.bar));
  }

  protected override _getHostAriaLabel(): string {
    const chartData = this.data?.chartData ?? [];
    if (chartData.length === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Stacked bar chart with no data.';
    }

    const total = chartData.reduce((sum, point) => sum + Math.max(point.data, 0), 0);
    const title = this.chartTitle || this.data?.chartTitle || 'Stacked bar chart';
    return `${title}. ${chartData.length} segments. Total value ${defaultNumberFormatter(total)}.`;
  }

  private _clearChart(): void {
    this._segments = [];
    if (!this.chartContainer) {
      return;
    }

    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }

  private _createFill(defs: SVGDefsElement, color: string, point: StackedBarChartDataPoint, index: number): string {
    if (!this.enableGradient && !point.gradient) {
      return color;
    }

    const gradientId = `stacked-bar-gradient-${index}`;
    const gradient = createSvgElement<SVGLinearGradientElement>('linearGradient');
    gradient.id = gradientId;
    gradient.setAttribute('x1', '0%');
    gradient.setAttribute('x2', '100%');
    gradient.setAttribute('y1', '0%');
    gradient.setAttribute('y2', '0%');

    const stops = point.gradient
      ? point.gradient.map(stopColor => getColorFromToken(stopColor))
      : [lightenColor(color, 0.35), color];

    stops.forEach((stopColor, stopIndex) => {
      const stop = createSvgElement<SVGStopElement>('stop');
      stop.setAttribute('offset', `${(stopIndex / Math.max(stops.length - 1, 1)) * 100}%`);
      stop.setAttribute('stop-color', stopColor);
      gradient.appendChild(stop);
    });

    defs.appendChild(gradient);
    return `url(#${gradientId})`;
  }

  private _showTooltip(
    dataPoint: StackedBarChartDataPoint,
    color: string,
    _event: MouseEvent | FocusEvent,
    el: Element,
  ) {
    const hostRect = this.getBoundingClientRect();
    const targetRect = el.getBoundingClientRect();
    const anchorX = targetRect.left - hostRect.left + targetRect.width / 2;
    const topY = targetRect.top - hostRect.top;
    const bottomY = targetRect.bottom - hostRect.top;
    const isFreshShow = !this.tooltipProps.isVisible;
    this._currentTooltipDataPoint = dataPoint;
    this.tooltipProps = {
      isVisible: true,
      legend: dataPoint.legend ?? '',
      yValue: defaultNumberFormatter(dataPoint.data ?? 0),
      color,
      xPos: anchorX,
      yPos: topY,
    };
    this._positionTooltipAvoidingOverlap(anchorX, topY, bottomY, isFreshShow);
  }
}
