import { extent, range as d3Range } from 'd3-array';
import { format as d3Format } from 'd3-format';
import { scaleLinear, scaleLog, scalePoint, scaleTime, scaleUtc } from 'd3-scale';
import {
  curveLinear,
  curveLinearClosed,
  curveNatural,
  curveStep,
  curveStepAfter,
  curveStepBefore,
  type CurveFactory,
} from 'd3-shape';
import { timeFormat, utcFormat } from 'd3-time-format';
import { sortCategoryGroups } from '../utils/cartesian-axis-shared.js';
import { formatLocaleNumber } from '../utils/chart-helpers.js';
import type {
  PolarAxisOptions,
  PolarChartDataPoint,
  PolarChartSeries,
  PolarChartValue,
  PolarLineOptions,
} from './polar-chart.options.js';

export interface NormalizedPolarPoint {
  source: PolarChartDataPoint;
  theta: string | number;
  r: PolarChartValue;
}

export interface NormalizedPolarSeries extends Omit<PolarChartSeries, 'data'> {
  data: NormalizedPolarPoint[];
  type: NonNullable<PolarChartSeries['type']>;
}

export interface PolarScale<TValue> {
  map: (value: TValue) => number | undefined;
  tickValues: TValue[];
  tickLabels: string[];
}

const normalizePolarValue = (value: PolarChartValue): PolarChartValue => {
  if (typeof value !== 'string' || !/^\d{4}-\d{2}-\d{2}(?:T|$)/.test(value)) {
    return value;
  }
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? value : date;
};

const normalizeAxisDates = (axis: PolarAxisOptions | undefined): PolarAxisOptions | undefined =>
  axis && {
    ...axis,
    tickValues: axis.tickValues?.map(normalizePolarValue),
    tick0: axis.tick0 === undefined ? undefined : normalizePolarValue(axis.tick0),
    rangeStart: axis.rangeStart === undefined ? undefined : normalizePolarValue(axis.rangeStart),
    rangeEnd: axis.rangeEnd === undefined ? undefined : normalizePolarValue(axis.rangeEnd),
  };

export const normalizeSeries = (series: PolarChartSeries[]): NormalizedPolarSeries[] =>
  series
    .map(entry => ({
      ...entry,
      type: entry.type ?? 'areapolar',
      data: entry.data.flatMap(point => {
        const theta = point.theta ?? point.x;
        const r = point.r ?? point.y;
        return theta === undefined || r === undefined ? [] : [{ source: point, theta, r: normalizePolarValue(r) }];
      }),
    }))
    .sort(
      (left, right) =>
        ['areapolar', 'linepolar', 'scatterpolar'].indexOf(left.type) -
        ['areapolar', 'linepolar', 'scatterpolar'].indexOf(right.type),
    );

const createStepTicks = (start: number, end: number, step: number, origin: number): number[] => {
  if (!Number.isFinite(step) || step <= 0) {
    return [];
  }
  const first = origin + Math.ceil((start - origin) / step) * step;
  return d3Range(first, end + step / 2, step);
};

export const formatPolarDate = (
  value: Date,
  axis: PolarAxisOptions | undefined,
  culture: string | undefined,
  useUTC: boolean,
  dateLocalizeOptions: Intl.DateTimeFormatOptions | undefined,
): string => {
  if (axis?.tickFormat) {
    return (useUTC ? utcFormat(axis.tickFormat) : timeFormat(axis.tickFormat))(value);
  }
  const options = useUTC ? { ...dateLocalizeOptions, timeZone: 'UTC' } : dateLocalizeOptions;
  return new Intl.DateTimeFormat(culture, options).format(value);
};

const createMonthlyTicks = (start: Date, end: Date, step: number, origin: Date, useUTC: boolean): Date[] => {
  const getMonth = (value: Date) => (useUTC ? value.getUTCMonth() : value.getMonth());
  const setMonth = (value: Date, month: number) =>
    useUTC ? new Date(value.setUTCMonth(month)) : new Date(value.setMonth(month));
  let startOffset = 0;
  for (let firstTick = new Date(origin); firstTick > start; ) {
    firstTick = setMonth(firstTick, getMonth(firstTick) - step);
    startOffset -= step;
  }
  const baseMonth = getMonth(origin);
  const ticks: Date[] = [];
  for (let offset = startOffset; ; offset += step) {
    let tick = setMonth(new Date(origin), baseMonth + offset);
    const expectedMonth = (((baseMonth + offset) % 12) + 12) % 12;
    if (getMonth(tick) !== expectedMonth) {
      tick = useUTC ? new Date(tick.setUTCDate(0)) : new Date(tick.setDate(0));
    }
    if (tick > end) {
      break;
    }
    if (tick >= start) {
      ticks.push(tick);
    }
  }
  return ticks;
};

