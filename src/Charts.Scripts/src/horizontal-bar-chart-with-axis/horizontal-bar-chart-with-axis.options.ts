import type { AccessibilityData } from '../utils/chart-options.js';

/**
 * A single data point in the horizontal bar chart with axis.
 *
 * @public
 */
export interface HorizontalBarChartWithAxisDataPoint {
  /** Numeric value plotted along the x-axis. */
  x: number;

  /** Category or numeric value plotted along the y-axis. */
  y: number | string;

  /** Legend label for this data point. */
  legend?: string;

  /** Fill color for the bar. Falls back to the default color palette when omitted. */
  color?: string;

  /**
   * Gradient fill for the bar. A tuple of two CSS color strings: [startColor, endColor].
   * Overrides `color` when `enableGradient` is set to true on the chart.
   */
  gradient?: [string, string];

  /** Custom label shown for the x value in the tooltip. */
  xAxisCalloutData?: string;

  /** Custom label shown for the y value in the tooltip. */
  yAxisCalloutData?: string;

  /** Click handler called when the user activates this data point. */
  onClick?: VoidFunction;

  /** Accessibility data for screen readers attached to this data point. */
  callOutAccessibilityData?: AccessibilityData;
}
