import { attr } from '@microsoft/fast-element';
import { extent, max } from 'd3-array';
import { type Axis, axisBottom, axisLeft, axisRight } from 'd3-axis';
import { format } from 'd3-format';
import {
  type ScaleBand,
  scaleBand,
  type ScaleLinear,
  scaleLinear,
  type ScaleLogarithmic,
  scaleLog,
  scaleTime,
  type ScaleTime,
} from 'd3-scale';
import { line as createLine } from 'd3-shape';
import { timeFormat, utcFormat } from 'd3-time-format';
import type { TooltipProps } from '../utils/chart-options.js';
import { appendVerticalGradient, resolveBarWidth, resolveChartColor } from '../utils/bar-chart-helpers.js';
import {
  applyAxisTickConfig,
  computePreparedNumericYAxis,
  createPreparedNumericContinuousScale,
  DEFAULT_NUMERIC_Y_TICK_COUNT,
  renderAxisGridLinesShared,
  renderBottomAxisShared,
  renderPrimaryYAxisShared,
  renderSecondaryYAxisShared,
  sortCategoryGroups,
  toAxisNumber as toNumber,
  toOptionalAxisNumber as toOptionalNumber,
} from '../utils/cartesian-axis-shared.js';
import {
  createNumberFormat,
  escapeHtml,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  parseDateOrNumber,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import { renderBorderedLinePath } from '../utils/line-path-helpers.js';
import type { VerticalBarChartDataPoint } from './vertical-bar-chart.options.js';
import { VerticalBarChartBase } from '../utils/vertical-bar-chart-base.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

export type TooltipEntry = { legend: string; color: string; value: string };
type TooltipState = TooltipProps & { xValue: string; yValue: string; entries: TooltipEntry[] };

const defaultMargins = { top: 40, right: 20, bottom: 50, left: 60 };
const defaultBarWidth = 16;
const defaultCategoricalInnerPadding = 2 / 3;

const clampScalePadding = (value: number): number => {
  return Math.max(0, Math.min(value, 1));
};

const calcTotalBandUnits = (numBands: number, innerPadding: number): number => {
  if (numBands <= 0) {
    return 0;
  }
  const gapToBandRatio = innerPadding / (1 - innerPadding);
  return numBands + (numBands - 1) * gapToBandRatio;
};

const calcRequiredCategoricalWidth = (bandwidth: number, numBands: number, innerPadding: number): number => {
  return bandwidth * calcTotalBandUnits(numBands, innerPadding);
};

const formatCompactNumber = (value: number, culture: string | undefined): string => {
  return createNumberFormat(culture || undefined, {
    maximumFractionDigits: Math.abs(value) >= 1000 ? 1 : 2,
    notation: Math.abs(value) >= 1000 ? 'compact' : 'standard',
  }).format(value);
};

const formatAxisNumber = (value: number, specifier: string | undefined, culture: string | undefined): string => {
  if (specifier) {
    try {
      return format(specifier)(value);
    } catch {
      // Fall back to locale formatting below.
    }
  }
  return formatCompactNumber(value, culture);
};

const formatDateValue = (chart: VerticalBarChart, value: Date): string => {
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
  const options = chart.dateLocalizeOptions ?? { year: 'numeric', month: '2-digit', day: '2-digit' };
  try {
    return new Intl.DateTimeFormat(chart.culture, options).format(value);
  } catch {
    return new Intl.DateTimeFormat(undefined, options).format(value);
  }
};

const formatXAxisCalloutValue = (
  chart: VerticalBarChart,
  value: VerticalBarChartDataPoint['xAxisCalloutData'],
  fallback: string,
): string => {
  if (Object.prototype.toString.call(value) === '[object Date]') {
    return formatDateValue(chart, new Date((value as Date).getTime()));
  }
  return typeof value === 'string' && value ? value : fallback;
};

const getNormalizedXValue = (value: VerticalBarChartDataPoint['x']): number | Date | undefined => {
  if (value instanceof Date) {
    return value;
  }
  if (typeof value === 'number' && Number.isFinite(value)) {
    return value;
  }
  if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}(?:T|$)/.test(value)) {
    const parsedDate = new Date(value);
    return Number.isNaN(parsedDate.getTime()) ? undefined : parsedDate;
  }
  return undefined;
};

/** @public */
export class VerticalBarChart extends VerticalBarChartBase {
  public declare tooltipProps: TooltipState;

  private _activeLineMarkerXValue: string | null = null;
  private _renderedBars: Array<{ legend: string; element: SVGRectElement }> = [];

  @attr({ converter: jsonConverter })
  public data!: VerticalBarChartDataPoint[];

