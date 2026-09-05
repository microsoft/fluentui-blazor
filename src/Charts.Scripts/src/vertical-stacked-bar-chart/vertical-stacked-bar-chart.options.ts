import type { AccessibilityData, VerticalBarDataPointMetadata } from '../utils/chart-options.js';

/** @public */
export interface VerticalStackedBarChartDataPoint extends VerticalBarDataPointMetadata {
  /** @public */ legend: string;
  /** @public */ data: number;
}

/** @public */
export interface VerticalStackedBarChartLineDataPoint {
  /** @public */ y: number;
  /** Text that overrides the line value displayed in the tooltip. */
  /** @public */ yAxisCalloutData?: string;
  /** @public */ legend: string;
  /** @public */ color?: string;
  /** @public */ useSecondaryYScale?: boolean;
  /** @public */ onClick?: VoidFunction;
}

/** @public */
export interface VerticalStackedBarChartProps {
  /** @public */ xAxisPoint: string | number | Date;
  /** @public */ chartData: VerticalStackedBarChartDataPoint[];
  /** @public */ lineData?: VerticalStackedBarChartLineDataPoint[];
  /** Accessibility data announced when the complete stack callout is shown. */
  /** @public */ stackCallOutAccessibilityData?: AccessibilityData;
}
