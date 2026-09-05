import type { Axis, AxisDomain } from 'd3-axis';
import { nice as d3Nice, ticks as d3Ticks } from 'd3-array';
import { scaleLinear, scaleLog, type ScaleLinear, type ScaleLogarithmic } from 'd3-scale';
import type { AxisCategoryOrder, AxisScaleType } from './chart-options.js';
import { SVG_NAMESPACE_URI, wrapText } from './chart-helpers.js';

export const DEFAULT_NUMERIC_Y_TICK_COUNT = 4;

export type AxisScaleLike<Domain extends AxisDomain> = {
  domain(): Domain[];
  ticks?: (count?: number) => Domain[];
  tickFormat?: (count?: number) => (value: Domain) => string;
  bandwidth?: () => number;
  step?: () => number;
  (value: Domain): number | undefined;
};

export type AxisLabelTooltipHandlers = {
  show: (target: SVGTextElement, fullLabel: string) => void;
  hide: () => void;
};

export const getAxisTickValues = <Domain extends AxisDomain>(
  axis: Axis<Domain>,
  scale: AxisScaleLike<Domain>,
): Domain[] => {
  const explicit = axis.tickValues();
  if (explicit) {
    return Array.from(explicit as Iterable<Domain>);
  }
  if (typeof scale.ticks === 'function') {
    const [count] = axis.tickArguments() as [number?];
    return scale.ticks(count);
  }
  return scale.domain();
};

const getAxisTickLabelFormatter = <Domain extends AxisDomain>(
  axis: Axis<Domain>,
  scale: AxisScaleLike<Domain>,
): ((value: Domain, index: number) => string) | undefined => {
  const axisFormatter = axis.tickFormat();
  if (axisFormatter) {
    return axisFormatter;
  }
  if (axis.tickValues() || typeof scale.tickFormat !== 'function') {
    return undefined;
  }
  const [count] = axis.tickArguments() as [number?];
  const scaleFormatter = scale.tickFormat(count);
  return value => scaleFormatter(value);
};

export const getAxisPosition = <Domain extends AxisDomain>(scale: AxisScaleLike<Domain>, value: Domain): number => {
  const start = scale(value) ?? 0;
  return typeof scale.bandwidth === 'function' ? start + scale.bandwidth() / 2 : start;
};

export const applyAxisTickConfig = <Domain extends AxisDomain>(
  axis: Axis<Domain>,
  tickCount: number | string | undefined,
  tickValues: readonly Domain[] | undefined,
) => {
  const parsedCount = Number(tickCount);
  if (Number.isFinite(parsedCount) && parsedCount > 0) {
    axis.ticks(parsedCount);
  }
  if (tickValues?.length) {
    axis.tickValues(tickValues as Iterable<Domain>);
  }
};

export const toAxisNumber = (value: number | string | undefined, fallback: number): number => {
  if (value === undefined || value === null || value === '') {
    return fallback;
  }
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
};

export const toOptionalAxisNumber = (value: number | string | undefined): number | undefined => {
  if (value === undefined || value === null || value === '') {
    return undefined;
  }
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : undefined;
};

export type NumericContinuousScale = ScaleLinear<number, number> | ScaleLogarithmic<number, number>;

export type NumericContinuousScaleOptions = {
  domainMin: number;
  domainMax: number;
  range: [number, number];
  scaleType?: AxisScaleType;
  roundedTicks?: boolean;
};

export const createNumericContinuousScale = ({
  domainMin,
  domainMax,
  range,
  scaleType = 'default',
  roundedTicks = false,
}: NumericContinuousScaleOptions): { scale: NumericContinuousScale; isLogarithmic: boolean } => {
  const isLogarithmic = scaleType === 'log' && domainMin > 0 && domainMax > 0;
  const scale = isLogarithmic
    ? scaleLog().domain([domainMin, domainMax]).range(range)
    : scaleLinear().domain([domainMin, domainMax]).range(range);

  if (roundedTicks) {
    scale.nice();
  }

  return { scale, isLogarithmic };
};

const handleFloatingPointPrecisionError = (value: number): number => {
  const rounded = Math.round(value);
  return Math.abs(value - rounded) < 1e-6 ? rounded : value;
};

const isPowerOf10 = (value: number): boolean => {
  const absValue = Math.abs(value);
  if (absValue === 0) {
    return false;
  }
  const exponent = Math.log10(absValue);
  return Math.abs(exponent - Math.round(exponent)) < 1e-9;
};

const calculateRoundedTicks = (minValue: number, maxValue: number, tickCount: number): number[] => {
  const finalMin = minValue >= 0 && minValue === maxValue ? 0 : minValue;
  const finalMax = minValue < 0 && minValue === maxValue ? 0 : maxValue;
  const niced = d3Nice(finalMin, finalMax, tickCount);
  const ticks = d3Ticks(niced[0], niced[niced.length - 1], tickCount).map(handleFloatingPointPrecisionError);
  if (ticks.length > 0 && ticks[ticks.length - 1] > finalMax && isPowerOf10(finalMax)) {
    ticks.pop();
  }
  return ticks;
};