export const formatPolarRadialValue = (
  value: PolarChartValue,
  culture: string | undefined,
  useUTC: boolean,
  dateLocalizeOptions: Intl.DateTimeFormatOptions | undefined,
): string =>
  value instanceof Date
    ? formatPolarDate(value, undefined, culture, useUTC, dateLocalizeOptions)
    : typeof value === 'number'
    ? formatLocaleNumber(value, culture)
    : value;

const getOrderedCategories = (
  values: string[],
  axis: PolarAxisOptions | undefined,
  points: NormalizedPolarPoint[],
  getKey: (point: NormalizedPolarPoint) => string,
  getAggregateValue: (point: NormalizedPolarPoint) => number | undefined,
): string[] => {
  const groups = [...new Set(values)].map(key => ({ key, points: points.filter(point => getKey(point) === key) }));
  return sortCategoryGroups(groups, axis?.categoryOrder, values, group =>
    group.points.map(getAggregateValue).filter((value): value is number => value !== undefined),
  ).map(group => group.key);
};

export const createRadialScale = (
  points: NormalizedPolarPoint[],
  axis: PolarAxisOptions | undefined,
  range: [number, number],
  culture: string | undefined,
  useUTC: boolean,
  dateLocalizeOptions: Intl.DateTimeFormatOptions | undefined,
): PolarScale<PolarChartValue> => {
  axis = normalizeAxisDates(axis);
  const values = points.map(point => point.r);
  const firstValue = values[0];

  if (typeof firstValue === 'string') {
    const categories = [...new Set(values.map(String))];
    const ordered = getOrderedCategories(
      categories,
      axis,
      points,
      point => String(point.r),
      point => (typeof point.theta === 'number' ? point.theta : undefined),
    );
    const scale = scalePoint<string>().domain(ordered).range(range);
    const tickValues = (axis?.tickValues?.map(String) ?? ordered) as PolarChartValue[];
    return {
      map: value => scale(String(value)),
      tickValues,
      tickLabels: tickValues.map((value, index) => axis?.tickText?.[index] ?? String(value)),
    };
  }

  if (firstValue instanceof Date) {
    const dateValues = values.filter((value): value is Date => value instanceof Date);
    const [dataMin, dataMax] = extent(dateValues, value => value.getTime());
    const domainStart = axis?.rangeStart instanceof Date ? axis.rangeStart : new Date(dataMin ?? 0);
    const domainEnd = axis?.rangeEnd instanceof Date ? axis.rangeEnd : new Date(dataMax ?? domainStart.getTime() + 1);
    const scale = (useUTC ? scaleUtc() : scaleTime()).domain([domainStart, domainEnd]).range(range).nice();
    let tickValues = axis?.tickValues?.filter((value): value is Date => value instanceof Date) ?? [];
    if (tickValues.length === 0 && typeof axis?.tickStep === 'number') {
      tickValues = createStepTicks(
        domainStart.getTime(),
        domainEnd.getTime(),
        axis.tickStep,
        axis.tick0 instanceof Date ? axis.tick0.getTime() : domainStart.getTime(),
      ).map(value => new Date(value));
    }
    const monthStep = typeof axis?.tickStep === 'string' ? /^M([1-9]\d*)$/.exec(axis.tickStep) : undefined;
    if (tickValues.length === 0 && monthStep) {
      tickValues = createMonthlyTicks(
        domainStart,
        domainEnd,
        Number(monthStep[1]),
        axis?.tick0 instanceof Date ? axis.tick0 : new Date('1970-01-01T00:00:00.000Z'),
        useUTC,
      );
    }
    if (tickValues.length === 0) {
      tickValues = scale.ticks(axis?.tickCount ?? 4);
    }
    return {
      map: value => (value instanceof Date ? scale(value) : undefined),
      tickValues,
      tickLabels: tickValues.map(
        (value, index) => axis?.tickText?.[index] ?? formatPolarDate(value, axis, culture, useUTC, dateLocalizeOptions),
      ),
    };
  }

  const numericValues = values.filter((value): value is number => typeof value === 'number' && Number.isFinite(value));
  const positiveValues = numericValues.filter(value => value > 0);
  const useLog = axis?.scaleType === 'log' && positiveValues.length > 0;
  const [dataMin = useLog ? 1 : 0, dataMax = useLog ? 10 : 1] = extent(useLog ? positiveValues : numericValues);
  const domainStart = typeof axis?.rangeStart === 'number' ? axis.rangeStart : useLog ? dataMin : Math.min(0, dataMin);
  const domainEnd = typeof axis?.rangeEnd === 'number' ? axis.rangeEnd : dataMax;
  const safeEnd = domainEnd === domainStart ? domainStart + (useLog ? domainStart || 1 : 1) : domainEnd;
  const scale = useLog
    ? scaleLog()
        .domain([Math.max(domainStart, Number.MIN_VALUE), safeEnd])
        .range(range)
        .nice()
    : scaleLinear().domain([domainStart, safeEnd]).range(range).nice();
  let tickValues = axis?.tickValues?.filter((value): value is number => typeof value === 'number') ?? [];
  if (tickValues.length === 0 && typeof axis?.tickStep === 'number') {
    tickValues = useLog
      ? createStepTicks(
          Math.log10(scale.domain()[0]),
          Math.log10(scale.domain()[1]),
          axis.tickStep,
          typeof axis.tick0 === 'number' ? axis.tick0 : Math.log10(scale.domain()[0]),
        ).map(exponent => 10 ** exponent)
      : createStepTicks(
          scale.domain()[0],
          scale.domain()[1],
          axis.tickStep,
          typeof axis.tick0 === 'number' ? axis.tick0 : scale.domain()[0],
        );
  }
  if (tickValues.length === 0) {
    tickValues = scale.ticks(axis?.tickCount ?? 4);
  }
  const formatter = axis?.tickFormat
    ? d3Format(axis.tickFormat)
    : (value: number) => formatLocaleNumber(value, culture);
  return {
    map: value => (typeof value === 'number' && (!useLog || value > 0) ? scale(value) : undefined),
    tickValues,
    tickLabels: tickValues.map((value, index) => axis?.tickText?.[index] ?? formatter(value)),
  };
};

