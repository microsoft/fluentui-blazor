import { attr } from '@microsoft/fast-element';
import { max } from 'd3-array';
import { type Axis, axisBottom, axisLeft, axisRight } from 'd3-axis';
import { format } from 'd3-format';
import { type ScaleBand, scaleBand, type ScaleLinear, scaleLinear, type ScaleLogarithmic, scaleLog } from 'd3-scale';
import { line as createLine } from 'd3-shape';
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
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type {
  GroupedVerticalBarChartData,
  GroupedVerticalBarChartLineDataPoint,
} from './grouped-vertical-bar-chart.options.js';
import { VerticalBarChartBase } from '../utils/vertical-bar-chart-base.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

export type TooltipEntry = { legend: string; color: string; value: string };
type TooltipState = TooltipProps & { xValue: string; entries: TooltipEntry[] };
type LinePlotPoint = {
  group: GroupedVerticalBarChartData;
  entry: GroupedVerticalBarChartLineDataPoint;
  xCenter: number;
};

const defaultMargins = { top: 40, right: 20, bottom: 50, left: 60 };
const defaultBarWidth = 16;
const groupInnerPadding = 0.1;

const calcTotalBandUnits = (numBands: number, innerPadding: number): number => {
  return numBands + Math.max(numBands - 1, 0) * (innerPadding / (1 - innerPadding));
};

const calcRequiredWidth = (bandwidth: number, numBands: number, innerPadding: number): number =>
  bandwidth * calcTotalBandUnits(numBands, innerPadding);

const formatNumberValue = (value: number, specifier: string | undefined, culture: string | undefined): string => {
  if (specifier) {
    try {
      return format(specifier)(value);
    } catch {
      // Fall back to locale formatting below.
    }
  }
  return createNumberFormat(culture || undefined, {
    maximumFractionDigits: Math.abs(value) >= 1000 ? 1 : 2,
    notation: Math.abs(value) >= 1000 ? 'compact' : 'standard',
  }).format(value);
};

/** @public */
export class GroupedVerticalBarChart extends VerticalBarChartBase {
  public declare tooltipProps: TooltipState;

  private _renderedBars: Array<{ legend: string; element: SVGRectElement }> = [];

  @attr({ converter: jsonConverter })
  public data!: GroupedVerticalBarChartData[];

