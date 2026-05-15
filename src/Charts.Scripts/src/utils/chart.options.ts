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