const prepareNumericDatapoints = (
  maxValue: number,
  minValue: number,
  tickCount: number,
  isIntegralDataset: boolean,
): number[] => {
  const span = maxValue - minValue;
  let interval = isIntegralDataset
    ? Math.ceil(span / tickCount)
    : span / tickCount >= 1
    ? Math.ceil(span / tickCount)
    : span / tickCount;

  if (!Number.isFinite(interval) || interval <= 0) {
    interval = 1;
  }

  const points: number[] = [minValue < 0 && maxValue >= 0 ? 0 : minValue];
  if (points[0] === minValue) {
    points.push(minValue + interval);
  }

  if (minValue < 0 && maxValue >= 0) {
    while (points[points.length - 1] > minValue) {
      points.push(points[points.length - 1] - interval);
    }
    points.reverse();
  }

  while (points[points.length - 1] < maxValue) {
    points.push(points[points.length - 1] + interval);
  }

  return points.map(handleFloatingPointPrecisionError);
};

export type PreparedNumericYAxisOptions = {
  minValue: number;
  maxValue: number;
  tickCount?: number;
  isIntegralDataset?: boolean;
  roundedTicks?: boolean;
};

export type PreparedNumericYAxis = {
  domainMin: number;
  domainMax: number;
  tickValues: number[];
};

export const computePreparedNumericYAxis = ({
  minValue,
  maxValue,
  tickCount = DEFAULT_NUMERIC_Y_TICK_COUNT,
  isIntegralDataset = false,
  roundedTicks = false,
}: PreparedNumericYAxisOptions): PreparedNumericYAxis => {
  const safeTickCount =
    Number.isFinite(tickCount) && tickCount > 0 ? Math.floor(tickCount) : DEFAULT_NUMERIC_Y_TICK_COUNT;
  const low = Math.min(minValue, maxValue);
  let high = Math.max(minValue, maxValue);
  if (low === high) {
    high = low < 0 ? 0 : low + 1;
  }

  const tickValues = roundedTicks
    ? calculateRoundedTicks(low, high, safeTickCount)
    : prepareNumericDatapoints(high, low, safeTickCount, isIntegralDataset);

  return {
    domainMin: tickValues[0] ?? low,
    domainMax: tickValues[tickValues.length - 1] ?? high,
    tickValues,
  };
};

export type PreparedNumericContinuousScaleOptions = {
  values: readonly number[];
  range: [number, number];
  scaleType?: AxisScaleType;
  tickCount?: number;
  roundedTicks?: boolean;
  includeZero?: boolean;
  minValue?: number;
  maxValue?: number;
};

export const createPreparedNumericContinuousScale = ({
  values,
  range,
  scaleType = 'default',
  tickCount,
  roundedTicks = false,
  includeZero = true,
  minValue,
  maxValue,
}: PreparedNumericContinuousScaleOptions): {
  scale: NumericContinuousScale;
  preparedAxis: PreparedNumericYAxis;
  isLogarithmic: boolean;
} => {
  const finiteValues = values.filter(Number.isFinite);
  const canUseLogarithmicScale =
    scaleType === 'log' && finiteValues.length > 0 && finiteValues.every(value => value > 0);
  const dataMin = finiteValues.length > 0 ? Math.min(...finiteValues) : 0;
  const dataMax = finiteValues.length > 0 ? Math.max(...finiteValues) : 1;
  const domainMin = minValue ?? (includeZero && !canUseLogarithmicScale ? Math.min(0, dataMin) : dataMin);
  let domainMax = maxValue ?? (includeZero && !canUseLogarithmicScale ? Math.max(0, dataMax) : dataMax);
  if (domainMin === domainMax) {
    domainMax += 1;
  }

  const preparedAxis = computePreparedNumericYAxis({
    minValue: domainMin,
    maxValue: domainMax,
    tickCount,
    roundedTicks,
  });
  const scaleDomain = canUseLogarithmicScale
    ? { domainMin, domainMax }
    : { domainMin: preparedAxis.domainMin, domainMax: preparedAxis.domainMax };
  const { scale, isLogarithmic } = createNumericContinuousScale({
    ...scaleDomain,
    range,
    scaleType: canUseLogarithmicScale ? 'log' : 'default',
  });

  return { scale, preparedAxis, isLogarithmic };
};

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const measureSvgTextWidth = (text: SVGTextElement, value: string): number => {
  text.textContent = value;
  const svgWidth = text.getComputedTextLength?.() ?? 0;
  if (svgWidth > 0) {
    return svgWidth;
  }

  const context = document.createElement('canvas').getContext('2d');
  if (!context) {
    return 0;
  }

  const computed = getComputedStyle(text);
  const font = computed.font || `${computed.fontSize || '10px'} ${computed.fontFamily || 'sans-serif'}`;
  context.font = font;
  return context.measureText(value).width;
};

type BottomAxisWrapLabelWidth<Domain extends AxisDomain> =
  | number
  | ((value: Domain, scale: AxisScaleLike<Domain>) => number | undefined);

