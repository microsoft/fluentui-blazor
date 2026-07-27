import type { Axis, AxisDomain } from 'd3-axis';
import { nice as d3Nice, ticks as d3Ticks } from 'd3-array';
import type { AxisCategoryOrder } from './chart-options.js';
import { SVG_NAMESPACE_URI, wrapText } from './chart-helpers.js';

export const DEFAULT_REACT_NUMERIC_Y_TICK_COUNT = 4;

export type AxisScaleLike<Domain extends AxisDomain> = {
  domain(): Domain[];
  ticks?: (count?: number) => Domain[];
  bandwidth?: () => number;
  (value: Domain): number | undefined;
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
  tickCount = DEFAULT_REACT_NUMERIC_Y_TICK_COUNT,
  isIntegralDataset = false,
  roundedTicks = false,
}: PreparedNumericYAxisOptions): PreparedNumericYAxis => {
  const safeTickCount =
    Number.isFinite(tickCount) && tickCount > 0 ? Math.floor(tickCount) : DEFAULT_REACT_NUMERIC_Y_TICK_COUNT;
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
  rotateXAxisLabels?: boolean;
  wrapXAxisLabels?: boolean;
  wrapLabelWidth?: BottomAxisWrapLabelWidth<Domain>;
  hideTickOverlap?: boolean;
  showXAxisLabelsTooltip?: boolean;
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
  rotateXAxisLabels = false,
  wrapXAxisLabels = false,
  wrapLabelWidth,
  hideTickOverlap = false,
  showXAxisLabelsTooltip = false,
  xAxisTitle,
  labelClassName = 'axis-text',
  titleClassName = 'x-axis-title',
  labelDominantBaseline = 'hanging',
  showTickLines = true,
}: BottomAxisRenderOptions<Domain>): void => {
  const group = createSvgElement<SVGGElement>('g');
  group.classList.add('x-axis');
  group.setAttribute('transform', `translate(${axisLeft}, ${axisTop + innerHeight})`);

  const domain = createSvgElement<SVGLineElement>('line');
  domain.classList.add('axis-domain');
  domain.setAttribute('x1', '0');
  domain.setAttribute('x2', String(innerWidth));
  group.appendChild(domain);

  getAxisTickValues(axis, scale).forEach(value => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(${getAxisPosition(scale, value)}, 0)`);

    if (showTickLines) {
      const line = createSvgElement<SVGLineElement>('line');
      line.classList.add('axis-tick-line');
      line.setAttribute('y2', '6');
      tick.appendChild(line);
    }

    const text = createSvgElement<SVGTextElement>('text');
    text.classList.add(labelClassName);
    text.setAttribute('y', String(6 + tickPadding));
    text.setAttribute('text-anchor', rotateXAxisLabels ? 'start' : 'middle');
    text.setAttribute('dominant-baseline', labelDominantBaseline);
    text.textContent = formatter(value);
    if (rotateXAxisLabels) {
      text.setAttribute('transform', 'rotate(45)');
    }
    if (showXAxisLabelsTooltip) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = text.textContent;
      text.appendChild(title);
    }
    tick.appendChild(text);
    group.appendChild(tick);

    if (wrapXAxisLabels) {
      let width: number | undefined;
      if (typeof wrapLabelWidth === 'number') {
        width = wrapLabelWidth;
      } else if (typeof wrapLabelWidth === 'function') {
        width = wrapLabelWidth(value, scale);
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

  svg.appendChild(group);

  if (hideTickOverlap && !rotateXAxisLabels) {
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

export type HorizontalGridLinesRenderOptions = {
  plotGroup: SVGGElement;
  scale: AxisScaleLike<number>;
  axis: Axis<number>;
  innerWidth: number;
  className?: string;
};

export const renderHorizontalGridLinesShared = ({
  plotGroup,
  scale,
  axis,
  innerWidth,
  className = 'y-axis-grid-line',
}: HorizontalGridLinesRenderOptions): void => {
  const gridGroup = createSvgElement<SVGGElement>('g');
  gridGroup.classList.add('y-axis-grid');

  getAxisTickValues(axis, scale).forEach(value => {
    const y = getAxisPosition(scale, value);
    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add(className);
    line.setAttribute('x1', '0');
    line.setAttribute('x2', String(innerWidth));
    line.setAttribute('y1', String(y));
    line.setAttribute('y2', String(y));
    gridGroup.appendChild(line);
  });

  plotGroup.appendChild(gridGroup);
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
  labels.forEach(label => {
    const box = label.getBBox?.();
    if (!box || box.width <= 0) {
      return;
    }

    const matrix = label.getCTM?.();
    const left = (matrix?.e ?? 0) + box.x;
    const right = left + box.width;
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

  getAxisTickValues(axis, scale).forEach(value => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(0, ${getAxisPosition(scale, value)})`);

    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add('axis-tick-line');
    line.setAttribute('x2', isRTL ? '6' : '-6');
    tick.appendChild(line);

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

  getAxisTickValues(axis, scale).forEach(value => {
    const tick = createSvgElement<SVGGElement>('g');
    tick.classList.add('tick');
    tick.setAttribute('transform', `translate(0, ${getAxisPosition(scale, value)})`);

    const line = createSvgElement<SVGLineElement>('line');
    line.classList.add('axis-tick-line');
    line.setAttribute('x2', isRTL ? '-6' : '6');
    tick.appendChild(line);

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

  svg.appendChild(group);
};

export type ContinuousBottomAxisRenderOptions = {
  axisLayer: SVGGElement;
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
  xAxisTitle?: string;
  formatTickLabel: (tick: number, range: [number, number]) => string;
};

export const renderContinuousBottomAxisShared = ({
  axisLayer,
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
  xAxisTitle,
  formatTickLabel,
}: ContinuousBottomAxisRenderOptions) => {
  const axisY = height - margins.bottom;
  const min = domain[0];
  const max = domain[1];
  const rangeStart = isRTL ? width - margins.right : margins.left;
  const rangeEnd = isRTL ? margins.left : width - margins.right;
  const span = max - min || 1;
  const toX = (value: number) => rangeStart + ((value - min) / span) * (rangeEnd - rangeStart);
  const range: [number, number] = [min, max];

  ticks.forEach(tick => {
    const x = toX(tick);
    const tickLine = createSvgElement<SVGLineElement>('line');
    tickLine.setAttribute('class', 'axis-tick-line');
    tickLine.setAttribute('x1', `${x}`);
    tickLine.setAttribute('x2', `${x}`);
    tickLine.setAttribute('y1', `${axisY}`);
    tickLine.setAttribute('y2', `${20}`);
    axisLayer.appendChild(tickLine);

    const labelY = axisY + tickPadding + 12;
    const rawLabel = formatTickLabel(tick, range);

    const text = createSvgElement<SVGTextElement>('text');
    text.setAttribute('class', 'axis-text');
    text.setAttribute('x', `${x}`);
    text.setAttribute('y', `${labelY}`);

    if (rotateXAxisLabels) {
      text.setAttribute('text-anchor', isRTL ? 'start' : 'end');
      text.setAttribute('transform', `rotate(-45, ${x}, ${labelY})`);
      text.textContent = rawLabel;
    } else if (wrapXAxisLabels) {
      text.setAttribute('text-anchor', 'middle');
      const words = rawLabel.split(' ');
      if (words.length > 1) {
        words.forEach((word, index) => {
          const tspan = createSvgElement<SVGTSpanElement>('tspan');
          tspan.setAttribute('x', `${x}`);
          tspan.setAttribute('dy', index === 0 ? '0' : '1.2em');
          tspan.textContent = word;
          text.appendChild(tspan);
        });
      } else {
        text.textContent = rawLabel;
      }
    } else {
      text.setAttribute('text-anchor', 'middle');
      text.textContent = rawLabel;
    }

    if (showXAxisLabelsTooltip) {
      const title = createSvgElement<SVGTitleElement>('title');
      title.textContent = rawLabel;
      text.appendChild(title);
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
