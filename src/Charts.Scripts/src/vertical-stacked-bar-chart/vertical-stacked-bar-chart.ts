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
  type ScaleTime,
  scaleTime,
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
  escapeHtml,
  formatLocaleNumber,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type {
  VerticalStackedBarChartDataPoint,
  VerticalStackedBarChartLineDataPoint,
  VerticalStackedBarChartProps,
} from './vertical-stacked-bar-chart.options.js';
import { VerticalBarChartBase } from '../utils/vertical-bar-chart-base.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

export type TooltipEntry = { legend: string; color: string; value: string };
type TooltipState = TooltipProps & { xValue: string; entries: TooltipEntry[] };
type LinePlotPoint = {
  xAxisPoint: string | number | Date;
  xCenter: number;
  entry: VerticalStackedBarChartLineDataPoint;
};

const defaultMargins = { top: 40, right: 20, bottom: 50, left: 60 };
const defaultCategoricalBarWidth = 16;

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

const formatDateValue = (chart: VerticalStackedBarChart, value: Date): string => {
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

const formatXAxisValue = (chart: VerticalStackedBarChart, value: string | number | Date): string =>
  value instanceof Date ? formatDateValue(chart, value) : String(value);

const formatYAxisTickValue = (chart: VerticalStackedBarChart, value: number): string =>
  chart.customYAxisTickFormatter?.(value) ?? formatNumberValue(value, chart.yAxisTickFormat, chart.culture);

const formatXAxisCalloutValue = (
  chart: VerticalStackedBarChart,
  value: VerticalStackedBarChartDataPoint['xAxisCalloutData'],
  fallback: string,
): string => {
  if (Object.prototype.toString.call(value) === '[object Date]') {
    return formatDateValue(chart, new Date((value as Date).getTime()));
  }
  return typeof value === 'string' && value ? value : fallback;
};

/** @public */
export class VerticalStackedBarChart extends VerticalBarChartBase {
  public declare tooltipProps: TooltipState;

  private _activeLineMarkerXValue: string | null = null;

  @attr({ converter: jsonConverter })
  public data!: VerticalStackedBarChartProps[];

  @attr({ attribute: 'bar-gap-max' })
  public barGapMax?: number | string;

  /** Shows all bar and line values for the hovered x-axis category in one tooltip. */
  @attr({ attribute: 'is-callout-for-stack', mode: 'boolean' })
  public isCalloutForStack: boolean = false;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'barGapMax', 'isCalloutForStack'] as const;
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

  /**
   * Shows/updates the tooltip for a bar segment, following the mouse's vertical position
   * within the segment (clamped to its bounds) instead of anchoring at a fixed edge —
   * matching React's behavior of the tooltip tracking the cursor as it moves through a bar.
   */
  private _showBarSegmentTooltipAtY(
    clientY: number,
    x: number,
    offset: number,
    actualWidth: number,
    top: number,
    bottom: number,
    margins: { top: number; bottom: number; left: number; right: number },
    svg: SVGSVGElement,
    content: { legend: string; xValue: string; yValue: string; color: string; entries: TooltipEntry[] },
  ): void {
    const isFreshShow = !this.tooltipProps.isVisible;
    const hostRect = this.getBoundingClientRect();
    const svgRect = svg.getBoundingClientRect();
    const anchorX = svgRect.left - hostRect.left + margins.left + x + offset + actualWidth / 2;
    const minY = svgRect.top - hostRect.top + margins.top + top;
    const maxY = svgRect.top - hostRect.top + margins.top + bottom;
    const rawY = clientY - hostRect.top;
    const anchorY = Math.min(Math.max(rawY, minY), maxY);

    this.tooltipProps = {
      isVisible: true,
      legend: content.legend,
      xValue: content.xValue,
      yValue: content.yValue,
      color: content.color,
      entries: content.entries,
      xPos: anchorX,
      yPos: anchorY,
    };
    this._positionTooltipAvoidingOverlap(anchorX, anchorY, anchorY, isFreshShow);
  }

  /**
   * Shows/updates the tooltip for a line point, following the mouse's vertical position
   * within `[cyMin, cyMax]` (plot-local, same space as `cy`) instead of anchoring at the
   * point's exact center. Defaults to a small window around the point itself.
   */
  private _showLinePointTooltipAtY(
    clientY: number,
    cx: number,
    cy: number,
    margins: { top: number; bottom: number; left: number; right: number },
    svg: SVGSVGElement,
    content: { legend: string; xValue: string; yValue: string; color: string; entries: TooltipEntry[] },
    cyMin: number = cy - 10,
    cyMax: number = cy + 10,
  ): void {
    const isFreshShow = !this.tooltipProps.isVisible;
    const hostRect = this.getBoundingClientRect();
    const svgRect = svg.getBoundingClientRect();
    const anchorX = svgRect.left - hostRect.left + margins.left + cx;
    const minY = svgRect.top - hostRect.top + margins.top + cyMin;
    const maxY = svgRect.top - hostRect.top + margins.top + cyMax;
    const rawY = clientY - hostRect.top;
    const anchorY = Math.min(Math.max(rawY, minY), maxY);

    this.tooltipProps = {
      isVisible: true,
      legend: content.legend,
      xValue: content.xValue,
      yValue: content.yValue,
      color: content.color,
      entries: content.entries,
      xPos: anchorX,
      yPos: anchorY,
    };
    this._positionTooltipAvoidingOverlap(anchorX, anchorY, anchorY, isFreshShow);
  }

  protected dataChanged(): void {
    this._requestRender();
  }

  protected barGapMaxChanged(): void {
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

  protected override tooltipPropsChanged(old: TooltipProps, newValue: TooltipProps): void {
    super.tooltipPropsChanged(old, newValue);
    if (newValue.isVisible && !this.hideTooltip) {
      const state = newValue as TooltipState;
      const stackAccessibilityLabel = this.isCalloutForStack
        ? (this._currentTooltipDataPoint as VerticalStackedBarChartProps | null)?.stackCallOutAccessibilityData
            ?.ariaLabel
        : undefined;
      this.liveRegionText =
        stackAccessibilityLabel ??
        [state.xValue, ...state.entries.map(entry => `${entry.legend}: ${entry.value}`)].filter(Boolean).join('. ');
    }
  }

  protected override _buildDefaultTooltipHTML(): string {
    const entries = this.tooltipProps.entries
      .map(
        entry =>
          `<div class="tooltip-info" style="border-color: ${escapeHtml(
            entry.color,
          )};"><div class="tooltip-legend-text">${escapeHtml(
            entry.legend,
          )}</div><div class="tooltip-primary-value" style="color: ${escapeHtml(entry.color)};">${escapeHtml(
            entry.value,
          )}</div></div>`,
      )
      .join('');
    return `<div class="tooltip-header">${escapeHtml(this.tooltipProps.xValue)}</div>${entries}`;
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();

    const stacks = Array.isArray(this.data) ? this.data : [];
    if (stacks.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const requestedChartWidth = toOptionalNumber(this.width);
    const measuredWidth = this.chartContainer.getBoundingClientRect().width;
    const width = requestedChartWidth ?? (measuredWidth || toNumber(this.width, 600));
    const height = toNumber(this.height, 350);
    const hasSecondaryY = stacks.some(stack => stack.lineData?.some(entry => entry.useSecondaryYScale));
    const { svg, plotGroup, margins, innerWidth, innerHeight } = this._createCartesianRenderContext({
      width,
      height,
      defaultMargins,
      hasSecondaryYAxis: hasSecondaryY,
    });
    const isDateAxis = stacks.every(
      stack => stack.xAxisPoint instanceof Date && !Number.isNaN(stack.xAxisPoint.getTime()),
    );
    let xScaleBand: ScaleBand<string> | undefined;
    let xScaleTime: ScaleTime<number, number> | undefined;
    let xAxis: Axis<string | Date>;
    const xRange: [number, number] = this._isRTL ? [innerWidth, 0] : [0, innerWidth];
    if (isDateAxis) {
      const dateValues = stacks.map(stack => stack.xAxisPoint as Date);
      const dateExtent = extent(dateValues, value => value.getTime());
      xScaleTime = scaleTime()
        .domain([new Date(dateExtent[0] ?? 0), new Date(dateExtent[1] ?? 0)])
        .range(xRange);
      xAxis = axisBottom(xScaleTime).tickPadding(toNumber(this.tickPadding, 6)) as unknown as Axis<string | Date>;
      const dateTickValues = (this.tickValues ?? [])
        .map(value => (value instanceof Date ? value : undefined))
        .filter((value): value is Date => value !== undefined);
      applyAxisTickConfig(xAxis as Axis<Date>, this.xAxisTickCount, dateTickValues);
    } else {
      const groupsByCategory = new Map<string, number[]>();
      stacks.forEach(stack => {
        groupsByCategory.set(
          String(stack.xAxisPoint),
          stack.chartData.map(point => point.data),
        );
      });
      const domain = sortCategoryGroups(
        Array.from(groupsByCategory.entries()).map(([key, values]) => ({ key, points: values })),
        this.xAxisCategoryOrder,
        stacks.map(stack => String(stack.xAxisPoint)),
        group => group.points,
      ).map(group => group.key);
      const xAxisInnerPadding = toOptionalNumber(this.xAxisInnerPadding) ?? 2 / 3;
      const xAxisOuterPadding = toOptionalNumber(this.xAxisOuterPadding) ?? 0;
      xScaleBand = scaleBand<string>()
        .domain(domain)
        .range(xRange)
        .paddingInner(xAxisInnerPadding)
        .paddingOuter(xAxisOuterPadding);
      xAxis = axisBottom(xScaleBand).tickPadding(toNumber(this.tickPadding, 6)) as unknown as Axis<string | Date>;
      applyAxisTickConfig(
        xAxis as Axis<string>,
        this.xAxisTickCount,
        this.tickValues?.map(value => String(value)),
      );
    }
    const getXCenter = (stack: VerticalStackedBarChartProps): number =>
      xScaleTime
        ? xScaleTime(stack.xAxisPoint as Date)
        : (xScaleBand!(String(stack.xAxisPoint)) ?? 0) + xScaleBand!.bandwidth() / 2;
    const positiveTotals = stacks.map(stack =>
      stack.chartData.reduce((sum, point) => sum + Math.max(point.data, 0), 0),
    );
    const negativeTotals = stacks.map(stack =>
      stack.chartData.reduce((sum, point) => sum + Math.min(point.data, 0), 0),
    );
    const maxTotal = max(positiveTotals) ?? 0;
    const minTotal = this.supportNegativeData ? Math.min(...negativeTotals, 0) : 0;
    const preparedYAxis = computePreparedNumericYAxis({
      minValue: toOptionalNumber(this.yMinValue) ?? minTotal,
      maxValue: toOptionalNumber(this.yMaxValue) ?? Math.max(maxTotal, 1),
      tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
      roundedTicks: this.roundedTicks,
    });
    const yScale = scaleLinear().domain([preparedYAxis.domainMin, preparedYAxis.domainMax]).range([innerHeight, 0]);

    const secondaryLineValues = stacks
      .flatMap(stack => stack.lineData ?? [])
      .filter(entry => entry.useSecondaryYScale)
      .map(entry => entry.y)
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

    const getLineScale = (
      entry: VerticalStackedBarChartLineDataPoint,
    ): ScaleLinear<number, number> | ScaleLogarithmic<number, number> => {
      return entry.useSecondaryYScale ? yScaleSecondary : yScale;
    };

    const legendNames = Array.from(new Set(stacks.flatMap(stack => stack.chartData.map(point => point.legend))));
    const colorMap = new Map<string, string>();
    const firstSegment = stacks.flatMap(stack => stack.chartData)[0];
    const singleColor = this.useSingleColor ? resolveChartColor(firstSegment?.color, this.colors, 0) : undefined;
    legendNames.forEach((legend, index) => {
      const match = stacks.flatMap(stack => stack.chartData).find(point => point.legend === legend);
      colorMap.set(legend, singleColor ?? resolveChartColor(match?.color, this.colors, index));
    });

    const lineLegendOrder: string[] = [];
    const lineColorMap = new Map<string, string>();
    stacks.forEach(stack => {
      stack.lineData?.forEach(entry => {
        if (!lineColorMap.has(entry.legend)) {
          lineColorMap.set(entry.legend, resolveChartColor(entry.color, this.colors, lineLegendOrder.length, 10));
          lineLegendOrder.push(entry.legend);
        }
      });
    });

    const getStackTooltipEntries = (stack: VerticalStackedBarChartProps): TooltipEntry[] => [
      ...stack.chartData
        .filter(entry => this._shouldShowTooltip(entry.legend))
        .map(entry => ({
          legend: entry.legend,
          color: colorMap.get(entry.legend) ?? getNextColor(0, 0),
          value: entry.yAxisCalloutData || formatNumberValue(entry.data, this.yAxisTickFormat, this.culture),
        })),
      ...(stack.lineData ?? [])
        .filter(entry => this._shouldShowTooltip(entry.legend))
        .map(entry => ({
          legend: entry.legend,
          color: lineColorMap.get(entry.legend) ?? getNextColor(0, 10),
          value: entry.yAxisCalloutData || formatNumberValue(entry.y, this.yAxisTickFormat, this.culture),
        })),
    ];

    const getSingleTooltipEntry = (legend: string, color: string, value: string): TooltipEntry[] => [
      { legend, color, value },
    ];

    const buildLinePoints = (legend: string): LinePlotPoint[] => {
      return stacks.reduce<LinePlotPoint[]>((points, stack) => {
        const entry = stack.lineData?.find(item => item.legend === legend);
        if (entry && typeof entry.y === 'number' && Number.isFinite(entry.y)) {
          const xCenter = getXCenter(stack);
          points.push({ xAxisPoint: stack.xAxisPoint, xCenter, entry });
        }
        return points;
      }, []);
    };

    // Only hide the tooltip when the pointer leaves the whole chart, not individual
    // segments/points — moving over blank space between them just leaves it in place.
    svg.addEventListener('mouseleave', () => {
      this._clearTooltip();
      this._activeLineMarkerXValue = null;
      this._syncLineMarkerVisibility();
    });

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const yAxis = axisLeft(yScale).tickPadding(toNumber(this.tickPadding, 6));
    applyAxisTickConfig(
      yAxis,
      this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
      this.yAxisTickValues ?? preparedYAxis.tickValues,
    );
    renderAxisGridLinesShared({
      layer: plotGroup,
      orientation: 'horizontal',
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      spanStart: 0,
      spanEnd: innerWidth,
    });

    const cornerRadius = this.roundCorners ? 3 : 0;
    const focusableData: SVGElement[] = [];

    stacks.forEach((stack, stackIndex) => {
      const xCenter = getXCenter(stack);
      const step = xScaleBand?.step() ?? innerWidth;
      // Match React's default categorical bar width cap; explicit bar-width can grow it.
      const actualWidth = resolveBarWidth(this.barWidth, this.maxBarWidth, step, defaultCategoricalBarWidth);
      const x = xCenter - actualWidth / 2;

      const positiveTotal = stack.chartData.reduce((sum, segment) => sum + Math.max(segment.data, 0), 0);
      const negativeTotal = this.supportNegativeData
        ? stack.chartData.reduce((sum, segment) => sum + Math.min(segment.data, 0), 0)
        : 0;
      const stackTotal = stack.chartData.reduce((sum, segment) => sum + segment.data, 0);
      // bar-gap-max controls the visual gap between stacked segments within a bar (matching
      // React's VerticalStackedBarChart), capped at 20% of the stack's height and never below 1px.
      // Defaults to 2px (this component's prior fixed gap) when the attribute is not set.
      const barGapMax = toOptionalNumber(this.barGapMax) ?? 2;
      const positiveValues = stack.chartData.map(segment => Math.max(segment.data, 0)).filter(value => value > 0);
      const negativeValues = this.supportNegativeData
        ? stack.chartData.map(segment => Math.abs(Math.min(segment.data, 0))).filter(value => value > 0)
        : [];
      const getSideMetrics = (values: number[], total: number, endpoint: number) => {
        const gapsCount = barGapMax > 0 ? Math.max(values.length - 1, 0) : 0;
        const totalHeightPx = Math.abs(yScale(0) - yScale(endpoint));
        const gap = gapsCount > 0 ? Math.max(1, Math.min(barGapMax, (totalHeightPx * 0.2) / gapsCount)) : 0;
        const usableHeightPx = Math.max(totalHeightPx - gap * gapsCount, 0);
        const sumOfPercent = values.reduce((sum, value) => {
          const percent = (value / total) * 100;
          return sum + (percent < 1 ? 1 : percent);
        }, 0);
        const scalingRatio = sumOfPercent > 0 ? sumOfPercent / 100 : 1;
        const heightValueScale = total > 0 ? usableHeightPx / (total * scalingRatio) : 0;
        return { gap, heightValueScale, minSegmentHeight: (heightValueScale * total) / 100 };
      };
      const positiveMetrics = getSideMetrics(positiveValues, positiveTotal, positiveTotal);
      const negativeMagnitude = Math.abs(negativeTotal);
      const negativeMetrics = getSideMetrics(negativeValues, negativeMagnitude, negativeTotal);

      let positiveBottom = yScale(0);
      let negativeTop = yScale(0);
      let positiveIndex = 0;
      let negativeIndex = 0;
      stack.chartData.forEach((segment, segmentIndex) => {
        const color = colorMap.get(segment.legend) ?? getNextColor(0, 0);
        const isNegative = this.supportNegativeData && segment.data < 0;
        const segmentValue = isNegative ? Math.abs(segment.data) : Math.max(segment.data, 0);
        const metrics = isNegative ? negativeMetrics : positiveMetrics;
        let segmentHeight = metrics.heightValueScale * segmentValue;
        if (segmentValue > 0 && segmentHeight < metrics.minSegmentHeight) {
          segmentHeight = metrics.minSegmentHeight;
        }
        let top: number;
        let bottom: number;
        if (isNegative) {
          top = negativeTop;
          bottom = top + segmentHeight;
          negativeIndex += 1;
          negativeTop = bottom + (negativeIndex < negativeValues.length ? metrics.gap : 0);
        } else {
          bottom = positiveBottom;
          top = bottom - segmentHeight;
          if (segmentValue > 0) {
            positiveIndex += 1;
            positiveBottom = top - (positiveIndex < positiveValues.length ? metrics.gap : 0);
          }
        }

        const rect = createSvgElement<SVGRectElement>('rect');
        rect.classList.add('bar');
        rect.dataset.legend = segment.legend;
        rect.dataset.value = String(segment.data);
        rect.setAttribute('x', String(x));
        rect.setAttribute('y', String(top));
        rect.setAttribute('width', String(actualWidth));
        rect.setAttribute('height', String(Math.max(bottom - top, 0)));
        const gradientId = appendVerticalGradient(
          defs,
          `vsbc-gradient-${stackIndex}-${segmentIndex}`,
          color,
          this.enableGradient,
          segment.gradient,
        );
        rect.setAttribute('fill', gradientId ? `url(#${gradientId})` : color);
        rect.setAttribute('rx', String(cornerRadius));
        rect.setAttribute('ry', String(cornerRadius));
        rect.setAttribute('role', 'img');
        rect.setAttribute('tabindex', focusableData.length === 0 ? '0' : '-1');
        rect.setAttribute(
          'aria-label',
          segment.callOutAccessibilityData?.ariaLabel ??
            `${formatXAxisValue(this, stack.xAxisPoint)}. ${segment.legend}, ${segment.data}.`,
        );
        if (this.strokeWidth !== undefined) {
          rect.setAttribute('stroke-width', String(this.strokeWidth));
          rect.setAttribute('stroke', color);
        }
        const showSegmentTooltip = (clientY: number) => {
          if (!this._shouldShowTooltip(segment.legend) || this.hideTooltip) {
            return;
          }
          this._currentTooltipDataPoint = this.isCalloutForStack ? stack : { ...segment, xAxisPoint: stack.xAxisPoint };
          const value = segment.yAxisCalloutData || formatNumberValue(segment.data, this.yAxisTickFormat, this.culture);
          this._showBarSegmentTooltipAtY(clientY, x, 0, actualWidth, top, bottom, margins, svg, {
            legend: segment.legend,
            xValue: formatXAxisCalloutValue(this, segment.xAxisCalloutData, formatXAxisValue(this, stack.xAxisPoint)),
            yValue: value,
            color,
            entries: this.isCalloutForStack
              ? getStackTooltipEntries(stack)
              : getSingleTooltipEntry(segment.legend, color, value),
          });
        };
        rect.addEventListener('mouseenter', event => showSegmentTooltip(event.clientY));
        rect.addEventListener('mousemove', event => {
          if (!this._shouldShowTooltip(segment.legend) || this.hideTooltip) {
            return;
          }
          this._showBarSegmentTooltipAtY(event.clientY, x, 0, actualWidth, top, bottom, margins, svg, {
            legend: segment.legend,
            xValue: formatXAxisCalloutValue(this, segment.xAxisCalloutData, formatXAxisValue(this, stack.xAxisPoint)),
            yValue: segment.yAxisCalloutData || formatNumberValue(segment.data, this.yAxisTickFormat, this.culture),
            color,
            entries: this.isCalloutForStack
              ? getStackTooltipEntries(stack)
              : getSingleTooltipEntry(
                  segment.legend,
                  color,
                  segment.yAxisCalloutData || formatNumberValue(segment.data, this.yAxisTickFormat, this.culture),
                ),
          });
        });
        rect.addEventListener('focus', () => {
          this._focusRovingElement(focusableData, rect);
          showSegmentTooltip(svg.getBoundingClientRect().top + margins.top + (top + bottom) / 2);
        });
        rect.addEventListener('blur', () => this._clearTooltip());
        rect.addEventListener('click', () => {
          this._focusRovingElement(focusableData, rect);
          segment.onClick?.();
        });
        rect.addEventListener('keydown', (event: KeyboardEvent) => {
          if (event.key === 'Enter' || event.key === ' ') {
            event.preventDefault();
            segment.onClick?.();
          } else {
            this._rovingKeydown(focusableData, event);
          }
        });
        focusableData.push(rect);
        plotGroup.appendChild(rect);
      });

      const shouldShowLabel =
        !this.hideLabels && actualWidth >= 16 && stackTotal !== 0 && this._getHighlightedLegends().length === 0;
      if (shouldShowLabel) {
        const label = createSvgElement<SVGTextElement>('text');
        label.classList.add('bar-label');
        label.setAttribute('x', String(x + actualWidth / 2));
        label.setAttribute('y', String(stackTotal >= 0 ? positiveBottom - 6 : negativeTop + 12));
        label.setAttribute('text-anchor', 'middle');
        label.textContent =
          stack.chartData.find(segment => segment.barLabel)?.barLabel ?? formatYAxisTickValue(this, stackTotal);
        plotGroup.appendChild(label);
      }
    });

    lineLegendOrder.forEach(legend => {
      const linePoints = buildLinePoints(legend);
      if (linePoints.length === 0) {
        return;
      }

      const lineColor = lineColorMap.get(legend) ?? getNextColor(0, 10);
      const resolvedLineStrokeWidth =
        this.lineStrokeWidth !== undefined ? Number.parseFloat(this.lineStrokeWidth.toString()) : 3;
      const lineBorderWidth =
        this.lineBorderWidth !== undefined ? Number.parseFloat(this.lineBorderWidth.toString()) : 0;
      const lineBorderColor = this.lineBorderColor || 'var(--colorNeutralBackground1, #fff)';
      const lineStrokeLinecap = this.lineStrokeLinecap || 'square';
      const buildPath = createLine<LinePlotPoint>()
        .x(point => point.xCenter)
        .y(point => getLineScale(point.entry)(point.entry.y));

      if (lineBorderWidth > 0) {
        const lineBorderPath = createSvgElement<SVGPathElement>('path');
        lineBorderPath.classList.add('line-border');
        lineBorderPath.dataset.legend = legend;
        lineBorderPath.setAttribute('fill', 'none');
        lineBorderPath.setAttribute('stroke', lineBorderColor);
        lineBorderPath.setAttribute('stroke-width', String(resolvedLineStrokeWidth + lineBorderWidth * 2));
        lineBorderPath.setAttribute('stroke-linecap', lineStrokeLinecap);
        if (this.lineStrokeDasharray !== undefined) {
          lineBorderPath.setAttribute('stroke-dasharray', String(this.lineStrokeDasharray));
        }
        if (this.lineStrokeDashoffset !== undefined) {
          lineBorderPath.setAttribute('stroke-dashoffset', String(this.lineStrokeDashoffset));
        }
        lineBorderPath.setAttribute('d', buildPath(linePoints) ?? '');
        plotGroup.appendChild(lineBorderPath);
      }

      const linePath = createSvgElement<SVGPathElement>('path');
      linePath.classList.add('line-path');
      linePath.dataset.legend = legend;
      linePath.setAttribute('fill', 'none');
      linePath.setAttribute('stroke', lineColor);
      linePath.setAttribute('stroke-width', String(resolvedLineStrokeWidth));
      linePath.setAttribute('stroke-linecap', lineStrokeLinecap);
      if (this.lineStrokeDasharray !== undefined) {
        linePath.setAttribute('stroke-dasharray', String(this.lineStrokeDasharray));
      }
      if (this.lineStrokeDashoffset !== undefined) {
        linePath.setAttribute('stroke-dashoffset', String(this.lineStrokeDashoffset));
      }
      linePath.setAttribute('d', buildPath(linePoints) ?? '');
      plotGroup.appendChild(linePath);

      // Wide invisible hit path so hovering anywhere along the line (not just near a
      // point) shows the tooltip for the nearest data point, following the cursor's Y.
      const lineCyValues = linePoints.map(point => getLineScale(point.entry)(point.entry.y));
      const lineCyMin = Math.min(...lineCyValues);
      const lineCyMax = Math.max(...lineCyValues);
      const findNearestLinePoint = (localX: number): LinePlotPoint =>
        linePoints.reduce((closest, point) =>
          Math.abs(point.xCenter - localX) < Math.abs(closest.xCenter - localX) ? point : closest,
        );
      const handleLineHover = (event: MouseEvent): void => {
        if (!this._shouldShowTooltip(legend) || this.hideTooltip) {
          return;
        }
        const svgRect = svg.getBoundingClientRect();
        const localX = event.clientX - svgRect.left - margins.left;
        const nearest = findNearestLinePoint(localX);
        const nearestCy = getLineScale(nearest.entry)(nearest.entry.y);
        const nearestStack = stacks.find(stack => String(stack.xAxisPoint) === String(nearest.xAxisPoint));
        this._activeLineMarkerXValue = String(nearest.xAxisPoint);
        this._syncLineMarkerVisibility();
        this._currentTooltipDataPoint =
          this.isCalloutForStack && nearestStack ? nearestStack : { ...nearest.entry, xAxisPoint: nearest.xAxisPoint };
        const value =
          nearest.entry.yAxisCalloutData || formatNumberValue(nearest.entry.y, this.yAxisTickFormat, this.culture);
        this._showLinePointTooltipAtY(
          event.clientY,
          nearest.xCenter,
          nearestCy,
          margins,
          svg,
          {
            legend,
            xValue: formatXAxisValue(this, nearest.xAxisPoint),
            yValue: value,
            color: lineColor,
            entries:
              this.isCalloutForStack && nearestStack
                ? getStackTooltipEntries(nearestStack)
                : getSingleTooltipEntry(legend, lineColor, value),
          },
          lineCyMin,
          lineCyMax,
        );
      };
      const lineHitArea = createSvgElement<SVGPathElement>('path');
      lineHitArea.classList.add('line-hit-area');
      lineHitArea.dataset.legend = legend;
      lineHitArea.setAttribute('fill', 'none');
      lineHitArea.setAttribute('stroke', 'transparent');
      lineHitArea.setAttribute('stroke-width', '16');
      lineHitArea.setAttribute('d', buildPath(linePoints) ?? '');
      lineHitArea.addEventListener('mouseenter', handleLineHover);
      lineHitArea.addEventListener('mousemove', handleLineHover);
      lineHitArea.addEventListener('mouseleave', () => {
        this._activeLineMarkerXValue = null;
        this._syncLineMarkerVisibility();
      });
      plotGroup.appendChild(lineHitArea);

      linePoints.forEach(point => {
        const cx = point.xCenter;
        const cy = getLineScale(point.entry)(point.entry.y);

        const marker = createSvgElement<SVGCircleElement>('circle');
        marker.classList.add('line-marker');
        marker.dataset.legend = legend;
        marker.dataset.xValue = String(point.xAxisPoint);
        marker.setAttribute('cx', String(cx));
        marker.setAttribute('cy', String(cy));
        marker.setAttribute('r', '0');
        marker.setAttribute('visibility', 'hidden');
        marker.setAttribute('fill', '#fff');
        marker.setAttribute('stroke', lineColor);
        marker.setAttribute('stroke-width', '2');
        marker.setAttribute('pointer-events', 'none');
        plotGroup.appendChild(marker);

        // Larger invisible hit area so the line point gets its own hover tooltip, independent of the bar.
        const markerHitArea = createSvgElement<SVGCircleElement>('circle');
        markerHitArea.classList.add('line-marker-hit-area');
        markerHitArea.dataset.legend = legend;
        markerHitArea.setAttribute('cx', String(cx));
        markerHitArea.setAttribute('cy', String(cy));
        markerHitArea.setAttribute('r', '10');
        markerHitArea.setAttribute('fill', 'transparent');
        markerHitArea.setAttribute('role', 'img');
        markerHitArea.setAttribute('tabindex', focusableData.length === 0 ? '0' : '-1');
        markerHitArea.setAttribute(
          'aria-label',
          `${formatXAxisValue(this, point.xAxisPoint)}. ${legend}, ${point.entry.yAxisCalloutData || point.entry.y}.`,
        );
        const showLinePointTooltip = (clientY: number) => {
          this._activeLineMarkerXValue = String(point.xAxisPoint);
          this._syncLineMarkerVisibility();
          if (!this._shouldShowTooltip(legend) || this.hideTooltip) {
            return;
          }
          const pointStack = stacks.find(stack => String(stack.xAxisPoint) === String(point.xAxisPoint));
          this._currentTooltipDataPoint =
            this.isCalloutForStack && pointStack ? pointStack : { ...point.entry, xAxisPoint: point.xAxisPoint };
          const value =
            point.entry.yAxisCalloutData || formatNumberValue(point.entry.y, this.yAxisTickFormat, this.culture);
          this._showLinePointTooltipAtY(clientY, cx, cy, margins, svg, {
            legend,
            xValue: formatXAxisValue(this, point.xAxisPoint),
            yValue: value,
            color: lineColor,
            entries:
              this.isCalloutForStack && pointStack
                ? getStackTooltipEntries(pointStack)
                : getSingleTooltipEntry(legend, lineColor, value),
          });
        };
        markerHitArea.addEventListener('mouseenter', event => showLinePointTooltip(event.clientY));
        markerHitArea.addEventListener('mousemove', event => {
          if (!this._shouldShowTooltip(legend) || this.hideTooltip) {
            return;
          }
          const pointStack = stacks.find(stack => String(stack.xAxisPoint) === String(point.xAxisPoint));
          const value =
            point.entry.yAxisCalloutData || formatNumberValue(point.entry.y, this.yAxisTickFormat, this.culture);
          this._showLinePointTooltipAtY(event.clientY, cx, cy, margins, svg, {
            legend,
            xValue: formatXAxisValue(this, point.xAxisPoint),
            yValue: value,
            color: lineColor,
            entries:
              this.isCalloutForStack && pointStack
                ? getStackTooltipEntries(pointStack)
                : getSingleTooltipEntry(legend, lineColor, value),
          });
        });
        markerHitArea.addEventListener('mouseleave', () => {
          this._activeLineMarkerXValue = null;
          this._syncLineMarkerVisibility();
        });
        markerHitArea.addEventListener('focus', () => {
          this._focusRovingElement(focusableData, markerHitArea);
          showLinePointTooltip(svg.getBoundingClientRect().top + margins.top + cy);
        });
        markerHitArea.addEventListener('blur', () => {
          this._activeLineMarkerXValue = null;
          this._syncLineMarkerVisibility();
          this._clearTooltip();
        });
        markerHitArea.addEventListener('click', () => {
          this._focusRovingElement(focusableData, markerHitArea);
          point.entry.onClick?.();
        });
        markerHitArea.addEventListener('keydown', (event: KeyboardEvent) => {
          if (event.key === 'Enter' || event.key === ' ') {
            event.preventDefault();
            point.entry.onClick?.();
          } else {
            this._rovingKeydown(focusableData, event);
          }
        });
        focusableData.push(markerHitArea);
        plotGroup.appendChild(markerHitArea);
      });
    });

    this._relocateFocusIfNeeded(focusableData);

    const axisRenderOptions = {
      svg,
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
      axisLabelTooltipHandlers: {
        show: (target: SVGTextElement, fullLabel: string) => this._showAxisLabelTooltip(target, fullLabel),
        hide: () => this._hideAxisLabelTooltip(),
      },
      xAxisTitle: this.xAxisTitle,
    };
    if (xScaleTime) {
      renderBottomAxisShared({
        ...axisRenderOptions,
        scale: xScaleTime,
        axis: xAxis as Axis<Date>,
        formatter: value => formatDateValue(this, value),
      });
    } else {
      renderBottomAxisShared({
        ...axisRenderOptions,
        scale: xScaleBand!,
        axis: xAxis as Axis<string>,
        formatter: value => value,
      });
    }
    renderPrimaryYAxisShared({
      svg,
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      formatter: value => formatYAxisTickValue(this, value),
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
        formatter: value => formatYAxisTickValue(this, value),
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
        const bandX = xScaleBand?.(String(value));
        return bandX === undefined || !xScaleBand ? undefined : bandX + xScaleBand.bandwidth() / 2;
      },
      mapDataY: (value, axis) => (axis === 'secondary' ? yScaleSecondary : yScale)(Number(value)),
    });

    this.chartContainer.appendChild(svg);
    this.legends = [
      ...lineLegendOrder.map(legend => ({
        legend,
        color: lineColorMap.get(legend) ?? getNextColor(0, 10),
        isLineLegendInBarChart: true,
      })),
      ...legendNames.map(legend => ({ legend, color: colorMap.get(legend) ?? getNextColor(0, 0) })),
    ];
    this._updateLegendInteractionState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this.chartContainer) {
      return;
    }
    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;
    this.chartContainer
      .querySelectorAll<SVGElement>('.bar, .line-path, .line-border, .line-marker')
      .forEach(element => {
        const legend = element.dataset.legend ?? '';
        const isActive = !hasSelection || highlighted.includes(legend);
        element.classList.toggle('inactive', !isActive);
        element.setAttribute('opacity', isActive ? '1' : '0.1');
      });
    this._syncLineMarkerVisibility();
  }

  protected override _getHostAriaLabel(): string {
    const count = Array.isArray(this.data) ? this.data.length : 0;
    if (count === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Vertical stacked bar chart with no data.';
    }
    return `${this.chartTitle || 'Vertical stacked bar chart'}. ${count} stacks.`;
  }

  private _clearChart(): void {
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
