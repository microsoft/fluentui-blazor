import { attr } from '@microsoft/fast-element';
import { extent, max } from 'd3-array';
import { type Axis, axisBottom, type AxisDomain, axisLeft } from 'd3-axis';
import { format } from 'd3-format';
import { scalePoint, scaleTime, scaleUtc } from 'd3-scale';
import { timeFormat, utcFormat } from 'd3-time-format';
import type { AxisScaleType, TooltipProps } from '../utils/chart-options.js';
import { CartesianChartBase } from '../utils/cartesian-chart-base.js';
import {
  applyAxisTickConfig,
  type AxisScaleLike,
  computePreparedNumericYAxis,
  createNumericContinuousScale,
  DEFAULT_NUMERIC_Y_TICK_COUNT,
  renderAxisGridLinesShared,
  renderBottomAxisShared,
  renderPrimaryYAxisShared,
  toAxisNumber as toNumber,
  toOptionalAxisNumber as toOptionalNumber,
} from '../utils/cartesian-axis-shared.js';
import {
  defaultYAxisTickFormatter,
  formatLocaleNumber,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { ScatterChartSeries } from './scatter-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

type ScatterTooltipEntry = {
  legend: string;
  yValue: string;
  color: string;
};

type TooltipState = TooltipProps & { xValue: string; entries: ScatterTooltipEntry[] };
type XValue = number | Date | string;

const defaultMargins = { top: 40, right: 20, bottom: 50, left: 60 };

const getMarkerDomainPadding = (minValue: number, maxValue: number, scaleType?: AxisScaleType) =>
  scaleType === 'log'
    ? { start: minValue * 0.5, end: maxValue }
    : { start: (maxValue - minValue) * 0.1, end: (maxValue - minValue) * 0.1 };

const calculateMarkerRadius = (
  markerSize: number | undefined,
  minMarkerSize: number,
  maxMarkerSize: number,
  extraMaxPixels: number,
  isContinuousXY: boolean,
): number => {
  if (!markerSize) {
    return 4;
  }

  let radius: number;
  if (isContinuousXY && maxMarkerSize !== 0) {
    radius = maxMarkerSize < extraMaxPixels ? markerSize : (markerSize / maxMarkerSize) * extraMaxPixels;
  } else if (!isContinuousXY && maxMarkerSize !== minMarkerSize) {
    radius = 4 + ((markerSize - minMarkerSize) / (maxMarkerSize - minMarkerSize)) * 12;
  } else {
    return 4;
  }

  return Math.max(radius, 4);
};

const formatNumberValue = (value: number, specifier: string | undefined, culture: string | undefined): string => {
  if (specifier) {
    try {
      return format(specifier)(value);
    } catch {
      // Fall back to locale formatting below.
    }
  }
  return formatLocaleNumber(value, culture);
};

const normalizeXValue = (value: XValue): XValue => {
  if (value instanceof Date) {
    return value;
  }
  if (typeof value === 'string' && !Number.isNaN(Date.parse(value))) {
    return new Date(value);
  }
  return value;
};

const formatDateValue = (chart: ScatterChart, value: Date): string => {
  if (chart.customDateTimeFormatter) {
    return chart.customDateTimeFormatter(value);
  }
  if (chart.tickFormat) {
    try {
      return (chart.useUTC ? utcFormat(chart.tickFormat) : timeFormat(chart.tickFormat))(value);
    } catch {
      // Fall back to Intl below.
    }
  }
  const options = chart.useUTC ? { ...chart.dateLocalizeOptions, timeZone: 'UTC' } : chart.dateLocalizeOptions;
  try {
    return new Intl.DateTimeFormat(chart.culture, options).format(value);
  } catch {
    return new Intl.DateTimeFormat(undefined, options).format(value);
  }
};

const formatDateAxisValue = (chart: ScatterChart, value: Date): string => {
  if (chart.customDateTimeFormatter || chart.tickFormat || chart.dateLocalizeOptions) {
    return formatDateValue(chart, value);
  }

  const options: Intl.DateTimeFormatOptions = {
    month: 'short',
    day: '2-digit',
    ...(chart.useUTC && { timeZone: 'UTC' }),
  };
  try {
    return new Intl.DateTimeFormat(chart.culture, options).format(value);
  } catch {
    return new Intl.DateTimeFormat(undefined, options).format(value);
  }
};

/** @public */
export class ScatterChart extends CartesianChartBase {
  public declare tooltipProps: TooltipState;

  @attr({ converter: jsonConverter })
  public data!: ScatterChartSeries[];

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data'] as const;
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

    this.tooltipProps = { ...this.tooltipProps, xValue: '', entries: [] } as TooltipState;
    this._requestRender();
  }

  protected dataChanged(): void {
    this._requestRender();
  }

  protected override _clearTooltip(): void {
    this.tooltipProps = {
      isVisible: false,
      legend: '',
      xValue: '',
      yValue: '',
      color: '',
      xPos: 0,
      yPos: 0,
      entries: [],
    };
  }

  protected override _buildDefaultTooltipHTML(): string {
    return [
      `<div class="tooltip-header">${this.tooltipProps.xValue}</div>`,
      ...this.tooltipProps.entries.map(entry =>
        [
          `<div class="tooltip-info" style="border-color: ${entry.color};">`,
          `<div class="tooltip-legend-text">${entry.legend}</div>`,
          `<div class="tooltip-primary-value" style="color: ${entry.color};">${entry.yValue}</div>`,
          `</div>`,
        ].join(''),
      ),
    ].join('');
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();

    const seriesData = Array.isArray(this.data) ? this.data : [];
    if (seriesData.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const normalizedSeries = seriesData.map((series, index) => ({
      legend: series.legend,
      color: series.color ? getColorFromToken(series.color) : getNextColor(index, 0),
      data: series.data.map(point => {
        const x = normalizeXValue(point.x);
        return {
          ...point,
          x,
          xLabel:
            x instanceof Date
              ? formatDateValue(this, x)
              : typeof x === 'number'
              ? formatNumberValue(x, this.xAxisTickFormat, this.culture)
              : x,
          yLabel: String(point.y),
        };
      }),
    }));
    const markerSizes = normalizedSeries.flatMap(series =>
      series.data.flatMap(point => (point.markerSize === undefined ? [] : point.markerSize)),
    );
    const minMarkerSize = markerSizes.length > 0 ? Math.min(...markerSizes) : 0;
    const maxMarkerSize = max(markerSizes) ?? 0;
    const xValues = normalizedSeries.flatMap(series => series.data.map(point => point.x));
    const isDateAxis = xValues.some(value => value instanceof Date);
    const isStringAxis = !isDateAxis && xValues.some(value => typeof value === 'string');

    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 500);
    const height = toNumber(this.height, 300);
    const { svg, plotGroup, margins, innerWidth, innerHeight } = this._createCartesianRenderContext({
      width,
      height,
      defaultMargins,
    });

    const yValues = normalizedSeries
      .flatMap(series => series.data.map(point => point.y))
      .filter(value => Number.isFinite(value) && (this.yScaleType !== 'log' || value > 0));
    const yExtent = extent(yValues);
    const yDataMin = yExtent[0] ?? 0;
    const yDataMax = yExtent[1] ?? 1;
    const yPadding = getMarkerDomainPadding(yDataMin, yDataMax, this.yScaleType);
    const automaticYMin = yDataMin - yPadding.start;
    const automaticYMax = yDataMax + yPadding.end;
    let yMin =
      toOptionalNumber(this.yMinValue) ?? (this.yScaleType === 'log' ? automaticYMin : Math.min(0, automaticYMin));
    let yMax =
      toOptionalNumber(this.yMaxValue) ?? (this.yScaleType === 'log' ? automaticYMax : Math.max(0, automaticYMax));
    if (yMin == yMax) {
      yMin -= 1;
      yMax += 1;
    }

    const xRange: [number, number] = this._isRTL ? [innerWidth, 0] : [0, innerWidth];
    let xScale: any;
    let xFormatter: (value: AxisDomain) => string;
    if (isDateAxis) {
      const dates = xValues.filter((value): value is Date => value instanceof Date);
      const dateExtent = extent(dates, value => value.getTime());
      const dateRange = (dateExtent[1] ?? 0) - (dateExtent[0] ?? 0);
      const datePadding = dateRange * 0.1;
      xScale = (this.useUTC ? scaleUtc() : scaleTime())
        .domain([new Date((dateExtent[0] ?? 0) - datePadding), new Date((dateExtent[1] ?? 0) + datePadding)])
        .range(xRange)
        .nice();
      xFormatter = value => formatDateAxisValue(this, value as Date);
    } else if (isStringAxis) {
      const categories = Array.from(new Set(xValues.filter((value): value is string => typeof value === 'string')));
      xScale = scalePoint<string>().domain(categories).range(xRange).padding(0.5);
      xFormatter = value => String(value);
    } else {
      const numericExtent = extent(
        xValues.filter(
          (value): value is number =>
            typeof value === 'number' && Number.isFinite(value) && (this.xScaleType !== 'log' || value > 0),
        ),
      );
      const xDataMin = numericExtent[0] ?? 0;
      const xDataMax = numericExtent[1] ?? 1;
      const xPadding = getMarkerDomainPadding(xDataMin, xDataMax, this.xScaleType);
      let xMin = toOptionalNumber(this.xMinValue) ?? xDataMin - xPadding.start;
      let xMax = toOptionalNumber(this.xMaxValue) ?? xDataMax + xPadding.end;
      if (xMin === xMax) {
        xMin -= 1;
        xMax += 1;
      }
      xScale = createNumericContinuousScale({
        domainMin: xMin,
        domainMax: xMax,
        range: xRange,
        scaleType: this.xScaleType,
        roundedTicks: this.roundedTicks,
      }).scale;
      if (this.xScaleType === 'log') {
        xScale.nice();
      }
      xFormatter = value => formatNumberValue(Number(value), this.xAxisTickFormat, this.culture);
    }
    const preparedYAxis = computePreparedNumericYAxis({
      minValue: yMin,
      maxValue: yMax,
      tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
      roundedTicks: this.roundedTicks,
    });
    const { scale: yScale, isLogarithmic: isLogarithmicY } = createNumericContinuousScale({
      domainMin: this.yScaleType === 'log' ? yMin : preparedYAxis.domainMin,
      domainMax: this.yScaleType === 'log' ? yMax : preparedYAxis.domainMax,
      range: [innerHeight, 0],
      scaleType: this.yScaleType,
    });
    if (isLogarithmicY) {
      yScale.nice();
    }

    let extraMaxPixels = 0;
    if (!isStringAxis) {
      const continuousXValues = xValues
        .map(value => (value instanceof Date ? value.getTime() : Number(value)))
        .filter(value => Number.isFinite(value) && (this.xScaleType !== 'log' || value > 0));
      const continuousXExtent = extent(continuousXValues);
      const xDataMin = continuousXExtent[0] ?? 0;
      const xDataMax = continuousXExtent[1] ?? 0;
      const xPadding = getMarkerDomainPadding(xDataMin, xDataMax, isDateAxis ? undefined : this.xScaleType);
      const toXDomain = (value: number): number | Date => (isDateAxis ? new Date(value) : value);
      const extraXPixels = Math.min(
        Math.abs(xScale(toXDomain(xDataMin)) - xScale(toXDomain(xDataMin - xPadding.start))),
        Math.abs(xScale(toXDomain(xDataMax + xPadding.end)) - xScale(toXDomain(xDataMax))),
      );

      const extraYPixels = Math.min(
        Math.abs(yScale(yDataMin - yPadding.start) - yScale(yDataMin)),
        Math.abs(yScale(yDataMax) - yScale(yDataMax + yPadding.end)),
      );
      extraMaxPixels = Math.min(extraXPixels, extraYPixels);
    }

    const xAxis = axisBottom(xScale).tickPadding(toNumber(this.tickPadding, 6));
    if (!isStringAxis) {
      xAxis.ticks(this.xScaleType === 'log' ? 10 : 6);
    }
    if (this.tickValues?.length) {
      xAxis.tickValues(
        this.tickValues.map(value =>
          isDateAxis ? normalizeXValue(value as XValue) : isStringAxis ? String(value) : Number(value),
        ),
      );
    }

    const yAxis = axisLeft(yScale).tickPadding(toNumber(this.tickPadding, 6));
    applyAxisTickConfig(
      yAxis,
      isLogarithmicY ? this.yAxisTickCount : this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
      this.yAxisTickValues ?? (isLogarithmicY ? undefined : preparedYAxis.tickValues),
    );

    renderAxisGridLinesShared({
      layer: plotGroup,
      orientation: 'horizontal',
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      spanStart: 0,
      spanEnd: innerWidth,
    });

    const hoverLine = createSvgElement<SVGLineElement>('line');
    hoverLine.classList.add('hover-line');
    hoverLine.setAttribute('y2', String(innerHeight + 6));
    hoverLine.style.display = 'none';
    plotGroup.appendChild(hoverLine);

    const renderedPoints: Array<{
      circle: SVGCircleElement;
      x: XValue;
      y: number;
      xLabel: string;
      yLabel: string;
      legend: string;
      color: string;
      markerRadius: number;
    }> = [];
    let activePoints: SVGCircleElement[] = [];
    const clearHoverState = (): void => {
      for (const activePoint of activePoints) {
        activePoint.setAttribute('fill', activePoint.dataset.color ?? '');
        activePoint.classList.remove('active');
      }
      activePoints = [];
      hoverLine.style.display = 'none';
      this._clearTooltip();
    };

    const getXKey = (value: XValue): string =>
      value instanceof Date ? `date:${value.getTime()}` : `${typeof value}:${value}`;

    const pointKeydown = (event: KeyboardEvent): void => {
      const currentTarget = event.currentTarget as SVGCircleElement | null;
      if (!currentTarget) {
        return;
      }

      let orderedPoints = renderedPoints
        .map(point => point.circle)
        .sort(
          (left, right) =>
            Number(left.getAttribute('cx')) - Number(right.getAttribute('cx')) ||
            Number(left.getAttribute('cy')) - Number(right.getAttribute('cy')),
        );
      if (event.key === 'ArrowUp' || event.key === 'ArrowDown') {
        const xKey = currentTarget.dataset.xKey;
        orderedPoints = renderedPoints
          .filter(point => getXKey(point.x) === xKey)
          .sort((left, right) => Number(left.circle.getAttribute('cy')) - Number(right.circle.getAttribute('cy')))
          .map(point => point.circle);
      }
      this._rovingKeydown(orderedPoints, event);
    };

    normalizedSeries.forEach(series => {
      series.data.forEach(point => {
        const circle = createSvgElement<SVGCircleElement>('circle');
        circle.classList.add('scatter-point');
        circle.dataset.legend = series.legend;
        circle.setAttribute('cx', String(xScale(point.x) ?? 0));
        circle.setAttribute('cy', String(yScale(point.y)));
        const markerRadius = calculateMarkerRadius(
          point.markerSize,
          minMarkerSize,
          maxMarkerSize,
          extraMaxPixels,
          !isStringAxis,
        );
        circle.setAttribute('r', String(markerRadius));
        circle.setAttribute('fill', series.color);
        circle.setAttribute('stroke', series.color);
        circle.setAttribute('role', 'img');
        circle.setAttribute('aria-label', `${point.xLabel}, ${series.legend}, ${point.yLabel}.`);
        circle.setAttribute('tabindex', renderedPoints.length === 0 ? '0' : '-1');
        circle.dataset.xKey = getXKey(point.x);
        circle.dataset.color = series.color;
        const renderedPoint = {
          circle,
          x: point.x,
          y: point.y,
          xLabel: point.xLabel,
          yLabel: point.yLabel,
          legend: series.legend,
          color: series.color,
          markerRadius,
        };
        renderedPoints.push(renderedPoint);
        const showTooltip = () => {
          if (!this._shouldShowTooltip(series.legend) || this.hideTooltip) {
            return;
          }
          clearHoverState();
          const xKey = getXKey(point.x);
          const matchingPoints = renderedPoints.filter(
            candidate => getXKey(candidate.x) === xKey && this._shouldShowTooltip(candidate.legend),
          );
          for (const matchingPoint of matchingPoints) {
            matchingPoint.circle.setAttribute('fill', 'var(--colorNeutralBackground1)');
            matchingPoint.circle.classList.add('active');
          }
          activePoints = matchingPoints.map(matchingPoint => matchingPoint.circle);

          const highestPoint = matchingPoints.reduce((highest, candidate) =>
            yScale(candidate.y) < yScale(highest.y) ? candidate : highest,
          );
          hoverLine.setAttribute('x1', circle.getAttribute('cx') ?? '0');
          hoverLine.setAttribute('x2', circle.getAttribute('cx') ?? '0');
          hoverLine.setAttribute('y1', String(yScale(highestPoint.y) + highestPoint.markerRadius));
          hoverLine.style.display = '';

          const hostRect = this.getBoundingClientRect();
          const svgRect = svg.getBoundingClientRect();
          const anchorX = svgRect.left - hostRect.left + margins.left + (xScale(point.x) ?? 0);
          const anchorY = svgRect.top - hostRect.top + margins.top + yScale(highestPoint.y);
          this._currentTooltipDataPoint = { legend: series.legend, ...point };
          const entries = matchingPoints.map(matchingPoint => ({
            legend: matchingPoint.legend,
            yValue: matchingPoint.yLabel,
            color: matchingPoint.color,
          }));
          this.tooltipProps = {
            isVisible: true,
            legend: series.legend,
            xValue: point.xLabel,
            yValue: point.yLabel,
            color: series.color,
            xPos: anchorX,
            yPos: anchorY,
            entries,
          };
          this._positionTooltipAvoidingOverlap(
            anchorX,
            anchorY - highestPoint.markerRadius,
            anchorY + highestPoint.markerRadius,
            true,
            { horizontalPlacement: 'side', gap: 15 },
          );
        };
        circle.addEventListener('mouseenter', showTooltip);
        circle.addEventListener('focus', showTooltip);
        circle.addEventListener('mouseleave', clearHoverState);
        circle.addEventListener('blur', clearHoverState);
        circle.addEventListener('click', () =>
          this._focusRovingElement(
            renderedPoints.map(renderedPoint => renderedPoint.circle),
            circle,
          ),
        );
        circle.addEventListener('keydown', pointKeydown);
        plotGroup.appendChild(circle);
      });
    });

    this._relocateFocusIfNeeded(renderedPoints.map(point => point.circle));

    renderBottomAxisShared({
      svg,
      scale: xScale as AxisScaleLike<AxisDomain>,
      axis: xAxis as unknown as Axis<AxisDomain>,
      formatter: xFormatter,
      axisLeft: margins.left,
      axisTop: margins.top,
      innerWidth,
      innerHeight,
      tickPadding: toNumber(this.tickPadding, 6),
      isRTL: this._isRTL,
      rotateXAxisLabels: this.rotateXAxisLabels,
      wrapXAxisLabels: this.wrapXAxisLabels,
      wrapLabelWidth: 48,
      hideTickOverlap: this.hideTickOverlap,
      showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
      axisLabelTooltipHandlers: {
        show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
        hide: () => this._hideAxisLabelTooltip(),
      },
      xAxisTitle: this.xAxisTitle,
    });
    renderPrimaryYAxisShared({
      svg,
      scale: yScale as AxisScaleLike<number>,
      axis: yAxis as unknown as Axis<number>,
      formatter: value =>
        this.customYAxisTickFormatter?.(Number(value)) ??
        (this.yAxisTickFormat
          ? formatNumberValue(Number(value), this.yAxisTickFormat, this.culture)
          : defaultYAxisTickFormatter(Number(value))),
      axisStartX: margins.left,
      axisTop: margins.top,
      innerHeight,
      innerWidth,
      tickPadding: toNumber(this.tickPadding, 6),
      isRTL: this._isRTL,
      yAxisTitle: this.yAxisTitle,
    });

    this._renderAnnotations({
      svg,
      collisionLayer: plotGroup,
      margins,
      innerWidth,
      innerHeight,
      mapDataX: value => xScale(normalizeXValue(value as XValue)) ?? 0,
      mapDataY: value => yScale(Number(value)),
    });

    this.chartContainer.appendChild(svg);
    this.legends = normalizedSeries.map(series => ({ legend: series.legend, color: series.color }));
    this._updateLegendInteractionState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this.chartContainer) {
      return;
    }
    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;
    this.chartContainer.querySelectorAll<SVGElement>('.scatter-point').forEach(element => {
      const legend = element.dataset.legend ?? '';
      const isActive = !hasSelection || highlighted.includes(legend);
      element.classList.toggle('inactive', !isActive);
      element.setAttribute('opacity', isActive ? '1' : '0.1');
    });
  }

  protected override _getHostAriaLabel(): string {
    const seriesCount = Array.isArray(this.data) ? this.data.length : 0;
    if (seriesCount === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Scatter chart with no data.';
    }
    return `${this.chartTitle || 'Scatter chart'}. ${seriesCount} series.`;
  }

  private _clearChart(): void {
    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }
}