export type BottomAxisRenderOptions<Domain extends AxisDomain> = {
  svg: SVGSVGElement;
  scale: AxisScaleLike<Domain>;
  axis: Axis<Domain>;
  formatter: (value: Domain) => string;
  axisLeft: number;
  axisTop: number;
  innerWidth: number;
  innerHeight: number;
  tickPadding: number;
  isRTL?: boolean;
  rotateXAxisLabels?: boolean;
  wrapXAxisLabels?: boolean;
  wrapLabelWidth?: BottomAxisWrapLabelWidth<Domain>;
  hideTickOverlap?: boolean;
  showXAxisLabelsTooltip?: boolean;
  noOfCharsToTruncate?: number;
  axisLabelTooltipHandlers?: AxisLabelTooltipHandlers;
  xAxisTitle?: string;
  labelClassName?: string;
  titleClassName?: string;
  labelDominantBaseline?: 'hanging' | 'middle' | 'auto';
  showTickLines?: boolean;
};

export const renderBottomAxisShared = <Domain extends AxisDomain>({
  svg,
  scale,
  axis,
  formatter,
  axisLeft,
  axisTop,
  innerWidth,
  innerHeight,
  tickPadding,
  isRTL = false,
  rotateXAxisLabels = false,
  wrapXAxisLabels = false,
  wrapLabelWidth,
  hideTickOverlap = false,
  showXAxisLabelsTooltip = false,
  noOfCharsToTruncate = 4,
  axisLabelTooltipHandlers,
  xAxisTitle,
  labelClassName = 'axis-text',
  titleClassName = 'x-axis-title',
  labelDominantBaseline = 'hanging',
  showTickLines = true,
}: BottomAxisRenderOptions<Domain>): void => {
  const safeTruncateChars = Number.isFinite(noOfCharsToTruncate) ? Math.max(1, Math.floor(noOfCharsToTruncate)) : 4;

  const group = createSvgElement<SVGGElement>('g');
  group.classList.add('x-axis');
  group.setAttribute('transform', `translate(${axisLeft}, ${axisTop + innerHeight})`);
  svg.appendChild(group);

  const domain = createSvgElement<SVGLineElement>('line');
  domain.classList.add('axis-domain');
  domain.setAttribute('x1', '0');
  domain.setAttribute('x2', String(innerWidth));
  group.appendChild(domain);

  const tickValues = getAxisTickValues(axis, scale);
  const tickPositions = tickValues.map(value => getAxisPosition(scale, value));
  const tickLabelFormatter = getAxisTickLabelFormatter(axis, scale);

  tickValues.forEach((value, tickIndex) => {
    const tick = createSvgElement<SVGGElement>('g');
    const tickPosition = tickPositions[tickIndex];
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(${tickPosition}, 0)`);

    if (showTickLines) {
      const line = createSvgElement<SVGLineElement>('line');
      line.classList.add('axis-tick-line');
      line.setAttribute('y2', '6');
      tick.appendChild(line);
    }

    if (tickLabelFormatter?.(value, tickIndex) !== '') {
      const text = createSvgElement<SVGTextElement>('text');
      text.classList.add(labelClassName);
      text.setAttribute('y', String(6 + tickPadding));
      text.setAttribute('text-anchor', rotateXAxisLabels ? 'end' : 'middle');
      text.setAttribute('dominant-baseline', labelDominantBaseline);
      const fullLabel = formatter(value);
      const shouldTruncateForTooltip = showXAxisLabelsTooltip && !wrapXAxisLabels;
      const renderedLabel =
        shouldTruncateForTooltip && fullLabel.length > safeTruncateChars
          ? `${fullLabel.slice(0, safeTruncateChars)}...`
          : fullLabel;
      text.textContent = renderedLabel;
      if (rotateXAxisLabels) {
        // Nudge rotated labels toward the chart center so the text midpoint aligns
        // closer to the tick mark (LTR: left, RTL: right).
        const rotatedLabelShiftX = isRTL ? 10 : -10;
        text.setAttribute('transform', `translate(${rotatedLabelShiftX}, 0) rotate(-45)`);
      }
      if (showXAxisLabelsTooltip && renderedLabel !== fullLabel) {
        if (axisLabelTooltipHandlers) {
          text.addEventListener('mouseover', () => axisLabelTooltipHandlers.show(text, fullLabel));
          text.addEventListener('mouseout', () => axisLabelTooltipHandlers.hide());
        } else {
          const title = createSvgElement<SVGTitleElement>('title');
          title.textContent = fullLabel;
          text.appendChild(title);
        }
      }
      tick.appendChild(text);
    }
    group.appendChild(tick);

    const text = tick.querySelector<SVGTextElement>(`.${labelClassName}`);
    if (wrapXAxisLabels && text) {
      let width: number | undefined;
      if (typeof wrapLabelWidth === 'number') {
        width = wrapLabelWidth;
      } else if (typeof wrapLabelWidth === 'function') {
        width = wrapLabelWidth(value, scale);
      } else if (tickPositions.length > 1) {
        if (tickIndex < tickPositions.length - 1) {
          width = Math.abs(tickPositions[tickIndex + 1] - tickPosition);
        } else if (tickIndex > 0) {
          width = Math.abs(tickPosition - tickPositions[tickIndex - 1]);
        }
      } else if (typeof scale.step === 'function') {
        width = scale.step();
      } else if (typeof scale.bandwidth === 'function') {
        width = scale.bandwidth();
      }
      if (width && width > 0) {
        wrapText(text, Math.max(width, 1));
      }
    }
  });

  if (xAxisTitle) {
    const title = createSvgElement<SVGTextElement>('text');
    title.classList.add(titleClassName);
    title.setAttribute('x', String(innerWidth / 2));
    title.setAttribute('y', '42');
    title.setAttribute('text-anchor', 'middle');
    title.textContent = xAxisTitle;
    group.appendChild(title);
  }

  if (hideTickOverlap && !rotateXAxisLabels && !wrapXAxisLabels) {
    hideOverlappingBottomAxisLabels(Array.from(group.querySelectorAll<SVGTextElement>(`.${labelClassName}`)));
  }
};

export type PrimaryYAxisRenderOptions = {
  svg: SVGSVGElement;
  scale: AxisScaleLike<number>;
  axis: Axis<number>;
  formatter: (value: number) => string;
  axisStartX: number;
  axisTop: number;
  innerHeight: number;
  innerWidth: number;
  tickPadding: number;
  isRTL: boolean;
  yAxisTitle?: string;
  axisClassName?: string;
  labelClassName?: string;
  titleClassName?: string;
  tickLabelMaxWidth?: number;
};

export type AxisGridLineOrientation = 'horizontal' | 'vertical';

type AxisGridLinePositionSource<Domain extends AxisDomain> =
  | { axis: Axis<Domain>; scale: AxisScaleLike<Domain>; positions?: never }
  | { axis?: never; scale?: never; positions: readonly number[] };

export type AxisGridLinesRenderOptions<Domain extends AxisDomain> = AxisGridLinePositionSource<Domain> & {
  layer: SVGGElement;
  orientation: AxisGridLineOrientation;
  spanStart: number;
  spanEnd: number;
};

/** Renders plot gridlines using one class and coordinate contract for either orientation. */
export const renderAxisGridLinesShared = <Domain extends AxisDomain>(
  options: AxisGridLinesRenderOptions<Domain>,
): void => {
  const { layer, orientation, spanStart, spanEnd } = options;
  const positions =
    options.positions ??
    getAxisTickValues(options.axis, options.scale).map(value => getAxisPosition(options.scale, value));
  const gridGroup = createSvgElement<SVGGElement>('g');
  gridGroup.classList.add('axis-grid');
  gridGroup.dataset.orientation = orientation;

  positions.forEach(position => {
    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add('axis-grid-line');
    if (orientation === 'horizontal') {
      line.setAttribute('x1', String(spanStart));
      line.setAttribute('x2', String(spanEnd));
      line.setAttribute('y1', String(position));
      line.setAttribute('y2', String(position));
    } else {
      line.setAttribute('x1', String(position));
      line.setAttribute('x2', String(position));
      line.setAttribute('y1', String(spanStart));
      line.setAttribute('y2', String(spanEnd));
    }
    gridGroup.appendChild(line);
  });

  layer.appendChild(gridGroup);
};

const truncateTextToWidth = (text: SVGTextElement, sourceText: string, maxWidth: number): string => {
  if (!Number.isFinite(maxWidth) || maxWidth <= 0 || measureSvgTextWidth(text, sourceText) <= maxWidth) {
    text.textContent = sourceText;
    return sourceText;
  }

  const ellipsis = '…';
  let low = 0;
  let high = sourceText.length;
  let best = '';

  while (low <= high) {
    const middle = Math.floor((low + high) / 2);
    const candidate = `${sourceText.slice(0, middle)}${ellipsis}`;
    if (measureSvgTextWidth(text, candidate) <= maxWidth) {
      best = candidate;
      low = middle + 1;
    } else {
      high = middle - 1;
    }
  }

  text.textContent = best || ellipsis;
  return text.textContent;
};

const hideOverlappingBottomAxisLabels = (labels: SVGTextElement[]) => {
  let previousRight = Number.NEGATIVE_INFINITY;
  labels
    .map(label => {
      const box = label.getBBox?.();
      const matrix = label.getCTM?.();
      return box && box.width > 0 ? { label, left: (matrix?.e ?? 0) + box.x, width: box.width } : undefined;
    })
    .filter((entry): entry is { label: SVGTextElement; left: number; width: number } => entry !== undefined)
    .sort((left, right) => left.left - right.left)
    .forEach(({ label, left, width }) => {
      const right = left + width;
      if (left < previousRight + 4) {
        label.style.display = 'none';
      } else {
        previousRight = right;
      }
    });
};

export const renderPrimaryYAxisShared = ({
  svg,
  scale,
  axis,
  formatter,
  axisStartX,
  axisTop,
  innerHeight,
  innerWidth,
  tickPadding,
  isRTL,
  yAxisTitle,
  axisClassName = 'y-axis',
  labelClassName = 'y-axis-text',
  titleClassName = 'y-axis-title',
  tickLabelMaxWidth,
}: PrimaryYAxisRenderOptions): void => {
  const group = createSvgElement<SVGGElement>('g');
  group.classList.add(axisClassName);
  const groupX = isRTL ? axisStartX + innerWidth : axisStartX;
  group.setAttribute('transform', `translate(${groupX}, ${axisTop})`);
  // Attach the group before measuring text so SVG text metrics are available.
  svg.appendChild(group);
  let maxTickLabelWidth = 0;
  let hasNegativeTickLabel = false;

  const domain = createSvgElement<SVGLineElement>('line');
  domain.classList.add('axis-domain');
  domain.setAttribute('y2', String(innerHeight));
  group.appendChild(domain);

  const tickLabelFormatter = getAxisTickLabelFormatter(axis, scale);
  getAxisTickValues(axis, scale).forEach((value, index) => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(0, ${getAxisPosition(scale, value)})`);

    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add('axis-tick-line');
    line.setAttribute('x2', isRTL ? '6' : '-6');
    tick.appendChild(line);

    if (tickLabelFormatter?.(value, index) !== '') {
      const text = createSvgElement<SVGTextElement>('text');
      text.classList.add(labelClassName);
      text.setAttribute('x', String(isRTL ? 6 + tickPadding : -(6 + tickPadding)));
      text.setAttribute('text-anchor', 'end');
      text.setAttribute('dominant-baseline', 'middle');
      const fullLabel = formatter(value);
      hasNegativeTickLabel ||= fullLabel.startsWith('-') || fullLabel.startsWith('−');
      const renderedLabel =
        tickLabelMaxWidth && Number.isFinite(tickLabelMaxWidth)
          ? truncateTextToWidth(text, fullLabel, tickLabelMaxWidth)
          : fullLabel;
      text.textContent = renderedLabel;
      if (renderedLabel !== fullLabel) {
        const title = createSvgElement<SVGTitleElement>('title');
        title.textContent = fullLabel;
        text.appendChild(title);
      }
      maxTickLabelWidth = Math.max(maxTickLabelWidth, measureSvgTextWidth(text, renderedLabel));
      tick.appendChild(text);
    }

    group.appendChild(tick);
  });

  if (yAxisTitle) {
    const title = createSvgElement<SVGTextElement>('text');
    title.classList.add(titleClassName);
    const rotation = isRTL ? 'rotate(90)' : 'rotate(-90)';
    const tx = isRTL ? String(innerHeight / 2) : String(-innerHeight / 2);
    const extraGap = hasNegativeTickLabel ? 2 : 8;
    const titleOffset = 6 + tickPadding + maxTickLabelWidth + extraGap;
    title.setAttribute('x', tx);
    title.setAttribute('y', String(-titleOffset));
    title.setAttribute('text-anchor', 'middle');
    title.setAttribute('transform', rotation);
    title.textContent = yAxisTitle;
    group.appendChild(title);
  }
};

