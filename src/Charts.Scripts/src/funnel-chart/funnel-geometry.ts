import type { FunnelDataPoint } from './funnel-chart.options.js';

export interface SimpleFunnelDataPoint extends FunnelDataPoint {
  value: number;
}

export function isSimpleFunnelDataPoint(dataPoint: FunnelDataPoint): dataPoint is SimpleFunnelDataPoint {
  return typeof dataPoint.value === 'number' && Number.isFinite(dataPoint.value);
}

export interface FunnelSegmentGeometry {
  pathD: string;
  textX: number;
  textY: number;
  availableWidth: number;
}

interface SubValue {
  category: string;
  value: number;
  color: string;
}

interface Stage {
  subValues: SubValue[];
}

const LAST_SEGMENT_TEXT_OFFSET_RATIO = 0.25;
// Width ratio for the label area in the tapered final segment (75% of segment width, then 60% padding factor).
const LAST_SEGMENT_LABEL_WIDTH_RATIO = 0.45;
const ROUNDED_CORNER_RADIUS = 6;

/**
 * Generates an SVG path string for a polygon with corners softened by quadratic
 * bezier curves.  Consecutive near-duplicate vertices (< 0.5 px apart) are
 * deduplicated first so that the tapered tip of the last funnel segment is
 * treated as a triangle whose point is also rounded.
 *
 * @param cornerModes - Per-corner style indexed by the original input point index.
 *   'inward'  (default): control = corner vertex → cuts the corner inward (concave).
 *   'outward': control placed so the bezier passes through the corner at t=0.5 →
 *              creates a smooth S-curve that keeps adjacent segments touching.
 */
function roundedPolygonPath(
  inputPoints: [number, number][],
  radius: number,
  cornerModes?: ('inward' | 'outward')[],
): string {
  const points: [number, number][] = [];
  const originalIndices: number[] = [];

  for (let j = 0; j < inputPoints.length; j++) {
    const p = inputPoints[j];
    const prevPoint = points.length > 0 ? points[points.length - 1] : inputPoints[inputPoints.length - 1];
    const dx = p[0] - prevPoint[0];
    const dy = p[1] - prevPoint[1];
    if (Math.sqrt(dx * dx + dy * dy) > 0.5) {
      points.push(p);
      originalIndices.push(j);
    }
  }

  const n = points.length;
  if (n < 2) return '';

  const parts: string[] = [];
  let first = true;

  for (let i = 0; i < n; i++) {
    const prev = points[(i - 1 + n) % n];
    const curr = points[i];
    const next = points[(i + 1) % n];

    const ix = prev[0] - curr[0];
    const iy = prev[1] - curr[1];
    const inLen = Math.sqrt(ix * ix + iy * iy);

    const ox = next[0] - curr[0];
    const oy = next[1] - curr[1];
    const outLen = Math.sqrt(ox * ox + oy * oy);

    const r = Math.min(radius, inLen * 0.45, outLen * 0.45);
    const p1x = curr[0] + (ix / inLen) * r;
    const p1y = curr[1] + (iy / inLen) * r;
    const p2x = curr[0] + (ox / outLen) * r;
    const p2y = curr[1] + (oy / outLen) * r;

    // 'outward': control = curr − 0.5·r·(û_toward_prev + û_toward_next)
    // This places the bezier midpoint exactly at curr, creating an S-curve
    // that smoothly bridges the gap at segment connection edges.
    const mode = cornerModes ? cornerModes[originalIndices[i]] : 'inward';
    const ctrlX = mode === 'outward' ? curr[0] - 0.5 * r * (ix / inLen + ox / outLen) : curr[0];
    const ctrlY = mode === 'outward' ? curr[1] - 0.5 * r * (iy / inLen + oy / outLen) : curr[1];

    if (first) {
      parts.push(`M${p1x},${p1y}`);
      first = false;
    } else {
      parts.push(`L${p1x},${p1y}`);
    }
    parts.push(`Q${ctrlX},${ctrlY} ${p2x},${p2y}`);
  }

  parts.push('Z');
  return parts.join(' ');
}

