import type { AccessibilityData } from '../utils/chart-options.js';

/** @public */
export interface AreaChartDataPoint {
  /** @public */ x: number | Date;
  /** @public */ y: number;
  /** Text or date that overrides the x value displayed in the tooltip. Dates are formatted using the chart culture. */
  /** @public */ xAxisCalloutData?: string | Date;
  /** Text that overrides the series value displayed in the tooltip. */
  /** @public */ yAxisCalloutData?: string;
  /** Custom aria-label for the x-axis callout when hovering this data point. */
  xAxisCalloutAccessibilityData?: AccessibilityData;
  /** Custom aria-label for the series callout entry when hovering this data point. */
  callOutAccessibilityData?: AccessibilityData;
}

/** @public */
export interface AreaChartSeries {
  /** @public */ legend: string;
  /** @public */ data: AreaChartDataPoint[];
  /** @public */ color?: string;
  /**
   * When true this series is plotted against the right-hand secondary Y axis rather than
   * the primary (left) Y axis.  The secondary axis has its own independent scale.
   * @public
   */
  useSecondaryYScale?: boolean;
}

/**
 * Controls how the area fill is computed.
 * - `'tonexty'` (default): Stacked — each area fills from the top of the previous series' line up to its own line.
 * - `'tozeroy'`: Non-stacked — each area fills independently from y=0 to its own line.
 * @public
 */
export type AreaChartMode = 'tonexty' | 'tozeroy';