export type SecondaryYAxisRenderOptions = {
  svg: SVGSVGElement;
  scale: AxisScaleLike<number>;
  axis: Axis<number>;
  formatter: (value: number) => string;
  axisStartX: number;
  axisTop: number;
  innerHeight: number;
  innerWidth: number;
  tickPadding: number;
  isRTL: boolean;
  yAxisTitle?: string;
  axisClassName?: string;
  labelClassName?: string;
  titleClassName?: string;
  tickLabelMaxWidth?: number;
};

export const renderSecondaryYAxisShared = ({
  svg,
  scale,
  axis,
  formatter,
  axisStartX,
  axisTop,
  innerHeight,
  innerWidth,
  tickPadding,
  isRTL,
  yAxisTitle,
  axisClassName = 'y-axis-secondary',
  labelClassName = 'y-axis-text',
  titleClassName = 'y-axis-title',
  tickLabelMaxWidth,
}: SecondaryYAxisRenderOptions): void => {
  const group = createSvgElement<SVGGElement>('g');
  group.classList.add(axisClassName);
  const groupX = isRTL ? axisStartX : axisStartX + innerWidth;
  group.setAttribute('transform', `translate(${groupX}, ${axisTop})`);
  let maxTickLabelWidth = 0;
  let hasNegativeTickLabel = false;

  const domain = createSvgElement<SVGLineElement>('line');
  domain.classList.add('axis-domain');
  domain.setAttribute('y2', String(innerHeight));
  group.appendChild(domain);

  const tickLabelFormatter = getAxisTickLabelFormatter(axis, scale);
  getAxisTickValues(axis, scale).forEach((value, index) => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(0, ${getAxisPosition(scale, value)})`);

    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add('axis-tick-line');
    line.setAttribute('x2', isRTL ? '-6' : '6');
    tick.appendChild(line);

    if (tickLabelFormatter?.(value, index) !== '') {
      const text = createSvgElement<SVGTextElement>('text');
      text.classList.add(labelClassName);
      text.setAttribute('x', String(isRTL ? -(6 + tickPadding) : 6 + tickPadding));
      text.setAttribute('text-anchor', 'start');
      text.setAttribute('dominant-baseline', 'middle');
      const fullLabel = formatter(value);
      hasNegativeTickLabel ||= fullLabel.startsWith('-') || fullLabel.startsWith('−');
      const renderedLabel =
        tickLabelMaxWidth && Number.isFinite(tickLabelMaxWidth)
          ? truncateTextToWidth(text, fullLabel, tickLabelMaxWidth)
          : fullLabel;
      text.textContent = renderedLabel;
      if (renderedLabel !== fullLabel) {
        const title = createSvgElement<SVGTitleElement>('title');
        title.textContent = fullLabel;
        text.appendChild(title);
      }
      maxTickLabelWidth = Math.max(maxTickLabelWidth, measureSvgTextWidth(text, renderedLabel));
      tick.appendChild(text);
    }

    group.appendChild(tick);
  });

  if (yAxisTitle) {
    const title = createSvgElement<SVGTextElement>('text');
    title.classList.add(titleClassName);
    const extraGap = hasNegativeTickLabel ? 2 : 8;
    const titleOffset = 6 + tickPadding + maxTickLabelWidth + extraGap;
    const ty = isRTL ? String(-titleOffset) : String(titleOffset);
    title.setAttribute('x', String(-innerHeight / 2));
    title.setAttribute('y', ty);
    title.setAttribute('text-anchor', 'middle');
    title.setAttribute('transform', 'rotate(-90)');
    title.textContent = yAxisTitle;
    group.appendChild(title);
  }

  svg.appendChild(group);
};

export type BandYAxisRenderOptions<Domain extends AxisDomain> = {
  svg: SVGSVGElement;
  scale: AxisScaleLike<Domain>;
  axis: Axis<Domain>;
  formatter: (value: Domain) => string;
  axisX: number;
  axisTop: number;
  innerHeight: number;
  isRTL: boolean;
  tickPadding: number;
  showTickLines?: boolean;
  tickLineLength?: number;
  ltrLabelX?: number;
  rtlLabelX?: number;
  axisClassName?: string;
  labelClassName?: string;
  yAxisTitle?: string;
  titleClassName?: string;
  tickLabelMaxWidth?: number;
};

export const renderBandYAxisShared = <Domain extends AxisDomain>({
  svg,
  scale,
  axis,
  formatter,
  axisX,
  axisTop,
  innerHeight,
  isRTL,
  tickPadding,
  showTickLines = true,
  tickLineLength = 6,
  ltrLabelX,
  rtlLabelX,
  axisClassName = 'y-axis',
  labelClassName = 'y-axis-text',
  yAxisTitle,
  titleClassName = 'y-axis-title',
  tickLabelMaxWidth,
}: BandYAxisRenderOptions<Domain>): void => {
  const group = createSvgElement<SVGGElement>('g');
  group.classList.add(axisClassName);
  group.setAttribute('transform', `translate(${axisX}, ${axisTop})`);

  const domain = createSvgElement<SVGLineElement>('line');
  domain.classList.add('axis-domain');
  domain.setAttribute('y2', String(innerHeight));
  group.appendChild(domain);

  getAxisTickValues(axis, scale).forEach(value => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(0, ${getAxisPosition(scale, value)})`);

    if (showTickLines) {
      const line = createSvgElement<SVGLineElement>('line');
      line.classList.add('axis-tick-line');
      line.setAttribute('x2', String(isRTL ? tickLineLength : -tickLineLength));
      tick.appendChild(line);
    }

    const text = createSvgElement<SVGTextElement>('text');
    text.classList.add(labelClassName);
    text.setAttribute(
      'x',
      String(isRTL ? rtlLabelX ?? tickLineLength + tickPadding : ltrLabelX ?? -(tickLineLength + tickPadding)),
    );
    text.setAttribute('text-anchor', isRTL ? 'start' : 'end');
    text.setAttribute('dominant-baseline', 'middle');
    const fullLabel = formatter(value);
    const renderedLabel =
      tickLabelMaxWidth && Number.isFinite(tickLabelMaxWidth)
        ? truncateTextToWidth(text, fullLabel, tickLabelMaxWidth)
        : fullLabel;
    text.textContent = renderedLabel;
    if (renderedLabel !== fullLabel) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = fullLabel;
      text.appendChild(title);
    }
    tick.appendChild(text);

    group.appendChild(tick);
  });

  if (yAxisTitle) {
    const title = createSvgElement<SVGTextElement>('text');
    title.classList.add(titleClassName);
    title.setAttribute('x', String(innerHeight / 2));
    title.setAttribute('y', String(isRTL ? -14 : 14));
    title.setAttribute('text-anchor', 'middle');
    title.setAttribute('transform', `rotate(${isRTL ? 90 : -90})`);
    title.textContent = yAxisTitle;
    group.appendChild(title);
  }

  svg.appendChild(group);
};

