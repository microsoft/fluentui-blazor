import type { AccessibilityData, VerticalBarDataPointMetadata } from '../utils/chart-options.js';

/** @public */
export interface GroupedVerticalBarChartDataPoint extends VerticalBarDataPointMetadata {
  /** @public */ key: string;
  /** @public */ data: number;
  /** Display text used in legends and callouts. Defaults to `key`. */
  /** @public */ legend?: string;
  /** Renders this series against the secondary y-axis. */
  /** @public */ useSecondaryYScale?: boolean;
}

/** @public */
export interface GroupedVerticalBarChartLineDataPoint {
  /** @public */ y: number;
  /** @public */ legend: string;
  /** @public */ color?: string;
  /** @public */ yAxisCalloutData?: string;
  /** @public */ useSecondaryYScale?: boolean;
  /** @public */ onClick?: VoidFunction;
  /** @public */ callOutAccessibilityData?: AccessibilityData;
}

/** @public */
export interface GroupedVerticalBarChartData {
  /** @public */ xAxisPoint: string;
  /** @public */ series: GroupedVerticalBarChartDataPoint[];
  /** Optional line-series points at this x-axis category. */
  /** @public */ lineData?: GroupedVerticalBarChartLineDataPoint[];
  /** Accessibility data announced when the complete group callout is shown. */
  /** @public */ stackCallOutAccessibilityData?: AccessibilityData;
}