  @attr({ attribute: 'line-legend-text' })
  public lineLegendText?: string;

  @attr({ attribute: 'line-legend-color' })
  public lineLegendColor?: string;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'lineLegendText', 'lineLegendColor'] as const;
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

    this.tooltipProps = { ...this.tooltipProps, xValue: '', yValue: '', entries: [] } as TooltipState;
    this._requestRender();
  }

  protected dataChanged(): void {
    this._requestRender();
  }

  protected lineLegendTextChanged(): void {
    this._requestRender();
  }

  protected lineLegendColorChanged(): void {
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
    this._activeLineMarkerXValue = null;
    this._syncLineMarkerVisibility();
  }

  protected override _buildDefaultTooltipHTML(): string {
    const state = this.tooltipProps as TooltipState;
    const header = `<div class="tooltip-header">${escapeHtml(state.xValue || state.yValue)}</div>`;
    const entries = (state.entries ?? [])
      .map(entry => {
        return [
          `<div class="tooltip-info" style="border-color: ${escapeHtml(entry.color)};">`,
          `<div class="tooltip-legend-text">${escapeHtml(entry.legend)}</div>`,
          `<div class="tooltip-primary-value" style="color: ${escapeHtml(entry.color)};">${escapeHtml(
            entry.value,
          )}</div>`,
          `</div>`,
        ].join('');
      })
      .join('');
    return header + entries;
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();

    const points = Array.isArray(this.data) ? this.data : [];
    if (points.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const hasSecondaryY = points.some(point => point.lineData?.useSecondaryYScale);
    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 500);
    const height = toNumber(this.height, 300);
    const { svg, plotGroup, margins, innerWidth, innerHeight } = this._createCartesianRenderContext({
      width,
      height,
      defaultMargins,
      hasSecondaryYAxis: hasSecondaryY,
    });
    const allNumericX = points.every(point => typeof point.x === 'number' && Number.isFinite(point.x));
    const isDateAxis = points.every(point => getNormalizedXValue(point.x) instanceof Date);

    let xScaleBand: ScaleBand<string> | undefined;
    let xScaleLinear: ScaleLinear<number, number> | ScaleLogarithmic<number, number> | undefined;
    let xScaleTime: ScaleTime<number, number> | undefined;
    let xAxis: Axis<string | number | Date>;
    let barAutoWidth = defaultBarWidth;
    const xRange: [number, number] = this._isRTL ? [innerWidth, 0] : [0, innerWidth];

    const getXCenter = (point: VerticalBarChartDataPoint): number => {
      if (xScaleTime) {
        return xScaleTime(point.x as Date) ?? 0;
      }
      if (xScaleLinear) {
        return xScaleLinear(point.x as number);
      }
      return (xScaleBand!(String(point.x)) ?? 0) + xScaleBand!.bandwidth() / 2;
    };

    if (allNumericX) {
      const numericXValues = points.map(point => point.x as number);
      const minX = Math.min(...numericXValues);
      const maxX = Math.max(...numericXValues);
      const span = maxX - minX || 1;
      const domainPadding = Math.max(Math.abs(span) * 0.05, 0.5);
      const domainMin = minX === maxX ? minX - 0.5 : minX - domainPadding;
      const domainMax = minX === maxX ? maxX + 0.5 : maxX + domainPadding;

      const useLogX = this.xScaleType === 'log' && numericXValues.every(value => value > 0);
      xScaleLinear = useLogX
        ? scaleLog()
            .domain([Math.max(minX / 1.05, Number.MIN_VALUE), maxX === minX ? maxX * 1.05 : maxX * 1.05])
            .range(xRange)
        : scaleLinear().domain([domainMin, domainMax]).range(xRange);
      if (this.roundedTicks) {
        xScaleLinear.nice();
      }

      xAxis = axisBottom(xScaleLinear).tickPadding(toNumber(this.tickPadding, 6)) as unknown as Axis<
        string | number | Date
      >;
      applyAxisTickConfig(
        xAxis as unknown as Axis<number>,
        this.xAxisTickCount,
        this.tickValues?.map(value => Number(value)),
      );

      const sortedUniqueX = [...new Set(numericXValues)].sort((left, right) => left - right);
      if (sortedUniqueX.length > 1) {
        const minGap = sortedUniqueX.slice(1).reduce((currentMin, value, index) => {
          return Math.min(currentMin, value - sortedUniqueX[index]);
        }, Number.POSITIVE_INFINITY);
        const minPxGap = Math.abs(xScaleLinear(sortedUniqueX[0] + minGap) - xScaleLinear(sortedUniqueX[0]));
        barAutoWidth = Math.max(8, Math.min(24, minPxGap * 0.5));
      }
    } else if (isDateAxis) {
      const dateValues = points.map(point => getNormalizedXValue(point.x) as Date);
      const rawExtent = extent(dateValues, value => value.getTime());
      const xMin =
        this.xMinValue !== undefined
          ? (parseDateOrNumber(this.xMinValue as string | number) as Date | number)
          : new Date(rawExtent[0] ?? 0);
      const xMax =
        this.xMaxValue !== undefined
          ? (parseDateOrNumber(this.xMaxValue as string | number) as Date | number)
          : new Date(rawExtent[1] ?? 0);
      const domainMin = xMin instanceof Date ? xMin : new Date(Number(xMin));
      const domainMax = xMax instanceof Date ? xMax : new Date(Number(xMax));

      xScaleTime = scaleTime().domain([domainMin, domainMax]).range(xRange);
      if (this.roundedTicks) {
        xScaleTime.nice();
      }

      xAxis = axisBottom(xScaleTime).tickPadding(toNumber(this.tickPadding, 6)) as unknown as Axis<
        string | number | Date
      >;
      const parsedTickValues = (this.tickValues ?? [])
        .map(value => {
          if (value instanceof Date) {
            return value;
          }
          if (typeof value === 'string') {
            const parsedDate = new Date(value);
            return Number.isNaN(parsedDate.getTime()) ? undefined : parsedDate;
          }
          return undefined;
        })
        .filter((value): value is Date => value !== undefined);
      applyAxisTickConfig(xAxis as unknown as Axis<Date>, this.xAxisTickCount, parsedTickValues);
    } else {
      const groupsByCategory = new Map<string, VerticalBarChartDataPoint[]>();
      points.forEach(point => {
        const key = String(point.x);
        groupsByCategory.set(key, [...(groupsByCategory.get(key) ?? []), point]);
      });
      const xDomain = sortCategoryGroups(
        Array.from(groupsByCategory.entries()).map(([key, groupedPoints]) => ({ key, points: groupedPoints })),
        this.xAxisCategoryOrder,
        points.map(point => String(point.x)),
        group => group.points.map(point => point.y),
      ).map(group => group.key);

      const explicitInnerPadding = toOptionalNumber(this.xAxisInnerPadding);
      const explicitOuterPadding = toOptionalNumber(this.xAxisOuterPadding);
      const xAxisInnerPadding = clampScalePadding(explicitInnerPadding ?? defaultCategoricalInnerPadding);
      const xAxisOuterPadding = clampScalePadding(explicitOuterPadding ?? 0);

      let xRangeStart = 0;
      let xRangeEnd = innerWidth;

      // Match React behavior for string-axis bars: when bar width is fixed and
      // outer padding is not explicitly set, center the rendered bars.
      const requestedBarWidth = toOptionalNumber(this.barWidth);
      if (
        explicitOuterPadding === undefined &&
        requestedBarWidth !== undefined &&
        requestedBarWidth > 0 &&
        xDomain.length > 0 &&
        xAxisInnerPadding < 1
      ) {
        const requiredWidth = calcRequiredCategoricalWidth(requestedBarWidth, xDomain.length, xAxisInnerPadding);
        if (innerWidth > requiredWidth) {
          const sideMargin = (innerWidth - requiredWidth) / 2;
          xRangeStart = sideMargin;
          xRangeEnd = innerWidth - sideMargin;
        }
      }

      xScaleBand = scaleBand<string>()
        .domain(xDomain)
        .range(this._isRTL ? [xRangeEnd, xRangeStart] : [xRangeStart, xRangeEnd])
        .paddingInner(xAxisInnerPadding)
        .paddingOuter(xAxisOuterPadding);
      xAxis = axisBottom(xScaleBand).tickPadding(toNumber(this.tickPadding, 6)) as unknown as Axis<
        string | number | Date
      >;
      applyAxisTickConfig(
        xAxis as unknown as Axis<string>,
        this.xAxisTickCount,
        this.tickValues?.map(value => String(value)),
      );
    }

    const primaryValues = [
      ...points.map(point => point.y),
      ...points.flatMap(point => (point.lineData && !point.lineData.useSecondaryYScale ? [point.lineData.y] : [])),
    ];
    const maxY = max(primaryValues) ?? 0;
    const minY = Math.min(...primaryValues);
    const useLogPrimary = this.yScaleType === 'log' && primaryValues.every(value => value > 0);
    const resolvedMinValue = toOptionalNumber(this.yMinValue) ?? (this.supportNegativeData ? Math.min(minY, 0) : 0);
    const resolvedMaxValue =
      toOptionalNumber(this.yMaxValue) ?? (this.supportNegativeData && maxY <= 0 ? minY : Math.max(maxY, 1));
    const preparedYAxis = computePreparedNumericYAxis({
      minValue: resolvedMinValue,
      maxValue: resolvedMaxValue,
      tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
      roundedTicks: this.roundedTicks,
    });
    const yScale: ScaleLinear<number, number> | ScaleLogarithmic<number, number> = useLogPrimary
      ? scaleLog()
          .domain([Math.max(minY / 10, Number.MIN_VALUE), maxY])
          .range([innerHeight, 0])
      : scaleLinear().domain([preparedYAxis.domainMin, preparedYAxis.domainMax]).range([innerHeight, 0]);

    const secondaryLineValues = points
      .flatMap(point => (point.lineData?.useSecondaryYScale ? [point.lineData.y] : []))
      .filter((value): value is number => typeof value === 'number' && Number.isFinite(value));
    let preparedSecondaryYAxis = preparedYAxis;
    let yScaleSecondary: ScaleLinear<number, number> | ScaleLogarithmic<number, number> = yScale;
    let useLogSecondary = false;
    if (hasSecondaryY) {
      const secondaryYAxis = createPreparedNumericContinuousScale({
        values: secondaryLineValues,
        range: [innerHeight, 0],
        scaleType: this.secondaryYScaleType,
        tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
        roundedTicks: this.roundedTicks,
      });
      preparedSecondaryYAxis = secondaryYAxis.preparedAxis;
      yScaleSecondary = secondaryYAxis.scale;
      useLogSecondary = secondaryYAxis.isLogarithmic;
    }

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const yAxis = axisLeft(yScale).tickPadding(toNumber(this.tickPadding, 6));
    applyAxisTickConfig(
      yAxis,
      this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
      this.yAxisTickValues ?? (useLogPrimary ? undefined : preparedYAxis.tickValues),
    );
    renderAxisGridLinesShared({
      layer: plotGroup,
      orientation: 'horizontal',
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      spanStart: 0,
      spanEnd: innerWidth,
    });

    const singleColor = this.useSingleColor ? resolveChartColor(points[0].color, this.colors, 0) : undefined;
    const cornerRadius = this.roundCorners ? 3 : 0;

    const barLegendMap = new Map<string, string>();
    this._renderedBars = [];

    const getLineScale = (
      point: VerticalBarChartDataPoint,
    ): ScaleLinear<number, number> | ScaleLogarithmic<number, number> => {
      return point.lineData?.useSecondaryYScale ? yScaleSecondary : yScale;
    };

    const showBarTooltip = (
      point: VerticalBarChartDataPoint,
      legend: string,
      tooltipLegend: string,
      xValueLabel: string,
      xCenter: number,
      color: string,
      barTop: number,
      barBottom: number,
      clientY?: number,
    ): void => {
      if (!this._shouldShowTooltip(legend) || this.hideTooltip) {
        return;
      }
      this._activeLineMarkerXValue = String(point.x);
      this._syncLineMarkerVisibility();
      const hostRect = this.getBoundingClientRect();
      const svgRect = svg.getBoundingClientRect();
      const anchorX = svgRect.left - hostRect.left + margins.left + xCenter;
      const minY = svgRect.top - hostRect.top + margins.top + barTop;
      const maxY = svgRect.top - hostRect.top + margins.top + barBottom;
      const fallbackY = (minY + maxY) / 2;
      const anchorY = clientY === undefined ? fallbackY : Math.min(Math.max(clientY - hostRect.top, minY), maxY);
      const isFreshShow = !this.tooltipProps.isVisible;
      this._currentTooltipDataPoint = point;
      const lineLegend = this.lineLegendText || 'Line';
      const lineColor = this.lineLegendColor
        ? getColorFromToken(this.lineLegendColor)
        : resolveChartColor(undefined, this.colors, 0, 10);
      const barCalloutValue = point.yAxisCalloutData || formatAxisNumber(point.y, this.yAxisTickFormat, this.culture);
      const entries: TooltipEntry[] = [
        {
          legend: tooltipLegend,
          color,
          value: barCalloutValue,
        },
      ];
      if (point.lineData && typeof point.lineData.y === 'number' && this._shouldShowTooltip(lineLegend)) {
        entries.unshift({
          legend: lineLegend,
          color: lineColor,
          value:
            point.lineData.yAxisCalloutData || formatAxisNumber(point.lineData.y, this.yAxisTickFormat, this.culture),
        });
      }
      this.tooltipProps = {
        isVisible: true,
        legend,
        xValue: formatXAxisCalloutValue(this, point.xAxisCalloutData, xValueLabel),
        yValue: barCalloutValue,
        color,
        xPos: anchorX,
        yPos: anchorY,
        entries,
      };
      this._positionTooltipAvoidingOverlap(anchorX, minY, maxY, isFreshShow);
    };

    points.forEach((point, index) => {
      const key = String(point.x);
      const legend = point.legend ?? key;
      const tooltipLegend = point.legend ?? '';
      const xValueLabel = isDateAxis ? formatDateValue(this, point.x as Date) : key;
      const actualWidth = resolveBarWidth(
        this.barWidth,
        this.maxBarWidth,
        xScaleBand ? xScaleBand.bandwidth() : barAutoWidth,
      );
      const xCenter = getXCenter(point);
      const x = xCenter - actualWidth / 2;
      const color = singleColor ?? resolveChartColor(point.color, this.colors, index);
      barLegendMap.set(legend, color);

      const isNegativeBar = point.y < 0;
      const yValue = yScale(point.y);
      const baselineY = useLogPrimary ? innerHeight : yScale(0);
      const barHeight = Math.max(Math.abs(yValue - baselineY), 0);
      const barTop = Math.min(yValue, baselineY);
      const barBottom = Math.max(yValue, baselineY);
      const shouldRenderBar = !isNegativeBar || this.supportNegativeData;
      if (!shouldRenderBar || barHeight <= 0) {
        return;
      }

      const rect = createSvgElement<SVGRectElement>('rect');
      rect.classList.add('bar');
      rect.dataset.legend = legend;
      rect.setAttribute('x', String(x));
      rect.setAttribute('y', String(isNegativeBar ? baselineY : yValue));
      rect.setAttribute('width', String(actualWidth));
      rect.setAttribute('height', String(barHeight));
      const gradientId = appendVerticalGradient(
        defs,
        `vbc-gradient-${index}`,
        color,
        this.enableGradient,
        point.gradient,
      );
      rect.setAttribute('fill', gradientId ? `url(#${gradientId})` : color);
      rect.setAttribute('rx', String(cornerRadius));
      rect.setAttribute('ry', String(cornerRadius));
      rect.setAttribute('role', 'img');
      rect.setAttribute('tabindex', this._renderedBars.length === 0 ? '0' : '-1');
      rect.setAttribute(
        'aria-label',
        point.callOutAccessibilityData?.ariaLabel ?? `${xValueLabel}. ${legend ? `${legend}, ` : ''}${point.y}.`,
      );
      if (this.strokeWidth !== undefined) {
        rect.setAttribute('stroke-width', String(this.strokeWidth));
        rect.setAttribute('stroke', color);
      }
      rect.addEventListener('mouseenter', event =>
        showBarTooltip(point, legend, tooltipLegend, xValueLabel, xCenter, color, barTop, barBottom, event.clientY),
      );
      rect.addEventListener('mousemove', event =>
        showBarTooltip(point, legend, tooltipLegend, xValueLabel, xCenter, color, barTop, barBottom, event.clientY),
      );
      rect.addEventListener('mouseleave', () => this._clearTooltip());
      rect.addEventListener('focus', () =>
        showBarTooltip(point, legend, tooltipLegend, xValueLabel, xCenter, color, barTop, barBottom),
      );
      rect.addEventListener('blur', () => this._clearTooltip());
      rect.addEventListener('click', () => {
        this._focusRovingElement(
          this._renderedBars.map(bar => bar.element),
          rect,
        );
        point.onClick?.();
      });
      rect.addEventListener('keydown', (e: KeyboardEvent) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault();
          point.onClick?.();
        } else {
          this._rovingKeydown(
            this._renderedBars.map(bar => bar.element),
            e,
          );
        }
      });
      plotGroup.appendChild(rect);
      this._renderedBars.push({ legend, element: rect });

      const shouldShowLabel =
        !this.hideLabels &&
        actualWidth >= 16 &&
        barHeight > 0 &&
        (this._shouldShowTooltip(legend) || this._getHighlightedLegends().length === 0);

      if (shouldShowLabel) {
        const label = createSvgElement<SVGTextElement>('text');
        label.classList.add('bar-label');
        label.setAttribute('x', String(xCenter));
        label.setAttribute('y', String(isNegativeBar ? yValue + 12 : yValue - 6));
        label.setAttribute('text-anchor', 'middle');
        label.textContent = point.barLabel ?? formatAxisNumber(point.y, this.yAxisTickFormat, this.culture);
        plotGroup.appendChild(label);
      }
    });

    const linePoints = points.filter(point => point.lineData && typeof point.lineData.y === 'number');
    if (linePoints.length > 0) {
      const lineLegend = this.lineLegendText || 'Line';
      const lineColor = this.lineLegendColor
        ? getColorFromToken(this.lineLegendColor)
        : resolveChartColor(undefined, this.colors, 0, 10);
      // Keep line legend first to match React VerticalBarChart legend ordering.
      this.legends = [
        { legend: lineLegend, color: lineColor, isLineLegendInBarChart: true },
        ...Array.from(barLegendMap.entries()).map(([legend, color]) => ({ legend, color })),
      ];

      const resolvedLineStrokeWidth =
        this.lineStrokeWidth !== undefined ? Number.parseFloat(this.lineStrokeWidth.toString()) : 3;
      const lineBorderWidth =
        this.lineBorderWidth !== undefined ? Number.parseFloat(this.lineBorderWidth.toString()) : 0;
      const lineBorderColor = this.lineBorderColor || 'var(--colorNeutralBackground1, #fff)';
      const lineStrokeLinecap = this.lineStrokeLinecap || 'square';
      const lineGenerator = createLine<VerticalBarChartDataPoint>()
        .x(point => getXCenter(point))
        .y(point => getLineScale(point)(point.lineData?.y ?? 0));
      const linePathData = lineGenerator(linePoints) ?? '';

      const showLineTooltip = (point: VerticalBarChartDataPoint, event?: MouseEvent): void => {
        if (!this._shouldShowTooltip(lineLegend) || this.hideTooltip || !point.lineData) {
          return;
        }

        const hostRect = this.getBoundingClientRect();
        const svgRect = svg.getBoundingClientRect();
        const anchorX = svgRect.left - hostRect.left + margins.left + getXCenter(point);
        const pointY = svgRect.top - hostRect.top + margins.top + getLineScale(point)(point.lineData.y);
        const anchorY = event ? Math.min(Math.max(event.clientY - hostRect.top, pointY - 10), pointY + 10) : pointY;
        const isFreshShow = !this.tooltipProps.isVisible;
        const xValueLabel = point.x instanceof Date ? formatDateValue(this, point.x) : String(point.x);
        const lineValue =
          point.lineData.yAxisCalloutData || formatAxisNumber(point.lineData.y, this.yAxisTickFormat, this.culture);
        const entries: TooltipEntry[] = [{ legend: lineLegend, color: lineColor, value: lineValue }];
        if (this._shouldShowTooltip(point.legend ?? String(point.x))) {
          const barColor = barLegendMap.get(point.legend ?? String(point.x)) ?? getNextColor(0, 0);
          entries.push({
            legend: point.legend ?? '',
            color: barColor,
            value: point.yAxisCalloutData || formatAxisNumber(point.y, this.yAxisTickFormat, this.culture),
          });
        }

        this._activeLineMarkerXValue = String(point.x);
        this._syncLineMarkerVisibility();
        this._currentTooltipDataPoint = { ...point.lineData, x: point.x };
        this.tooltipProps = {
          isVisible: true,
          legend: lineLegend,
          xValue: formatXAxisCalloutValue(this, point.xAxisCalloutData, xValueLabel),
          yValue: lineValue,
          color: lineColor,
          xPos: anchorX,
          yPos: anchorY,
          entries,
        };
        this._positionTooltipAvoidingOverlap(anchorX, anchorY, anchorY, isFreshShow);
      };

      const { linePath } = renderBorderedLinePath({
        layer: plotGroup,
        pathData: linePathData,
        legend: lineLegend,
        color: lineColor,
        strokeWidth: resolvedLineStrokeWidth,
        borderWidth: lineBorderWidth,
        borderColor: lineBorderColor,
        strokeLinecap: lineStrokeLinecap,
        strokeDasharray: this.lineStrokeDasharray,
        strokeDashoffset: this.lineStrokeDashoffset,
      });

      const lineHitArea = createSvgElement<SVGPathElement>('path');
      lineHitArea.classList.add('line-hit-area');
      lineHitArea.dataset.legend = lineLegend;
      lineHitArea.setAttribute('fill', 'none');
      lineHitArea.setAttribute('stroke', 'transparent');
      lineHitArea.setAttribute('stroke-width', '16');
      lineHitArea.setAttribute('d', linePathData);
      const showNearestLineTooltip = (event: MouseEvent): void => {
        const svgRect = svg.getBoundingClientRect();
        const localX = event.clientX - svgRect.left - margins.left;
        const nearestPoint = linePoints.reduce((nearest, point) =>
          Math.abs(getXCenter(point) - localX) < Math.abs(getXCenter(nearest) - localX) ? point : nearest,
        );
        showLineTooltip(nearestPoint, event);
      };
      lineHitArea.addEventListener('mouseenter', showNearestLineTooltip);
      lineHitArea.addEventListener('mousemove', showNearestLineTooltip);
      lineHitArea.addEventListener('mouseleave', () => this._clearTooltip());
      plotGroup.appendChild(lineHitArea);

      linePoints.forEach((point, index) => {
        const marker = createSvgElement<SVGCircleElement>('circle');
        marker.classList.add('line-marker');
        marker.dataset.legend = lineLegend;
        marker.dataset.xValue = String(point.x);
        marker.setAttribute('cx', String(getXCenter(point)));
        marker.setAttribute('cy', String(getLineScale(point)(point.lineData?.y ?? 0)));
        marker.setAttribute('r', '0');
        marker.setAttribute('visibility', 'hidden');
        marker.setAttribute('fill', '#fff');
        marker.setAttribute('stroke', lineColor);
        marker.setAttribute('stroke-width', '2');
        marker.setAttribute('pointer-events', 'none');
        marker.addEventListener('click', () => point.lineData?.onClick?.());
        marker.id = `vbc-line-marker-${index}`;
        plotGroup.appendChild(marker);

        const markerHitArea = createSvgElement<SVGCircleElement>('circle');
        markerHitArea.classList.add('line-marker-hit-area');
        markerHitArea.dataset.legend = lineLegend;
        markerHitArea.setAttribute('cx', String(getXCenter(point)));
        markerHitArea.setAttribute('cy', String(getLineScale(point)(point.lineData?.y ?? 0)));
        markerHitArea.setAttribute('r', '10');
        markerHitArea.setAttribute('fill', 'transparent');
        markerHitArea.setAttribute('role', 'img');
        markerHitArea.setAttribute(
          'aria-label',
          `${lineLegend}: ${point.lineData?.yAxisCalloutData || point.lineData?.y}`,
        );
        markerHitArea.addEventListener('mouseenter', event => showLineTooltip(point, event));
        markerHitArea.addEventListener('mousemove', event => showLineTooltip(point, event));
        markerHitArea.addEventListener('mouseleave', () => this._clearTooltip());
        markerHitArea.addEventListener('click', event => {
          showLineTooltip(point, event);
          point.lineData?.onClick?.();
        });
        plotGroup.appendChild(markerHitArea);
      });
    }

    if (xScaleBand) {
      renderBottomAxisShared({
        svg,
        scale: xScaleBand,
        axis: xAxis as Axis<string>,
        formatter: value => String(value),
        axisLeft: margins.left,
        axisTop: margins.top,
        innerWidth,
        innerHeight,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: this._isRTL,
        rotateXAxisLabels: this.rotateXAxisLabels,
        wrapXAxisLabels: this.wrapXAxisLabels,
        wrapLabelWidth: this.wrapXAxisLabels ? Math.max(xScaleBand.step() - 2, 1) : undefined,
        hideTickOverlap: this.hideTickOverlap,
        showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
        noOfCharsToTruncate: toNumber(this.noOfCharsToTruncate, 4),
        axisLabelTooltipHandlers: {
          show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
          hide: () => this._hideAxisLabelTooltip(),
        },
        xAxisTitle: this.xAxisTitle,
      });
    } else if (xScaleTime) {
      renderBottomAxisShared({
        svg,
        scale: xScaleTime,
        axis: xAxis as Axis<Date>,
        formatter: value => formatDateValue(this, value as Date),
        axisLeft: margins.left,
        axisTop: margins.top,
        innerWidth,
        innerHeight,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: this._isRTL,
        rotateXAxisLabels: this.rotateXAxisLabels,
        wrapXAxisLabels: this.wrapXAxisLabels,
        hideTickOverlap: this.hideTickOverlap,
        showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
        noOfCharsToTruncate: toNumber(this.noOfCharsToTruncate, 4),
        axisLabelTooltipHandlers: {
          show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
          hide: () => this._hideAxisLabelTooltip(),
        },
        xAxisTitle: this.xAxisTitle,
      });
    } else {
      renderBottomAxisShared({
        svg,
        scale: xScaleLinear!,
        axis: xAxis as Axis<number>,
        formatter: value => String(value),
        axisLeft: margins.left,
        axisTop: margins.top,
        innerWidth,
        innerHeight,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: this._isRTL,
        rotateXAxisLabels: this.rotateXAxisLabels,
        wrapXAxisLabels: this.wrapXAxisLabels,
        hideTickOverlap: this.hideTickOverlap,
        showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
        noOfCharsToTruncate: toNumber(this.noOfCharsToTruncate, 4),
        axisLabelTooltipHandlers: {
          show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
          hide: () => this._hideAxisLabelTooltip(),
        },
        xAxisTitle: this.xAxisTitle,
      });
    }
    renderPrimaryYAxisShared({
      svg,
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      formatter: value => formatAxisNumber(value, this.yAxisTickFormat, this.culture).toLowerCase(),
      axisStartX: margins.left,
      axisTop: margins.top,
      innerHeight,
      innerWidth,
      tickPadding: toNumber(this.tickPadding, 6),
      isRTL: this._isRTL,
      yAxisTitle: this.yAxisTitle,
    });

    if (hasSecondaryY) {
      const yAxisSecondary = axisRight(yScaleSecondary).tickPadding(toNumber(this.tickPadding, 6));
      applyAxisTickConfig(
        yAxisSecondary,
        this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
        this.yAxisTickValues ?? (useLogSecondary ? undefined : preparedSecondaryYAxis.tickValues),
      );
      renderSecondaryYAxisShared({
        svg,
        scale: yScaleSecondary,
        axis: yAxisSecondary as unknown as Axis<number>,
        formatter: value => formatAxisNumber(value, this.yAxisTickFormat, this.culture).toLowerCase(),
        axisStartX: margins.left,
        axisTop: margins.top,
        innerHeight,
        innerWidth,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: this._isRTL,
        yAxisTitle: this.secondaryYAxisTitle,
      });
    }

    this._renderAnnotations({
      svg,
      collisionLayer: plotGroup,
      margins,
      innerWidth,
      innerHeight,
      mapDataX: value => {
        if (xScaleTime) return xScaleTime(new Date(value));
        if (xScaleLinear) return xScaleLinear(Number(value));
        const bandX = xScaleBand?.(String(value));
        return bandX === undefined || !xScaleBand ? undefined : bandX + xScaleBand.bandwidth() / 2;
      },
      mapDataY: (value, axis) => (axis === 'secondary' ? yScaleSecondary : yScale)(Number(value)),
    });

    this.chartContainer.appendChild(svg);
    if (linePoints.length === 0) {
      this.legends = Array.from(barLegendMap.entries()).map(([legend, color]) => ({ legend, color }));
    }
    this._updateLegendInteractionState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this.chartContainer) {
      return;
    }
    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;
    this.chartContainer.querySelectorAll<SVGElement>('.bar, .line-path, .line-marker').forEach(element => {
      const legend = element.dataset.legend ?? '';
      const isActive = !hasSelection || highlighted.includes(legend);
      element.classList.toggle('inactive', !isActive);
      element.setAttribute('opacity', isActive ? '1' : '0.1');
    });

    this._renderedBars.forEach(({ legend, element }) => {
      const isActive = !hasSelection || highlighted.includes(legend);
      if (!isActive) {
        element.tabIndex = -1;
      }
    });

    const activeBars = this._renderedBars
      .filter(({ legend }) => !hasSelection || highlighted.includes(legend))
      .map(bar => bar.element);
    if (activeBars.length > 0 && !activeBars.some(el => el.tabIndex === 0)) {
      activeBars[0].tabIndex = 0;
    }
    this._relocateFocusIfNeeded(this._renderedBars.map(bar => bar.element));
    this._syncLineMarkerVisibility();
  }

  protected override _getHostAriaLabel(): string {
    const count = Array.isArray(this.data) ? this.data.length : 0;
    if (count === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Vertical bar chart with no data.';
    }
    return `${this.chartTitle || 'Vertical bar chart'}. ${count} bars.`;
  }

  private _clearChart(): void {
    this._renderedBars = [];
    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }

  private _syncLineMarkerVisibility(): void {
    if (!this.chartContainer) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;

    this.chartContainer.querySelectorAll<SVGCircleElement>('.line-marker').forEach(marker => {
      const markerLegend = marker.dataset.legend ?? '';
      const markerXValue = marker.dataset.xValue ?? '';
      const legendIsActive = !hasSelection || highlighted.includes(markerLegend);
      const shouldShow =
        legendIsActive && this._activeLineMarkerXValue !== null && markerXValue === this._activeLineMarkerXValue;

      marker.setAttribute('visibility', shouldShow ? 'visible' : 'hidden');
      marker.setAttribute('r', shouldShow ? '8' : '0');
    });
  }
}