export type ContinuousBottomAxisRenderOptions = {
  axisLayer: SVGGElement;
  gridLayer: SVGGElement;
  gridLineSpan: { start: number; end: number };
  width: number;
  height: number;
  margins: { left: number; right: number; bottom: number };
  domain: [number, number];
  ticks: number[];
  tickPadding: number;
  isRTL: boolean;
  rotateXAxisLabels?: boolean;
  wrapXAxisLabels?: boolean;
  hideTickOverlap?: boolean;
  showXAxisLabelsTooltip?: boolean;
  noOfCharsToTruncate?: number;
  axisLabelTooltipHandlers?: AxisLabelTooltipHandlers;
  xAxisTitle?: string;
  formatTickLabel: (tick: number, range: [number, number]) => string;
};

export const renderContinuousBottomAxisShared = ({
  axisLayer,
  gridLayer,
  gridLineSpan,
  width,
  height,
  margins,
  domain,
  ticks,
  tickPadding,
  isRTL,
  rotateXAxisLabels = false,
  wrapXAxisLabels = false,
  hideTickOverlap = false,
  showXAxisLabelsTooltip = false,
  noOfCharsToTruncate = 4,
  axisLabelTooltipHandlers,
  xAxisTitle,
  formatTickLabel,
}: ContinuousBottomAxisRenderOptions) => {
  const safeTruncateChars = Number.isFinite(noOfCharsToTruncate) ? Math.max(1, Math.floor(noOfCharsToTruncate)) : 4;

  const axisY = height - margins.bottom;
  const min = domain[0];
  const max = domain[1];
  const rangeStart = isRTL ? width - margins.right : margins.left;
  const rangeEnd = isRTL ? margins.left : width - margins.right;
  const span = max - min || 1;
  const toX = (value: number) => rangeStart + ((value - min) / span) * (rangeEnd - rangeStart);
  const range: [number, number] = [min, max];
  const tickPositions = ticks.map(toX);

  renderAxisGridLinesShared({
    layer: gridLayer,
    orientation: 'vertical',
    positions: tickPositions,
    spanStart: gridLineSpan.start,
    spanEnd: gridLineSpan.end,
  });

  ticks.forEach((tick, index) => {
    const x = tickPositions[index];
    const tickLine = createSvgElement<SVGLineElement>('line');
    tickLine.setAttribute('class', 'axis-tick-line');
    tickLine.setAttribute('x1', `${x}`);
    tickLine.setAttribute('x2', `${x}`);
    tickLine.setAttribute('y1', `${axisY}`);
    tickLine.setAttribute('y2', `${axisY + 6}`);
    axisLayer.appendChild(tickLine);

    const labelY = axisY + tickPadding + 12;
    const rawLabel = formatTickLabel(tick, range);
    const renderedLabel =
      showXAxisLabelsTooltip && !wrapXAxisLabels && rawLabel.length > safeTruncateChars
        ? `${rawLabel.slice(0, safeTruncateChars)}...`
        : rawLabel;

    const text = createSvgElement<SVGTextElement>('text');
    text.setAttribute('class', 'axis-text');
    text.setAttribute('x', `${x}`);
    text.setAttribute('y', `${labelY}`);

    if (rotateXAxisLabels) {
      text.setAttribute('text-anchor', isRTL ? 'start' : 'end');
      text.setAttribute('transform', `rotate(-45, ${x}, ${labelY})`);
      text.textContent = renderedLabel;
    } else if (wrapXAxisLabels) {
      text.setAttribute('text-anchor', 'middle');
      const words = renderedLabel.split(' ');
      if (words.length > 1) {
        words.forEach((word, index) => {
          const tspan = createSvgElement<SVGTSpanElement>('tspan');
          tspan.setAttribute('x', `${x}`);
          tspan.setAttribute('dy', index === 0 ? '0' : '1.2em');
          tspan.textContent = word;
          text.appendChild(tspan);
        });
      } else {
        text.textContent = renderedLabel;
      }
    } else {
      text.setAttribute('text-anchor', 'middle');
      text.textContent = renderedLabel;
    }

    if (showXAxisLabelsTooltip && renderedLabel !== rawLabel) {
      if (axisLabelTooltipHandlers) {
        text.addEventListener('mouseover', () => axisLabelTooltipHandlers.show(text, rawLabel));
        text.addEventListener('mouseout', () => axisLabelTooltipHandlers.hide());
      } else {
        const title = createSvgElement<SVGTitleElement>('title');
        title.textContent = rawLabel;
        text.appendChild(title);
      }
    }

    axisLayer.appendChild(text);
  });

  if (hideTickOverlap) {
    const textEls = Array.from(axisLayer.querySelectorAll<SVGTextElement>('text.axis-text'));
    let previousRight = Number.NEGATIVE_INFINITY;
    textEls.forEach(el => {
      const bbox = el.getBBox?.();
      if (!bbox) {
        return;
      }
      const left = bbox.x;
      const right = bbox.x + bbox.width;
      if (left < previousRight + 4) {
        el.style.display = 'none';
      } else {
        previousRight = right;
      }
    });
  }

  if (xAxisTitle) {
    const titleX = (rangeStart + rangeEnd) / 2;
    const titleY = height - 4;
    const titleText = createSvgElement<SVGTextElement>('text');
    titleText.setAttribute('class', 'axis-title');
    titleText.setAttribute('x', `${titleX}`);
    titleText.setAttribute('y', `${titleY}`);
    titleText.setAttribute('text-anchor', 'middle');
    titleText.textContent = xAxisTitle;
    axisLayer.appendChild(titleText);
  }
};

