import { attr } from '@microsoft/fast-element';
import { bisector, extent } from 'd3-array';
import { axisBottom, axisLeft, axisRight, type Axis, type AxisDomain } from 'd3-axis';
import { format } from 'd3-format';
import { scaleLinear, scaleTime, type ScaleLinear, type ScaleTime } from 'd3-scale';
import { area as createArea, curveMonotoneX, line as createLine, stack as createStack } from 'd3-shape';
import { timeFormat, utcFormat } from 'd3-time-format';
import type { TooltipProps } from '../utils/chart-options.js';
import { CartesianChartBase } from '../utils/cartesian-chart-base.js';
import {
  applyAxisTickConfig,
  type AxisScaleLike,
  computePreparedNumericYAxis,
  createNumericContinuousScale,
  createPreparedNumericContinuousScale,
  DEFAULT_NUMERIC_Y_TICK_COUNT,
  renderAxisGridLinesShared,
  renderBottomAxisShared,
  renderPrimaryYAxisShared,
  renderSecondaryYAxisShared,
  toAxisNumber as toNumber,
  toOptionalAxisNumber as toOptionalNumber,
} from '../utils/cartesian-axis-shared.js';
import {
  defaultYAxisTickFormatter,
  escapeHtml,
  formatLocaleNumber,
  getColorFromToken,
  getNextColor,
  getRTL,
  jsonConverter,
  parseDateOrNumber,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { AreaChartDataPoint, AreaChartMode, AreaChartSeries } from './area-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

/** A single series entry shown in the multi-series hover tooltip. */
export type TooltipEntry = { legend: string; color: string; value: string; callOutAriaLabel?: string };

type TooltipState = TooltipProps & { xLabel: string; xAxisAriaLabel?: string; entries: TooltipEntry[] };
type XValue = number | Date;
type ContinuousScale = ScaleLinear<number, number> | ScaleTime<number, number>;
type NormalizedPoint = AreaChartDataPoint & { x: XValue; xLabel: string; cx: number; cy: number };
type NormalizedSeries = { legend: string; color: string; data: NormalizedPoint[] };

const defaultMargins = { top: 40, right: 20, bottom: 50, left: 60 };

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

const formatDateValue = (chart: AreaChart, value: Date): string => {
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
  chart: AreaChart,
  value: AreaChartDataPoint['xAxisCalloutData'],
  fallback: string,
): string => {
  if (Object.prototype.toString.call(value) === '[object Date]') {
    return formatDateValue(chart, new Date((value as Date).getTime()));
  }
  return typeof value === 'string' && value ? value : fallback;
};

const getNormalizedXValue = (value: number | Date): XValue => {
  const parsed = parseDateOrNumber(value as number | Date | string);
  return parsed instanceof Date ? parsed : Number(parsed);
};

/** @public */
export class AreaChart extends CartesianChartBase {
  public declare tooltipProps: TooltipState;

  @attr({ converter: jsonConverter })
  public data!: AreaChartSeries[];

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  /**
   * Controls how areas are filled.
   * - `'tonexty'` (default): Stacked — each area fills from the previous series up.
   * - `'tozeroy'`: Non-stacked — each series fills independently from y=0.
   */
  @attr()
  public mode: AreaChartMode = 'tonexty';

  /** Max width in px for secondary y-axis tick labels before truncating with ellipsis. */
  @attr({ attribute: 'secondary-y-axis-tick-label-max-width' })
  public secondaryYAxisTickLabelMaxWidth?: number | string;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'enableGradient', 'mode', 'secondaryYAxisTickLabelMaxWidth'] as const;
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

    this.tooltipProps = { ...this.tooltipProps, xLabel: '', entries: [] } as TooltipState;
    this._requestRender();
  }

  protected dataChanged(): void {
    this._requestRender();
  }

  protected enableGradientChanged(): void {
    this._requestRender();
  }

  protected modeChanged(): void {
    this._requestRender();
  }

  protected secondaryYAxisTickLabelMaxWidthChanged(): void {
    this._requestRender();
  }

  protected override _clearTooltip(): void {
    this.tooltipProps = {
      isVisible: false,
      legend: '',
      xLabel: '',
      xAxisAriaLabel: undefined,
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
      const xText = state.xAxisAriaLabel ?? state.xLabel;
      const parts = [xText, ...(state.entries ?? []).map(e => e.callOutAriaLabel ?? `${e.legend}: ${e.value}`)].filter(
        Boolean,
      );
      this.liveRegionText = parts.join('. ');
    }
  }

  protected override _buildDefaultTooltipHTML(): string {
    const state = this.tooltipProps as TooltipState;
    const header = `<div class="tooltip-header">${escapeHtml(state.xLabel ?? '')}</div>`;
    const entries = (state.entries ?? [])
      .map(e =>
        [
          `<div class="tooltip-info" style="border-color: ${escapeHtml(e.color)};">`,
          `<div class="tooltip-legend-text">${escapeHtml(e.legend)}</div>`,
          `<div class="tooltip-primary-value" style="color: ${escapeHtml(e.color)};">${escapeHtml(e.value)}</div>`,
          `</div>`,
        ].join(''),
      )
      .join('');
    return header + entries;
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

    const flattened = seriesData.flatMap(series => series.data.map(point => getNormalizedXValue(point.x)));
    const isDateAxis = flattened.some(value => value instanceof Date);

    const normalizedSeries: NormalizedSeries[] = seriesData.map((series, index) => {
      const color = series.color ? getColorFromToken(series.color) : getNextColor(index, 0);
      const data = series.data.map(point => {
        const x = getNormalizedXValue(point.x);
        const defaultXLabel =
          x instanceof Date ? formatDateValue(this, x) : formatNumberValue(x, this.xAxisTickFormat, this.culture);
        return {
          ...point,
          x,
          xLabel: formatXAxisCalloutValue(this, point.xAxisCalloutData, defaultXLabel),
          cx: 0,
          cy: 0,
        };
      });
      return { legend: series.legend, color, data };
    });

    // Determine which series are plotted on the secondary (right) Y axis.
    const isSecondaryByIndex: boolean[] = seriesData.map(s => Boolean(s.useSecondaryYScale));
    const hasSecondaryY = isSecondaryByIndex.some(Boolean);

    const isRtl = getRTL(this);
    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 500);
    const height = toNumber(this.height, 300);
    const { svg, plotGroup, margins, innerWidth, innerHeight } = this._createCartesianRenderContext({
      width,
      height,
      defaultMargins,
      hasSecondaryYAxis: hasSecondaryY,
    });

    // Build a unified dataset keyed by x-value so d3Stack can compute stacked layers.
    // Each entry holds all series values at that x position (missing values default to 0).
    const legendKeys = normalizedSeries.map(s => s.legend);
    // Only primary-axis series participate in stacking.
    const primaryLegendKeys = legendKeys.filter((_, i) => !isSecondaryByIndex[i]);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const xEntryMap = new Map<string, any>();
    normalizedSeries.forEach(series => {
      series.data.forEach(point => {
        const key = String(point.x instanceof Date ? point.x.getTime() : point.x);
        if (!xEntryMap.has(key)) {
          xEntryMap.set(key, { xVal: point.x, xLabel: point.xLabel });
        }
        xEntryMap.get(key)[series.legend] = point.y;
      });
    });
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const stackDataset: any[] = [...xEntryMap.values()].sort((a, b) => {
      const ax = a.xVal instanceof Date ? a.xVal.getTime() : Number(a.xVal);
      const bx = b.xVal instanceof Date ? b.xVal.getTime() : Number(b.xVal);
      return ax - bx;
    });
    // Ensure every entry has a value for every legend (fill missing with 0)
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    stackDataset.forEach((dp: any) =>
      legendKeys.forEach(k => {
        if (dp[k] === undefined) dp[k] = 0;
      }),
    );

    // mode='tonexty' (default): stacked — d3Stack computes cumulative bands.
    // mode='tozeroy': non-stacked — each series' area fills independently from y=0.
    // Secondary-axis series always use tozeroy ([0, y]) regardless of mode.
    // This mirrors React's _shouldFillToZeroY() / _getDataPoints() logic.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let primaryStackedLayers: any[];
    if (this.mode === 'tozeroy' || primaryLegendKeys.length === 0) {
      primaryStackedLayers = primaryLegendKeys.map(key =>
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        stackDataset.map((dp: any) => {
          const entry = [0, dp[key] as number] as any;
          entry.data = dp;
          return entry;
        }),
      );
    } else {
      primaryStackedLayers = createStack<any, any, string>().keys(primaryLegendKeys)(stackDataset);
    }
    // Secondary series always use [0, y] (independent scale, no stacking).
    const secondaryLegendKeys = legendKeys.filter((_, i) => isSecondaryByIndex[i]);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const secondaryStackedLayers: any[] = secondaryLegendKeys.map(key =>
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      stackDataset.map((dp: any) => {
        const entry = [0, dp[key] as number] as any;
        entry.data = dp;
        return entry;
      }),
    );
    // Merge back in original series order.
    let primaryIdx = 0;
    let secondaryIdx = 0;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const stackedLayers: any[] = legendKeys.map((_, i) => {
      if (isSecondaryByIndex[i]) {
        return secondaryStackedLayers[secondaryIdx++];
      }
      return primaryStackedLayers[primaryIdx++];
    });

    // Compute primary-axis raw and stacked ranges.
    // React AreaChart uses raw min/max for the base range and a stacked `maxOfYVal`
    // override in tonexty mode for the primary axis.
    const primaryRawValues = normalizedSeries
      .filter((_, i) => !isSecondaryByIndex[i])
      .flatMap(series => series.data.map(point => point.y));
    const rawPrimaryMin = primaryRawValues.length > 0 ? Math.min(...primaryRawValues) : 0;
    const rawPrimaryMax = primaryRawValues.length > 0 ? Math.max(...primaryRawValues) : 1;

    // Compute the stacked upper/lower bounds for primary layers.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const primaryLayersForDomain = primaryStackedLayers.length > 0 ? primaryStackedLayers : secondaryStackedLayers;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const allPrimaryValues: number[] = (primaryLayersForDomain as any[]).flatMap(layer =>
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (layer as any[]).flatMap((d: any) => [d[0] as number, d[1] as number]),
    );
    const stackedDataMin = allPrimaryValues.length > 0 ? Math.min(...allPrimaryValues) : 0;
    const stackedDataMax = allPrimaryValues.length > 0 ? Math.max(...allPrimaryValues) : 1;
    const maxOfYVal = hasSecondaryY ? rawPrimaryMax : stackedDataMax;
    const yTickCount = toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT);

    let yMin = toOptionalNumber(this.yMinValue) ?? Math.min(0, rawPrimaryMin, stackedDataMin);
    let yMax = toOptionalNumber(this.yMaxValue) ?? Math.max(0, maxOfYVal);
    if (yMin === yMax) {
      yMax += 1;
    }

    const preparedPrimaryYAxis = computePreparedNumericYAxis({
      minValue: yMin,
      maxValue: yMax,
      tickCount: yTickCount,
      roundedTicks: this.roundedTicks,
    });

    let xScale: ContinuousScale;
    let xFormatter: (value: AxisDomain) => string;
    const xRange: [number, number] = isRtl ? [innerWidth, 0] : [0, innerWidth];
    if (isDateAxis) {
      const dateValues = normalizedSeries
        .flatMap(series => series.data.map(point => point.x))
        .filter((value): value is Date => value instanceof Date);
      const rawExtent = extent(dateValues, value => value.getTime());
      const xMin =
        this.xMinValue !== undefined
          ? parseDateOrNumber(this.xMinValue as string | number)
          : new Date(rawExtent[0] ?? 0);
      const xMax =
        this.xMaxValue !== undefined
          ? parseDateOrNumber(this.xMaxValue as string | number)
          : new Date(rawExtent[1] ?? 0);
      const domainMin = xMin instanceof Date ? xMin : new Date(Number(xMin));
      const domainMax = xMax instanceof Date ? xMax : new Date(Number(xMax));
      xScale = scaleTime().domain([domainMin, domainMax]).range(xRange);
      if (this.roundedTicks) {
        xScale.nice();
      }
      xFormatter = value => formatDateValue(this, value as Date);
    } else {
      const xValues = normalizedSeries
        .flatMap(series => series.data.map(point => point.x))
        .filter((value): value is number => typeof value === 'number');
      const rawExtent = extent(xValues);
      let xMin = toOptionalNumber(this.xMinValue) ?? rawExtent[0] ?? 0;
      let xMax = toOptionalNumber(this.xMaxValue) ?? rawExtent[1] ?? 1;
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
      xFormatter = value => formatNumberValue(Number(value), this.xAxisTickFormat, this.culture);
    }

    const yScale = scaleLinear()
      .domain([preparedPrimaryYAxis.domainMin, preparedPrimaryYAxis.domainMax])
      .range([innerHeight, 0]);

    // Secondary Y axis scale: derived from secondary series raw values only.
    let yScaleSecondary = yScale; // fallback: same as primary when no secondary series
    let preparedSecondaryYAxis = preparedPrimaryYAxis;
    if (hasSecondaryY) {
      const secValues = seriesData
        .flatMap((s, i) => (isSecondaryByIndex[i] ? s.data.map(p => p.y) : []))
        .filter(v => typeof v === 'number');
      const secondaryYAxis = createPreparedNumericContinuousScale({
        values: secValues,
        range: [innerHeight, 0],
        tickCount: yTickCount,
        roundedTicks: this.roundedTicks,
      });
      preparedSecondaryYAxis = secondaryYAxis.preparedAxis;
      yScaleSecondary = secondaryYAxis.scale;
    }

    normalizedSeries.forEach((series, si) => {
      const layer = stackedLayers[si];
      const scale = isSecondaryByIndex[si] ? yScaleSecondary : yScale;
      series.data.forEach(point => {
        point.cx = xScale(point.x as never) ?? 0;
        // cy reflects the top of the stacked layer at this x-value (for tooltip placement)
        const xKey = String(point.x instanceof Date ? point.x.getTime() : point.x);
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const stackPoint = layer.find(
          (d: any) => String(d.data.xVal instanceof Date ? d.data.xVal.getTime() : d.data.xVal) === xKey,
        );
        point.cy = stackPoint ? scale(stackPoint[1]) : scale(point.y);
      });
    });

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const xAxis = axisBottom(xScale).tickPadding(toNumber(this.tickPadding, 6));
    if (!isDateAxis) {
      xAxis.ticks(6);
    }
    if (this.tickValues?.length) {
      if (isDateAxis) {
        xAxis.tickValues(this.tickValues.map(value => parseDateOrNumber(value as string | number | Date) as Date));
      } else {
        xAxis.tickValues(this.tickValues.map(value => Number(value)));
      }
    }

    const yAxis = axisLeft(yScale).tickPadding(toNumber(this.tickPadding, 6));
    applyAxisTickConfig(
      yAxis,
      this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
      this.yAxisTickValues ?? preparedPrimaryYAxis.tickValues,
    );

    renderAxisGridLinesShared({
      layer: plotGroup,
      orientation: 'horizontal',
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      spanStart: 0,
      spanEnd: innerWidth,
    });

    // Pre-compute a lookup map from x-value key → all series entries at that x.
    // This mirrors React's calloutData() utility: group all series by their x value
    // so the mousemove handler can retrieve every series in O(1) without re-iterating.
    // Entries are stored as a SPARSE ARRAY indexed by series index so sparse x-values
    // across series don't cause index mismatches.
    type CalloutEntry = {
      legend: string;
      color: string;
      y: number;
      stackedY1: number;
      isSecondaryY: boolean;
      value: string;
      callOutAriaLabel?: string;
    };
    type CalloutPoint = { xLabel: string; cx: number; xAxisAriaLabel?: string; entries: (CalloutEntry | undefined)[] };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const calloutPointsByX = new Map<number, CalloutPoint>();
    normalizedSeries.forEach((series, si) => {
      const layer = stackedLayers[si];
      series.data.forEach(point => {
        const key = point.x instanceof Date ? point.x.getTime() : Number(point.x);
        if (!calloutPointsByX.has(key)) {
          calloutPointsByX.set(key, {
            xLabel: point.xLabel,
            cx: point.cx,
            xAxisAriaLabel: point.xAxisCalloutAccessibilityData?.ariaLabel,
            entries: [],
          });
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const xKey = String(point.x instanceof Date ? point.x.getTime() : point.x);
        const stackPoint = layer.find(
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          (entry: any) =>
            String(entry.data.xVal instanceof Date ? entry.data.xVal.getTime() : entry.data.xVal) === xKey,
        );
        const stackedY1 = stackPoint?.[1] ?? point.y;
        // Store at series index so sparse datasets don't misalign entries.
        calloutPointsByX.get(key)!.entries[si] = {
          legend: series.legend,
          color: series.color,
          y: point.y,
          stackedY1,
          isSecondaryY: isSecondaryByIndex[si],
          value: point.yAxisCalloutData || formatNumberValue(point.y, this.yAxisTickFormat, this.culture),
          callOutAriaLabel: point.callOutAccessibilityData?.ariaLabel,
        };
      });
    });

    // Bisector mirrors React's: bisector((d) => d.x).left on the first series data,
    // then compare d0/d1 neighbors to pick the closer domain value.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const bisect = bisector((d: any) => d.xVal).left;

    const isMultiSeriesChart = normalizedSeries.length > 1;

    stackedLayers.forEach((layer: any, index: number) => {
      const series = normalizedSeries[index];
      const scale = isSecondaryByIndex[index] ? yScaleSecondary : yScale;
      if (this.enableGradient) {
        const gradient = createSvgElement<SVGLinearGradientElement>('linearGradient');
        gradient.id = `area-gradient-${index}`;
        gradient.setAttribute('x1', '0%');
        gradient.setAttribute('x2', '0%');
        gradient.setAttribute('y1', '0%');
        gradient.setAttribute('y2', '100%');

        const start = createSvgElement<SVGStopElement>('stop');
        start.setAttribute('offset', '0%');
        start.setAttribute('stop-color', series.color);
        start.setAttribute('stop-opacity', '0.8');
        gradient.appendChild(start);

        const end = createSvgElement<SVGStopElement>('stop');
        end.setAttribute('offset', '100%');
        end.setAttribute('stop-color', series.color);
        end.setAttribute('stop-opacity', '0.1');
        gradient.appendChild(end);
        defs.appendChild(gradient);
      }

      const areaPath = createSvgElement<SVGPathElement>('path');
      areaPath.classList.add('area-path');
      areaPath.dataset.legend = series.legend;
      areaPath.setAttribute('fill', this.enableGradient ? `url(#area-gradient-${index})` : series.color);
      areaPath.setAttribute('fill-opacity', '0.7');
      areaPath.setAttribute(
        'd',
        createArea<any>()
          .x((d: any) => xScale(d.data.xVal as never) ?? 0)
          .y0((d: any) => scale(d[0]))
          .y1((d: any) => scale(d[1]))
          .curve(curveMonotoneX)(layer) ?? '',
      );
      plotGroup.appendChild(areaPath);

      const linePath = createSvgElement<SVGPathElement>('path');
      linePath.classList.add('area-line');
      if (isMultiSeriesChart) {
        linePath.classList.add('multi-series');
      }
      linePath.dataset.legend = series.legend;
      linePath.setAttribute('fill', 'none');
      linePath.setAttribute('stroke', series.color);
      linePath.setAttribute(
        'd',
        createLine<any>()
          .x((d: any) => xScale(d.data.xVal as never) ?? 0)
          .y((d: any) => scale(d[1]))
          .curve(curveMonotoneX)(layer) ?? '',
      );
      plotGroup.appendChild(linePath);
    });

    // Hover elements: vertical intercept line + one dot per series (matching React's behaviour).
    const hoverLine = createSvgElement<SVGLineElement>('line');
    hoverLine.classList.add('hover-line');
    const hoverLineY1 = margins.top / 2;
    const hoverLineY2 = margins.top + innerHeight;
    hoverLine.setAttribute('y1', String(hoverLineY1));
    hoverLine.setAttribute('y2', String(hoverLineY2));
    hoverLine.style.display = 'none';
    svg.appendChild(hoverLine);

    const hoverDotLayer = createSvgElement<SVGGElement>('g');
    hoverDotLayer.classList.add('hover-dot-layer');
    hoverDotLayer.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(hoverDotLayer);

    const hoverDots = normalizedSeries.map(series => {
      const dot = createSvgElement<SVGCircleElement>('circle');
      dot.classList.add('hover-dot');
      dot.setAttribute('r', '6');
      dot.setAttribute('fill', '#fff');
      dot.setAttribute('stroke', series.color);
      dot.setAttribute('stroke-width', '2');
      dot.style.display = 'none';
      hoverDotLayer.appendChild(dot);
      return dot;
    });

    const focusablePoints: SVGCircleElement[] = [];
    const focusLayer = createSvgElement<SVGGElement>('g');
    focusLayer.classList.add('data-point-focus-layer');
    focusLayer.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);

    const updateCalloutMarkers = (found: CalloutPoint, cx: number) => {
      const svgX = margins.left + cx;
      hoverLine.setAttribute('x1', String(svgX));
      hoverLine.setAttribute('x2', String(svgX));
      hoverLine.style.display = '';

      let topY = innerHeight;
      const markerEntries: Array<TooltipEntry & { cy: number }> = [];
      normalizedSeries.forEach((series, seriesIndex) => {
        const entry = found.entries[seriesIndex];
        const active = entry && this._shouldShowTooltip(series.legend);
        if (active) {
          const dotScale = entry.isSecondaryY ? yScaleSecondary : yScale;
          const cy = dotScale(entry.stackedY1);
          hoverDots[seriesIndex].setAttribute('cx', String(cx));
          hoverDots[seriesIndex].setAttribute('cy', String(cy));
          hoverDots[seriesIndex].style.display = '';
          topY = Math.min(topY, cy);
          markerEntries.push({
            legend: series.legend,
            color: series.color,
            value: entry.value,
            callOutAriaLabel: entry.callOutAriaLabel,
            cy,
          });
        } else {
          hoverDots[seriesIndex].style.display = 'none';
        }
      });

      plotGroup.querySelectorAll<SVGPathElement>('.area-line').forEach(path => path.classList.add('hovered'));

      return {
        topY,
        entries: markerEntries
          .sort((left, right) => left.cy - right.cy)
          .map(({ cy: _cy, ...tooltipEntry }) => tooltipEntry),
      };
    };

    const clearCalloutMarkers = () => {
      hoverLine.style.display = 'none';
      hoverDots.forEach(dot => {
        dot.style.display = 'none';
      });
      plotGroup.querySelectorAll<SVGPathElement>('.area-line').forEach(path => path.classList.remove('hovered'));
    };

    const showPointTooltip = (
      event: FocusEvent,
      series: NormalizedSeries,
      point: NormalizedPoint,
      element: SVGCircleElement,
    ): void => {
      const hostRect = this.getBoundingClientRect();
      const targetRect = element.getBoundingClientRect();
      const anchorX = targetRect.left - hostRect.left + targetRect.width / 2;
      const key = point.x instanceof Date ? point.x.getTime() : Number(point.x);
      const calloutPoint = calloutPointsByX.get(key);
      if (!calloutPoint) {
        return;
      }

      const cx = xScale(point.x as never) ?? 0;
      const { entries, topY } = updateCalloutMarkers(calloutPoint, cx);

      if (this.hideTooltip || entries.length === 0) {
        return;
      }

      const svgRect = svg.getBoundingClientRect();
      const anchorY = svgRect.top - hostRect.top + margins.top + topY;
      const value = point.yAxisCalloutData || formatNumberValue(point.y, this.yAxisTickFormat, this.culture);

      element.setAttribute('fill', '#fff');
      element.setAttribute('stroke', series.color);
      element.setAttribute('stroke-width', '2');

      const isFreshShow = !this.tooltipProps.isVisible;
      this._currentTooltipDataPoint =
        entries.length > 1
          ? { xLabel: calloutPoint?.xLabel ?? point.xLabel, entries }
          : { legend: series.legend, ...point };
      this.tooltipProps = {
        isVisible: true,
        legend: entries[0]?.legend ?? series.legend,
        yValue: entries[0]?.value ?? value,
        color: entries[0]?.color ?? series.color,
        xLabel: calloutPoint?.xLabel ?? point.xLabel,
        xAxisAriaLabel: calloutPoint?.xAxisAriaLabel ?? point.xAxisCalloutAccessibilityData?.ariaLabel,
        entries,
        xPos: anchorX,
        yPos: anchorY,
      };
      this._positionTooltipAvoidingOverlap(anchorX, anchorY, anchorY, isFreshShow, {
        horizontalPlacement: 'side',
        gap: 15,
      });
    };

    const pointKeydown = (event: KeyboardEvent): void => {
      if (event.key === 'Enter' || event.key === ' ') {
        event.preventDefault();
        return;
      }

      const currentTarget = event.currentTarget as SVGCircleElement | null;
      if (!currentTarget) {
        return;
      }

      let orderedPoints = focusablePoints;
      if (event.key === 'ArrowUp' || event.key === 'ArrowDown') {
        const xKey = currentTarget.getAttribute('data-x-key');
        if (!xKey) {
          return;
        }

        orderedPoints = Array.from(
          this.shadowRoot?.querySelectorAll<SVGCircleElement>('.data-point-focus-target') ?? [],
        )
          .filter(point => point.getAttribute('data-x-key') === xKey)
          .sort(
            (left, right) => Number(left.getAttribute('data-cy') ?? '0') - Number(right.getAttribute('data-cy') ?? '0'),
          );

        if (orderedPoints.length === 0) {
          return;
        }
      }

      const currentIndex = orderedPoints.indexOf(currentTarget);
      if (currentIndex === -1) {
        return;
      }

      const forward = event.key === 'ArrowRight' || event.key === 'ArrowDown';
      const backward = event.key === 'ArrowLeft' || event.key === 'ArrowUp';
      if (!forward && !backward) {
        return;
      }

      event.preventDefault();
      const nextIndex = forward
        ? (currentIndex + 1) % orderedPoints.length
        : (currentIndex - 1 + orderedPoints.length) % orderedPoints.length;
      orderedPoints[currentIndex].tabIndex = -1;
      orderedPoints[nextIndex].tabIndex = 0;
      orderedPoints[nextIndex].focus();
    };

    // Transparent overlay rect captures all mouse events across the plot area.
    // Use fill="white" + fill-opacity="0" (not fill="transparent") so SVG hit-testing
    // still fires even when the cursor is between visible paths.
    const overlay = createSvgElement<SVGRectElement>('rect');
    overlay.setAttribute('x', '0');
    overlay.setAttribute('y', '0');
    overlay.setAttribute('width', String(innerWidth));
    overlay.setAttribute('height', String(innerHeight));
    overlay.setAttribute('fill', 'white');
    overlay.setAttribute('fill-opacity', '0');
    overlay.setAttribute('pointer-events', 'all');
    plotGroup.appendChild(overlay);

    const onOverlayMouseMove = (event: MouseEvent) => {
      const svgRect = svg.getBoundingClientRect();
      // localX is in plot-group coordinates (same coordinate space as xScale's range).
      const localX = event.clientX - svgRect.left - margins.left;

      // Invert pixel → domain value, then bisect by domain value.
      // This is the exact same approach React uses:
      //   xOffset = xScale.invert(pointer(event)[0])
      //   i = bisect(data, xOffset)  → compare d0/d1 to find nearest
      const xOffset = xScale.invert(localX);
      const i = bisect(stackDataset, xOffset);
      const d0 = stackDataset[i - 1] as (typeof stackDataset)[0] | undefined;
      const d1 = stackDataset[i] as (typeof stackDataset)[0] | undefined;

      let nearestDataPoint: (typeof stackDataset)[0] | undefined;
      if (d0 === undefined) {
        nearestDataPoint = d1;
      } else if (d1 === undefined) {
        nearestDataPoint = d0;
      } else if (isDateAxis) {
        const x0 = (xOffset as Date).getTime();
        const p0 = (d0.xVal as Date).getTime();
        const p1 = (d1.xVal as Date).getTime();
        nearestDataPoint = Math.abs(x0 - p0) > Math.abs(x0 - p1) ? d1 : d0;
      } else {
        const x0 = xOffset as number;
        const p0 = d0.xVal as number;
        const p1 = d1.xVal as number;
        nearestDataPoint = Math.abs(x0 - p0) > Math.abs(x0 - p1) ? d1 : d0;
      }

      if (!nearestDataPoint) {
        return;
      }

      const xKey =
        nearestDataPoint.xVal instanceof Date ? nearestDataPoint.xVal.getTime() : Number(nearestDataPoint.xVal);
      const found = calloutPointsByX.get(xKey);
      if (!found) {
        return;
      }

      const cx = xScale(nearestDataPoint.xVal as never) ?? 0;

      const { entries, topY } = updateCalloutMarkers(found, cx);

      if (!this.hideTooltip && entries.length > 0) {
        const hostRect = this.getBoundingClientRect();
        const anchorX = svgRect.left - hostRect.left + margins.left + cx;
        const anchorY = svgRect.top - hostRect.top + margins.top + topY;
        const isFreshShow = !this.tooltipProps.isVisible || this.tooltipProps.xLabel !== found.xLabel;
        this._currentTooltipDataPoint = { xLabel: found.xLabel, entries };
        this.tooltipProps = {
          isVisible: true,
          legend: entries[0].legend,
          yValue: entries[0].value,
          color: entries[0].color,
          xLabel: found.xLabel,
          xAxisAriaLabel: found.xAxisAriaLabel,
          entries,
          xPos: anchorX,
          yPos: anchorY,
        };
        this._positionTooltipAvoidingOverlap(anchorX, anchorY, anchorY, isFreshShow, {
          horizontalPlacement: 'side',
          gap: 15,
        });
      }
    };

    const onChartMouseLeave = () => {
      clearCalloutMarkers();
      this._clearTooltip();
    };

    overlay.addEventListener('mousemove', onOverlayMouseMove);
    svg.addEventListener('mouseleave', onChartMouseLeave);

    normalizedSeries.forEach(series => {
      series.data.forEach(point => {
        const pointCircle = createSvgElement<SVGCircleElement>('circle');
        pointCircle.classList.add('data-point-focus-target');
        pointCircle.setAttribute('cx', String(point.cx));
        pointCircle.setAttribute('cy', String(point.cy));
        pointCircle.setAttribute('r', '6');
        pointCircle.setAttribute('fill', 'transparent');
        pointCircle.setAttribute('stroke', 'transparent');
        pointCircle.setAttribute('stroke-width', '0');
        pointCircle.setAttribute('role', 'img');
        pointCircle.setAttribute(
          'aria-label',
          point.callOutAccessibilityData?.ariaLabel ??
            `${point.xLabel}, ${series.legend}, ${formatNumberValue(point.y, this.yAxisTickFormat, this.culture)}.`,
        );
        pointCircle.setAttribute('data-x-key', String(point.x instanceof Date ? point.x.getTime() : point.x));
        pointCircle.setAttribute('data-cy', String(point.cy));
        pointCircle.setAttribute('tabindex', focusablePoints.length === 0 ? '0' : '-1');
        pointCircle.setAttribute('pointer-events', 'all');
        pointCircle.addEventListener('mouseenter', onOverlayMouseMove);
        pointCircle.addEventListener('mousemove', onOverlayMouseMove);
        pointCircle.addEventListener('focus', event => {
          focusablePoints.forEach(pointEl => {
            pointEl.tabIndex = pointEl === pointCircle ? 0 : -1;
          });
          this._currentTooltipDataPoint = { legend: series.legend, x: point.x, y: point.y };
          pointCircle.setAttribute('fill', '#fff');
          pointCircle.setAttribute('stroke', series.color);
          pointCircle.setAttribute('stroke-width', '2');
          showPointTooltip(event, series, point, pointCircle);
        });
        pointCircle.addEventListener('blur', () => {
          this._currentTooltipDataPoint = null;
          pointCircle.setAttribute('fill', 'transparent');
          pointCircle.setAttribute('stroke', 'transparent');
          pointCircle.setAttribute('stroke-width', '0');
          clearCalloutMarkers();
          this._clearTooltip();
        });
        pointCircle.addEventListener('keydown', pointKeydown);
        pointCircle.addEventListener('click', () => this._focusRovingElement(focusablePoints, pointCircle));
        focusablePoints.push(pointCircle);
        focusLayer.appendChild(pointCircle);
      });
    });

    renderBottomAxisShared({
      svg,
      scale: xScale as AxisScaleLike<AxisDomain>,
      axis: xAxis as Axis<AxisDomain>,
      formatter: xFormatter,
      axisLeft: margins.left,
      axisTop: margins.top,
      innerWidth,
      innerHeight,
      tickPadding: toNumber(this.tickPadding, 6),
      isRTL: isRtl,
      rotateXAxisLabels: this.rotateXAxisLabels,
      wrapXAxisLabels: this.wrapXAxisLabels,
      hideTickOverlap: this.hideTickOverlap,
      showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
      axisLabelTooltipHandlers: {
        show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
        hide: () => this._hideAxisLabelTooltip(),
      },
      xAxisTitle: this.xAxisTitle,
      labelDominantBaseline: 'hanging',
    });

    const yFormatter = (value: number): string => {
      if (this.yAxisTickFormat) {
        try {
          return format(this.yAxisTickFormat)(value);
        } catch {
          // Fall through to default.
        }
      }
      return defaultYAxisTickFormatter(value);
    };

    // Skip left axis when all series are secondary (no primary data to scale against).
    if (primaryStackedLayers.length > 0) {
      renderPrimaryYAxisShared({
        svg,
        scale: yScale as AxisScaleLike<number>,
        axis: yAxis as unknown as Axis<number>,
        formatter: yFormatter,
        axisStartX: margins.left,
        axisTop: margins.top,
        innerHeight,
        innerWidth,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: isRtl,
        yAxisTitle: this.yAxisTitle,
      });
    }

    if (hasSecondaryY) {
      const yAxisSecondary = axisRight(yScaleSecondary).tickPadding(toNumber(this.tickPadding, 6));
      applyAxisTickConfig(
        yAxisSecondary,
        this.yAxisTickCount ?? DEFAULT_NUMERIC_Y_TICK_COUNT,
        preparedSecondaryYAxis.tickValues,
      );
      renderSecondaryYAxisShared({
        svg,
        scale: yScaleSecondary as AxisScaleLike<number>,
        axis: yAxisSecondary as unknown as Axis<number>,
        formatter: yFormatter,
        axisStartX: margins.left,
        axisTop: margins.top,
        innerHeight,
        innerWidth,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: isRtl,
        yAxisTitle: this.secondaryYAxisTitle,
        tickLabelMaxWidth: toOptionalNumber(this.secondaryYAxisTickLabelMaxWidth),
      });
    }

    this._renderAnnotations({
      svg,
      collisionLayer: plotGroup,
      margins,
      innerWidth,
      innerHeight,
      mapDataX: value => xScale((isDateAxis ? new Date(value) : Number(value)) as never),
      mapDataY: (value, axis) => (axis === 'secondary' ? yScaleSecondary : yScale)(Number(value)),
    });

    svg.appendChild(focusLayer);
    this._relocateFocusIfNeeded(focusablePoints);

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
    this.chartContainer.querySelectorAll<SVGElement>('.area-path, .area-line').forEach(element => {
      const legend = element.dataset.legend ?? '';
      const isActive = !hasSelection || highlighted.includes(legend);
      element.classList.toggle('inactive', !isActive);
      element.setAttribute('opacity', isActive ? '1' : '0.1');
    });
  }

  protected override _getHostAriaLabel(): string {
    const seriesCount = Array.isArray(this.data) ? this.data.length : 0;
    if (seriesCount === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Area chart with no data.';
    }
    return `${this.chartTitle || 'Area chart'}. ${seriesCount} series.`;
  }

  private _clearChart(): void {
    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }
}
