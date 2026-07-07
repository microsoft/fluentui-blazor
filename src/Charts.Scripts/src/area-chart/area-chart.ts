import { attr } from '@microsoft/fast-element';
import { bisector, extent } from 'd3-array';
import { axisBottom, axisLeft, axisRight, type Axis, type AxisDomain } from 'd3-axis';
import { format, formatPrefix } from 'd3-format';
import { scaleLinear, scaleTime, type ScaleLinear, type ScaleTime } from 'd3-scale';
import { area as createArea, curveMonotoneX, line as createLine, stack as createStack } from 'd3-shape';
import { timeFormat, utcFormat } from 'd3-time-format';
import type { TooltipProps } from '../utils/chart-options.js';
import { CartesianChartBase } from '../utils/cartesian-chart-base.js';
import {
  applyAxisTickConfig,
  type AxisScaleLike,
  computePreparedNumericYAxis,
  DEFAULT_REACT_NUMERIC_Y_TICK_COUNT,
  renderBottomAxisShared,
  renderPrimaryYAxisShared,
  renderSecondaryYAxisShared,
  toAxisNumber as toNumber,
  toOptionalAxisNumber as toOptionalNumber,
} from '../utils/cartesian-axis-shared.js';
import {
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

const escapeHtml = (str: string): string =>
  str.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');

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

/**
 * Default y-axis tick formatter matching React charting's `defaultYAxisTickFormatter`.
 * Uses d3 SI-prefix notation (e.g. 10k, 1.5M) for values ≥ 1 and general format for
 * small values, keeping up to 2 significant digits and trimming trailing zeros.
 */
const defaultYAxisTickFormatter = (value: number): string => {
  if (Math.abs(value) < 1) {
    return format('.2~g')(value);
  }
  return formatPrefix('.2~', value)(value);
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
  try {
    return new Intl.DateTimeFormat(chart.culture, chart.dateLocalizeOptions).format(value);
  } catch {
    return new Intl.DateTimeFormat(undefined, chart.dateLocalizeOptions).format(value);
  }
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

  /** Optional title for the secondary (right) Y axis. */
  @attr({ attribute: 'secondary-y-axis-title' })
  public secondaryYAxisTitle: string = '';

  /** Max width in px for secondary y-axis tick labels before truncating with ellipsis. */
  @attr({ attribute: 'secondary-y-axis-tick-label-max-width' })
  public secondaryYAxisTickLabelMaxWidth?: number | string;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = [
      'data',
      'enableGradient',
      'mode',
      'secondaryYAxisTitle',
      'secondaryYAxisTickLabelMaxWidth',
    ] as const;
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

  public get tooltipInlineTransform(): string {
    // Position the tooltip to the right (LTR) or left (RTL) of the hover crosshair so it
    // does not cover the indicator line and hover dots.  React's Callout uses
    // DirectionalHint.topAutoEdge anchored on the highlighted circle, which has the same
    // visual result: the tooltip appears beside the data point, not on top of it.
    return this._isRTL ? 'translateX(calc(-100% - 16px))' : 'translateX(16px)';
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

  protected secondaryYAxisTitleChanged(): void {
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
        return {
          x,
          y: point.y,
          xLabel:
            x instanceof Date ? formatDateValue(this, x) : formatNumberValue(x, this.xAxisTickFormat, this.culture),
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
    // In RTL the primary Y axis moves to the right side and the secondary to the left,
    // so the two margin sides swap compared to LTR.
    const primaryAxisSpace = defaultMargins.left; // 60 px — space for Y axis ticks + labels
    const secondaryAxisSpace = 70; // extra space when a secondary Y axis is present
    const leftMargin = isRtl ? (hasSecondaryY ? secondaryAxisSpace : defaultMargins.right) : primaryAxisSpace;
    const rightMargin = isRtl ? primaryAxisSpace : hasSecondaryY ? secondaryAxisSpace : defaultMargins.right;
    const innerWidth = Math.max(width - leftMargin - rightMargin, 1);
    const innerHeight = Math.max(height - defaultMargins.top - defaultMargins.bottom, 1);

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
    const yTickCount = toNumber(this.yAxisTickCount, DEFAULT_REACT_NUMERIC_Y_TICK_COUNT);

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
      xScale = scaleTime().domain([domainMin, domainMax]).range([0, innerWidth]);
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
      xScale = scaleLinear().domain([xMin, xMax]).range([0, innerWidth]);
      if (this.roundedTicks) {
        xScale.nice();
      }
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
      const secMin = secValues.length > 0 ? Math.min(0, ...secValues) : 0;
      let secMax = secValues.length > 0 ? Math.max(0, ...secValues) : 1;
      if (secMin === secMax) {
        secMax += 1;
      }
      preparedSecondaryYAxis = computePreparedNumericYAxis({
        minValue: secMin,
        maxValue: secMax,
        tickCount: yTickCount,
        roundedTicks: this.roundedTicks,
      });
      yScaleSecondary = scaleLinear()
        .domain([preparedSecondaryYAxis.domainMin, preparedSecondaryYAxis.domainMax])
        .range([innerHeight, 0]);
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

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart-svg');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const plotGroup = createSvgElement<SVGGElement>('g');
    plotGroup.setAttribute('transform', `translate(${leftMargin}, ${defaultMargins.top})`);
    svg.appendChild(plotGroup);

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
      this.yAxisTickCount ?? DEFAULT_REACT_NUMERIC_Y_TICK_COUNT,
      this.yAxisTickValues ?? preparedPrimaryYAxis.tickValues,
    );

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
      callOutAriaLabel?: string;
    };
    type CalloutPoint = { xLabel: string; cx: number; xAxisAriaLabel?: string; entries: (CalloutEntry | undefined)[] };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const calloutPointsByX = new Map<number, CalloutPoint>();
    normalizedSeries.forEach((series, si) => {
      const layer = stackedLayers[si];
      series.data.forEach((point, di) => {
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
        const stackedY1 = (layer[di] as any)?.[1] ?? point.y;
        // Store at series index so sparse datasets don't misalign entries.
        calloutPointsByX.get(key)!.entries[si] = {
          legend: series.legend,
          color: series.color,
          y: point.y,
          stackedY1,
          isSecondaryY: isSecondaryByIndex[si],
          callOutAriaLabel: point.callOutAccessibilityData?.ariaLabel,
        };
      });
    });

    // Bisector mirrors React's: bisector((d) => d.x).left on the first series data,
    // then compare d0/d1 neighbors to pick the closer domain value.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const bisect = bisector((d: any) => d.xVal).left;

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
    const hoverLineY1 = defaultMargins.top / 2;
    const hoverLineY2 = height - defaultMargins.bottom / 2;
    hoverLine.setAttribute('y1', String(hoverLineY1));
    hoverLine.setAttribute('y2', String(hoverLineY2));
    hoverLine.style.display = 'none';
    svg.appendChild(hoverLine);

    const hoverDots = normalizedSeries.map(series => {
      const dot = createSvgElement<SVGCircleElement>('circle');
      dot.classList.add('hover-dot');
      dot.setAttribute('r', '6');
      dot.setAttribute('fill', '#fff');
      dot.setAttribute('stroke', series.color);
      dot.setAttribute('stroke-width', '2');
      dot.style.display = 'none';
      plotGroup.appendChild(dot);
      return dot;
    });

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
      const localX = event.clientX - svgRect.left - leftMargin;

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

      // Position the vertical intercept line.
      const svgX = leftMargin + cx;
      hoverLine.setAttribute('x1', String(svgX));
      hoverLine.setAttribute('x2', String(svgX));
      hoverLine.style.display = '';

      // Position hover dots at the top of each stacked layer; collect tooltip entries.
      let topY = innerHeight;
      const entries: TooltipEntry[] = [];
      normalizedSeries.forEach((series, si) => {
        const entry = found.entries[si];
        const active = entry && this._shouldShowTooltip(series.legend);
        if (active) {
          const dotScale = entry.isSecondaryY ? yScaleSecondary : yScale;
          const cy = dotScale(entry.stackedY1);
          hoverDots[si].setAttribute('cx', String(cx));
          hoverDots[si].setAttribute('cy', String(cy));
          hoverDots[si].style.display = '';
          topY = Math.min(topY, cy);
          entries.push({
            legend: series.legend,
            color: series.color,
            value: formatNumberValue(entry.y, this.yAxisTickFormat, this.culture),
            callOutAriaLabel: entry.callOutAriaLabel,
          });
        } else {
          hoverDots[si].style.display = 'none';
        }
      });

      // Apply .hovered class to series line paths.
      plotGroup.querySelectorAll<SVGPathElement>('.area-line').forEach(p => p.classList.add('hovered'));

      if (!this.hideTooltip && entries.length > 0) {
        const hostRect = this.getBoundingClientRect();
        const anchorX = svgRect.left - hostRect.left + leftMargin + cx;
        const anchorY = svgRect.top - hostRect.top + defaultMargins.top + topY;
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
        this._positionTooltipFromAnchor(anchorX, anchorY, { outputAnchorX: true, preferredVertical: 'above' });
      }
    };

    const onOverlayMouseLeave = () => {
      hoverLine.style.display = 'none';
      hoverDots.forEach(dot => {
        dot.style.display = 'none';
      });
      plotGroup.querySelectorAll<SVGPathElement>('.area-line').forEach(p => p.classList.remove('hovered'));
      this._clearTooltip();
    };

    overlay.addEventListener('mousemove', onOverlayMouseMove);
    overlay.addEventListener('mouseleave', onOverlayMouseLeave);

    renderBottomAxisShared({
      svg,
      scale: xScale as AxisScaleLike<AxisDomain>,
      axis: xAxis as Axis<AxisDomain>,
      formatter: xFormatter,
      axisLeft: leftMargin,
      axisTop: defaultMargins.top,
      innerWidth,
      innerHeight,
      tickPadding: toNumber(this.tickPadding, 6),
      rotateXAxisLabels: this.rotateXAxisLabels,
      wrapXAxisLabels: this.wrapXAxisLabels,
      hideTickOverlap: this.hideTickOverlap,
      showXAxisLabelsTooltip: this.showXAxisLabelsTooltip,
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
        axisStartX: leftMargin,
        axisTop: defaultMargins.top,
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
        this.yAxisTickCount ?? DEFAULT_REACT_NUMERIC_Y_TICK_COUNT,
        preparedSecondaryYAxis.tickValues,
      );
      renderSecondaryYAxisShared({
        svg,
        scale: yScaleSecondary as AxisScaleLike<number>,
        axis: yAxisSecondary as unknown as Axis<number>,
        formatter: yFormatter,
        axisStartX: leftMargin,
        axisTop: defaultMargins.top,
        innerHeight,
        innerWidth,
        tickPadding: toNumber(this.tickPadding, 6),
        isRTL: isRtl,
        yAxisTitle: this.secondaryYAxisTitle,
        tickLabelMaxWidth: toOptionalNumber(this.secondaryYAxisTickLabelMaxWidth),
      });
    }

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
