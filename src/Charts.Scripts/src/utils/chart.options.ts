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
 * A single legend item shared across all chart components.
 *
 * @public
 */
export interface Legend {
  legend: string;
  color: string;
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
