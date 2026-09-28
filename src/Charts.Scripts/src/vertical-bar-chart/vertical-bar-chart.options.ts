import type { VerticalBarDataPointMetadata } from '../utils/chart-options.js';

/** @public */
export interface VerticalBarChartLineDataPoint {
  /** @public */ y: number;
  /** @public */ yAxisCalloutData?: string;
  /** @public */ useSecondaryYScale?: boolean;
  /** @public */ onClick?: VoidFunction;
}

/** @public */
export interface VerticalBarChartDataPoint extends VerticalBarDataPointMetadata {
  /** @public */ x: string | number | Date;
  /** @public */ y: number;
  /** @public */ legend?: string;
  /** @public */ lineData?: VerticalBarChartLineDataPoint;
}