/**
 * Gets vertical funnel segment geometry scaled by `maxValue`, the maximum stage value across all simple data points.
 */
export function getVerticalFunnelSegmentGeometry({
  d,
  i,
  data,
  maxValue,
  funnelWidth,
  funnelHeight,
  isRTL,
  roundCorners = false,
}: {
  d: SimpleFunnelDataPoint;
  i: number;
  data: SimpleFunnelDataPoint[];
  maxValue: number;
  funnelWidth: number;
  funnelHeight: number;
  isRTL: boolean;
  roundCorners?: boolean;
}): FunnelSegmentGeometry {
  const segmentHeight = funnelHeight / data.length;
  const widthScale = (value: number) => (value / maxValue) * funnelWidth;
  const topWidth = widthScale(d.value);
  const bottomWidth = i < data.length - 1 ? widthScale(data[i + 1].value) : 0;
  const xOffset = (funnelWidth - topWidth) / 2;
  const nextXOffset = (funnelWidth - bottomWidth) / 2;
  const xStart = isRTL ? funnelWidth - xOffset : xOffset;
  const xEnd = isRTL ? funnelWidth - nextXOffset : nextXOffset;

  const isLastSegment = i === data.length - 1;
  const textY = isLastSegment ? i * segmentHeight + segmentHeight * 0.33 : i * segmentHeight + segmentHeight / 2;
  const textX = funnelWidth / 2;

  let availableWidth: number;
  if (isLastSegment) {
    const yFromTop = textY - i * segmentHeight;
    const widthAtY = topWidth * (1 - yFromTop / segmentHeight);
    availableWidth = Math.max(widthAtY * 0.8, 0);
  } else {
    availableWidth = Math.min(topWidth, bottomWidth) * 0.9;
  }

  let pathD: string;
  if (roundCorners) {
    // Top edge (corners 0,1): outer on first segment, connecting to prev on all others.
    // Bottom edge (corners 2,3): connecting to next on all but last, outer on last.
    const isTopOuter = i === 0;
    const isBotOuter = i === data.length - 1;
    const cornerModes: ('inward' | 'outward')[] = [
      isTopOuter ? 'inward' : 'outward',
      isTopOuter ? 'inward' : 'outward',
      isBotOuter ? 'inward' : 'outward',
      isBotOuter ? 'inward' : 'outward',
    ];
    pathD = roundedPolygonPath(
      [
        [xStart, i * segmentHeight],
        [funnelWidth - xStart, i * segmentHeight],
        [funnelWidth - xEnd, (i + 1) * segmentHeight],
        [xEnd, (i + 1) * segmentHeight],
      ],
      ROUNDED_CORNER_RADIUS,
      cornerModes,
    );
  } else {
    pathD = `M${xStart},${i * segmentHeight}
    L${funnelWidth - xStart},${i * segmentHeight}
    L${funnelWidth - xEnd},${(i + 1) * segmentHeight}
    L${xEnd},${(i + 1) * segmentHeight}
    Z`;
  }
  return { pathD, textX, textY, availableWidth };
}

/**
 * Gets horizontal funnel segment geometry scaled by `maxValue`, mirroring x coordinates when `isRTL` is true.
 */