const normalizeDegrees = (degrees: number, direction: 'clockwise' | 'counterclockwise'): number =>
  (((direction === 'clockwise' ? degrees : 450 - degrees) % 360) + 360) % 360;

export const formatPolarAngle = (value: string | number, axis: PolarAxisOptions | undefined): string => {
  if (typeof value === 'string') {
    return value;
  }
  if (axis?.tickFormat) {
    return d3Format(axis.tickFormat)(value);
  }
  return axis?.unit === 'radians' ? `${Number((value / 180).toPrecision(6))}π` : `${value}°`;
};

export const createAngularScale = (
  points: NormalizedPolarPoint[],
  axis: PolarAxisOptions | undefined,
  direction: 'clockwise' | 'counterclockwise',
): PolarScale<string | number> => {
  const values = points.map(point => point.theta);
  if (typeof values[0] === 'number') {
    let tickValues = axis?.tickValues?.filter((value): value is number => typeof value === 'number') ?? [];
    if (tickValues.length === 0 && typeof axis?.tickStep === 'number') {
      const period = axis.unit === 'radians' ? 2 * Math.PI : 360;
      tickValues = createStepTicks(0, period - Number.EPSILON, axis.tickStep, Number(axis.tick0 ?? 0)).map(value =>
        axis.unit === 'radians' ? (value * 180) / Math.PI : value,
      );
    }
    if (tickValues.length === 0) {
      tickValues = d3Range(0, 360, 360 / (axis?.tickCount ?? 8));
    }
    return {
      map: value => (typeof value === 'number' ? (normalizeDegrees(value, direction) * Math.PI) / 180 : undefined),
      tickValues,
      tickLabels: tickValues.map((value, index) => axis?.tickText?.[index] ?? formatPolarAngle(value, axis)),
    };
  }

  const stringValues = values.map(String);
  const categories = getOrderedCategories(
    stringValues,
    axis,
    points,
    point => String(point.theta),
    point => (typeof point.r === 'number' ? point.r : undefined),
  );
  const categoryIndex = new Map(categories.map((category, index) => [category, index]));
  const tickValues = (axis?.tickValues?.map(String) ?? categories) as Array<string | number>;
  return {
    map: value => {
      const index = categoryIndex.get(String(value));
      return index === undefined
        ? undefined
        : (normalizeDegrees((index * 360) / categories.length, direction) * Math.PI) / 180;
    },
    tickValues,
    tickLabels: tickValues.map((value, index) => axis?.tickText?.[index] ?? String(value)),
  };
};

export const getCurveFactory = (curve: PolarLineOptions['curve'], closed = false): CurveFactory => {
  switch (curve) {
    case 'linear':
      return curveLinear;
    case 'natural':
      return curveNatural;
    case 'step':
      return curveStep;
    case 'stepAfter':
      return curveStepAfter;
    case 'stepBefore':
      return curveStepBefore;
    default:
      return closed ? curveLinearClosed : curveLinear;
  }
};
