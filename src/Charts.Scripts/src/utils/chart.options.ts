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
