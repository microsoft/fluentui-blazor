/**
 * Horizontal alignment of the chart title.
 *
 * @public
 */
export type ChartTitleAlign = 'start' | 'center' | 'end';

/**
 * Vertical position of the chart title relative to the chart body.
 *
 * @public
 */
export type ChartTitlePosition = 'top' | 'bottom';

/**
 * Position of the legend relative to the chart body.
 *
 * @public
 */
export type ChartLegendPosition = 'top' | 'bottom' | 'start' | 'end';

/**
 * Marker shape displayed for a chart series and its legend item.
 *
 * @public
 */
export type ChartMarkerShape =
  | 'circle'
  | 'square'
  | 'triangle'
  | 'diamond'
  | 'pyramid'
  | 'hexagon'
  | 'pentagon'
  | 'octagon';

/**
 * A single legend item shared across all chart components.
 *
 * @public
 */
export interface Legend {
  legend: string;
  color: string;
  isLineLegendInBarChart?: boolean;
  lineStrokeDasharray?: string | number;
  shape?: ChartMarkerShape;
}

/**
 * Controls the ordering of categories along the y-axis for bar charts.
 *
 * @public
 */
export type AxisCategoryOrder =
  | 'default'
  | 'data'
  | 'category ascending'
  | 'category descending'
  | 'total ascending'
  | 'total descending'
  | 'min ascending'
  | 'min descending'
  | 'max ascending'
  | 'max descending'
  | 'sum ascending'
  | 'sum descending'
  | 'mean ascending'
  | 'mean descending'
  | 'median ascending'
  | 'median descending';

/** Numeric Cartesian axis scale type. */
/** @public */
export type AxisScaleType = 'default' | 'log';

/** Plot margins in pixels. */
/** @public */
export interface ChartMargins {
  top: number;
  right: number;
  bottom: number;
  left: number;
}

/** @public */
export interface ChartAnnotationCoordinate {
  type: 'data' | 'relative' | 'pixel';
  x: number | string | Date;
  y: number | string | Date;
  yAxis?: 'primary' | 'secondary';
}

/** @public */
export interface ChartAnnotationTextRun {
  text: string;
  textColor?: string;
  fontWeight?: string | number;
  fontStyle?: 'normal' | 'italic';
}

/** @public */
export interface ChartAnnotationTextLine {
  runs: ChartAnnotationTextRun[];
  bullet?: boolean;
  indent?: number;
}

/** @public */
export interface ChartAnnotation {
  id?: string;
  text: string;
  textLines?: ChartAnnotationTextLine[];
  coordinates: ChartAnnotationCoordinate;
  layout?: {
    align?: 'start' | 'center' | 'end';
    verticalAlign?: 'top' | 'middle' | 'bottom';
    offsetX?: number;
    offsetY?: number;
    rotation?: number;
  };
  style?: {
    textColor?: string;
    fontSize?: string;
    fontWeight?: string | number;
    opacity?: number;
  };
  connector?: {
    strokeColor?: string;
    strokeWidth?: number;
    dashArray?: string;
    arrow?: boolean;
  };
  accessibility?: AccessibilityData & { role?: string };
  data?: Record<string, unknown>;
}

/**
 * Accessibility data attached to an individual chart data point.
 *
 * @public
 */
export interface AccessibilityData {
  /** Custom aria-label that overrides the auto-generated one. */
  ariaLabel?: string;
}

/**
 * Display, callout, accessibility, and interaction metadata shared by vertical bar data points.
 *
 * @public
 */
export interface VerticalBarDataPointMetadata {
  /** Text or date that overrides the x value displayed in the tooltip. */
  xAxisCalloutData?: string | Date;
  /** Text that overrides the numeric value displayed in the tooltip. */
  yAxisCalloutData?: string;
  /** Color token or CSS color for the data point. */
  color?: string;
  /** Explicit start and end colors for the vertical bar gradient. */
  gradient?: [string, string];
  /** Text rendered as the visible bar label instead of the formatted numeric value. */
  barLabel?: string;
  /** Invoked when the data point is activated. */
  onClick?: VoidFunction;
  /** Accessibility data attached to the rendered data point. */
  callOutAccessibilityData?: AccessibilityData;
}

/**
 * Shared base tooltip state used by all chart components.
 *
 * @public
 */
export interface TooltipProps {
  isVisible: boolean;
  legend: string;
  yValue: string;
  color: string;
  xPos: number;
  yPos: number;
}

/**
 * A function that customizes the tooltip content for a chart data point.
 *
 * @param dataPoint - The data point for which the tooltip is being shown.
 * @param defaultRender - A function that renders the default tooltip HTML string for the given data point.
 * @returns Either an HTML string or a DOM Node to inject into the tooltip body.
 *
 * @public
 */
export type TooltipRenderer<T> = (
  dataPoint: T,
  defaultRender: (point: T) => string,
) => string | Node | Promise<string | Node>;