export function getHorizontalFunnelSegmentGeometry({
  d,
  i,
  data,
  maxValue,
  funnelWidth,
  funnelHeight,
  isRTL,
  roundCorners = false,
}: {
  d: SimpleFunnelDataPoint;
  i: number;
  data: SimpleFunnelDataPoint[];
  maxValue: number;
  funnelWidth: number;
  funnelHeight: number;
  isRTL: boolean;
  roundCorners?: boolean;
}): FunnelSegmentGeometry {
  const segmentWidth = funnelWidth / data.length;
  const heightScale = (value: number) => (value / maxValue) * funnelHeight;
  // Mirror segment x-coordinates in RTL so stages render from right to left.
  const x0 = isRTL ? funnelWidth - (i + 1) * segmentWidth : i * segmentWidth;
  const x1 = isRTL ? funnelWidth - i * segmentWidth : (i + 1) * segmentWidth;
  const segmentPixelWidth = Math.abs(x1 - x0);

  // In RTL the x-positions are mirrored, so the height assignments must be
  // swapped too: the left edge (x0) connects to the next segment and the right
  // edge (x1) represents the current stage — the opposite of LTR.
  const ltrLeftHeight = heightScale(d.value);
  const ltrRightHeight = i < data.length - 1 ? heightScale(data[i + 1].value) : 0;
  const leftHeight = isRTL ? ltrRightHeight : ltrLeftHeight;
  const rightHeight = isRTL ? ltrLeftHeight : ltrRightHeight;

  const yOffset = (funnelHeight - leftHeight) / 2;
  const nextYOffset = (funnelHeight - rightHeight) / 2;

  const isLastSegment = i === data.length - 1;
  let textX: number;
  let textY: number;
  let availableWidth = segmentWidth * 0.8;

  // The wide end of the last (tapered) segment is opposite the taper point.
  const wideEndHeight = isRTL ? rightHeight : leftHeight;
  if (isLastSegment) {
    // Place text near the wide end, away from the taper point.
    textX = isRTL
      ? x1 - segmentPixelWidth * LAST_SEGMENT_TEXT_OFFSET_RATIO
      : x0 + segmentPixelWidth * LAST_SEGMENT_TEXT_OFFSET_RATIO;
    textY = funnelHeight / 2;
    const segmentArea = (wideEndHeight * segmentPixelWidth) / 2;
    if (wideEndHeight < 40 || segmentArea < 800) {
      availableWidth = 0;
    } else {
      availableWidth = segmentPixelWidth * LAST_SEGMENT_LABEL_WIDTH_RATIO;
    }
  } else {
    textX = (x0 + x1) / 2;
    textY = funnelHeight / 2;
    const minHeight = Math.min(leftHeight, rightHeight);
    availableWidth = minHeight > 20 ? segmentPixelWidth * 0.8 : 0;
  }

  let pathD: string;
  if (roundCorners) {
    // x0-side corners (0=TL, 3=BL): outer for LTR first / RTL last, otherwise connects to prev stage.
    // x1-side corners (1=TR, 2=BR): outer for LTR last / RTL first, otherwise connects to next stage.
    const n = data.length;
    const isX0Outer = isRTL ? i === n - 1 : i === 0;
    const isX1Outer = isRTL ? i === 0 : i === n - 1;
    const cornerModes: ('inward' | 'outward')[] = [
      isX0Outer ? 'inward' : 'outward',
      isX1Outer ? 'inward' : 'outward',
      isX1Outer ? 'inward' : 'outward',
      isX0Outer ? 'inward' : 'outward',
    ];
    pathD = roundedPolygonPath(
      [
        [x0, yOffset],
        [x1, nextYOffset],
        [x1, funnelHeight - nextYOffset],
        [x0, funnelHeight - yOffset],
      ],
      ROUNDED_CORNER_RADIUS,
      cornerModes,
    );
  } else {
    pathD = `M${x0},${yOffset}
    L${x1},${nextYOffset}
    L${x1},${funnelHeight - nextYOffset}
    L${x0},${funnelHeight - yOffset}
    Z`;
  }
  return { pathD, textX, textY, availableWidth };
}