export type HorizontalYAxisTickEntry = {
  y: number;
  label: string;
  tooltipText?: string;
};

export type HorizontalYAxisRenderOptions = {
  axisLayer: SVGGElement;
  axisX: number;
  isRTL: boolean;
  tickPadding: number;
  ticks: HorizontalYAxisTickEntry[];
  yAxisTitle?: string;
  width: number;
  height: number;
  margins: { top: number; bottom: number; right: number };
};

export const renderHorizontalYAxisShared = ({
  axisLayer,
  axisX,
  isRTL,
  tickPadding,
  ticks,
  yAxisTitle,
  width,
  height,
  margins,
}: HorizontalYAxisRenderOptions) => {
  ticks.forEach(tick => {
    const tickLine = createSvgElement<SVGLineElement>('line');
    tickLine.setAttribute('class', 'axis-tick-line');
    tickLine.setAttribute('x1', `${axisX}`);
    tickLine.setAttribute('x2', `${axisX + tickPadding}`);
    tickLine.setAttribute('y1', `${tick.y}`);
    tickLine.setAttribute('y2', `${tick.y}`);
    axisLayer.appendChild(tickLine);

    const text = createSvgElement<SVGTextElement>('text');
    text.setAttribute('class', 'y-axis-text');
    text.setAttribute('x', `${axisX + (isRTL ? tickPadding + 6 : -(tickPadding + 6))}`);
    text.setAttribute('y', `${tick.y}`);
    text.setAttribute('dominant-baseline', 'central');
    text.setAttribute('text-anchor', isRTL ? 'start' : 'end');
    text.textContent = tick.label;

    if (tick.tooltipText) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = tick.tooltipText;
      text.appendChild(title);
    }

    axisLayer.appendChild(text);
  });

  if (yAxisTitle) {
    const midY = (margins.top + (height - margins.bottom)) / 2;
    const titleX = isRTL ? width - margins.right + 12 : 12;
    const titleText = createSvgElement<SVGTextElement>('text');
    titleText.setAttribute('class', 'axis-title');
    titleText.setAttribute('x', `${titleX}`);
    titleText.setAttribute('y', `${midY}`);
    titleText.setAttribute('text-anchor', 'middle');
    titleText.setAttribute('transform', `rotate(-90, ${titleX}, ${midY})`);
    titleText.textContent = yAxisTitle;
    axisLayer.appendChild(titleText);
  }
};