  @attr({ attribute: 'is-callout-for-stack', mode: 'boolean' })
  public isCalloutForStack: boolean = false;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'isCalloutForStack'] as const;
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

  protected isCalloutForStackChanged(): void {
    this._clearTooltip();
  }

  protected override tooltipPropsChanged(oldValue: TooltipProps, newValue: TooltipProps): void {
    super.tooltipPropsChanged(oldValue, newValue);
    if (newValue.isVisible && !this.hideTooltip) {
      const state = newValue as TooltipState;
      const groupAccessibilityLabel = this.isCalloutForStack
        ? (this._currentTooltipDataPoint as GroupedVerticalBarChartData | null)?.stackCallOutAccessibilityData
            ?.ariaLabel
        : undefined;
      this.liveRegionText =
        groupAccessibilityLabel ??
        [state.xValue, ...state.entries.map(entry => `${entry.legend}: ${entry.value}`)].filter(Boolean).join('. ');
    }
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
    this._renderedBars = [];

    const groups = Array.isArray(this.data) ? this.data : [];
    if (groups.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 600);
    const height = toNumber(this.height, 300);
    const hasSecondaryY = groups.some(
      group =>
        group.series.some(point => point.useSecondaryYScale) || group.lineData?.some(entry => entry.useSecondaryYScale),
    );
    const { svg, plotGroup, margins, innerWidth, innerHeight } = this._createCartesianRenderContext({
      width,
      height,
      defaultMargins,
      hasSecondaryYAxis: hasSecondaryY,
    });

    const groupedByCategory = new Map<string, number[]>();
    groups.forEach(group => {
      groupedByCategory.set(
        group.xAxisPoint,
        group.series.map(point => point.data),
      );
    });
    const groupDomain = sortCategoryGroups(
      Array.from(groupedByCategory.entries()).map(([key, values]) => ({ key, points: values })),
      this.xAxisCategoryOrder,
      groups.map(group => group.xAxisPoint),
      group => group.points,
    ).map(group => group.key);
    const keyDomain = Array.from(new Set(groups.flatMap(group => group.series.map(point => point.key))));
    const legendByKey = new Map<string, string>();
    groups.flatMap(group => group.series).forEach(point => legendByKey.set(point.key, point.legend ?? point.key));
    const groupWidthInBarUnits = calcTotalBandUnits(keyDomain.length, groupInnerPadding);
    const xAxisInnerPadding = toOptionalNumber(this.xAxisInnerPadding) ?? 2 / (2 + groupWidthInBarUnits);
    const configuredOuterPadding = toOptionalNumber(this.xAxisOuterPadding);
    const xAxisOuterPadding = configuredOuterPadding ?? 0;
    let xRangeStart = 0;
    let xRangeEnd = innerWidth;
    let centeredBarWidth: number | undefined;
    if (configuredOuterPadding === undefined && this.barWidth !== 'auto') {
      const fixedBarWidth = resolveBarWidth(this.barWidth, this.maxBarWidth, Number.POSITIVE_INFINITY, defaultBarWidth);
      const requiredGroupWidth = fixedBarWidth * groupWidthInBarUnits;
      const requiredChartWidth = calcRequiredWidth(requiredGroupWidth, groupDomain.length, xAxisInnerPadding);
      const domainMargin = Math.max((innerWidth - requiredChartWidth) / 2, 0);
      xRangeStart = domainMargin;
      xRangeEnd = innerWidth - domainMargin;
      if (domainMargin > 0) {
        centeredBarWidth = fixedBarWidth;
      }
    }
    const xScale = scaleBand<string>()
      .domain(groupDomain)
      .range(this._isRTL ? [xRangeEnd, xRangeStart] : [xRangeStart, xRangeEnd])
      .paddingInner(xAxisInnerPadding)
      .paddingOuter(xAxisOuterPadding);
    const availableBarWidth = xScale.bandwidth() / groupWidthInBarUnits;
    const actualBarWidth =
      centeredBarWidth ?? resolveBarWidth(this.barWidth, this.maxBarWidth, availableBarWidth, defaultBarWidth);
    const effectiveGroupWidth = actualBarWidth * groupWidthInBarUnits;
    const innerScale = scaleBand<string>()
      .domain(keyDomain)
      .range(this._isRTL ? [effectiveGroupWidth, 0] : [0, effectiveGroupWidth])
      .paddingInner(groupInnerPadding);
    const primaryLineValues = groups
      .flatMap(group => group.lineData ?? [])
      .filter(entry => !entry.useSecondaryYScale)
      .map(entry => entry.y);
    const primaryValues = [
      ...groups.flatMap(group => group.series.filter(point => !point.useSecondaryYScale).map(point => point.data)),
      ...primaryLineValues,
    ];
    const maxY = max(primaryValues) ?? 0;
    const minY = Math.min(...primaryValues);
    const useLogPrimary = this.yScaleType === 'log' && primaryValues.every(value => value > 0);
    const preparedYAxis = computePreparedNumericYAxis({
      minValue: toOptionalNumber(this.yMinValue) ?? (this.supportNegativeData ? Math.min(minY, 0) : 0),
      maxValue:
        toOptionalNumber(this.yMaxValue) ??
        (this.supportNegativeData && maxY <= 0 ? Math.max(maxY, -1) : Math.max(maxY, 1)),
      tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
      roundedTicks: this.roundedTicks,
    });
    const yScale: ScaleLinear<number, number> | ScaleLogarithmic<number, number> = useLogPrimary
      ? scaleLog()
          .domain([Math.max(minY / 10, Number.MIN_VALUE), maxY])
          .range([innerHeight, 0])
      : scaleLinear().domain([preparedYAxis.domainMin, preparedYAxis.domainMax]).range([innerHeight, 0]);

    const secondaryValues = [
      ...groups.flatMap(group =>
        group.series.filter(point => point.useSecondaryYScale && Number.isFinite(point.data)).map(point => point.data),
      ),
      ...groups
        .flatMap(group => group.lineData ?? [])
        .filter(entry => entry.useSecondaryYScale && Number.isFinite(entry.y))
        .map(entry => entry.y),
    ];
    const secondaryYAxis = createPreparedNumericContinuousScale({
      values: secondaryValues,
      range: [innerHeight, 0],
      scaleType: hasSecondaryY ? this.secondaryYScaleType : 'default',
      tickCount: toNumber(this.yAxisTickCount, DEFAULT_NUMERIC_Y_TICK_COUNT),
      roundedTicks: this.roundedTicks,
    });
    const preparedSecondaryYAxis = secondaryYAxis.preparedAxis;
    const yScaleSecondary = secondaryYAxis.scale;
    const useLogSecondary = secondaryYAxis.isLogarithmic;

    const firstPoint = groups.flatMap(group => group.series)[0];
    const singleColor = this.useSingleColor ? resolveChartColor(firstPoint?.color, this.colors, 0) : undefined;
    const colorMap = new Map<string, string>();
    keyDomain.forEach((key, index) => {
      const match = groups.flatMap(group => group.series).find(point => point.key === key);
      colorMap.set(key, singleColor ?? resolveChartColor(match?.color, this.colors, index));
    });
    const lineLegendOrder = Array.from(
      new Set(groups.flatMap(group => (group.lineData ?? []).map(entry => entry.legend))),
    );
    const lineColorMap = new Map<string, string>();
    lineLegendOrder.forEach((legend, index) => {
      const match = groups.flatMap(group => group.lineData ?? []).find(entry => entry.legend === legend);
      lineColorMap.set(legend, resolveChartColor(match?.color, this.colors, index, 10));
    });

    const defs = createSvgElement<SVGDefsElement>('defs');
    svg.appendChild(defs);

    const xAxis = axisBottom(xScale).tickPadding(toNumber(this.tickPadding, 6));
    applyAxisTickConfig(
      xAxis,
      this.xAxisTickCount,
      this.tickValues?.map(value => String(value)),
    );
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

    const cornerRadius = this.roundCorners ? 3 : 0;

    let pointIndex = 0;
    groups.forEach(group => {
      const groupX = (xScale(group.xAxisPoint) ?? 0) + (xScale.bandwidth() - effectiveGroupWidth) / 2;
      group.series.forEach(point => {
        const slotX = innerScale(point.key) ?? 0;
        const offset = (innerScale.bandwidth() - actualBarWidth) / 2;
        const color = colorMap.get(point.key) ?? getNextColor(0, 0);
        const legend = legendByKey.get(point.key) ?? point.key;
        const pointScale = point.useSecondaryYScale ? yScaleSecondary : yScale;
        const usesLogScale = point.useSecondaryYScale ? useLogSecondary : useLogPrimary;
        const pointY = pointScale(point.data);
        const baselineY = usesLogScale ? innerHeight : pointScale(0);
        const barTop = Math.min(pointY, baselineY);
        const barBottom = Math.max(pointY, baselineY);

        const rect = createSvgElement<SVGRectElement>('rect');
        rect.classList.add('bar');
        rect.dataset.legend = legend;
        rect.setAttribute('x', String(groupX + slotX + offset));
        rect.setAttribute('y', String(barTop));
        rect.setAttribute('width', String(actualBarWidth));
        rect.setAttribute('height', String(Math.max(barBottom - barTop, 0)));
        const gradientId = appendVerticalGradient(
          defs,
          `gvbc-gradient-${pointIndex++}`,
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
          point.callOutAccessibilityData?.ariaLabel ?? `${group.xAxisPoint}. ${legend}, ${point.data}.`,
        );
        if (this.strokeWidth !== undefined) {
          rect.setAttribute('stroke-width', String(this.strokeWidth));
          rect.setAttribute('stroke', color);
        }
        const showTooltip = (event?: MouseEvent) => {
          if (!this._shouldShowTooltip(legend) || this.hideTooltip) {
            return;
          }
          const hostRect = this.getBoundingClientRect();
          const svgRect = svg.getBoundingClientRect();
          const anchorX = svgRect.left - hostRect.left + margins.left + groupX + slotX + offset + actualBarWidth / 2;
          const minY = svgRect.top - hostRect.top + margins.top + barTop;
          const maxY = svgRect.top - hostRect.top + margins.top + barBottom;
          const anchorY = event ? Math.min(Math.max(event.clientY - hostRect.top, minY), maxY) : (minY + maxY) / 2;
          const isFreshShow = !this.tooltipProps.isVisible;
          this._currentTooltipDataPoint = this.isCalloutForStack ? group : { ...point, xAxisPoint: group.xAxisPoint };
          const entries: TooltipEntry[] = (this.isCalloutForStack ? group.series : [point])
            .filter(entry => this._shouldShowTooltip(legendByKey.get(entry.key) ?? entry.key))
            .map(entry => ({
              legend: legendByKey.get(entry.key) ?? entry.key,
              color: colorMap.get(entry.key) ?? getNextColor(0, 0),
              value: entry.yAxisCalloutData ?? formatNumberValue(entry.data, this.yAxisTickFormat, this.culture),
            }));
          if (this.isCalloutForStack) {
            entries.push(
              ...(group.lineData ?? [])
                .filter(entry => this._shouldShowTooltip(entry.legend))
                .map(entry => ({
                  legend: entry.legend,
                  color: lineColorMap.get(entry.legend) ?? getNextColor(0, 10),
                  value: entry.yAxisCalloutData ?? formatNumberValue(entry.y, this.yAxisTickFormat, this.culture),
                })),
            );
          }
          this.tooltipProps = {
            isVisible: true,
            legend,
            xValue:
              typeof point.xAxisCalloutData === 'string' && point.xAxisCalloutData
                ? point.xAxisCalloutData
                : group.xAxisPoint,
            yValue: point.yAxisCalloutData ?? formatNumberValue(point.data, this.yAxisTickFormat, this.culture),
            color,
            xPos: anchorX,
            yPos: anchorY,
            entries,
          };
          this._positionTooltipAvoidingOverlap(anchorX, minY, maxY, isFreshShow);
        };
        rect.addEventListener('mouseenter', showTooltip);
        rect.addEventListener('mousemove', showTooltip);
        rect.addEventListener('mouseleave', () => this._clearTooltip());
        rect.addEventListener('focus', () => showTooltip());
        rect.addEventListener('blur', () => this._clearTooltip());
        rect.addEventListener('click', () => {
          this._focusRovingElement(
            this._renderedBars.map(bar => bar.element),
            rect,
          );
          point.onClick?.();
        });
        rect.addEventListener('keydown', (event: KeyboardEvent) => {
          if (event.key === 'Enter' || event.key === ' ') {
            event.preventDefault();
            point.onClick?.();
          } else {
            this._rovingKeydown(
              this._renderedBars.map(bar => bar.element),
              event,
            );
          }
        });
        plotGroup.appendChild(rect);
        this._renderedBars.push({ legend, element: rect });

        if (!this.hideLabels) {
          const label = createSvgElement<SVGTextElement>('text');
          label.classList.add('bar-label');
          label.dataset.legend = legend;
          label.setAttribute('x', String(groupX + slotX + offset + actualBarWidth / 2));
          label.setAttribute('y', String(point.data < 0 ? barBottom + 12 : barTop - 6));
          label.setAttribute('text-anchor', 'middle');
          label.textContent = point.barLabel ?? formatNumberValue(point.data, this.yAxisTickFormat, this.culture);
          plotGroup.appendChild(label);
        }
      });
    });

    lineLegendOrder.forEach(legend => {
      const points = groups.reduce<LinePlotPoint[]>((result, group) => {
        const entry = group.lineData?.find(item => item.legend === legend);
        const groupX = xScale(group.xAxisPoint);
        if (entry && groupX !== undefined && Number.isFinite(entry.y)) {
          result.push({ group, entry, xCenter: groupX + xScale.bandwidth() / 2 });
        }
        return result;
      }, []);
      if (points.length === 0) return;

      const color = lineColorMap.get(legend) ?? getNextColor(0, 10);
      const getScale = (entry: GroupedVerticalBarChartLineDataPoint) =>
        entry.useSecondaryYScale ? yScaleSecondary : yScale;
      const pathBuilder = createLine<LinePlotPoint>()
        .x(point => point.xCenter)
        .y(point => getScale(point.entry)(point.entry.y));
      const resolvedLineStrokeWidth = Number(this.lineStrokeWidth ?? 3);
      const resolvedLineBorderWidth = Number(this.lineBorderWidth ?? 0);
      if (resolvedLineBorderWidth > 0) {
        const borderPath = createSvgElement<SVGPathElement>('path');
        borderPath.classList.add('line-border');
        borderPath.dataset.legend = legend;
        borderPath.setAttribute('fill', 'none');
        borderPath.setAttribute('stroke', this.lineBorderColor ?? 'var(--colorNeutralBackground1, #fff)');
        borderPath.setAttribute('stroke-width', String(resolvedLineStrokeWidth + resolvedLineBorderWidth * 2));
        borderPath.setAttribute('stroke-linecap', this.lineStrokeLinecap ?? 'square');
        if (this.lineStrokeDasharray !== undefined)
          borderPath.setAttribute('stroke-dasharray', String(this.lineStrokeDasharray));
        if (this.lineStrokeDashoffset !== undefined)
          borderPath.setAttribute('stroke-dashoffset', String(this.lineStrokeDashoffset));
        borderPath.setAttribute('d', pathBuilder(points) ?? '');
        plotGroup.appendChild(borderPath);
      }
      const path = createSvgElement<SVGPathElement>('path');
      path.classList.add('line-path');
      path.dataset.legend = legend;
      path.setAttribute('fill', 'none');
      path.setAttribute('stroke', color);
      path.setAttribute('stroke-width', String(resolvedLineStrokeWidth));
      path.setAttribute('stroke-linecap', this.lineStrokeLinecap ?? 'square');
      if (this.lineStrokeDasharray !== undefined)
        path.setAttribute('stroke-dasharray', String(this.lineStrokeDasharray));
      if (this.lineStrokeDashoffset !== undefined)
        path.setAttribute('stroke-dashoffset', String(this.lineStrokeDashoffset));
      path.setAttribute('d', pathBuilder(points) ?? '');
      plotGroup.appendChild(path);

      points.forEach(point => {
        const marker = createSvgElement<SVGCircleElement>('circle');
        marker.classList.add('line-marker');
        marker.dataset.legend = legend;
        marker.setAttribute('cx', String(point.xCenter));
        marker.setAttribute('cy', String(getScale(point.entry)(point.entry.y)));
        marker.setAttribute('r', '5');
        marker.setAttribute('fill', 'var(--colorNeutralBackground1, #fff)');
        marker.setAttribute('stroke', color);
        marker.setAttribute('stroke-width', '2');
        marker.setAttribute('role', 'img');
        marker.setAttribute(
          'aria-label',
          point.entry.callOutAccessibilityData?.ariaLabel ?? `${point.group.xAxisPoint}. ${legend}, ${point.entry.y}.`,
        );
        const showTooltip = () => {
          if (!this._shouldShowTooltip(legend) || this.hideTooltip) return;
          const hostRect = this.getBoundingClientRect();
          const svgRect = svg.getBoundingClientRect();
          const value =
            point.entry.yAxisCalloutData ?? formatNumberValue(point.entry.y, this.yAxisTickFormat, this.culture);
          const entries: TooltipEntry[] = this.isCalloutForStack
            ? [
                ...point.group.series
                  .filter(entry => this._shouldShowTooltip(legendByKey.get(entry.key) ?? entry.key))
                  .map(entry => ({
                    legend: legendByKey.get(entry.key) ?? entry.key,
                    color: colorMap.get(entry.key) ?? getNextColor(0, 0),
                    value: entry.yAxisCalloutData ?? formatNumberValue(entry.data, this.yAxisTickFormat, this.culture),
                  })),
                ...(point.group.lineData ?? [])
                  .filter(entry => this._shouldShowTooltip(entry.legend))
                  .map(entry => ({
                    legend: entry.legend,
                    color: lineColorMap.get(entry.legend) ?? getNextColor(0, 10),
                    value: entry.yAxisCalloutData ?? formatNumberValue(entry.y, this.yAxisTickFormat, this.culture),
                  })),
              ]
            : [{ legend, color, value }];
          this._currentTooltipDataPoint = this.isCalloutForStack ? point.group : point.entry;
          this.tooltipProps = {
            isVisible: true,
            legend,
            xValue: point.group.xAxisPoint,
            yValue: value,
            color,
            xPos: svgRect.left - hostRect.left + margins.left + point.xCenter,
            yPos: svgRect.top - hostRect.top + margins.top + getScale(point.entry)(point.entry.y),
            entries,
          };
        };
        marker.addEventListener('mouseenter', showTooltip);
        marker.addEventListener('focus', showTooltip);
        marker.addEventListener('mouseleave', () => this._clearTooltip());
        marker.addEventListener('blur', () => this._clearTooltip());
        marker.addEventListener('click', () => point.entry.onClick?.());
        plotGroup.appendChild(marker);
      });
    });

    renderBottomAxisShared({
      svg,
      scale: xScale,
      axis: xAxis,
      formatter: value => value,
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
        show: (target, fullLabel) => this._showAxisLabelTooltip(target, fullLabel),
        hide: () => this._hideAxisLabelTooltip(),
      },
      xAxisTitle: this.xAxisTitle,
    });
    renderPrimaryYAxisShared({
      svg,
      scale: yScale,
      axis: yAxis as unknown as Axis<number>,
      formatter: value => formatNumberValue(value, this.yAxisTickFormat, this.culture),
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
        formatter: value => formatNumberValue(value, this.yAxisTickFormat, this.culture),
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
        const bandX = xScale(String(value));
        return bandX === undefined ? undefined : bandX + xScale.bandwidth() / 2;
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
      ...keyDomain.map(key => ({
        legend: legendByKey.get(key) ?? key,
        color: colorMap.get(key) ?? getNextColor(0, 0),
      })),
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
      .querySelectorAll<SVGElement>('.bar, .bar-label, .line-border, .line-path, .line-marker')
      .forEach(element => {
        const legend = element.dataset.legend ?? '';
        const isActive = !hasSelection || highlighted.includes(legend);
        element.classList.toggle('inactive', !isActive);
        element.setAttribute('opacity', isActive ? '1' : '0.1');
      });

    this._renderedBars.forEach(({ legend, element }) => {
      if (hasSelection && !highlighted.includes(legend)) {
        element.tabIndex = -1;
      }
    });
    const activeBars = this._renderedBars
      .filter(({ legend }) => !hasSelection || highlighted.includes(legend))
      .map(bar => bar.element);
    if (activeBars.length > 0 && !activeBars.some(element => element.tabIndex === 0)) {
      activeBars[0].tabIndex = 0;
    }
    this._relocateFocusIfNeeded(this._renderedBars.map(bar => bar.element));
  }

  protected override _getHostAriaLabel(): string {
    const count = Array.isArray(this.data) ? this.data.length : 0;
    if (count === 0) {
      return this.chartTitle ? `${this.chartTitle}. No data.` : 'Grouped vertical bar chart with no data.';
    }
    return `${this.chartTitle || 'Grouped vertical bar chart'}. ${count} groups.`;
  }

  private _clearChart(): void {
    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }
}