export function getStackedVerticalFunnelSegmentGeometry({
  i,
  k,
  stages,
  totals,
  maxTotal,
  funnelWidth,
  funnelHeight,
  roundCorners = false,
}: {
  i: number;
  k: number;
  stages: Stage[];
  totals: number[];
  maxTotal: number;
  funnelWidth: number;
  funnelHeight: number;
  roundCorners?: boolean;
}): FunnelSegmentGeometry {
  const segmentHeight = funnelHeight / stages.length;
  const cur = stages[i];
  const next = stages[i + 1] || { subValues: [] };
  const curTotal = totals[i] || 1;
  const nextTotal = totals[i + 1] || 0;

  let cumTop = 0;
  let cumBot = 0;
  for (let idx = 0; idx < k; idx++) {
    const v = cur.subValues[idx];
    const vNext = next.subValues?.find((x: SubValue) => x.category === v.category);
    cumTop += (v.value / curTotal) * (curTotal / maxTotal) * funnelWidth;
    cumBot += (vNext ? vNext.value / nextTotal : 0) * (nextTotal / maxTotal) * funnelWidth;
  }
  const v = cur.subValues[k];
  const vNext = next.subValues?.find((x: SubValue) => x.category === v.category);
  const topW = (v.value / curTotal) * (curTotal / maxTotal) * funnelWidth;
  const botW = (vNext ? vNext.value / nextTotal : 0) * (nextTotal / maxTotal) * funnelWidth;
  const topStart = (funnelWidth - (curTotal / maxTotal) * funnelWidth) / 2 + cumTop;
  const topEnd = topStart + topW;
  const botStart = (funnelWidth - (nextTotal / maxTotal) * funnelWidth) / 2 + cumBot;
  const botEnd = botStart + botW;
  const textX = (topStart + topEnd + botStart + botEnd) / 4;

  const isLastSegment = i === stages.length - 1;
  const textY = isLastSegment ? i * segmentHeight + segmentHeight * 0.33 : (i + 0.5) * segmentHeight;

  let availableWidth: number;
  if (isLastSegment) {
    const yFromTop = textY - i * segmentHeight;
    availableWidth = topW * (1 - yFromTop / segmentHeight);
  } else {
    availableWidth = Math.min(topW, botW);
  }

  let pathD: string;
  if (roundCorners) {
    const isTopOuter = i === 0;
    const isBotOuter = i === stages.length - 1;
    const cornerModes: ('inward' | 'outward')[] = [
      isTopOuter ? 'inward' : 'outward',
      isTopOuter ? 'inward' : 'outward',
      isBotOuter ? 'inward' : 'outward',
      isBotOuter ? 'inward' : 'outward',
    ];
    pathD = roundedPolygonPath(
      [
        [topStart, i * segmentHeight],
        [topEnd, i * segmentHeight],
        [botEnd, (i + 1) * segmentHeight],
        [botStart, (i + 1) * segmentHeight],
      ],
      ROUNDED_CORNER_RADIUS,
      cornerModes,
    );
  } else {
    pathD = `M${topStart},${i * segmentHeight}
    L${topEnd},${i * segmentHeight}
    L${botEnd},${(i + 1) * segmentHeight}
    L${botStart},${(i + 1) * segmentHeight}
    Z`;
  }
  return { pathD, textX, textY, availableWidth };
}