export type CategoryGroup<TPoint> = {
  key: string;
  points: readonly TPoint[];
};

type CategoryAggregateOrder = Exclude<
  AxisCategoryOrder,
  'default' | 'data' | 'category ascending' | 'category descending'
>;

const getMedian = (values: readonly number[]) => {
  if (values.length === 0) {
    return 0;
  }
  const sorted = [...values].sort((left, right) => left - right);
  const middle = Math.floor(sorted.length / 2);
  if (sorted.length % 2 === 0) {
    return (sorted[middle - 1] + sorted[middle]) / 2;
  }
  return sorted[middle];
};

const getAggregateValue = (values: readonly number[], order: CategoryAggregateOrder): number => {
  switch (order) {
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

export const sortCategoryGroups = <TPoint>(
  groups: readonly CategoryGroup<TPoint>[],
  order: AxisCategoryOrder | undefined,
  dataOrderKeys: readonly string[],
  getValues: (group: CategoryGroup<TPoint>) => readonly number[],
) => {
  const resolvedOrder = order || 'default';
  if (resolvedOrder === 'default' || resolvedOrder === 'data') {
    const firstIndex = new Map<string, number>();
    dataOrderKeys.forEach((key, index) => {
      if (!firstIndex.has(key)) {
        firstIndex.set(key, index);
      }
    });
    return [...groups].sort(
      (left, right) =>
        (firstIndex.get(left.key) ?? Number.MAX_SAFE_INTEGER) - (firstIndex.get(right.key) ?? Number.MAX_SAFE_INTEGER),
    );
  }

  if (resolvedOrder.startsWith('category')) {
    const sorted = [...groups].sort((left, right) => left.key.localeCompare(right.key));
    if (resolvedOrder.endsWith('descending')) {
      sorted.reverse();
    }
    return sorted;
  }

  const sorted = [...groups].sort(
    (left, right) =>
      getAggregateValue(getValues(left), resolvedOrder as CategoryAggregateOrder) -
      getAggregateValue(getValues(right), resolvedOrder as CategoryAggregateOrder),
  );
  if (resolvedOrder.endsWith('descending')) {
    sorted.reverse();
  }
  return sorted;
};
