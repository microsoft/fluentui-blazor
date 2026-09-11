import { getColorFromToken, getNextColor, lightenColor, SVG_NAMESPACE_URI } from './chart-helpers.js';

const minimumBarWidth = 1;

export const resolveBarWidth = (
  barWidth: number | string | undefined,
  maxBarWidth: number | string | undefined,
  availableWidth: number,
  defaultWidth: number = 16,
): number => {
  const requestedWidth = barWidth === 'auto' ? availableWidth : toOptionalNumber(barWidth);
  const maximumWidth = toOptionalNumber(maxBarWidth);
  return Math.max(
    minimumBarWidth,
    Math.min(
      requestedWidth ?? Math.min(availableWidth, defaultWidth),
      maximumWidth ?? Number.POSITIVE_INFINITY,
      availableWidth,
    ),
  );
};

export const resolveChartColor = (
  color: string | undefined,
  colors: string[] | undefined,
  index: number,
  offset: number = 0,
): string => {
  const paletteColor = colors && colors.length > 0 ? colors[(index + offset) % colors.length] : undefined;
  return getColorFromToken(color ?? paletteColor ?? getNextColor(index, offset));
};

export const appendVerticalGradient = (
  defs: SVGDefsElement,
  id: string,
  color: string,
  enabled: boolean,
  colors?: [string, string],
): string | undefined => {
  if (!enabled && !colors) {
    return undefined;
  }

  const gradient = document.createElementNS(SVG_NAMESPACE_URI, 'linearGradient');
  gradient.setAttribute('id', id);
  gradient.setAttribute('x1', '0%');
  gradient.setAttribute('x2', '0%');
  gradient.setAttribute('y1', '100%');
  gradient.setAttribute('y2', '0%');

  const [from, to] = colors ?? [lightenColor(color, 0.35), color];
  for (const [offset, stopColor] of [
    ['0%', from],
    ['100%', to],
  ]) {
    const stop = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
    stop.setAttribute('offset', offset);
    stop.setAttribute('stop-color', stopColor);
    gradient.appendChild(stop);
  }

  defs.appendChild(gradient);
  return id;
};

const toOptionalNumber = (value: number | string | undefined): number | undefined => {
  if (value === undefined || value === '') {
    return undefined;
  }
  const parsed = typeof value === 'number' ? value : Number.parseFloat(value);
  return Number.isFinite(parsed) ? parsed : undefined;
};