export function getStackedHorizontalFunnelSegmentGeometry({
  i,
  k,
  stages,
  totals,
  maxTotal,
  funnelWidth,
  funnelHeight,
  roundCorners = false,
}: {
  i: number;
  k: number;
  stages: Stage[];
  totals: number[];
  maxTotal: number;
  funnelWidth: number;
  funnelHeight: number;
  roundCorners?: boolean;
}): FunnelSegmentGeometry {
  const segmentWidth = funnelWidth / stages.length;
  const cur = stages[i];
  const next = stages[i + 1] || { subValues: [] };
  const curTotal = totals[i] || 1;
  const nextTotal = totals[i + 1] || 0;

  let cumTop = 0;
  let cumBot = 0;
  for (let idx = 0; idx < k; idx++) {
    const v = cur.subValues[idx];
    const vNext = next.subValues?.find((x: SubValue) => x.category === v.category);
    cumTop += (v.value / curTotal) * (curTotal / maxTotal) * funnelHeight;
    cumBot += (vNext ? vNext.value / nextTotal : 0) * (nextTotal / maxTotal) * funnelHeight;
  }
  const v = cur.subValues[k];
  const vNext = next.subValues?.find((x: SubValue) => x.category === v.category);
  const topH = (v.value / curTotal) * (curTotal / maxTotal) * funnelHeight;
  const botH = (vNext ? vNext.value / nextTotal : 0) * (nextTotal / maxTotal) * funnelHeight;
  const leftStart = i * segmentWidth;
  const leftEnd = (i + 1) * segmentWidth;
  const topStart = (funnelHeight - (curTotal / maxTotal) * funnelHeight) / 2 + cumTop;
  const topEnd = topStart + topH;
  const botStart = (funnelHeight - (nextTotal / maxTotal) * funnelHeight) / 2 + cumBot;
  const botEnd = botStart + botH;

  const isLastSegment = i === stages.length - 1;
  let textX: number;
  let textY: number;
  let availableWidth: number;

  if (isLastSegment) {
    textX = leftStart + (leftEnd - leftStart) * 0.25;
    textY = (topStart + topEnd) / 2;
    const segmentArea = (topH * segmentWidth) / 2;
    availableWidth = topH < 24 || segmentArea < 600 ? 0 : (leftEnd - leftStart) * 0.5 * 0.8;
  } else {
    textX = (leftStart + leftEnd) / 2;
    textY = (topStart + topEnd + botStart + botEnd) / 4;
    const avgHeight = (topH + botH) / 2;
    availableWidth = avgHeight < 20 ? 0 : Math.abs(leftEnd - leftStart) * 0.9;
  }

  let pathD: string;
  if (roundCorners) {
    const isX0Outer = i === 0;
    const isX1Outer = i === stages.length - 1;
    const cornerModes: ('inward' | 'outward')[] = [
      isX0Outer ? 'inward' : 'outward',
      isX1Outer ? 'inward' : 'outward',
      isX1Outer ? 'inward' : 'outward',
      isX0Outer ? 'inward' : 'outward',
    ];
    pathD = roundedPolygonPath(
      [
        [leftStart, topStart],
        [leftEnd, botStart],
        [leftEnd, botEnd],
        [leftStart, topEnd],
      ],
      ROUNDED_CORNER_RADIUS,
      cornerModes,
    );
  } else {
    pathD = `M${leftStart},${topStart}
    L${leftEnd},${botStart}
    L${leftEnd},${botEnd}
    L${leftStart},${topEnd}
    Z`;
  }
  return { pathD, textX, textY, availableWidth };
}

/**
 * Computes whether segment value text should be shown and at what position.
 */
export function getSegmentTextProps({
  availableWidth,
  minTextWidth = 24,
  textX,
  textY,
  value,
}: {
  availableWidth: number;
  minTextWidth?: number;
  textX: number;
  textY: number;
  value: number;
}): {
  show: boolean;
  x: number;
  y: number;
  value: number;
} {
  return {
    show: availableWidth > minTextWidth && availableWidth > 0,
    x: textX,
    y: textY,
    value,
  };
}

/**
 * Returns a contrasting text color (black or white) for a given hex fill color.
 */
export function getContrastTextColor(hexColor: string): string {
  if (!hexColor || !hexColor.startsWith('#')) {
    return '#000000';
  }
  const normalized = hexColor.replace('#', '');
  if (normalized.length !== 6) {
    return '#000000';
  }
  const toLinear = (c: number) => (c <= 0.03928 ? c / 12.92 : Math.pow((c + 0.055) / 1.055, 2.4));
  const r = toLinear(parseInt(normalized.slice(0, 2), 16) / 255);
  const g = toLinear(parseInt(normalized.slice(2, 4), 16) / 255);
  const b = toLinear(parseInt(normalized.slice(4, 6), 16) / 255);
  const luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;
  return luminance > 0.179 ? '#000000' : '#ffffff';
}

/**
 * Returns true when the data array uses stacked sub-values for every stage.
 */
export function isStackedFunnelData(data: FunnelDataPoint[]): boolean {
  return (
    Array.isArray(data) &&
    data.length > 0 &&
    data.every(stage => Array.isArray(stage.subValues) && stage.subValues.length > 0)
  );
}

/**
 * Returns the stage geometry params for stacked mode.
 */
export function buildStackedGeometryParams(data: FunnelDataPoint[]): {
  stages: Array<{ subValues: SubValue[] }>;
  totals: number[];
  maxTotal: number;
} {
  const stages = data.map(s => ({
    subValues: (s.subValues ?? []) as SubValue[],
  }));
  const totals = stages.map(s => s.subValues.reduce((sum, sv) => sum + sv.value, 0));
  const maxTotal = Math.max(...totals);
  return { stages, totals, maxTotal };
}
